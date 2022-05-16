/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Configuration
{
	public interface IUnknownEndToEndConfiguration<out TEndToEndSpecification>
		: IUnknownSolderConfiguration<TEndToEndSpecification>,
			IEndToEndConfiguration
		where TEndToEndSpecification : class, IEndToEndSpecification, new()
	{
	}
}