/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Component
{
	public abstract partial class SpecifiableConfigurableComponent<TComponentConfiguration, TComponentSpecification>
		: ConfigurableComponent<TComponentConfiguration>, ISpecifiableConfigurableComponent<TComponentConfiguration, TComponentSpecification>
		where TComponentConfiguration : IUnknownComponentConfigurationObject<TComponentSpecification>
		where TComponentSpecification : IComponentConfigurationObject
	{
		#region Methods/Operators

		protected async override ValueTask CoreCreateAsync(bool creating)
		{
			//IUnknownComponentConfigurationObject untypedComponentConfiguration;
			IUnknownComponentConfigurationObject<TComponentSpecification> typedComponentConfiguration;

			if (this.IsCreated)
				return;

			if (creating)
			{
				await base.CoreCreateAsync(creating);

				//untypedComponentConfiguration = this.Configuration;
				typedComponentConfiguration = this.Configuration;
				//typedComponentConfiguration = new UnknownComponentConfigurationObject<TComponentSpecification>(untypedComponentConfiguration /*, this.SpecificationType*/);

				this.Specification = typedComponentConfiguration.ComponentSpecificConfiguration;

				this.AssertValidSpecification();
			}
		}

		protected async override ValueTask CoreDisposeAsync(bool disposing)
		{
			if (this.IsDisposed)
				return;

			if (disposing)
			{
				this.Configuration = default(TComponentConfiguration);

				await base.CoreDisposeAsync(disposing);
			}
		}

		#endregion
	}
}