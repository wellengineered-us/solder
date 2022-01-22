/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Threading;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Configuration
{
	/// <summary>
	/// Provides a base for all configuration objects.
	/// </summary>
	public abstract partial class SolderConfiguration
		: ISolderConfiguration
	{
		#region Methods/Operators

		protected abstract IAsyncEnumerable<IMessage> CoreValidateAsync(object context, CancellationToken cancellationToken = default);

		public IAsyncEnumerable<IMessage> ValidateAsync(CancellationToken cancellationToken = default)
		{
			return this.CoreValidateAsync(null, cancellationToken);
		}

		public IAsyncEnumerable<IMessage> ValidateAsync(object context, CancellationToken cancellationToken = default)
		{
			return this.CoreValidateAsync(context, cancellationToken);
		}

		#endregion
	}
}
#endif