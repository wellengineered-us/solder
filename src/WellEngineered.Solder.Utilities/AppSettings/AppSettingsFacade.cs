/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Utilities.AppSettings
{
	public sealed partial class AppSettingsFacade : IAppSettingsFacade
	{
		#region Constructors/Destructors

		public AppSettingsFacade(IConfigurationRoot configurationRoot)
		{
			if ((object)configurationRoot == null)
				throw new ArgumentNullException(nameof(configurationRoot));

			this.configurationRoot = configurationRoot;
		}

		#endregion

		#region Fields/Constants

		private const string APP_SETTINGS_FILE_NAME = "appsettings.json";
		private readonly static AppSettingsFacade @default = LoadFromJsonFile(APP_SETTINGS_FILE_NAME);
		private readonly IConfigurationRoot configurationRoot;

		#endregion

		#region Properties/Indexers/Events

		public static AppSettingsFacade Default
		{
			get
			{
				return @default;
			}
		}

		private IConfigurationRoot ConfigurationRoot
		{
			get
			{
				return this.configurationRoot;
			}
		}

		#endregion

		#region Methods/Operators

		public static AppSettingsFacade LoadFromJsonFile(string appConfigFilePath)
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationBuilder configurationBuilder;
			JsonConfigurationSource configurationSource;
			IConfigurationProvider configurationProvider;
			IConfigurationRoot configurationRoot;

			if ((object)appConfigFilePath == null)
				throw new ArgumentNullException(nameof(appConfigFilePath));

			configurationBuilder = new ConfigurationBuilder();
			configurationSource = new JsonConfigurationSource() { Optional = false, Path = appConfigFilePath };
			configurationProvider = new JsonConfigurationProvider(configurationSource);
			configurationBuilder.Add(configurationSource);
			configurationRoot = configurationBuilder.Build();

			appSettingsFacade = new AppSettingsFacade(configurationRoot);
			return appSettingsFacade;
		}

		/// <summary>
		/// Gets the value of an app settings for the current application's default configuration.
		/// An AppSettingsException is thrown if the key does not exist.
		/// </summary>
		/// <typeparam name="TValue"> The type to convert the app settings value. </typeparam>
		/// <param name="key"> The key to get a value. </param>
		/// <returns> The app settings as type TValue. </returns>
		public TValue GetAppSetting<TValue>(string key)
		{
			string svalue;
			TValue ovalue;
			Type typeOfValue;

			if ((object)key == null)
				throw new ArgumentNullException(nameof(key));

			typeOfValue = typeof(TValue);
			svalue = this.ConfigurationRoot[key];

			if ((object)svalue == null)
				throw new AppSettingsException(string.Format("Key '{0}' was not found in app.config file.", key));

			if (!svalue.TryParse<TValue>(out ovalue))
				throw new AppSettingsException(string.Format("App.config key '{0}' value '{1}' is not a valid '{2}'.", key, svalue, typeOfValue.FullName));

			return ovalue;
		}

		/// <summary>
		/// Gets the value of an app settings for the current application's default configuration.
		/// An AppSettingsException is thrown if the key does not exist.
		/// </summary>
		/// <param name="valueType"> The type to convert the app settings value. </param>
		/// <param name="key"> The key to get a value. </param>
		/// <returns> The app settings as a string. </returns>
		public object GetAppSetting(Type valueType, string key)
		{
			string svalue;
			object ovalue;

			if ((object)valueType == null)
				throw new ArgumentNullException(nameof(valueType));

			if ((object)key == null)
				throw new ArgumentNullException(nameof(key));

			svalue = this.ConfigurationRoot[key];

			if ((object)svalue == null)
				throw new AppSettingsException(string.Format("Key '{0}' was not found in app.config file.", key));

			if (!svalue.TryParse(valueType, out ovalue))
				throw new AppSettingsException(string.Format("App.config key '{0}' value '{1}' is not a valid '{2}'.", key, svalue, valueType.FullName));

			return ovalue;
		}

		/// <summary>
		/// Checks to see if an app settings key exists for the current application's default configuration.
		/// </summary>
		/// <param name="key"> The key to check. </param>
		/// <returns> A boolean value indicating the app setting key presence. </returns>
		public bool HasAppSetting(string key)
		{
			string value;

			if ((object)key == null)
				throw new ArgumentNullException(nameof(key));

			value = this.ConfigurationRoot[key];

			return ((object)value != null);
		}

		#endregion
	}
}