/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection.Resolutions
{
	/// <summary>
	/// A dependency resolution implementation that uses Activator.CreateInstance(...)
	/// on the activation type each time a dependency resolution occurs and is the only
	/// implementation that allows for auto-wiring using the DependencyInjectionAttribute.
	/// </summary>
	public sealed partial class TransientActivatorAutoWiringDependencyResolution : DependencyResolution
	{
		#region Methods/Operators

		internal static async ValueTask<TResolution> AutoWireResolveAsync<TResolution>(Type activatorType, IDependencyManager dependencyManager, string selectorKey, CancellationToken cancellationToken = default)
		{
			Type resolutionType;
			ConstructorInfo constructorInfo;
			ConstructorInfo[] constructorInfos;
			ParameterInfo[] parameterInfos;

			DependencyInjectionAttribute dependencyInjectionAttribute;
			ValueTask<TResolution> lazyConstructorInvokation = default;

			if ((object)activatorType == null)
				throw new ArgumentNullException(nameof(activatorType));

			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			resolutionType = typeof(TResolution);

			if (!resolutionType.IsAssignableFrom(activatorType))
				throw new DependencyException(string.Format("Resolution type '{1}' is not assignable from activator type '{0}'; selector key '{2}'.", activatorType.FullName, resolutionType.FullName, selectorKey));

			// get public, instance .ctors for activation type
			constructorInfos = activatorType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

			if ((object)constructorInfos != null)
			{
				for (int constructorIndex = 0; constructorIndex < constructorInfos.Length; constructorIndex++)
				{
					ValueTask<object>[] lazyConstructorArguments;

					constructorInfo = constructorInfos[constructorIndex];

					// on constructor
					dependencyInjectionAttribute = constructorInfo.GetOneAttribute<DependencyInjectionAttribute>();

					if ((object)dependencyInjectionAttribute == null)
						continue;

					if (dependencyInjectionAttribute.SelectorKey != selectorKey)
						continue;

					if (lazyConstructorInvokation != default)
						throw new DependencyException(string.Format("More than one constructor for activator type '{0}' specified the '{1}' with selector key '{2}'.", activatorType.FullName, nameof(DependencyInjectionAttribute), selectorKey));

					parameterInfos = constructorInfo.GetParameters();
					lazyConstructorArguments = new ValueTask<object>[parameterInfos.Length];

					for (int parameterIndex = 0; parameterIndex < parameterInfos.Length; parameterIndex++)
					{
						ParameterInfo parameterInfo;
						Type parameterType;
						ValueTask<object> lazyConstructorArgument;
						DependencyInjectionAttribute parameterDependencyInjectionAttribute;

						parameterInfo = parameterInfos[parameterIndex];
						parameterType = parameterInfo.ParameterType;

						// on parameter
						parameterDependencyInjectionAttribute = parameterInfo.GetOneAttribute<DependencyInjectionAttribute>();

						if ((object)parameterDependencyInjectionAttribute == null)
							throw new DependencyException(string.Format("A constructor for activator type '{0}' specifying the '{1}' with selector key '{2}' had at least one parameter missing the '{1}': index='{3}';name='{4}';type='{5}'.", activatorType.FullName, nameof(DependencyInjectionAttribute), selectorKey, parameterIndex, parameterInfo.Name, parameterInfo.ParameterType.FullName));

						lazyConstructorArgument = LazyConstructorArgumentAsync(cancellationToken);

						async ValueTask<Object> LazyConstructorArgumentAsync(CancellationToken cts = default)
						{
							// prevent modified closure bug
							IDependencyManager _dependencyManager = dependencyManager;
							Type _resolutionType = parameterType;
							DependencyInjectionAttribute _parameterDependencyInjectionAttribute = parameterDependencyInjectionAttribute;

							return await _dependencyManager.ResolveDependencyAsync(_resolutionType, _parameterDependencyInjectionAttribute.SelectorKey, true, cts);
						}

						lazyConstructorArguments[parameterIndex] = lazyConstructorArgument;
					}

					lazyConstructorInvokation = LazyConstructorAsync(cancellationToken);

					async ValueTask<TResolution> LazyConstructorAsync(CancellationToken cts = default)
					{
						// prevent modified closure bug
						Type _activatorType = activatorType;
						ValueTask<object>[] _lazyConstructorArguments = lazyConstructorArguments;
						TResolution value;
						object[] arguments;
						object argument;

						arguments = new object[_lazyConstructorArguments.Length];
						for (int i = 0; i < _lazyConstructorArguments.Length; i++)
						{
							argument = await _lazyConstructorArguments[i];
							arguments[i] = argument;
						}

						value = (TResolution)Activator.CreateInstance(_activatorType, arguments);
						return value;
					}
				}
			}

			if ((object)lazyConstructorInvokation == null)
				throw new DependencyException(string.Format("Cannot find a dependency injection constructor for activator type '{0}' with selector key '{1}'.", activatorType.FullName, selectorKey));

			return await lazyConstructorInvokation; // .Value will load a cascading chain of Lazy's...
		}

		protected override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		protected override ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		protected override async ValueTask<object> CoreResolveAsync(IDependencyManager dependencyManager, Type resolutionType, string selectorKey, CancellationToken cancellationToken = default)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			return await AutoWireResolveAsync<object>(this.ActivatorType, dependencyManager, selectorKey, cancellationToken);
		}

		#endregion
	}
}
#endif