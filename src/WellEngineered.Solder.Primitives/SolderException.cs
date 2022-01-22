/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Primitives
{
	/// <summary>
	/// The exception thrown when a framework error occurs.
	/// </summary>
	public class SolderException : Exception
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the SolderException class.
		/// </summary>
		public SolderException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the SolderException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		public SolderException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the SolderException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		/// <param name="innerException"> The inner exception. </param>
		public SolderException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}