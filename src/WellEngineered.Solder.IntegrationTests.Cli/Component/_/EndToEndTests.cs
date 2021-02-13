/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using NUnit.Framework;

using WellEngineered.Solder.Component;
using WellEngineered.Solder.Primitives;

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
			using (IComponent component = new StubComponent())
			{
				component.Create();

				//stubComponent.Dispose();
			}

			using (IConfigurableComponent<StubComponentConfiguration> component = new StubConfigurableComponent())
			{
				component.Configuration = new StubComponentConfiguration();
				component.Create();

				//stubComponent.Dispose();
			}

			using (ISpecifiableConfigurableComponent<UnknownComponentConfigurationObject<StubComponentSpecification>, StubComponentSpecification> component = new StubSpecifiableConfigurableComponent())
			{
				var that = new UnknownComponentConfigurationObject();

				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropA), "test");
				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropB), 100);
				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropC), true);
				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropD), 10.50);

				component.Configuration = new UnknownComponentConfigurationObject<StubComponentSpecification>(that);
				component.Create();

				//Assert.AreSame(component.Configuration.ComponentSpecificConfiguration, component.Specification);

				Assert.AreEqual("test", component.Specification.PropA);
				Assert.AreEqual(100, component.Specification.PropB);
				Assert.AreEqual(true, component.Specification.PropC);
				Assert.AreEqual(10.50, component.Specification.PropD);

				Assert.AreEqual("test", component.Configuration.ComponentSpecificConfiguration.PropA);
				Assert.AreEqual(100, component.Configuration.ComponentSpecificConfiguration.PropB);
				Assert.AreEqual(true, component.Configuration.ComponentSpecificConfiguration.PropC);
				Assert.AreEqual(10.50, component.Configuration.ComponentSpecificConfiguration.PropD);

				//stubComponent.Dispose();
			}
		}

		[Test]
		public void ShouldCreateTest2()
		{
			using (IComponent component = new StubComponent())
			{
				component.Create();

				//stubComponent.Dispose();
			}

			using (IConfigurableComponent component = new StubConfigurableComponent())
			{
				component.Configuration = new StubComponentConfiguration();
				component.Create();

				//stubComponent.Dispose();
			}

			using (ISpecifiableConfigurableComponent component = new StubSpecifiableConfigurableComponent())
			{
				var that = new UnknownComponentConfigurationObject();

				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropA), "test");
				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropB), 100);
				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropC), true);
				that.ComponentSpecificConfiguration.Add(nameof(StubComponentSpecification.PropD), 10.50);

				component.Configuration = new UnknownComponentConfigurationObject<StubComponentSpecification>(that);
				component.Create();

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

				//stubComponent.Dispose();
			}
		}

		#endregion
	}

	public class StubComponent : Solder.Component.Component
	{
	}

	public class StubConfigurableComponent : ConfigurableComponent<StubComponentConfiguration>
	{
	}

	public class StubSpecifiableConfigurableComponent : SpecifiableConfigurableComponent<UnknownComponentConfigurationObject<StubComponentSpecification>, StubComponentSpecification>
	{
	}

	public class StubComponentConfiguration : ComponentConfigurationObject
	{
		#region Methods/Operators

		protected override IEnumerable<IMessage> CoreValidate(object context)
		{
			yield break;
		}

		protected override IAsyncEnumerable<IMessage> CoreValidateAsync(object context)
		{
			return null;
		}

		#endregion
	}

	public class StubComponentSpecification : ComponentConfigurationObject
	{
		#region Constructors/Destructors

		public StubComponentSpecification()
		{
		}

		#endregion

		#region Properties/Indexers/Events

		public string PropA
		{
			get;
			set;
		}

		public int? PropB
		{
			get;
			set;
		}

		public bool? PropC
		{
			get;
			set;
		}

		public double? PropD
		{
			get;
			set;
		}

		#endregion

		#region Methods/Operators

		protected override IEnumerable<IMessage> CoreValidate(object context)
		{
			yield break;
		}

		protected override IAsyncEnumerable<IMessage> CoreValidateAsync(object context)
		{
			return null;
		}

		#endregion
	}
}