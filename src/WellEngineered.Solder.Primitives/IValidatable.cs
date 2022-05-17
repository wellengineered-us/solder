/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace WellEngineered.Solder.Primitives
{
	public interface IValidatable
	{
		#region Methods/Operators

		/// <summary>
		/// Validates this instance.
		/// </summary>
		/// <returns> A enumerable of zero or more messages. </returns>
		IEnumerable<IMessage> Validate();

		/// <summary>
		/// Validates this instance.
		/// </summary>
		/// <param name="context"> A contextual label supplied by the caller. </param>
		/// <returns> A enumerable of zero or more messages. </returns>
		IEnumerable<IMessage> Validate(object context);

		#endregion
	}
}