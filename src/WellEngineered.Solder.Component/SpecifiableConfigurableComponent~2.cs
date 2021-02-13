/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Component
{
	public abstract partial class SpecifiableConfigurableComponent<TComponentConfiguration, TComponentSpecification>
		: ConfigurableComponent<TComponentConfiguration>, ISpecifiableConfigurableComponent<TComponentConfiguration, TComponentSpecification>
		where TComponentConfiguration : IUnknownComponentConfigurationObject<TComponentSpecification>
		where TComponentSpecification : IComponentConfigurationObject
	{
		#region Constructors/Destructors

		protected SpecifiableConfigurableComponent()
		{
		}

		#endregion

		#region Fields/Constants

		private TComponentSpecification specification;

		#endregion

		#region Properties/Indexers/Events

		public Type SpecificationType
		{
			get
			{
				return typeof(TComponentSpecification);
			}
		}

		public TComponentSpecification Specification
		{
			get
			{
				return this.specification;
			}
			set
			{
				this.specification = value;
			}
		}

		IComponentConfigurationObject ISpecifiable.Specification
		{
			get
			{
				return this.Specification;
			}
			set
			{
				this.Specification = (TComponentSpecification)value;
			}
		}

		#endregion

		#region Methods/Operators

		private void AssertValidSpecification()
		{
			if ((object)this.Specification == null)
				throw new InvalidOperationException(string.Format("Specification missing: '{0}'.", nameof(this.Specification)));
		}

		protected override void CoreCreate(bool creating)
		{
			//IUnknownComponentConfigurationObject untypedComponentConfiguration;
			IUnknownComponentConfigurationObject<TComponentSpecification> typedComponentConfiguration;

			if (this.IsCreated)
				return;

			if (creating)
			{
				base.CoreCreate(creating);

				//untypedComponentConfiguration = this.Configuration;
				typedComponentConfiguration = this.Configuration;
				//typedComponentConfiguration = new UnknownComponentConfigurationObject<TComponentSpecification>(untypedComponentConfiguration /*, this.SpecificationType*/);

				this.Specification = typedComponentConfiguration.ComponentSpecificConfiguration;

				this.AssertValidSpecification();
			}
		}

		protected override void CoreDispose(bool disposing)
		{
			if (this.IsDisposed)
				return;

			if (disposing)
			{
				this.Specification = default(TComponentSpecification);

				base.CoreDispose(disposing);
			}
		}

		#endregion
	}
}