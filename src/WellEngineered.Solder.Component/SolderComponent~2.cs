/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Component
{
	public abstract class SolderComponent<TSolderConfiguration, TSolderSpecification>
		: SolderComponent2,
			ISolderComponent<TSolderConfiguration, TSolderSpecification>
		where TSolderConfiguration : class, IUnknownSolderConfiguration // *** DO NOT USE ARITY 1: <TSpecification>
		where TSolderSpecification : class, ISolderSpecification, new()
	{
		#region Constructors/Destructors

		protected SolderComponent()
		{
		}

		#endregion

		#region Properties/Indexers/Events

		public sealed override Type ConfigurationType
		{
			get
			{
				return typeof(TSolderConfiguration);
			}
		}

		public sealed override Type SpecificationType
		{
			get
			{
				return typeof(TSolderSpecification);
			}
		}

		public new TSolderConfiguration Configuration
		{
			get
			{
				return (TSolderConfiguration)base.Configuration;
			}
			set
			{
				base.Configuration = value;
			}
		}

		public new TSolderSpecification Specification
		{
			get
			{
				return (TSolderSpecification)base.Specification;
			}
			set
			{
				base.Specification = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected abstract IUnknownSolderConfiguration<TSolderSpecification> CoreCreateGenericTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration);

		protected sealed override IUnknownSolderConfiguration<ISolderSpecification> CoreCreateTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration)
		{
			IUnknownSolderConfiguration<TSolderSpecification> typedUnknownSolderConfiguration;

			typedUnknownSolderConfiguration = this.CoreCreateGenericTypedUnknownConfiguration(untypedUnknownSolderConfiguration);

			return typedUnknownSolderConfiguration;
		}

		#endregion
	}
}