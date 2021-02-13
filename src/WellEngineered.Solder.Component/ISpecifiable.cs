/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Component
{
	public interface ISpecifiable
	{
		#region Properties/Indexers/Events

		Type SpecificationType
		{
			get;
		}

		IComponentConfigurationObject Specification
		{
			get;
			set;
		}

		#endregion
	}
}