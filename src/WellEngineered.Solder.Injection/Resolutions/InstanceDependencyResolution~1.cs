/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Injection.Resolutions
{
	/// <summary>
	/// A dependency resolution implementation that allows only a specific instance
	/// to be provided, cached, and reused; the specific instance is passed as a constructor parameter.
	/// </summary>
	public sealed partial class InstanceDependencyResolution<TResolution>
		: DependencyResolution<TResolution>
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the InstanceDependencyResolution`1 class.
		/// </summary>
		/// <param name="instance"> The instance to use for resolution. </param>
		public InstanceDependencyResolution(TResolution instance)
			: base(DependencyLifetime.Instance)
		{
			this.instance = instance;
		}

		#endregion

		#region Fields/Constants

		private readonly TResolution instance;

		#endregion

		#region Properties/Indexers/Events

		public TResolution Instance
		{
			get
			{
				return this.instance;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreCreate(bool creating)
		{
			// do nothing
		}

		protected override void CoreDispose(bool disposing)
		{
			// do nothing
		}

		protected override TResolution CoreResolve(IDependencyManager dependencyManager, string selectorKey)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			return this.Instance;
		}

		#endregion
	}
}