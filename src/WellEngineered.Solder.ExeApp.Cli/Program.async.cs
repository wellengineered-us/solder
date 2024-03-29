/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

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

#if ASYNC_MAIN_ENTRY_POINT
		[STAThread]
		public static async Task<int> Main(string[] args)
		{
			return await ExecutableApplication.ResolveRunAsync<SolderExeApp>(args);
		}
#endif
		[AsyncDependencyMagicMethod]
		public static async ValueTask OnDependencyMagicAsync(IDependencyManager dependencyManager, CancellationToken cancellationToken = default)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			await dependencyManager.AddResolutionAsync<SolderExeApp>(string.Empty, false, new SingletonWrapperDependencyResolution<SolderExeApp>(new TransientActivatorAutoWiringDependencyResolution<SolderExeApp>()), cancellationToken);
		}

		#endregion
	}
}
#endif