/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Interception
{
	/// <summary>
	/// Represents a dynamic run-time interception.
	/// </summary>
	public abstract partial class RuntimeInterception
		: DualLifecycle, IRuntimeInterception
	{
		#region Methods/Operators

		protected virtual async ValueTask<Exception> CoreAfterInvokeAsync(bool proceedWithInvocation, IRuntimeInvocation runtimeInvocation, Exception thrownException, CancellationToken cancellationToken = default)
		{
			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeInvocation));

			await Task.CompletedTask;
			return thrownException;
		}

		protected async virtual ValueTask<bool> CoreBeforeInvokeAsync(IRuntimeInvocation runtimeInvocation, CancellationToken cancellationToken = default)
		{
			await Task.CompletedTask;
			return true;
		}

		protected sealed override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		protected sealed override ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		protected virtual async ValueTask CoreInvokeAsync(IRuntimeInvocation runtimeInvocation, IRuntimeContext runtimeContext, CancellationToken cancellationToken)
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

			proceedWithInvocation = await this.CoreBeforeInvokeAsync(runtimeInvocation, cancellationToken);

			if (proceedWithInvocation)
				thrownException = await this.CoreProceedInvokeAsync(runtimeInvocation, cancellationToken);

			thrownException = await this.CoreAfterInvokeAsync(proceedWithInvocation, runtimeInvocation, thrownException, cancellationToken);

			if ((object)thrownException != null)
			{
				await runtimeContext.AbortInterceptionChainAsync(cancellationToken);
				throw thrownException;
			}
		}

		protected async virtual ValueTask CoreMagicalSpellInvokeAsync(IRuntimeInvocation runtimeInvocation, CancellationToken cancellationToken = default)
		{
			if ((object)runtimeInvocation.WrappedInstance != null)
				runtimeInvocation.InvocationReturnValue = runtimeInvocation.TargetMethod.Invoke(runtimeInvocation.WrappedInstance, runtimeInvocation.InvocationArguments);

			await Task.CompletedTask;
		}

		private async ValueTask<Exception> CoreProceedInvokeAsync(IRuntimeInvocation runtimeInvocation, CancellationToken cancellationToken = default)
		{
			Exception thrownException;

			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeInvocation));

			try
			{
				thrownException = null;

				await this.CoreMagicalSpellInvokeAsync(runtimeInvocation, cancellationToken);
			}
			catch (Exception ex)
			{
				thrownException = ex;
			}

			return thrownException;
		}

		public async ValueTask InvokeAsync(IRuntimeInvocation runtimeInvocation, IRuntimeContext runtimeContext, CancellationToken cancellationToken = default)
		{
			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeInvocation));

			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeContext));

			try
			{
				await this.CoreInvokeAsync(runtimeInvocation, runtimeContext, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new InterceptionException(string.Format("The runtime invocation failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}
#endif