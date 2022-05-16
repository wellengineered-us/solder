/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Interception;

namespace WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure
{
	public class MockCloneable : IMockCloneable
	{
		#region Constructors/Destructors

		public MockCloneable()
		{
		}

		#endregion

		#region Methods/Operators

		public object Clone()
		{
			return null;
		}

		#endregion
	}

	public class MockRuntimeInterception : RuntimeInterception
	{
		#region Constructors/Destructors

		public MockRuntimeInterception()
		{
		}

		#endregion

		#region Fields/Constants

		private string lastOperationName;

		#endregion

		#region Properties/Indexers/Events

		public string LastOperationName
		{
			get
			{
				return this.lastOperationName;
			}
			private set
			{
				this.lastOperationName = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreInvoke(IRuntimeInvocation runtimeInvocation, IRuntimeContext runtimeContext)
		{
			this.LastOperationName = string.Format("{0}::{1}", (object)runtimeInvocation.TargetType == null ? "<null>" : runtimeInvocation.TargetType.Name, (object)runtimeInvocation.TargetMethod == null ? "<null>" : runtimeInvocation.TargetMethod.Name);

			if ((object)runtimeInvocation.TargetMethod != null)
			{
				if (runtimeInvocation.TargetMethod.DeclaringType == typeof(object) ||
					runtimeInvocation.TargetMethod.DeclaringType == typeof(IDisposable))
					runtimeInvocation.InvocationReturnValue = runtimeInvocation.TargetMethod.Invoke(this, runtimeInvocation.InvocationArguments);
				else if (runtimeInvocation.TargetMethod.DeclaringType == typeof(IMockCloneable))
					runtimeInvocation.InvocationReturnValue = runtimeInvocation.TargetMethod.Invoke(runtimeInvocation.ProxyInstance, runtimeInvocation.InvocationArguments);

				return;
			}

			throw new InvalidOperationException(string.Format("Method '{0}' not supported on '{1}'.", (object)runtimeInvocation.TargetMethod == null ? "<null>" : runtimeInvocation.TargetMethod.Name, runtimeInvocation.TargetType.FullName));
		}

		#endregion
	}
}