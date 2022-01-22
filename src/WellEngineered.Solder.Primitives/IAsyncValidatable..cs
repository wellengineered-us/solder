/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Threading;

namespace WellEngineered.Solder.Primitives
{
	public interface IAsyncValidatable
	{
		#region Methods/Operators

		IAsyncEnumerable<IMessage> ValidateAsync(CancellationToken cancellationToken = default);

		IAsyncEnumerable<IMessage> ValidateAsync(object context, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif