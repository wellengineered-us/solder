/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Injection
{
	internal partial class UsingBlockProxy
	{
		#region Constructors/Destructors

		public UsingBlockProxy(IResourceManager resourceManager, Guid? slotId, IAsyncDisposable asyncDisposable)
			: this(resourceManager, slotId)
		{
			if ((object)asyncDisposable == null)
				throw new ArgumentNullException(nameof(asyncDisposable));

			this.disposable = null;
			this.asyncDisposable = asyncDisposable;
		}

		#endregion

		#region Fields/Constants

		private readonly IAsyncDisposable asyncDisposable;

		#endregion

		#region Properties/Indexers/Events

		private IAsyncDisposable AsyncDisposable
		{
			get
			{
				return this.asyncDisposable;
			}
		}

		#endregion

		#region Methods/Operators

		protected override ValueTask CoreCreateAsync(bool creating)
		{
			// do nothing
			return default;
		}

		protected override async ValueTask CoreDisposeAsync(bool disposing)
		{
			if (this.IsAsyncDisposed)
				return;

			if (disposing)
			{
				if ((object)this.AsyncDisposable != null)
				{
					await this.AsyncDisposable.DisposeAsync();
					await this.ResourceManager.DisposeAsync(this.SlotId, this.AsyncDisposable);
				}
			}
		}

		#endregion
	}
}