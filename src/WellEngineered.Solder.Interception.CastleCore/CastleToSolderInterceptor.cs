/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using Castle.DynamicProxy;

namespace WellEngineered.Solder.Interception.CastleCore
{
	internal class CastleToSolderInterceptor : IInterceptor
	{
		#region Constructors/Destructors

		public CastleToSolderInterceptor(IRuntimeInterception[] runtimeInterceptionChain)
		{
			if ((object)runtimeInterceptionChain == null)
				throw new ArgumentNullException(nameof(runtimeInterceptionChain));

			this.runtimeInterceptionChain = runtimeInterceptionChain;
		}

		#endregion

		#region Fields/Constants

		private readonly IRuntimeInterception[] runtimeInterceptionChain;

		#endregion

		#region Properties/Indexers/Events

		private IRuntimeInterception[] RuntimeInterceptionChain
		{
			get
			{
				return this.runtimeInterceptionChain;
			}
		}

		#endregion

		#region Methods/Operators

		public void Intercept(IInvocation invocation)
		{
			RuntimeContext runtimeContext;
			IRuntimeInvocation runtimeInvocation;
			IRuntimeInterception runtimeInterception;

			if ((object)invocation == null)
				throw new ArgumentNullException(nameof(invocation));

			runtimeContext = new RuntimeContext(true);

			runtimeInvocation = new RuntimeInvocation(invocation.Proxy, /*invocation.TargetType*/ invocation.Method.DeclaringType, invocation.Method, invocation.Arguments, invocation.InvocationTarget);

			for (int index = 0; index < this.RuntimeInterceptionChain.Length && runtimeContext.ContinueInterception; index++)
			{
				runtimeContext.InterceptionCount = this.RuntimeInterceptionChain.Length + 1;
				runtimeContext.InterceptionIndex = index;

				runtimeInterception = this.RuntimeInterceptionChain[index];
				runtimeInterception.Invoke(runtimeInvocation, runtimeContext);
			}
		}

		#endregion
	}
}