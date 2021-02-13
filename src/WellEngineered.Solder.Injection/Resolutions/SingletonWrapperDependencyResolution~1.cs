/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;

namespace WellEngineered.Solder.Injection.Resolutions
{
	/// <summary>
	/// A composing dependency resolution implementation that allows only a specific instance
	/// to be lazy created on demand/first use, cached, and reused; the specific instance is
	/// retrieved from the inner dependency resolution once per the lifetime of this dependency resolution instance.
	/// Uses reader-writer lock for asynchronous protection (i.e. thread-safety).
	/// </summary>
	public sealed class SingletonWrapperDependencyResolution<TResolution> : DependencyResolution<TResolution>
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the SingletonWrapperDependencyResolution`1 class.
		/// Lazy loading semantics are assumed by design.
		/// </summary>
		/// <param name="innerDependencyResolution"> The chained dependency resolution which is called only once. </param>
		public SingletonWrapperDependencyResolution(IDependencyResolution<TResolution> innerDependencyResolution)
			: base(DependencyLifetime.Singleton)
		{
			if ((object)innerDependencyResolution == null)
				throw new ArgumentNullException(nameof(innerDependencyResolution));

			this.innerDependencyResolution = innerDependencyResolution;
		}

		#endregion

		#region Fields/Constants

		private readonly IDependencyResolution<TResolution> innerDependencyResolution;
		private readonly ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		private bool frozen;
		private TResolution instance;

		#endregion

		#region Properties/Indexers/Events

		private IDependencyResolution<TResolution> InnerDependencyResolution
		{
			get
			{
				return this.innerDependencyResolution;
			}
		}

		private ReaderWriterLockSlim ReaderWriterLock
		{
			get
			{
				return this.readerWriterLock;
			}
		}

		private bool Frozen
		{
			get
			{
				return this.frozen;
			}
			set
			{
				this.frozen = value;
			}
		}

		private TResolution Instance
		{
			get
			{
				return this.instance;
			}
			set
			{
				this.instance = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreCreate(bool creating)
		{
			if (creating)
			{
				if ((object)this.InnerDependencyResolution != null)
					this.InnerDependencyResolution.Create();
			}
		}

		protected override void CoreDispose(bool disposing)
		{
			if (disposing)
			{
				if ((object)this.InnerDependencyResolution != null)
					this.InnerDependencyResolution.Dispose();
			}
		}

		protected override TResolution CoreResolve(IDependencyManager dependencyManager, string selectorKey)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			// cop a reader lock
			this.ReaderWriterLock.EnterUpgradeableReadLock();

			try
			{
				if (this.Frozen)
					return this.Instance;

				// cop a writer lock
				this.ReaderWriterLock.EnterWriteLock();

				try
				{
					this.Instance = this.InnerDependencyResolution.Resolve(dependencyManager, selectorKey);
					return this.Instance;
				}
				finally
				{
					this.Frozen = true;
					this.ReaderWriterLock.ExitWriteLock();
				}
			}
			finally
			{
				this.ReaderWriterLock.ExitUpgradeableReadLock();
			}
		}

		#endregion
	}
}