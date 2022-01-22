/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Interception
{
	public partial interface IRuntimeContext
		: IDualLifecycle
	{
		#region Properties/Indexers/Events

		bool ContinueInterception
		{
			get;
		}

		int InterceptionCount
		{
			get;
		}

		int InterceptionIndex
		{
			get;
		}

		#endregion

		#region Methods/Operators

		void AbortInterceptionChain();

		#endregion
	}
}