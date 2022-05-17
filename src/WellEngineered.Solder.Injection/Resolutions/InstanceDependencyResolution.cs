/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Injection.Resolutions
{
	public sealed partial class InstanceDependencyResolution
		: DependencyResolution
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the InstanceDependencyResolution class.
		/// </summary>
		/// <param name="instance"> The instance to use for resolution. </param>
		public InstanceDependencyResolution(object instance)
			: base(DependencyLifetime.Instance)
		{
			this.instance = instance;
		}

		#endregion

		#region Fields/Constants

		private readonly object instance;

		#endregion

		#region Properties/Indexers/Events

		public object Instance
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

		protected override object CoreResolve(IDependencyManager dependencyManager, Type resolutionType, string selectorKey)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			return this.Instance;
		}

		#endregion
	}
}