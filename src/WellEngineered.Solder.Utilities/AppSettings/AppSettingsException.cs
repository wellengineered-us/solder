/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Utilities.AppSettings
{
	/// <summary>
	/// The exception thrown when a application configuration error occurs.
	/// </summary>
	public class AppSettingsException : SolderException
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the AppSettingsException class.
		/// </summary>
		public AppSettingsException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the AppSettingsException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		public AppSettingsException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the AppSettingsException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		/// <param name="innerException"> The inner exception. </param>
		public AppSettingsException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}