/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Interception
{
	public partial class LoggingRuntimeInterception
		: RuntimeInterception
	{
		#region Constructors/Destructors

		public LoggingRuntimeInterception()
		{
		}

		#endregion

		#region Methods/Operators

		protected override void CoreAfterInvoke(bool proceedWithInvocation, IRuntimeInvocation runtimeInvocation, ref Exception thrownException)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("after invoke: {0}", runtimeInvocation.TargetMethod.ToString());
			Console.ForegroundColor = oldConsoleColor;
		}

		protected override void CoreBeforeInvoke(IRuntimeInvocation runtimeInvocation, out bool proceedWithInvocation)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("before invoke: {0}", runtimeInvocation.TargetMethod.ToString());
			Console.ForegroundColor = oldConsoleColor;
			proceedWithInvocation = true;
		}

		protected override void CoreMagicalSpellInvoke(IRuntimeInvocation runtimeInvocation)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("magic invoke: {0}", runtimeInvocation.TargetMethod.ToString());
			Console.ForegroundColor = oldConsoleColor;

			base.CoreMagicalSpellInvoke(runtimeInvocation);
		}

		#endregion
	}
}