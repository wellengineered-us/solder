/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Context
{
	public interface IContextualStorageStrategy
	{
		#region Methods/Operators

		T GetValue<T>(string key);

		bool HasValue(string key);

		void RemoveValue(string key);

		void SetValue<T>(string key, T value);

		#endregion
	}
}