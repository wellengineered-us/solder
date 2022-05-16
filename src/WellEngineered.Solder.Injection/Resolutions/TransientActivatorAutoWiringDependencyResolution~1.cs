/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Injection.Resolutions
{
	/// <summary>
	/// A dependency resolution implementation that uses Activator.CreateInstance(...)
	/// on the activation type each time a dependency resolution occurs and is the only
	/// implementation that allows for auto-wiring using the DependencyInjectionAttribute.
	/// </summary>
	public sealed partial class TransientActivatorAutoWiringDependencyResolution<TResolution> : DependencyResolution<TResolution>
	{
		#region Constructors/Destructors

		public TransientActivatorAutoWiringDependencyResolution()
			: base(DependencyLifetime.Transient)
		{
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

			return TransientActivatorAutoWiringDependencyResolution.AutoWireResolve<TResolution>(typeof(TResolution), dependencyManager, selectorKey);
		}

		#endregion
	}
}