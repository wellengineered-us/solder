/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Component;
using WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Configuration;

namespace WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Component
{
	public abstract class EndToEndComponent<TEndToEndConfiguration>
		: SolderComponent<TEndToEndConfiguration>,
			IEndToEndComponent<TEndToEndConfiguration>
		where TEndToEndConfiguration : class, IEndToEndConfiguration
	{
	}
}