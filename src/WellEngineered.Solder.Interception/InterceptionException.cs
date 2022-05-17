/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Interception
{
	/// <summary>
	/// The exception thrown when a specific interception error occurs.
	/// </summary>
	public class InterceptionException : SolderException
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the InterceptionException class.
		/// </summary>
		public InterceptionException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the InterceptionException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		public InterceptionException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the InterceptionException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		/// <param name="innerException"> The inner exception. </param>
		public InterceptionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}