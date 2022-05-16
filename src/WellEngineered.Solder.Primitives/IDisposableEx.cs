/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Primitives
{
	public interface IDisposableEx : IDisposable
	{
		#region Properties/Indexers/Events

		bool IsDisposed
		{
			get;
		}

		#endregion
	}
}