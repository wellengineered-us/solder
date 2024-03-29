/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;

using WellEngineered.Solder.Executive;
using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Primitives;
using WellEngineered.Solder.Utilities.AppSettings;

namespace WellEngineered.Solder.ExeApp.Cli
{
	public sealed partial class SolderExeApp
		: ExecutableApplication
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public SolderExeApp([DependencyInjection] IAppSettingsFacade appSettingsFacade,
			[DependencyInjection] AssemblyInformation assemblyInformation)
			: base(appSettingsFacade, assemblyInformation)
		{
		}

		#endregion

		#region Properties/Indexers/Events

		protected override TimeSpan ProcessExitDelayTimeSpan
		{
			get
			{
				return TimeSpan.Zero;
			}
		}

		#endregion

		#region Methods/Operators

		protected override IDictionary<string, ArgumentSpec> CoreGetArgumentMap()
		{
			return new Dictionary<string, ArgumentSpec>();
		}

		protected override int CoreRun(IDictionary<string, IList<object>> arguments)
		{
			Console.Out.WriteLine("{0}: enter, sleeping...", nameof(this.CoreRun));
			Thread.Sleep(TimeSpan.FromSeconds(5));
			Console.Out.WriteLine("{0}: leave.", nameof(this.CoreRun));

			return 0;
		}

		protected override void CoreSignal(Signal signal)
		{
			Console.Out.WriteLine(string.Format("HCF: signal={0}", signal));
		}

		#endregion
	}
}