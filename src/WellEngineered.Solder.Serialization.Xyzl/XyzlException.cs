/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Serialization.Xyzl
{
	/// <summary>
	/// The exception thrown when a specific invocation error occurs.
	/// </summary>
	public sealed class XyzlException : SolderException
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the XyzlException class.
		/// </summary>
		public XyzlException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the XyzlException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		public XyzlException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the XyzlException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		/// <param name="innerException"> The inner exception. </param>
		public XyzlException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}