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
	public sealed partial class RuntimeContext
		: DualLifecycle, IRuntimeContext
	{
		#region Methods/Operators

		public async ValueTask AbortInterceptionChainAsync(CancellationToken cancellationToken = default)
		{
			if (!this.ContinueInterception)
				throw new InvalidOperationException(string.Format("Cannot abort interception chain; the chain has was either previously aborted or pending graceful completion."));

			this.ContinueInterception = false;
			await Task.CompletedTask;
		}

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