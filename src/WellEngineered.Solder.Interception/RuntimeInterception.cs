/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Interception
{
	/// <summary>
	/// Represents a dynamic run-time interception.
	/// </summary>
	public abstract class RuntimeInterception : Lifecycle, IRuntimeInterception
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

		protected override void CoreCreate(bool creating)
		{
			// do nothing
		}

		protected override void CoreDispose(bool disposing)
		{
			// do nothing
		}

		protected sealed override ValueTask CoreCreateAsync(bool creating)
		{
			throw new NotSupportedException();
		}

		protected sealed override ValueTask CoreDisposeAsync(bool disposing)
		{
			throw new NotSupportedException();
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

			this.OnInvoke(runtimeInvocation, runtimeContext);
		}

		protected virtual void OnAfterInvoke(bool proceedWithInvocation, IRuntimeInvocation runtimeInvocation, ref Exception thrownException)
		{
			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeInvocation));
		}

		protected virtual void OnBeforeInvoke(IRuntimeInvocation runtimeInvocation, out bool proceedWithInvocation)
		{
			proceedWithInvocation = true;
		}

		protected virtual void OnInvoke(IRuntimeInvocation runtimeInvocation, IRuntimeContext runtimeContext)
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

			this.OnBeforeInvoke(runtimeInvocation, out proceedWithInvocation);

			if (proceedWithInvocation)
				this.OnProceedInvoke(runtimeInvocation, out thrownException);

			this.OnAfterInvoke(proceedWithInvocation, runtimeInvocation, ref thrownException);

			if ((object)thrownException != null)
			{
				runtimeContext.AbortInterceptionChain();
				throw thrownException;
			}
		}

		protected virtual void OnMagicalSpellInvoke(IRuntimeInvocation runtimeInvocation)
		{
			if ((object)runtimeInvocation.WrappedInstance != null)
				runtimeInvocation.InvocationReturnValue = runtimeInvocation.TargetMethod.Invoke(runtimeInvocation.WrappedInstance, runtimeInvocation.InvocationArguments);
		}

		protected virtual void OnProceedInvoke(IRuntimeInvocation runtimeInvocation, out Exception thrownException)
		{
			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeInvocation));

			try
			{
				thrownException = null;

				this.OnMagicalSpellInvoke(runtimeInvocation);
			}
			catch (Exception ex)
			{
				thrownException = ex;
			}
		}

		#endregion
	}
}