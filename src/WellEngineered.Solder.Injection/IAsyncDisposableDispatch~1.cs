/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;

namespace WellEngineered.Solder.Injection;

public interface IAsyncDisposableDispatch<out TAsyncDisposable> : IAsyncDisposable
	where TAsyncDisposable : IAsyncDisposable
{
	#region Properties/Indexers/Events

	TAsyncDisposable AsyncTarget
	{
		get;
	}

	#endregion
}
#endif