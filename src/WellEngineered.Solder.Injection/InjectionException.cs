/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	/// <summary>
	/// The exception thrown when a specific injection error occurs.
	/// </summary>
	public abstract class InjectionException : SolderException
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the InjectionException class.
		/// </summary>
		public InjectionException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the InjectionException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		public InjectionException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the InjectionException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		/// <param name="innerException"> The inner exception. </param>
		public InjectionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}