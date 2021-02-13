/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Component
{
	public abstract partial class ConfigurableComponent<TComponentConfiguration>
		: Component, IConfigurableComponent<TComponentConfiguration>
		where TComponentConfiguration : IComponentConfigurationObject
	{
		#region Methods/Operators

		protected async override ValueTask CoreCreateAsync(bool creating)
		{
			if (this.IsCreated)
				return;

			if (creating)
			{
				await base.CoreCreateAsync(creating);

				this.AssertValidConfiguration();
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