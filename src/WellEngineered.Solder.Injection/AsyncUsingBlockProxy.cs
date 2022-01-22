/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	internal sealed class AsyncUsingBlockProxy
		: AsyncLifecycle
	{
		#region Constructors/Destructors

		public AsyncUsingBlockProxy(IResourceManager resourceManager, Guid? slotId, IAsyncDisposable asyncDisposable)
		{
			if ((object)resourceManager == null)
				throw new ArgumentNullException(nameof(resourceManager));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)asyncDisposable == null)
				throw new ArgumentNullException(nameof(asyncDisposable));

			this.resourceManager = resourceManager;
			this.slotId = slotId;
			this.asyncDisposable = asyncDisposable;
		}

		#endregion

		#region Fields/Constants

		private readonly IAsyncDisposable asyncDisposable;
		private readonly IResourceManager resourceManager;
		private readonly Guid? slotId;

		#endregion

		#region Properties/Indexers/Events

		private IAsyncDisposable AsyncDisposable
		{
			get
			{
				return this.asyncDisposable;
			}
		}

		private IResourceManager ResourceManager
		{
			get
			{
				return this.resourceManager;
			}
		}

		private Guid? SlotId
		{
			get
			{
				return this.slotId;
			}
		}

		#endregion

		#region Methods/Operators

		protected override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		protected async override ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			if ((object)this.AsyncDisposable != null)
			{
				await this.AsyncDisposable.SafeDisposeAsync(cancellationToken);
				await this.ResourceManager.DisposeAsync(this.SlotId, this.AsyncDisposable, cancellationToken);
			}
		}

		#endregion
	}
}
#endif