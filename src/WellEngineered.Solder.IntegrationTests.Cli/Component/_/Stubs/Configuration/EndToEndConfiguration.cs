/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Configuration
{
	public abstract class EndToEndConfiguration
		: SolderConfiguration,
			IEndToEndConfiguration
	{
		#region Constructors/Destructors

		protected EndToEndConfiguration()
		{
		}

		#endregion
	}
}