/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using Nito.AsyncEx;

namespace WellEngineered.Solder.Injection.Resolutions
{
	/// <summary>
	/// A composing dependency resolution implementation that allows only a specific instance
	/// to be lazy created on demand/first use, cached, and reused; the specific instance is
	/// retrieved from the inner dependency resolution once per the lifetime of this dependency resolution instance.
	/// Uses reader-writer lock for asynchronous protection (i.e. thread-safety).
	/// </summary>
	public sealed partial class SingletonWrapperDependencyResolution
		: DependencyResolution
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

		protected override async ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			if (creating)
			{
				if ((object)this.InnerDependencyResolution != null)
					await this.InnerDependencyResolution.CreateAsync();
			}
		}

		protected override async ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			if (disposing)
			{
				if ((object)this.InnerDependencyResolution != null)
					await this.InnerDependencyResolution.DisposeAsync();
			}
		}

		protected override async ValueTask<object> CoreResolveAsync(IDependencyManager dependencyManager, Type resolutionType, string selectorKey, CancellationToken cancellationToken = default)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			// cop a writer lock
			using (this.AsyncReaderWriterLock.WriterLock())
			{
				// ...
				if (!this.Frozen)
				{
					this.Instance = await this.InnerDependencyResolution.ResolveAsync(dependencyManager, resolutionType, selectorKey, cancellationToken);
					this.Frozen = true;
				}

				return this.Instance;
			}
		}

		#endregion
	}
}
#endif