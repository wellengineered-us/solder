/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Tokenization
{
	/// <summary>
	/// The exception thrown when a application configuration error occurs.
	/// </summary>
	public class TokenizationException : SolderException
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TokenizationException class.
		/// </summary>
		public TokenizationException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the TokenizationException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		public TokenizationException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the TokenizationException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		/// <param name="innerException"> The inner exception. </param>
		public TokenizationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}