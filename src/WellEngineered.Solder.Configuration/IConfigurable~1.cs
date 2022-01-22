/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Configuration
{
	public interface IConfigurable<TSolderConfigurationObject>
		: IConfigurable
		where TSolderConfigurationObject : ISolderConfiguration
	{
		#region Properties/Indexers/Events

		new TSolderConfigurationObject Configuration
		{
			get;
			set;
		}

		#endregion
	}
}