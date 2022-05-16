/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	internal sealed class UsingBlockProxy<TDisposable>
		: Lifecycle,
			IDisposableDispatch<TDisposable>
		where TDisposable : IDisposable
	{
		#region Constructors/Destructors

		public UsingBlockProxy(IResourceManager resourceManager, Guid? slotId, TDisposable target)
		{
			if ((object)resourceManager == null)
				throw new ArgumentNullException(nameof(resourceManager));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)target == null)
				throw new ArgumentNullException(nameof(target));

			this.resourceManager = resourceManager;
			this.slotId = slotId;
			this.target = target;
		}

		#endregion

		#region Fields/Constants

		private readonly IResourceManager resourceManager;
		private readonly Guid? slotId;

		private readonly TDisposable target;

		#endregion

		#region Properties/Indexers/Events

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

		public TDisposable Target
		{
			get
			{
				return this.target;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreCreate(bool creating)
		{
			// do nothing
		}

		protected override void CoreDispose(bool disposing)
		{
			if ((object)this.Target != null)
			{
				this.Target.Dispose();
				this.ResourceManager.Dispose(this.SlotId, this.Target);
			}
		}

		#endregion
	}
}