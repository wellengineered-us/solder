/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Injection
{
	/// <summary>
	/// The exception thrown when a specific resource error occurs.
	/// </summary>
	public sealed class ResourceException : InjectionException
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the ResourceException class.
		/// </summary>
		public ResourceException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResourceException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		public ResourceException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ResourceException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		/// <param name="innerException"> The inner exception. </param>
		public ResourceException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}