/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Interception
{
	/// <summary>
	/// Represents a dynamic run-time interception.
	/// </summary>
	public abstract partial class RuntimeInterception
		: DualLifecycle, IRuntimeInterception
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the RuntimeInterception class.
		/// </summary>
		protected RuntimeInterception()
		{
		}

		#endregion

		#region Methods/Operators

		protected virtual void CoreAfterInvoke(bool proceedWithInvocation, IRuntimeInvocation runtimeInvocation, ref Exception thrownException)
		{
			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeInvocation));
		}

		protected virtual void CoreBeforeInvoke(IRuntimeInvocation runtimeInvocation, out bool proceedWithInvocation)
		{
			proceedWithInvocation = true;
		}

		protected override void CoreCreate(bool creating)
		{
			// do nothing
		}

		protected override void CoreDispose(bool disposing)
		{
			// do nothing
		}

		protected virtual void CoreInvoke(IRuntimeInvocation runtimeInvocation, IRuntimeContext runtimeContext)
		{
			Exception thrownException = null;
			bool proceedWithInvocation;

			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeInvocation));

			if ((object)runtimeContext == null)
				throw new ArgumentNullException(nameof(runtimeContext));

			if (!((object)runtimeInvocation.TargetMethod != null &&
				runtimeInvocation.TargetMethod.DeclaringType == typeof(IDisposable)) &&
				this.IsDisposed) // always forward dispose invocations
				throw new ObjectDisposedException(typeof(RuntimeInterception).FullName);

			this.CoreBeforeInvoke(runtimeInvocation, out proceedWithInvocation);

			if (proceedWithInvocation)
				this.CoreProceedInvoke(runtimeInvocation, out thrownException);

			this.CoreAfterInvoke(proceedWithInvocation, runtimeInvocation, ref thrownException);

			if ((object)thrownException != null)
			{
				runtimeContext.AbortInterceptionChain();
				throw thrownException;
			}
		}

		protected virtual void CoreMagicalSpellInvoke(IRuntimeInvocation runtimeInvocation)
		{
			if ((object)runtimeInvocation.WrappedInstance != null)
				runtimeInvocation.InvocationReturnValue = runtimeInvocation.TargetMethod.Invoke(runtimeInvocation.WrappedInstance, runtimeInvocation.InvocationArguments);
		}

		protected virtual void CoreProceedInvoke(IRuntimeInvocation runtimeInvocation, out Exception thrownException)
		{
			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeInvocation));

			try
			{
				thrownException = null;

				this.CoreMagicalSpellInvoke(runtimeInvocation);
			}
			catch (Exception ex)
			{
				thrownException = ex;
			}
		}

		/// <summary>
		/// Represents a discrete run-time invocation.
		/// </summary>
		/// <param name="runtimeInvocation"> </param>
		/// <param name="runtimeContext"> </param>
		public void Invoke(IRuntimeInvocation runtimeInvocation, IRuntimeContext runtimeContext)
		{
			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeInvocation));

			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeContext));

			try
			{
				this.CoreInvoke(runtimeInvocation, runtimeContext);
			}
			catch (Exception ex)
			{
				throw new InterceptionException(string.Format("The runtime invocation failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}