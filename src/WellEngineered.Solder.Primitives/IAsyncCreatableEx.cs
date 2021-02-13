/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Primitives
{
	public interface IAsyncCreatableEx : IAsyncCreatable
	{
		#region Properties/Indexers/Events

		bool IsAsyncCreated
		{
			get;
		}

		#endregion
	}
}