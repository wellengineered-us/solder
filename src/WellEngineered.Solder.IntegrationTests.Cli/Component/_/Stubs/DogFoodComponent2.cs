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
	public sealed class DogFoodComponent2 : EndToEndComponent2
	{
		#region Constructors/Destructors

		public DogFoodComponent2()
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

		public override Type SpecificationType
		{
			get
			{
				return typeof(DogFoodSpecification);
			}
		}

		#endregion

		#region Methods/Operators

		protected override IUnknownSolderConfiguration<ISolderSpecification> CoreCreateTypedUnknownConfiguration(IUnknownSolderConfiguration unknownSolderConfiguration)
		{
			return new UnknownEndToEndConfiguration<DogFoodSpecification>(this.Configuration);
		}

		#endregion
	}
}