/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Utilities.AdoNet
{
	/// <summary>
	/// The exception thrown when a application configuration error occurs.
	/// </summary>
	public class AdoNetException : SolderException
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the AdoNetException class.
		/// </summary>
		public AdoNetException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the AdoNetException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		public AdoNetException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the AdoNetException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		/// <param name="innerException"> The inner exception. </param>
		public AdoNetException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}