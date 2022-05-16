/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Injection;

public interface IDisposableDispatch<out TDisposable> : IDisposable
	where TDisposable : IDisposable
{
	#region Properties/Indexers/Events

	TDisposable Target
	{
		get;
	}

	#endregion
}