/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Primitives
{
	public interface ICreatableEx : ICreatable
	{
		#region Properties/Indexers/Events

		bool IsCreated
		{
			get;
		}

		#endregion
	}
}