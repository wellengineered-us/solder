﻿/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	internal sealed class UsingBlockProxy
		: Lifecycle
	{
		#region Constructors/Destructors

		public UsingBlockProxy(IResourceManager resourceManager, Guid? slotId, IDisposable disposable)
		{
			if ((object)resourceManager == null)
				throw new ArgumentNullException(nameof(resourceManager));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)disposable == null)
				throw new ArgumentNullException(nameof(disposable));

			this.resourceManager = resourceManager;
			this.slotId = slotId;
			this.disposable = disposable;
		}

		#endregion

		#region Fields/Constants

		private readonly IDisposable disposable;
		private readonly IResourceManager resourceManager;
		private readonly Guid? slotId;

		#endregion

		#region Properties/Indexers/Events

		private IDisposable Disposable
		{
			get
			{
				return this.disposable;
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

		protected override void CoreCreate(bool creating)
		{
			// do nothing
		}

		protected override void CoreDispose(bool disposing)
		{
			if ((object)this.Disposable != null)
			{
				this.Disposable.Dispose();
				this.ResourceManager.Dispose(this.SlotId, this.Disposable);
			}
		}

		#endregion
	}
}