/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Configuration
{
	public partial class SolderConfigurationCollection
		: Collection<ISolderConfiguration>,
			ISolderConfigurationCollection
	{
		#region Methods/Operators

		private async IAsyncEnumerable<IMessage> CoreValidateAsync(object context, CancellationToken cancellationToken = default)
		{
			int index = 0;

			foreach (ISolderConfiguration item in this)
			{
				string value = context == null ? string.Format("{0}", index++) : string.Format("{0}[{1}]", context, index++);

				if (item == null)
					continue;

				var childMessages = item.ValidateAsync(value, cancellationToken);

				await foreach (var childMessage in childMessages)
				{
					yield return childMessage;
				}
			}
		}

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