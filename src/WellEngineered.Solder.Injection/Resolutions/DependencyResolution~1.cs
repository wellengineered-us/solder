/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Injection.Resolutions
{
	public abstract partial class DependencyResolution<TResolution>
		: DependencyResolution,
			IDependencyResolution<TResolution>
	{
		#region Constructors/Destructors

		protected DependencyResolution(DependencyLifetime dependencyLifetime)
			: base(dependencyLifetime)
		{
		}

		#endregion

		#region Methods/Operators

		protected override object CoreResolve(IDependencyManager dependencyManager, Type resolutionType, string selectorKey)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			return this.CoreResolve(dependencyManager, selectorKey);
		}

		protected abstract TResolution CoreResolve(IDependencyManager dependencyManager, string selectorKey);

		public TResolution Resolve(IDependencyManager dependencyManager, string selectorKey)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			try
			{
				return this.CoreResolve(dependencyManager, selectorKey);
			}
			catch (Exception ex)
			{
				throw new DependencyException(string.Format("The dependency resolution failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}