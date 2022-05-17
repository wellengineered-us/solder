/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Interception
{
	public partial class LoggingRuntimeInterception
		: RuntimeInterception
	{
		#region Methods/Operators

		protected override async ValueTask<Exception> CoreAfterInvokeAsync(bool proceedWithInvocation, IRuntimeInvocation runtimeInvocation, Exception thrownException, CancellationToken cancellationToken = default)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Yellow;
			await Console.Out.WriteLineAsync(string.Format("after invoke: {0}", runtimeInvocation.TargetMethod.ToString()));
			Console.ForegroundColor = oldConsoleColor;

			return thrownException;
		}

		protected override async ValueTask<bool> CoreBeforeInvokeAsync(IRuntimeInvocation runtimeInvocation, CancellationToken cancellationToken = default)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Blue;
			await Console.Out.WriteLineAsync(string.Format("before invoke: {0}", runtimeInvocation.TargetMethod.ToString()));
			Console.ForegroundColor = oldConsoleColor;
			return true;
		}

		protected override async ValueTask CoreMagicalSpellInvokeAsync(IRuntimeInvocation runtimeInvocation, CancellationToken cancellationToken = default)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Green;
			await Console.Out.WriteLineAsync(string.Format("magic invoke: {0}", runtimeInvocation.TargetMethod.ToString()));
			Console.ForegroundColor = oldConsoleColor;

			await base.CoreMagicalSpellInvokeAsync(runtimeInvocation, cancellationToken);
		}

		#endregion
	}
}
#endif