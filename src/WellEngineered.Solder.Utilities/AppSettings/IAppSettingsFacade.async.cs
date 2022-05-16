/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Utilities.AppSettings
{
	public partial interface IAppSettingsFacade
	{
		#region Methods/Operators

		/// <summary>
		/// Gets the value of an app settings for the current application's default configuration.
		/// An AppSettingsException is thrown if the key does not exist.
		/// </summary>
		/// <typeparam name="TValue"> The type to convert the app settings value. </typeparam>
		/// <param name="key"> The key to get a value. </param>
		/// <returns> The app settings as type TValue. </returns>
		ValueTask<TValue> GetAppSettingAsync<TValue>(string key, CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets the value of an app settings for the current application's default configuration.
		/// An AppSettingsException is thrown if the key does not exist.
		/// </summary>
		/// <param name="valueType"> The type to convert the app settings value. </param>
		/// <param name="key"> The key to get a value. </param>
		/// <returns> The app settings as a string. </returns>
		ValueTask<object> GetAppSettingAsync(Type valueType, string key, CancellationToken cancellationToken = default);

		/// <summary>
		/// Checks to see if an app settings key exists for the current application's default configuration.
		/// </summary>
		/// <param name="key"> The key to check. </param>
		/// <returns> A boolean value indicating the app setting key presence. </returns>
		ValueTask<bool> HasAppSettingAsync(string key, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif