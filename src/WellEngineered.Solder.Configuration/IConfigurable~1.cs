/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Configuration
{
	public interface IConfigurable<TConfiguration> : IConfigurable
		where TConfiguration : IConfigurationObject
	{
		#region Properties/Indexers/Events

		new TConfiguration Configuration
		{
			get;
			set;
		}

		#endregion
	}
}