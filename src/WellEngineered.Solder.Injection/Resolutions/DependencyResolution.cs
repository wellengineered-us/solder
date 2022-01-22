/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection.Resolutions
{
	public abstract partial class DependencyResolution
		: DualLifecycle,
			IDependencyResolution
	{
		#region Constructors/Destructors

		protected DependencyResolution(DependencyLifetime dependencyLifetime)
		{
			this.dependencyLifetime = dependencyLifetime;
		}

		#endregion

		#region Fields/Constants

		private readonly DependencyLifetime dependencyLifetime;

		#endregion

		#region Properties/Indexers/Events

		public DependencyLifetime DependencyLifetime
		{
			get
			{
				return this.dependencyLifetime;
			}
		}

		#endregion

		#region Methods/Operators

		protected abstract object CoreResolve(IDependencyManager dependencyManager, Type resolutionType, string selectorKey);

		public object Resolve(IDependencyManager dependencyManager, Type resolutionType, string selectorKey)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			try
			{
				return this.CoreResolve(dependencyManager, resolutionType, selectorKey);
			}
			catch (Exception ex)
			{
				throw new DependencyException(string.Format("The dependency resolution failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}