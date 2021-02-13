/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace WellEngineered.Solder.Component
{
	public interface IUnknownComponentConfigurationObject : IComponentConfigurationObject
	{
		#region Properties/Indexers/Events

		IDictionary<string, object> ComponentSpecificConfiguration
		{
			get;
		}

		Type ComponentSpecificConfigurationType
		{
			get;
		}

		string ComponentAssemblyQualifiedTypeName
		{
			get;
			set;
		}

		#endregion
	}
}