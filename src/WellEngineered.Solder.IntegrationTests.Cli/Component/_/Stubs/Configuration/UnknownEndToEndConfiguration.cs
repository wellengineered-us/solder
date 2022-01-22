/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Solder.Component;

namespace WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Configuration
{
	public class UnknownEndToEndConfiguration
		: UnknownSolderConfiguration,
			IUnknownEndToEndConfiguration
	{
		#region Constructors/Destructors

		public UnknownEndToEndConfiguration()
		{
		}

		public UnknownEndToEndConfiguration(IDictionary<string, object> componentSpecificConfiguration, Type componentSpecificConfigurationType)
			: base(componentSpecificConfiguration, componentSpecificConfigurationType)
		{
		}

		#endregion
	}
}