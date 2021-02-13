/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Component
{
	public interface IUnknownComponentConfigurationObject<TSpecification> : IUnknownComponentConfigurationObject
		where TSpecification : IConfigurationObject
	{
		#region Properties/Indexers/Events

		new TSpecification ComponentSpecificConfiguration
		{
			get;
			set;
		}

		#endregion
	}
}