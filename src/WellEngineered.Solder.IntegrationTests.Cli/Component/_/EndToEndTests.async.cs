/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

using NUnit.Framework;

using WellEngineered.Solder.Component;

namespace WellEngineered.Solder.IntegrationTests.Cli.Component._
{
	[TestFixture]
	public partial class EndToEndTests
	{
		#region Methods/Operators

		[Test]
		public async ValueTask ShouldCreateTest2Async()
		{
			await using (IComponent component = new StubComponent())
			{
				await component.CreateAsync();

				//stubComponent.DisposeAsync();
			}

			await using (IConfigurableComponent component = new StubConfigurableComponent())
			{
				component.Configuration = new StubComponentConfiguration();
				await component.CreateAsync();

				//stubComponent.DisposeAsync();
			}

			await using (ISpecifiableConfigurableComponent component = new StubSpecifiableConfigurableComponent())
			{
				var that = new UnknownComponentConfigurationObject();

				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropA), "test");
				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropB), 100);
				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropC), true);
				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropD), 10.50);

				component.Configuration = new UnknownComponentConfigurationObject<StubComponentSpecification>(that);
				await component.CreateAsync();

				//Assert.AreSame(component.Configuration.ComponentSpecificConfiguration, component.Specification);

				dynamic dynSpecification = component.Specification;
				dynamic dynConfig = component.Configuration;
				dynamic dynComponentSpecificConfiguration = dynConfig.ComponentSpecificConfiguration;

				Assert.AreEqual("test", dynSpecification.PropA);
				Assert.AreEqual(100, dynSpecification.PropB);
				Assert.AreEqual(true, dynSpecification.PropC);
				Assert.AreEqual(10.50, dynSpecification.PropD);

				Assert.AreEqual("test", dynComponentSpecificConfiguration.PropA);
				Assert.AreEqual(100, dynComponentSpecificConfiguration.PropB);
				Assert.AreEqual(true, dynComponentSpecificConfiguration.PropC);
				Assert.AreEqual(10.50, dynComponentSpecificConfiguration.PropD);

				//stubComponent.DisposeAsync();
			}
		}

		[Test]
		public async ValueTask ShouldCreateTestAsync()
		{
			await using (IComponent component = new StubComponent())
			{
				await component.CreateAsync();

				//stubComponent.DisposeAsync();
			}

			await using (IConfigurableComponent<StubComponentConfiguration> component = new StubConfigurableComponent())
			{
				component.Configuration = new StubComponentConfiguration();
				await component.CreateAsync();

				//stubComponent.DisposeAsync();
			}

			await using (ISpecifiableConfigurableComponent<UnknownComponentConfigurationObject<StubComponentSpecification>, StubComponentSpecification> component = new StubSpecifiableConfigurableComponent())
			{
				var that = new UnknownComponentConfigurationObject();

				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropA), "test");
				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropB), 100);
				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropC), true);
				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropD), 10.50);

				component.Configuration = new UnknownComponentConfigurationObject<StubComponentSpecification>(that);
				await component.CreateAsync();

				//Assert.AreSame(component.Configuration.ComponentSpecificConfiguration, component.Specification);

				Assert.AreEqual("test", component.Specification.PropA);
				Assert.AreEqual(100, component.Specification.PropB);
				Assert.AreEqual(true, component.Specification.PropC);
				Assert.AreEqual(10.50, component.Specification.PropD);

				Assert.AreEqual("test", component.Configuration.ComponentSpecificConfiguration.PropA);
				Assert.AreEqual(100, component.Configuration.ComponentSpecificConfiguration.PropB);
				Assert.AreEqual(true, component.Configuration.ComponentSpecificConfiguration.PropC);
				Assert.AreEqual(10.50, component.Configuration.ComponentSpecificConfiguration.PropD);

				//stubComponent.DisposeAsync();
			}
		}

		#endregion
	}
}