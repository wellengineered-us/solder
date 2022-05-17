/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Component
{
	public abstract partial class SolderComponent1
		: SolderComponent0,
			ISolderComponent1

	{
		#region Constructors/Destructors

		protected SolderComponent1()
		{
		}

		#endregion

		#region Fields/Constants

		private ISolderConfiguration configuration;

		#endregion

		#region Properties/Indexers/Events

		public abstract Type ConfigurationType
		{
			get;
		}

		public ISolderConfiguration Configuration
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

		#endregion

		#region Methods/Operators

		private void AssertValidConfiguration()
		{
			if ((object)this.Configuration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(this.Configuration)));
		}

		protected override void CoreCreate(bool creating)
		{
			if (creating)
			{
				base.CoreCreate(creating);

				this.AssertValidConfiguration();
			}
		}

		protected override void CoreDispose(bool disposing)
		{
			if (disposing)
			{
				this.Configuration = null;

				base.CoreDispose(disposing);
			}
		}

		#endregion
	}
}