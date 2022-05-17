/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Executive
{
	public partial interface IExecutableApplication : IAsyncLifecycle
	{
		#region Methods/Operators

		ValueTask<int> RunAsync(string[] args, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif