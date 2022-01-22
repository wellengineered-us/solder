/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Injection.Resolutions;

namespace WellEngineered.Solder.Injection
{
	public static class InjectionExtensions
	{
		#region Methods/Operators

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