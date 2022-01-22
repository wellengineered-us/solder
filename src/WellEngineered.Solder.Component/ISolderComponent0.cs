/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Component
{
	public partial interface ISolderComponent0
		: ILifecycle
	{
		#region Properties/Indexers/Events

		Guid ComponentId
		{
			get;
		}

		bool IsReusable
		{
			get;
		}

		#endregion
	}
}