/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Injection.Resolutions;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	public static class InjectionExtensions
	{
		#region Methods/Operators

		public static TTarget GetObjectAssignableToTargetType<TTarget>(this Type instantiateType, IDependencyManager dependencyManager, bool autoWire)
		{
			TTarget obj;

			if ((object)instantiateType == null)
				throw new ArgumentNullException(nameof(instantiateType));

			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if (autoWire)
				obj = instantiateType.ResolveAutoWireAssignableToTargetType<TTarget>(dependencyManager);
			else
				obj = instantiateType.CreateInstanceAssignableToTargetType<TTarget>();

			if ((object)obj == null)
				throw new SolderException(string.Format("Failed to create object of type: '{0}', auto-wire: {1}.", instantiateType.FullName, autoWire));

			return obj;
		}

		public static TTarget ResolveAutoWireAssignableToTargetType<TTarget>(this Type instantiateType, IDependencyManager dependencyManager, string selectorKey = null, bool throwOnError = true)
		{
			Type targetType;
			object obj;

			if ((object)instantiateType == null)
				throw new ArgumentNullException(nameof(instantiateType));

			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			targetType = typeof(TTarget);

			// TTarget obj = new [instantiateType]() ???
			if (!targetType.IsAssignableFrom(instantiateType))
			{
				if (throwOnError)
					throw new InvalidOperationException(string.Format("Target type '{0} is not assignable from instantiate type '{1}.", targetType, instantiateType));
				else
					return default;
			}

			obj = TransientActivatorAutoWiringDependencyResolution.AutoWireResolve<TTarget>(instantiateType, dependencyManager, selectorKey ?? string.Empty);

			return (TTarget)obj;
		}

		#endregion
	}
}