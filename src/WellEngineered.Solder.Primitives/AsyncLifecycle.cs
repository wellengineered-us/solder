/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Primitives
{
	public abstract class AsyncLifecycle
		: IAsyncLifecycle
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

		protected abstract ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default);

		protected abstract ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default);

		public async ValueTask CreateAsync()
		{
			await this.CreateAsync(default);
		}

		public async ValueTask CreateAsync(CancellationToken cancellationToken)
		{
			await this.InitializeAsync(cancellationToken);
		}

		public async ValueTask DisposeAsync()
		{
			await this.DisposeAsync(default);
		}

		public async ValueTask DisposeAsync(CancellationToken cancellationToken)
		{
			await this.TerminateAsync(cancellationToken);
		}

		private async ValueTask ExplicitInitializeAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				if (this.IsAsyncCreated)
					return;

				await this.CoreCreateAsync(true, cancellationToken);
				await this.MaybeSetIsAsyncCreatedAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				throw new SolderException(string.Format("The lifecycle failed (see inner exception)."), ex);
			}
		}

		protected async ValueTask ExplicitSetIsAsyncCreatedAsync(CancellationToken cancellationToken = default)
		{
			//GC.ReRegisterForFinalize(this);
			this.IsAsyncCreated = true;
			await Task.CompletedTask;
		}

		protected async ValueTask ExplicitSetIsAsyncDisposedAsync(CancellationToken cancellationToken = default)
		{
			this.IsAsyncDisposed = true;
			GC.SuppressFinalize(this);
			await Task.CompletedTask;
		}

		private async ValueTask ExplicitTerminateAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				if (this.IsAsyncDisposed)
					return;

				await this.CoreDisposeAsync(true, cancellationToken);
				await this.MaybeSetIsAsyncDisposedAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				throw new SolderException(string.Format("The lifecycle failed (see inner exception)."), ex);
			}
		}

		public async ValueTask InitializeAsync(CancellationToken cancellationToken = default)
		{
			if (this.IsAsyncCreated)
				return;

			await this.MaybeInitializeAsync(cancellationToken);
		}

		protected virtual async ValueTask MaybeInitializeAsync(CancellationToken cancellationToken = default)
		{
			await this.ExplicitInitializeAsync(cancellationToken);
		}

		protected virtual async ValueTask MaybeSetIsAsyncCreatedAsync(CancellationToken cancellationToken = default)
		{
			await this.ExplicitSetIsAsyncCreatedAsync(cancellationToken);
		}

		protected virtual async ValueTask MaybeSetIsAsyncDisposedAsync(CancellationToken cancellationToken = default)
		{
			await this.ExplicitSetIsAsyncDisposedAsync(cancellationToken);
		}

		protected virtual async ValueTask MaybeTerminateAsync(CancellationToken cancellationToken = default)
		{
			await this.ExplicitTerminateAsync(cancellationToken);
		}

		public async ValueTask TerminateAsync(CancellationToken cancellationToken = default)
		{
			if (this.IsAsyncDisposed)
				return;

			await this.MaybeTerminateAsync(cancellationToken);
		}

		#endregion
	}
}
#endif