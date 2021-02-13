/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Utilities;

namespace WellEngineered.Solder.Injection.Resolutions
{
	/// <summary>
	/// A dependency resolution implementation that uses Activator.CreateInstance(...)
	/// on the activation type each time a dependency resolution occurs and is the only
	/// implementation that allows for auto-wiring using the DependencyInjectionAttribute.
	/// </summary>
	public sealed class TransientActivatorAutoWiringDependencyResolution<TResolution> : DependencyResolution<TResolution>
	{
		#region Constructors/Destructors

		public TransientActivatorAutoWiringDependencyResolution(IReflectionFascade reflectionFascade)
			: base(DependencyLifetime.Transient)
		{
			if ((object)reflectionFascade == null)
				throw new ArgumentNullException(nameof(reflectionFascade));

			this.reflectionFascade = reflectionFascade;
		}

		#endregion

		#region Fields/Constants

		private readonly IReflectionFascade reflectionFascade;

		#endregion

		#region Properties/Indexers/Events

		private IReflectionFascade ReflectionFascade
		{
			get
			{
				return this.reflectionFascade;
			}
		}

		#endregion

		#region Methods/Operators

		public static TransientActivatorAutoWiringDependencyResolution<TResolution> From(IDependencyManager dependencyManager)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			return new TransientActivatorAutoWiringDependencyResolution<TResolution>(dependencyManager.ResolveDependency<IReflectionFascade>(string.Empty, false));
		}

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

			return TransientActivatorAutoWiringDependencyResolution.AutoWireResolve<TResolution>(this.ReflectionFascade, typeof(TResolution), dependencyManager, typeof(TResolution), selectorKey);
		}

		#endregion
	}
}