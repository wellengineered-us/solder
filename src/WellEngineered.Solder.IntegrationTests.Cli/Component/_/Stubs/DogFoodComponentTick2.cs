/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Component;
using WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Configuration;

namespace WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs
{
	public sealed class DogFoodComponentTick2
		: EndToEndComponent<UnknownEndToEndConfiguration<DogFoodSpecification>, DogFoodSpecification>
	{
		#region Constructors/Destructors

		public DogFoodComponentTick2()
		{
		}

		#endregion

		#region Methods/Operators

		protected override IUnknownSolderConfiguration<DogFoodSpecification> CoreCreateGenericTypedUnknownConfiguration(IUnknownSolderConfiguration unknownSolderConfiguration)
		{
			return new UnknownEndToEndConfiguration<DogFoodSpecification>(this.Configuration);
		}

		#endregion
	}
}