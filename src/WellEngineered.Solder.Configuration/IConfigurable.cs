/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Configuration
{
	public interface IConfigurable
	{
		#region Properties/Indexers/Events

		Type ConfigurationType
		{
			get;
		}

		IConfigurationObject Configuration
		{
			get;
			set;
		}

		#endregion
	}
}