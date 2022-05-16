﻿/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

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
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the DependencyManager class.
		/// </summary>
		public DependencyManager()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly IDictionary<Tuple<Type, string>, IDependencyResolution> dependencyResolutionRegistrations = new Dictionary<Tuple<Type, string>, IDependencyResolution>();
		private readonly AsyncReaderWriterLock readerWriterLockDual = new AsyncReaderWriterLock();

		#endregion

		#region Properties/Indexers/Events

		private IDictionary<Tuple<Type, string>, IDependencyResolution> DependencyResolutionRegistrations
		{
			get
			{
				return this.dependencyResolutionRegistrations;
			}
		}

		private AsyncReaderWriterLock ReaderWriterLock
		{
			get
			{
				return this.readerWriterLockDual;
			}
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Adds a new dependency resolution for a given resolution type and selector key. Throws a DependencyException if the resolution type and selector key combination has been previously registered in this instance. This is the generic overload.
		/// </summary>
		/// <typeparam name="TResolution"> The resolution type of resolution. </typeparam>
		/// <param name="selectorKey"> An non-null, zero or greater length string selector key. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution existence check. </param>
		/// <param name="dependencyResolution"> The dependency resolution. </param>
		public void AddResolution<TResolution>(string selectorKey, bool includeAssignableTypes, IDependencyResolution<TResolution> dependencyResolution)
		{
			Type resolutionType;

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			if ((object)dependencyResolution == null)
				throw new ArgumentNullException(nameof(dependencyResolution));

			resolutionType = typeof(TResolution);

			this.AddResolution(resolutionType, selectorKey, includeAssignableTypes, dependencyResolution);
		}

		/// <summary>
		/// Adds a new dependency resolution for a given resolution type and selector key. Throws a DependencyException if the resolution type and selector key combination has been previously registered in this instance. This is the non-generic overload.
		/// </summary>
		/// <param name="resolutionType"> The resolution type of resolution. </param>
		/// <param name="selectorKey"> An non-null, zero or greater length string selector key. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution existence check. </param>
		/// <param name="dependencyResolution"> The dependency resolution. </param>
		public void AddResolution(Type resolutionType, string selectorKey, bool includeAssignableTypes, IDependencyResolution dependencyResolution)
		{
			Tuple<Type, string> trait;
			IEnumerable<KeyValuePair<Tuple<Type, string>, IDependencyResolution>> candidateResolutions;

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			if ((object)dependencyResolution == null)
				throw new ArgumentNullException(nameof(dependencyResolution));

			// cop a writer lock
			using (this.ReaderWriterLock.WriterLock())
			{
				if (this.IsDisposed)
					throw new ObjectDisposedException(typeof(DependencyManager).FullName);

				// ...
				{
					trait = new Tuple<Type, string>(resolutionType, selectorKey);
					candidateResolutions = this.GetCandidateResolutionsMustReadLock(resolutionType, selectorKey, includeAssignableTypes);

					if (candidateResolutions.Any())
						throw new DependencyException(string.Format("Dependency resolution already exists in the dependency manager for resolution type '{0}' and selector key '{1}' (include assignable types: '{2}').", resolutionType.FullName, selectorKey, includeAssignableTypes));

					this.DependencyResolutionRegistrations.Add(trait, dependencyResolution);
				}
			}
		}

		/// <summary>
		/// Clears all registered dependency resolutions from this instance.
		/// </summary>
		/// <returns> A value indicating if at least one dependency resolution was removed. </returns>
		public bool ClearAllResolutions()
		{
			bool result;

			// cop a writer lock
			using (this.ReaderWriterLock.WriterLock())
			{
				if (this.IsDisposed)
					throw new ObjectDisposedException(typeof(DependencyManager).FullName);

				// ...
				{
					result = this.FreeDependencyResolutionsMustReadLock();
					return result;
				}
			}
		}

		/// <summary>
		/// Clears all registered dependency resolutions of the specified resolution type from this instance. This is the generic overload.
		/// </summary>
		/// <typeparam name="TResolution"> The resolution type of removal. </typeparam>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution removal list. </param>
		/// <returns> A value indicating if at least one dependency resolution was removed. </returns>
		public bool ClearTypeResolutions<TResolution>(bool includeAssignableTypes)
		{
			Type resolutionType;

			resolutionType = typeof(TResolution);

			return this.ClearTypeResolutions(resolutionType, includeAssignableTypes);
		}

		/// <summary>
		/// Clears all registered dependency resolutions of the specified resolution type from this instance. This is the non-generic overload.
		/// </summary>
		/// <param name="resolutionType"> The resolution type of removal. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution removal list. </param>
		/// <returns> A value indicating if at least one dependency resolution was removed. </returns>
		public bool ClearTypeResolutions(Type resolutionType, bool includeAssignableTypes)
		{
			int count = 0;
			IEnumerable<KeyValuePair<Tuple<Type, string>, IDependencyResolution>> candidateResolutions;

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			// cop a writer lock
			using (this.ReaderWriterLock.WriterLock())
			{
				if (this.IsDisposed)
					throw new ObjectDisposedException(typeof(DependencyManager).FullName);

				// ...
				{
					// force execution to prevent 'System.InvalidOperationException : Collection was modified; enumeration operation may not execute.'
					candidateResolutions = this.GetCandidateResolutionsMustReadLock(resolutionType, null, includeAssignableTypes).ToArray();

					foreach (KeyValuePair<Tuple<Type, string>, IDependencyResolution> dependencyResolutionRegistration in candidateResolutions)
					{
						if ((object)dependencyResolutionRegistration.Value != null)
							dependencyResolutionRegistration.Value.Dispose();

						this.DependencyResolutionRegistrations.Remove(dependencyResolutionRegistration.Key);

						count++;
					}

					return count > 0;
				}
			}
		}

		protected override void CoreCreate(bool creating)
		{
			// do nothing
		}

		/// <summary>
		/// Clears all dependency resolutions and cleans up any resources. Once disposed, the instance cannot be reused.
		/// </summary>
		protected override void CoreDispose(bool disposing)
		{
			if (disposing)
			{
				// cop a reader lock
				//this.ReaderWriterLock.EnterUpgradeableReadLock();

				try
				{
					if (this.IsDisposed) // must check under read lock
						return;

					// cop a writer lock
					//this.ReaderWriterLock.EnterWriteLock();

					try
					{
						this.FreeDependencyResolutionsMustReadLock();
					}
					finally
					{
						//this.ReaderWriterLock.ExitWriteLock();
					}
				}
				finally
				{
					//this.ReaderWriterLock.ExitUpgradeableReadLock();
				}
			}
		}

		private bool FreeDependencyResolutionsMustReadLock()
		{
			bool result;

			result = this.DependencyResolutionRegistrations.Count > 0;

			foreach (KeyValuePair<Tuple<Type, string>, IDependencyResolution> dependencyResolutionRegistration in this.DependencyResolutionRegistrations)
			{
				if ((object)dependencyResolutionRegistration.Value != null)
					dependencyResolutionRegistration.Value.Dispose();
			}

			this.DependencyResolutionRegistrations.Clear();

			return result;
		}

		private IEnumerable<KeyValuePair<Tuple<Type, string>, IDependencyResolution>> GetCandidateResolutionsMustReadLock(Type resolutionType, string selectorKey, bool includeAssignableTypes)
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

			return candidateResolutions;
		}

		private IDependencyResolution GetDependencyResolutionMustReadLock(Type resolutionType, string selectorKey, bool includeAssignableTypes)
		{
			IEnumerable<KeyValuePair<Tuple<Type, string>, IDependencyResolution>> candidateResolutions;
			IDependencyResolution dependencyResolution;
			Tuple<Type, string> trait;

			// selector key can be null in this context
			//if ((object)selectorKey == null)
			//throw new ArgumentNullException(nameof(selectorKey));

			trait = new Tuple<Type, string>(resolutionType, selectorKey);
			candidateResolutions = this.GetCandidateResolutionsMustReadLock(resolutionType, selectorKey, includeAssignableTypes);

			// first attempt direct resolution: exact type and selector key
			dependencyResolution = candidateResolutions
				.OrderBy(drr => drr.Key == trait)
				.Select(drr => drr.Value)
				.FirstOrDefault();

			return dependencyResolution;
		}

		/// <summary>
		/// Gets a value indicating whether there are any registered dependency resolutions of the specified resolution type in this instance. If selector key is a null value, then this method will return true if any resolution exists for the specified resolution type, regardless of selector key; otherwise, this method will return true only if a resolution exists for the specified resolution type and selector key. This is the generic overload.
		/// </summary>
		/// <param name="selectorKey"> An null or zero or greater length string selector key. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution existence check. </param>
		/// <typeparam name="TResolution"> The resolution type of the check. </typeparam>
		/// <returns> A value indicating if at least one dependency resolution is present. </returns>
		public bool HasTypeResolution<TResolution>(string selectorKey, bool includeAssignableTypes)
		{
			Type resolutionType;

			// selector key can be null in this context
			//if ((object)selectorKey == null)
			//throw new ArgumentNullException(nameof(selectorKey));

			resolutionType = typeof(TResolution);

			return this.HasTypeResolution(resolutionType, selectorKey, includeAssignableTypes);
		}

		/// <summary>
		/// Gets a value indicating whether there are any registered dependency resolutions of the specified resolution type in this instance. If selector key is a null value, then this method will return true if any resolution exists for the specified resolution type, regardless of selector key; otherwise, this method will return true only if a resolution exists for the specified resolution type and selector key. This is the non-generic overload.
		/// </summary>
		/// <param name="resolutionType"> The resolution type of the check. </param>
		/// <param name="selectorKey"> An null or zero or greater length string selector key. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution existence check. </param>
		/// <returns> A value indicating if at least one dependency resolution is present. </returns>
		public bool HasTypeResolution(Type resolutionType, string selectorKey, bool includeAssignableTypes)
		{
			IDependencyResolution dependencyResolution;

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			// selector key can be null in this context
			//if ((object)selectorKey == null)
			//throw new ArgumentNullException(nameof(selectorKey));

			// cop a reader lock
			using (this.ReaderWriterLock.ReaderLock())
			{
				if (this.IsDisposed)
					throw new ObjectDisposedException(typeof(DependencyManager).FullName);

				dependencyResolution = this.GetDependencyResolutionMustReadLock(resolutionType, selectorKey, includeAssignableTypes);

				return (object)dependencyResolution != null;
			}
		}

		/// <summary>
		/// Removes the registered dependency resolution of the specified resolution type and selector key from this instance. Throws a DependencyException if the resolution type and selector key combination has not been previously registered in this instance. This is the generic overload.
		/// </summary>
		/// <typeparam name="TResolution"> The resolution type of removal. </typeparam>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution removal list. </param>
		/// <param name="selectorKey"> An non-null, zero or greater length string selector key. </param>
		public void RemoveResolution<TResolution>(string selectorKey, bool includeAssignableTypes)
		{
			Type resolutionType;

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			resolutionType = typeof(TResolution);

			this.RemoveResolution(resolutionType, selectorKey, includeAssignableTypes);
		}

		/// <summary>
		/// Removes the registered dependency resolution of the specified resolution type and selector key from this instance. Throws a DependencyException if the resolution type and selector key combination has not been previously registered in this instance. This is the non-generic overload.
		/// </summary>
		/// <param name="resolutionType"> The resolution type of removal. </param>
		/// <param name="selectorKey"> An non-null, zero or greater length string selector key. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution removal list. </param>
		public void RemoveResolution(Type resolutionType, string selectorKey, bool includeAssignableTypes)
		{
			Tuple<Type, string> trait;
			IDependencyResolution dependencyResolution;

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			// cop a writer lock
			using (this.ReaderWriterLock.WriterLock())
			{
				if (this.IsDisposed)
					throw new ObjectDisposedException(typeof(DependencyManager).FullName);

				// ...
				{
					trait = new Tuple<Type, string>(resolutionType, selectorKey);
					dependencyResolution = this.GetDependencyResolutionMustReadLock(resolutionType, selectorKey, includeAssignableTypes);

					if ((object)dependencyResolution == null) // nothing to offer up
						throw new DependencyException(string.Format("Dependency resolution in the in-effect dependency manager failed to match for resolution type '{0}' and selector key '{1}' (include assignable types: '{2}').", resolutionType.FullName, selectorKey, includeAssignableTypes));

					dependencyResolution.Dispose();
					this.DependencyResolutionRegistrations.Remove(trait);
				}
			}
		}

		/// <summary>
		/// Resolves a dependency for the given resolution type and selector key combination. Throws a DependencyException if the resolution type and selector key combination has not been previously registered in this instance. Throws a DependencyException if the resolved value cannot be assigned to the resolution type. This is the non-generic overload.
		/// </summary>
		/// <typeparam name="TResolution"> The resolution type of resolution. </typeparam>
		/// <param name="selectorKey"> An non-null, zero or greater length string selector key. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution lookup list. </param>
		/// <returns> An object instance of assignable to the resolution type. </returns>
		public TResolution ResolveDependency<TResolution>(string selectorKey, bool includeAssignableTypes)
		{
			Type resolutionType;
			TResolution value;

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			resolutionType = typeof(TResolution);

			value = (TResolution)this.ResolveDependency(resolutionType, selectorKey, includeAssignableTypes);

			return value;
		}

		/// <summary>
		/// Resolves a dependency for the given resolution type and selector key combination. Throws a DependencyException if the resolution type and selector key combination has not been previously registered in this instance. Throws a DependencyException if the resolved value cannot be assigned to the resolution type. This is the non-generic overload.
		/// </summary>
		/// <param name="resolutionType"> The resolution type of resolution. </param>
		/// <param name="selectorKey"> An non-null, zero or greater length string selector key. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution lookup list. </param>
		/// <returns> An object instance of assignable to the resolution type. </returns>
		public object ResolveDependency(Type resolutionType, string selectorKey, bool includeAssignableTypes)
		{
			object value;
			IDependencyResolution dependencyResolution;
			Type typeOfValue;

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			// cop a reader lock
			using (this.ReaderWriterLock.ReaderLock())
			{
				if (this.IsDisposed)
					throw new ObjectDisposedException(typeof(DependencyManager).FullName);

				dependencyResolution = this.GetDependencyResolutionMustReadLock(resolutionType, selectorKey, includeAssignableTypes);

				if ((object)dependencyResolution == null) // nothing to offer up
					throw new DependencyException(string.Format("Dependency resolution in the in-effect dependency manager failed to match for resolution type '{0}' and selector key '{1}' (include assignable types: '{2}').", resolutionType.FullName, selectorKey, includeAssignableTypes));

				value = dependencyResolution.Resolve(this, resolutionType, selectorKey);

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