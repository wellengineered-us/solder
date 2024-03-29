/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Primitives
{
	public interface IAsyncDisposableEx : IAsyncDisposable
	{
		#region Properties/Indexers/Events

		bool IsAsyncDisposed
		{
			get;
		}

		#endregion

		#region Methods/Operators

		ValueTask DisposeAsync(CancellationToken cancellationToken);

		#endregion
	}
}
#endif