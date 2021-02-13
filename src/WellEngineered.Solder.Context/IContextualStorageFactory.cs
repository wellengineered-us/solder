/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Context
{
	public interface IContextualStorageFactory
	{
		#region Methods/Operators

		IContextualStorageStrategy GetContextualStorage();

		#endregion
	}
}