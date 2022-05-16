/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Component
{
	public abstract class SolderComponent<TSolderConfiguration>
		: SolderComponent1,
			ISolderComponent<TSolderConfiguration>
		where TSolderConfiguration : class, ISolderConfiguration
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

		#endregion
	}
}