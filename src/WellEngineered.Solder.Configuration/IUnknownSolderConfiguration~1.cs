/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Configuration
{
	public interface IUnknownSolderConfiguration<out TSolderSpecification>
		: IUnknownSolderConfiguration
		where TSolderSpecification : class, ISolderSpecification
	{
		#region Properties/Indexers/Events

		new TSolderSpecification Specification
		{
			get;
		}

		#endregion
	}
}