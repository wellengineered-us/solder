/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Interception
{
	public sealed partial class RuntimeInvocation
		: DualLifecycle, IRuntimeInvocation
	{
		#region Methods/Operators

		protected override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		protected override ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		#endregion
	}
}
#endif