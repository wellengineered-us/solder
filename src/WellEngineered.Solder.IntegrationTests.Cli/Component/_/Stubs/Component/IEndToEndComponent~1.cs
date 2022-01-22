/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Component;
using WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Configuration;

namespace WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Component
{
	public interface IEndToEndComponent<TEndToEndConfiguration>
		: IEndToEndComponent1,
			ISolderComponent<TEndToEndConfiguration>
		where TEndToEndConfiguration : class, IEndToEndConfiguration
	{
	}
}