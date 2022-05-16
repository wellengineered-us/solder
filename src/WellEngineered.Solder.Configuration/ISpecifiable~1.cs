/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Configuration
{
	public interface ISpecifiable<TSpecification>
		: ISpecifiable
		where TSpecification : ISolderSpecification
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