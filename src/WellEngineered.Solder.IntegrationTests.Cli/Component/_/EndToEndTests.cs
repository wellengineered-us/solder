/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NUnit.Framework;

using WellEngineered.Solder.Component;
using WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs;
using WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Configuration;

namespace WellEngineered.Solder.IntegrationTests.Cli.Component._
{
	[TestFixture]
	public partial class EndToEndTests
	{
		#region Constructors/Destructors

		public EndToEndTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			using (ISolderComponent0 solderComponent = new DogFoodComponent0())
			{
				solderComponent.Create();
			}

			using (ISolderComponent1 solderComponent = new DogFoodComponent1())
			{
				var configuration = new DogFoodConfiguration();

				solderComponent.Configuration = configuration;
				solderComponent.Create();

				configuration = solderComponent.Configuration as DogFoodConfiguration;

				Assert.IsNotNull(configuration);
			}

			using (ISolderComponent2 solderComponent = new DogFoodComponent2())
			{
				var that = new UnknownEndToEndConfiguration();

				that.Specification.Add(nameof(DogFoodSpecification.PropA), "test");
				that.Specification.Add(nameof(DogFoodSpecification.PropB), 100);
				that.Specification.Add(nameof(DogFoodSpecification.PropC), true);
				that.Specification.Add(nameof(DogFoodSpecification.PropD), 10.50);

				var configuration = new UnknownEndToEndConfiguration<DogFoodSpecification>(that);
				solderComponent.Configuration = configuration;
				solderComponent.Create();

				configuration = solderComponent.Configuration as UnknownEndToEndConfiguration<DogFoodSpecification>;

				Assert.IsNotNull(configuration);

				Assert.AreEqual("test", configuration.Specification.PropA);
				Assert.AreEqual(100, configuration.Specification.PropB);
				Assert.AreEqual(true, configuration.Specification.PropC);
				Assert.AreEqual(10.50, configuration.Specification.PropD);

				var specification = solderComponent.Specification as DogFoodSpecification;

				Assert.IsNotNull(specification);

				Assert.AreEqual("test", specification.PropA);
				Assert.AreEqual(100, specification.PropB);
				Assert.AreEqual(true, specification.PropC);
				Assert.AreEqual(10.50, specification.PropD);
			}

			using (ISolderComponent2 solderComponent = new DogFoodComponent2())
			{
				var that = new UnknownEndToEndConfiguration();

				that.Specification.Add(nameof(DogFoodSpecification.PropA), "test");
				that.Specification.Add(nameof(DogFoodSpecification.PropB), 100);
				that.Specification.Add(nameof(DogFoodSpecification.PropC), true);
				that.Specification.Add(nameof(DogFoodSpecification.PropD), 10.50);

				var configuration = that;
				solderComponent.Configuration = configuration;
				solderComponent.Create();

				dynamic dynSpecification = solderComponent.Specification;

				Assert.IsNotNull(dynSpecification);

				Assert.AreEqual("test", dynSpecification.PropA);
				Assert.AreEqual(100, dynSpecification.PropB);
				Assert.AreEqual(true, dynSpecification.PropC);
				Assert.AreEqual(10.50, dynSpecification.PropD);

				var specification = solderComponent.Specification as DogFoodSpecification;

				Assert.IsNotNull(specification);

				Assert.AreEqual("test", specification.PropA);
				Assert.AreEqual(100, specification.PropB);
				Assert.AreEqual(true, specification.PropC);
				Assert.AreEqual(10.50, specification.PropD);
			}

			using (ISolderComponent<DogFoodConfiguration> solderComponent = new DogFoodComponentTick1())
			{
				var configuration = new DogFoodConfiguration();

				configuration.Prop1 = "test";
				configuration.Prop2 = 100;
				configuration.Prop3 = true;
				configuration.Prop4 = 10.50;

				solderComponent.Configuration = configuration;
				solderComponent.Create();

				configuration = solderComponent.Configuration as DogFoodConfiguration;

				Assert.IsNotNull(configuration);

				Assert.AreEqual("test", configuration.Prop1);
				Assert.AreEqual(100, configuration.Prop2);
				Assert.AreEqual(true, configuration.Prop3);
				Assert.AreEqual(10.50, configuration.Prop4);
			}

			using (ISolderComponent<UnknownEndToEndConfiguration<DogFoodSpecification>, DogFoodSpecification> solderComponent = new DogFoodComponentTick2())
			{
				var that = new UnknownEndToEndConfiguration();

				that.Specification.Add(nameof(DogFoodSpecification.PropA), "test");
				that.Specification.Add(nameof(DogFoodSpecification.PropB), 100);
				that.Specification.Add(nameof(DogFoodSpecification.PropC), true);
				that.Specification.Add(nameof(DogFoodSpecification.PropD), 10.50);

				var configuration = new UnknownEndToEndConfiguration<DogFoodSpecification>(that);
				solderComponent.Configuration = configuration;
				solderComponent.Create();

				configuration = solderComponent.Configuration as UnknownEndToEndConfiguration<DogFoodSpecification>;

				Assert.IsNotNull(configuration);

				var specification = solderComponent.Specification as DogFoodSpecification;

				Assert.IsNotNull(specification);

				Assert.AreEqual("test", specification.PropA);
				Assert.AreEqual(100, specification.PropB);
				Assert.AreEqual(true, specification.PropC);
				Assert.AreEqual(10.50, specification.PropD);
			}
		}

		#endregion
	}
}