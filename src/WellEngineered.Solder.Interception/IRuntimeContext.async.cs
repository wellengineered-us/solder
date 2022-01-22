/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Interception
{
	public partial interface IRuntimeContext
		: IDualLifecycle
	{
		#region Methods/Operators

		ValueTask AbortInterceptionChainAsync(CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif