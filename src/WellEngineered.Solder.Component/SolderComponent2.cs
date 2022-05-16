/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Component
{
	public abstract partial class SolderComponent2
		: SolderComponent1,
			ISolderComponent2

	{
		#region Constructors/Destructors

		protected SolderComponent2()
		{
		}

		#endregion

		#region Fields/Constants

		private ISolderSpecification specification;

		#endregion

		#region Properties/Indexers/Events

		public abstract Type SpecificationType
		{
			get;
		}

		public new IUnknownSolderConfiguration Configuration
		{
			get
			{
				return (IUnknownSolderConfiguration)base.Configuration;
			}
			set
			{
				base.Configuration = value;
			}
		}

		public ISolderSpecification Specification
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

		#endregion

		#region Methods/Operators

		private void AssertValidSpecification()
		{
			if ((object)this.Specification == null)
				throw new InvalidOperationException(string.Format("Specification missing: '{0}'.", nameof(this.Specification)));
		}

		protected override void CoreCreate(bool creating)
		{
			IUnknownSolderConfiguration untypedUnknownSolderConfiguration;
			IUnknownSolderConfiguration<ISolderSpecification> typedUnknownSolderConfiguration;

			if (creating)
			{
				base.CoreCreate(creating);

				untypedUnknownSolderConfiguration = this.Configuration;

				typedUnknownSolderConfiguration = this.CoreCreateTypedUnknownConfiguration(untypedUnknownSolderConfiguration);

				this.Specification = typedUnknownSolderConfiguration.Specification;

				this.AssertValidSpecification();
			}
		}

		protected abstract IUnknownSolderConfiguration<ISolderSpecification> CoreCreateTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration);

		protected override void CoreDispose(bool disposing)
		{
			if (disposing)
			{
				this.Specification = null;

				base.CoreDispose(disposing);
			}
		}

		#endregion
	}
}