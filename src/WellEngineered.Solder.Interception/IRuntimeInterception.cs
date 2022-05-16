/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

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
		void Invoke(IRuntimeInvocation runtimeInvocation, IRuntimeContext runtimeContext);

		#endregion
	}
}