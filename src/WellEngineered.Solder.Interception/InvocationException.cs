/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Interception
{
	/// <summary>
	/// The exception thrown when a specific invocation error occurs.
	/// </summary>
	public sealed class InvocationException : InterceptionException
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the InvocationException class.
		/// </summary>
		public InvocationException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the InvocationException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		public InvocationException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the InvocationException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		/// <param name="innerException"> The inner exception. </param>
		public InvocationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}