/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Collections.Generic;
using System.Threading;

using WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs
{
	public sealed partial class DogFoodSpecification : EndToEndSpecification
	{
		#region Methods/Operators

		protected override IAsyncEnumerable<IMessage> CoreValidateAsync(object context, CancellationToken cancellationToken = default)
		{
			return null;
		}

		#endregion
	}
}
#endif