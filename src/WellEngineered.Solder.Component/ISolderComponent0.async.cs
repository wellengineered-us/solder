/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Component
{
	public partial interface ISolderComponent0
		: IAsyncLifecycle
	{
		#region Properties/Indexers/Events

		Guid AsyncComponentId
		{
			get;
		}

		bool IsAsyncReusable
		{
			get;
		}

		#endregion
	}
}
#endif