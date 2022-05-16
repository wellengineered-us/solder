/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Component;
using WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Configuration;

namespace WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs
{
	public sealed class DogFoodComponent1 : EndToEndComponent1
	{
		#region Constructors/Destructors

		public DogFoodComponent1()
		{
		}

		#endregion

		#region Properties/Indexers/Events

		public override Type ConfigurationType
		{
			get
			{
				return typeof(UnknownEndToEndConfiguration<DogFoodSpecification>);
			}
		}

		#endregion
	}
}