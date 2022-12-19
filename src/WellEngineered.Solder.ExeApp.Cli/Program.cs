/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Executive;
using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Injection.Resolutions;

namespace WellEngineered.Solder.ExeApp.Cli
{
	/// <summary>
	/// Entry point static class for the application.
	/// </summary>
	public static partial class Program
	{
		#region Methods/Operators

#if !ASYNC_ALL_THE_WAY_DOWN || !ASYNC_MAIN_ENTRY_POINT
		[STAThread]
		public static int Main(string[] args)
		{
			return ExecutableApplication.ResolveRun<SolderExeApp>(args);
		}
#endif

		[DependencyMagicMethod]
		public static void OnDependencyMagic(IDependencyManager dependencyManager)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			dependencyManager.AddResolution<SolderExeApp>(string.Empty, false, new SingletonWrapperDependencyResolution<SolderExeApp>(new TransientActivatorAutoWiringDependencyResolution<SolderExeApp>()));
		}

		#endregion
	}
}