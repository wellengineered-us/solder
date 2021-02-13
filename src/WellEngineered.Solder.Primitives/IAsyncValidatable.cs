/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace WellEngineered.Solder.Primitives
{
	public interface IAsyncValidatable
	{
		#region Methods/Operators

		IAsyncEnumerable<IMessage> ValidateAsync();

		IAsyncEnumerable<IMessage> ValidateAsync(object context);

		#endregion
	}
}