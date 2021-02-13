/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Component
{
	public abstract partial class ConfigurableComponent<TComponentConfiguration>
		: Component, IConfigurableComponent<TComponentConfiguration>
		where TComponentConfiguration : IComponentConfigurationObject
	{
		#region Constructors/Destructors

		protected ConfigurableComponent()
		{
		}

		#endregion

		#region Fields/Constants

		private TComponentConfiguration configuration;

		#endregion

		#region Properties/Indexers/Events

		public Type ConfigurationType
		{
			get
			{
				return typeof(TComponentConfiguration);
			}
		}

		public TComponentConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
			set
			{
				this.configuration = value;
			}
		}

		IConfigurationObject IConfigurable.Configuration
		{
			get
			{
				return this.Configuration;
			}
			set
			{
				this.Configuration = (TComponentConfiguration)value;
			}
		}

		#endregion

		#region Methods/Operators

		private void AssertValidConfiguration()
		{
			if ((object)this.Configuration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(this.Configuration)));
		}

		protected override void CoreCreate(bool creating)
		{
			if (this.IsCreated)
				return;

			if (creating)
			{
				base.CoreCreate(creating);

				this.AssertValidConfiguration();
			}
		}

		protected override void CoreDispose(bool disposing)
		{
			if (this.IsDisposed)
				return;

			if (disposing)
			{
				this.Configuration = default(TComponentConfiguration);

				base.CoreDispose(disposing);
			}
		}

		#endregion
	}
}