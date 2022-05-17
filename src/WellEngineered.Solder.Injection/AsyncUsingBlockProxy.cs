/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	internal sealed class AsyncUsingBlockProxy<TAsyncDisposable>
		: AsyncLifecycle,
			IAsyncDisposableDispatch<TAsyncDisposable>
		where TAsyncDisposable : IAsyncDisposable
	{
		#region Constructors/Destructors

		public AsyncUsingBlockProxy(IResourceManager resourceManager, Guid? slotId, TAsyncDisposable asyncTarget)
		{
			if ((object)resourceManager == null)
				throw new ArgumentNullException(nameof(resourceManager));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)asyncTarget == null)
				throw new ArgumentNullException(nameof(asyncTarget));

			this.resourceManager = resourceManager;
			this.slotId = slotId;
			this.asyncTarget = asyncTarget;
		}

		#endregion

		#region Fields/Constants

		private readonly TAsyncDisposable asyncTarget;
		private readonly IResourceManager resourceManager;
		private readonly Guid? slotId;

		#endregion

		#region Properties/Indexers/Events

		public TAsyncDisposable AsyncTarget
		{
			get
			{
				return this.asyncTarget;
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
			if ((object)this.AsyncTarget != null)
			{
				await this.AsyncTarget.SafeDisposeAsync(cancellationToken);
				await this.ResourceManager.DisposeAsync(this.SlotId, this.AsyncTarget, cancellationToken);
			}
		}

		#endregion
	}
}
#endif