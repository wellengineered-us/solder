/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using Castle.DynamicProxy;

namespace WellEngineered.Solder.Interception.CastleCore
{
	/// <summary>
	/// Represents a dynamic run-time interception.
	/// </summary>
	public abstract class CastleCoreRuntimeInterception : RuntimeInterception
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the RuntimeInterception class.
		/// </summary>
		protected CastleCoreRuntimeInterception()
		{
		}

		#endregion

		#region Methods/Operators

		public static TTarget CreateProxy<TTarget>(IRuntimeInterception[] runtimeInterceptionChain)
		{
			Type targetType;

			if ((object)runtimeInterceptionChain == null)
				throw new ArgumentNullException(nameof(runtimeInterceptionChain));

			targetType = typeof(TTarget);

			return (TTarget)CreateProxy(targetType, runtimeInterceptionChain);
		}

		public static object CreateProxy(Type targetType, IRuntimeInterception[] runtimeInterceptionChain)
		{
			Type proxyType;
			object proxyInstance;
			object[] activationArgs;
			IProxyBuilder proxyBuilder;
			IInterceptor interceptor;

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)runtimeInterceptionChain == null)
				throw new ArgumentNullException(nameof(runtimeInterceptionChain));

			proxyBuilder = new DefaultProxyBuilder();
			proxyType = proxyBuilder.CreateInterfaceProxyTypeWithoutTarget(targetType, new Type[] { }, ProxyGenerationOptions.Default);
			interceptor = new CastleToSolderInterceptor(runtimeInterceptionChain);
			activationArgs = new object[] { new IInterceptor[] { interceptor }, new object() };
			proxyInstance = Activator.CreateInstance(proxyType, activationArgs);
			return proxyInstance;
		}

		#endregion
	}
}