﻿/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Utilities.AppSettings
{
	public sealed partial class AppSettingsFacade : IAppSettingsFacade
	{
		#region Methods/Operators

		public ValueTask<TValue> GetAppSettingAsync<TValue>(string key, CancellationToken cancellationToken = default)
		{
			return default;
		}

		public ValueTask<object> GetAppSettingAsync(Type valueType, string key, CancellationToken cancellationToken = default)
		{
			return default;
		}

		public ValueTask<bool> HasAppSettingAsync(string key, CancellationToken cancellationToken = default)
		{
			return default;
		}

		#endregion
	}
}
#endif