/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Primitives
{
	public abstract partial class Lifecycle : IAsyncCreatableEx, IAsyncDisposableEx
	{
		#region Fields/Constants

		private bool isAsyncCreated;
		private bool isAsyncDisposed;

		#endregion

		#region Properties/Indexers/Events

		public bool IsAsyncCreated
		{
			get
			{
				return this.isAsyncCreated;
			}
			private set
			{
				this.isAsyncCreated = value;
			}
		}

		public bool IsAsyncDisposed
		{
			get
			{
				return this.isAsyncDisposed;
			}
			private set
			{
				this.isAsyncDisposed = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected abstract ValueTask CoreCreateAsync(bool creating);

		protected abstract ValueTask CoreDisposeAsync(bool disposing);

		public async ValueTask CreateAsync()
		{
			await this.InitializeAsync();
		}

		public async ValueTask DisposeAsync()
		{
			await this.TerminateAsync();
		}

		protected async ValueTask ExplicitSetIsAsyncCreated()
		{
			//GC.ReRegisterForFinalize(this);
			this.IsAsyncCreated = true;
			await Task.CompletedTask;
		}

		protected async ValueTask ExplicitSetIsAsyncDisposed()
		{
			this.IsAsyncDisposed = true;
			GC.SuppressFinalize(this);
			await Task.CompletedTask;
		}

		public async ValueTask InitializeAsync()
		{
			if (this.IsAsyncCreated)
				return;

			await this.CoreCreateAsync(true);
			await this.MaybeSetIsAsyncCreated();
		}

		protected async virtual ValueTask MaybeSetIsAsyncCreated()
		{
			await this.ExplicitSetIsAsyncCreated();
		}

		protected async virtual ValueTask MaybeSetIsAsyncDisposed()
		{
			await this.ExplicitSetIsAsyncDisposed();
		}

		public async ValueTask TerminateAsync()
		{
			if (this.IsAsyncDisposed)
				return;

			await this.CoreDisposeAsync(true);
			await this.MaybeSetIsAsyncDisposed();
		}

		#endregion
	}
}