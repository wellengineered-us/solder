/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Injection
{
	/// <summary>
	/// The exception thrown when a specific dependency error occurs.
	/// </summary>
	public sealed class DependencyException : InjectionException
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the DependencyException class.
		/// </summary>
		public DependencyException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the DependencyException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		public DependencyException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the DependencyException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		/// <param name="innerException"> The inner exception. </param>
		public DependencyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}