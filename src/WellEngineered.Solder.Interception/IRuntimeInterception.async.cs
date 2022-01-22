/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Interception
{
	/// <summary>
	/// Represents a run-time interception.
	/// </summary>
	public partial interface IRuntimeInterception : IDualLifecycle
	{
		#region Methods/Operators

		/// <summary>
		/// Represents a discrete run-time invocation.
		/// </summary>
		/// <param name="runtimeInvocation"> </param>
		/// <param name="runtimeContext"> </param>
		ValueTask InvokeAsync(IRuntimeInvocation runtimeInvocation, IRuntimeContext runtimeContext, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif