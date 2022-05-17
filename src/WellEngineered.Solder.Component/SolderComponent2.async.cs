/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Component
{
	public abstract partial class SolderComponent2
		: SolderComponent1,
			ISolderComponent2

	{
		#region Methods/Operators

		protected override async ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			IUnknownSolderConfiguration untypedUnknownSolderConfiguration;
			IUnknownSolderConfiguration<ISolderSpecification> typedUnknownSolderConfiguration;

			if (creating)
			{
				await base.CoreCreateAsync(creating, cancellationToken);

				untypedUnknownSolderConfiguration = this.Configuration;

				typedUnknownSolderConfiguration = this.CoreCreateTypedUnknownConfiguration(untypedUnknownSolderConfiguration);

				this.Specification = typedUnknownSolderConfiguration.Specification;

				this.AssertValidSpecification();
			}
		}

		protected override async ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			if (disposing)
			{
				this.Specification = null;

				await base.CoreDisposeAsync(disposing, cancellationToken);
			}
		}

		#endregion
	}
}
#endif