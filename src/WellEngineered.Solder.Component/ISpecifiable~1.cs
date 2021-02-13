/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Component
{
	public interface ISpecifiable<TSpecification> : ISpecifiable
		where TSpecification : IComponentConfigurationObject
	{
		#region Properties/Indexers/Events

		new TSpecification Specification
		{
			get;
			set;
		}

		#endregion
	}
}