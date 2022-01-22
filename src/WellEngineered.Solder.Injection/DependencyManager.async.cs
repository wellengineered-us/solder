/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Nito.AsyncEx;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	/// <summary>
	/// Provides dependency registration and resolution services.
	/// Uses reader-writer lock for asynchronous protection (i.e. thread-safety).
	/// </summary>
	public sealed partial class DependencyManager : DualLifecycle, IDependencyManager
	{
		#region Properties/Indexers/Events

		private AsyncReaderWriterLock AsyncReaderWriterLock
		{
			get
			{
				return this.readerWriterLockDual;
			}
		}

		#endregion

		#region Methods/Operators

		public async ValueTask AddResolutionAsync<TResolution>(string selectorKey, bool includeAssignableTypes, IDependencyResolution<TResolution> dependencyResolution, CancellationToken cancellationToken = default)
		{
			Type resolutionType;

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			if ((object)dependencyResolution == null)
				throw new ArgumentNullException(nameof(dependencyResolution));

			resolutionType = typeof(TResolution);

			await this.AddResolutionAsync(resolutionType, selectorKey, includeAssignableTypes, dependencyResolution, cancellationToken);
		}

		public async ValueTask AddResolutionAsync(Type resolutionType, string selectorKey, bool includeAssignableTypes, IDependencyResolution dependencyResolution, CancellationToken cancellationToken = default)
		{
			Tuple<Type, string> trait;
			IAsyncEnumerable<KeyValuePair<Tuple<Type, string>, IDependencyResolution>> candidateResolutions;

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			if ((object)dependencyResolution == null)
				throw new ArgumentNullException(nameof(dependencyResolution));

			// cop a writer lock
			using (await this.AsyncReaderWriterLock.WriterLockAsync(cancellationToken))
			{
				if (this.IsAsyncDisposed)
					throw new ObjectDisposedException(typeof(DependencyManager).FullName);

				// ...
				{
					trait = new Tuple<Type, string>(resolutionType, selectorKey);
					candidateResolutions = this.GetCandidateResolutionsMustReadLockAsync(resolutionType, selectorKey, includeAssignableTypes, cancellationToken);

					if (await candidateResolutions.CountAsync(cancellationToken) > 0)
						throw new DependencyException(string.Format("Dependency resolution already exists in the dependency manager for resolution type '{0}' and selector key '{1}' (include assignable types: '{2}').", resolutionType.FullName, selectorKey, includeAssignableTypes));

					this.DependencyResolutionRegistrations.Add(trait, dependencyResolution);
				}
			}
		}

		public async ValueTask<bool> ClearAllResolutionsAsync(CancellationToken cancellationToken = default)
		{
			bool result;

			// cop a writer lock
			using (await this.AsyncReaderWriterLock.WriterLockAsync(cancellationToken))
			{
				if (this.IsAsyncDisposed)
					throw new ObjectDisposedException(typeof(DependencyManager).FullName);

				// ...
				{
					result = await this.FreeDependencyResolutionsMustReadLockAsync();
					return result;
				}
			}
		}

		public async ValueTask<bool> ClearTypeResolutionsAsync<TResolution>(bool includeAssignableTypes, CancellationToken cancellationToken = default)
		{
			Type resolutionType;

			resolutionType = typeof(TResolution);

			return await this.ClearTypeResolutionsAsync(resolutionType, includeAssignableTypes, cancellationToken);
		}

		public async ValueTask<bool> ClearTypeResolutionsAsync(Type resolutionType, bool includeAssignableTypes, CancellationToken cancellationToken = default)
		{
			int count = 0;
			IAsyncEnumerable<KeyValuePair<Tuple<Type, string>, IDependencyResolution>> candidateResolutions;

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			// cop a writer lock
			using (await this.AsyncReaderWriterLock.WriterLockAsync(cancellationToken))
			{
				if (this.IsAsyncDisposed)
					throw new ObjectDisposedException(typeof(DependencyManager).FullName);

				// ...
				{
					// force execution to prevent 'System.InvalidOperationException : Collection was modified; enumeration operation may not execute.'
					candidateResolutions = this.GetCandidateResolutionsMustReadLockAsync(resolutionType, null, includeAssignableTypes, cancellationToken);

					await foreach (KeyValuePair<Tuple<Type, string>, IDependencyResolution> dependencyResolutionRegistration in candidateResolutions.WithCancellation(cancellationToken))
					{
						if ((object)dependencyResolutionRegistration.Value != null)
							await dependencyResolutionRegistration.Value.DisposeAsync();

						this.DependencyResolutionRegistrations.Remove(dependencyResolutionRegistration.Key);

						count++;
					}

					return count > 0;
				}
			}
		}

		protected override async ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			// do nothing
			await Task.CompletedTask;
		}

		protected override async ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			if (disposing)
			{
				// cop a writer lock
				using (await this.AsyncReaderWriterLock.WriterLockAsync(cancellationToken))
				{
					if (this.IsAsyncDisposed) // must check under read lock
						return;

					// ...
					{
						await this.FreeDependencyResolutionsMustReadLockAsync(cancellationToken);
					}
				}
			}
		}

		private async ValueTask<bool> FreeDependencyResolutionsMustReadLockAsync(CancellationToken cancellationToken = default)
		{
			bool result;

			result = this.DependencyResolutionRegistrations.Count > 0;

			foreach (KeyValuePair<Tuple<Type, string>, IDependencyResolution> dependencyResolutionRegistration in this.DependencyResolutionRegistrations)
			{
				if ((object)dependencyResolutionRegistration.Value != null)
					await dependencyResolutionRegistration.Value.DisposeAsync();
			}

			this.DependencyResolutionRegistrations.Clear();

			return result;
		}

		private async IAsyncEnumerable<KeyValuePair<Tuple<Type, string>, IDependencyResolution>> GetCandidateResolutionsMustReadLockAsync(Type resolutionType, string selectorKey, bool includeAssignableTypes, CancellationToken cancellationToken = default)
		{
			IEnumerable<KeyValuePair<Tuple<Type, string>, IDependencyResolution>> candidateResolutions;

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			// selector key can be null in this context
			//if ((object)selectorKey == null)
			//throw new ArgumentNullException(nameof(selectorKey));

			candidateResolutions = this.DependencyResolutionRegistrations.Where(drr =>
																					(drr.Key.Item1 == resolutionType || (includeAssignableTypes && resolutionType.IsAssignableFrom(drr.Key.Item1))) &&
																					((object)selectorKey == null || drr.Key.Item2 == selectorKey)
			);

			foreach (KeyValuePair<Tuple<Type, string>, IDependencyResolution> candidateResolution in candidateResolutions)
			{
				yield return candidateResolution;
			}

			await Task.CompletedTask;
		}

		private async ValueTask<IDependencyResolution> GetDependencyResolutionMustReadLockAsync(Type resolutionType, string selectorKey, bool includeAssignableTypes, CancellationToken cancellationToken = default)
		{
			IAsyncEnumerable<KeyValuePair<Tuple<Type, string>, IDependencyResolution>> candidateResolutions;
			IDependencyResolution dependencyResolution;
			Tuple<Type, string> trait;

			// selector key can be null in this context
			//if ((object)selectorKey == null)
			//throw new ArgumentNullException(nameof(selectorKey));

			trait = new Tuple<Type, string>(resolutionType, selectorKey);
			candidateResolutions = this.GetCandidateResolutionsMustReadLockAsync(resolutionType, selectorKey, includeAssignableTypes, cancellationToken);

			// first attempt direct resolution: exact type and selector key
			dependencyResolution = await candidateResolutions
				.OrderBy(drr => drr.Key == trait)
				.Select(drr => drr.Value)
				.FirstOrDefaultAsync(cancellationToken);

			return dependencyResolution;
		}

		public async ValueTask<bool> HasTypeResolutionAsync<TResolution>(string selectorKey, bool includeAssignableTypes, CancellationToken cancellationToken = default)
		{
			Type resolutionType;

			// selector key can be null in this context
			//if ((object)selectorKey == null)
			//throw new ArgumentNullException(nameof(selectorKey));

			resolutionType = typeof(TResolution);

			return await this.HasTypeResolutionAsync(resolutionType, selectorKey, includeAssignableTypes, cancellationToken);
		}

		public async ValueTask<bool> HasTypeResolutionAsync(Type resolutionType, string selectorKey, bool includeAssignableTypes, CancellationToken cancellationToken = default)
		{
			IDependencyResolution dependencyResolution;

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			// selector key can be null in this context
			//if ((object)selectorKey == null)
			//throw new ArgumentNullException(nameof(selectorKey));

			// cop a reader lock
			using (await this.AsyncReaderWriterLock.ReaderLockAsync(cancellationToken))
			{
				if (this.IsAsyncDisposed)
					throw new ObjectDisposedException(typeof(DependencyManager).FullName);

				dependencyResolution = await this.GetDependencyResolutionMustReadLockAsync(resolutionType, selectorKey, includeAssignableTypes, cancellationToken);

				return (object)dependencyResolution != null;
			}
		}

		public async ValueTask RemoveResolutionAsync<TResolution>(string selectorKey, bool includeAssignableTypes, CancellationToken cancellationToken = default)
		{
			Type resolutionType;

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			resolutionType = typeof(TResolution);

			await this.RemoveResolutionAsync(resolutionType, selectorKey, includeAssignableTypes, cancellationToken);
		}

		public async ValueTask RemoveResolutionAsync(Type resolutionType, string selectorKey, bool includeAssignableTypes, CancellationToken cancellationToken = default)
		{
			Tuple<Type, string> trait;
			IDependencyResolution dependencyResolution;

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			// cop a writer lock
			using (await this.AsyncReaderWriterLock.WriterLockAsync(cancellationToken))
			{
				if (this.IsAsyncDisposed)
					throw new ObjectDisposedException(typeof(DependencyManager).FullName);

				// ...
				{
					trait = new Tuple<Type, string>(resolutionType, selectorKey);
					dependencyResolution = await this.GetDependencyResolutionMustReadLockAsync(resolutionType, selectorKey, includeAssignableTypes, cancellationToken);

					if ((object)dependencyResolution == null) // nothing to offer up
						throw new DependencyException(string.Format("Dependency resolution in the in-effect dependency manager failed to match for resolution type '{0}' and selector key '{1}' (include assignable types: '{2}').", resolutionType.FullName, selectorKey, includeAssignableTypes));

					await dependencyResolution.DisposeAsync();
					this.DependencyResolutionRegistrations.Remove(trait);
				}
			}
		}

		public async ValueTask<TResolution> ResolveDependencyAsync<TResolution>(string selectorKey, bool includeAssignableTypes, CancellationToken cancellationToken = default)
		{
			Type resolutionType;
			TResolution value;

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			resolutionType = typeof(TResolution);

			value = (TResolution)await this.ResolveDependencyAsync(resolutionType, selectorKey, includeAssignableTypes, cancellationToken);

			return value;
		}

		public async ValueTask<object> ResolveDependencyAsync(Type resolutionType, string selectorKey, bool includeAssignableTypes, CancellationToken cancellationToken = default)
		{
			object value;
			IDependencyResolution dependencyResolution;
			Type typeOfValue;

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			// cop a reader lock
			using (await this.AsyncReaderWriterLock.ReaderLockAsync(cancellationToken))
			{
				if (this.IsAsyncDisposed)
					throw new ObjectDisposedException(typeof(DependencyManager).FullName);

				dependencyResolution = await this.GetDependencyResolutionMustReadLockAsync(resolutionType, selectorKey, includeAssignableTypes, cancellationToken);

				if ((object)dependencyResolution == null) // nothing to offer up
					throw new DependencyException(string.Format("Dependency resolution in the in-effect dependency manager failed to match for resolution type '{0}' and selector key '{1}' (include assignable types: '{2}').", resolutionType.FullName, selectorKey, includeAssignableTypes));

				value = await dependencyResolution.ResolveAsync(this, resolutionType, selectorKey, cancellationToken);

				if ((object)value != null)
				{
					typeOfValue = value.GetType();

					if (!resolutionType.IsAssignableFrom(typeOfValue))
						throw new DependencyException(string.Format("Dependency resolution in the dependency manager matched for resolution type '{0}' and selector key '{1}' but the resolved value type '{2}' is not assignable the resolution type '{3}'.", resolutionType.FullName, selectorKey, typeOfValue.FullName, resolutionType.FullName));
				}

				return value;
			}
		}

		#endregion
	}
}
#endif