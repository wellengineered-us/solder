/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Executive;

namespace WellEngineered.Solder.ExeApp.Cli
{
	public sealed partial class SolderExeApp
		: ExecutableApplication
	{
		#region Methods/Operators
		
		protected override async ValueTask CoreSignalAsync(Signal signal, CancellationToken cancellationToken = default)
		{
			await Console.Out.WriteLineAsync(string.Format("HCF*: signal={0}", signal));
		}
		

		protected override async ValueTask<int> CoreRunAsync(IDictionary<string, IList<object>> arguments, CancellationToken cancellationToken = default)
		{
			await Console.Out.WriteLineAsync(string.Format("{0}: enter, sleeping...", nameof(CoreRunAsync)));
			await Task.Delay(TimeSpan.FromSeconds(5));
			await Console.Out.WriteLineAsync(string.Format("{0}: leave.", nameof(CoreRunAsync)));
			
			return 0;
		}

		#endregion
	}
}
#endif