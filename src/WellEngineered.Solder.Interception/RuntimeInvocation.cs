/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Reflection;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Interception
{
	public sealed partial class RuntimeInvocation
		: DualLifecycle, IRuntimeInvocation
	{
		#region Constructors/Destructors

		public RuntimeInvocation(object proxyInstance, Type targetType, MethodInfo targetMethod, object[] invocationArguments, object wrappedInstance)
		{
			if ((object)proxyInstance == null)
				throw new ArgumentNullException(nameof(proxyInstance));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)targetMethod == null)
				throw new ArgumentNullException(nameof(targetMethod));

			if ((object)invocationArguments == null)
				throw new ArgumentNullException(nameof(invocationArguments));

			//if ((object)wrappedInstance == null)
			//throw new ArgumentNullException(nameof(wrappedInstance));

			this.proxyInstance = proxyInstance;
			this.targetType = targetType;
			this.targetMethod = targetMethod;
			this.invocationArguments = invocationArguments;
			this.wrappedInstance = wrappedInstance;
		}

		#endregion

		#region Fields/Constants

		private readonly object[] invocationArguments;
		private readonly object proxyInstance;
		private readonly MethodInfo targetMethod;
		private readonly Type targetType;
		private readonly object wrappedInstance;
		private object invocationReturnValue;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the arguments passed to the invoked method, if appliable.
		/// </summary>
		public object[] InvocationArguments
		{
			get
			{
				return this.invocationArguments;
			}
		}

		/// <summary>
		/// Gets the proxy object instance.
		/// </summary>
		public object ProxyInstance
		{
			get
			{
				return this.proxyInstance;
			}
		}

		/// <summary>
		/// Gets the MethodInfo of the target method (or property underlying method).
		/// </summary>
		public MethodInfo TargetMethod
		{
			get
			{
				return this.targetMethod;
			}
		}

		/// <summary>
		/// Gets the run-time type of the target type (may differ from MethodInfo.DeclaringType).
		/// </summary>
		public Type TargetType
		{
			get
			{
				return this.targetType;
			}
		}

		/// <summary>
		/// Gets the wrapped object instance or null if there is none.
		/// </summary>
		public object WrappedInstance
		{
			get
			{
				return this.wrappedInstance;
			}
		}

		/// <summary>
		/// Gets or sets the return value of the invoked method, if appliable.
		/// </summary>
		public object InvocationReturnValue
		{
			get
			{
				return this.invocationReturnValue;
			}
			set
			{
				this.invocationReturnValue = value;
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

		#endregion
	}
}