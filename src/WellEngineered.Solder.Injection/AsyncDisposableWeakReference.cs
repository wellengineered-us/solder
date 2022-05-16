/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	public sealed class AsyncDisposableWeakReference
		: AsyncLifecycle,
			IEquatable<AsyncDisposableWeakReference>
	{
		#region Constructors/Destructors

		public AsyncDisposableWeakReference(IAsyncDisposable target)
		{
			if ((object)target == null)
				throw new ArgumentNullException(nameof(target));

			this.hashCode = target.GetHashCode();
			this.handle = GCHandle.Alloc(target, GCHandleType.Weak);
		}

		~AsyncDisposableWeakReference()
		{
			// special case
			this.DisposeAsync()
				.GetAwaiter()
				.GetResult();
		}

		#endregion

		#region Fields/Constants

		readonly int hashCode;
		GCHandle handle;

		#endregion

		#region Properties/Indexers/Events

		public bool IsAlive
		{
			get
			{
				return (this.handle.Target != null);
			}
		}

		public IAsyncDisposable Target
		{
			get
			{
				return this.handle.Target as IAsyncDisposable;
			}
		}

		#endregion

		#region Methods/Operators

		protected override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		protected override async ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			if (disposing)
			{
				this.handle.Free();
			}

			await Task.CompletedTask;
		}

		public bool Equals(AsyncDisposableWeakReference other)
		{
			return ReferenceEquals(other.Target, this.Target);
		}

		public override bool Equals(object obj)
		{
			if (obj is AsyncDisposableWeakReference)
				return this.Equals((AsyncDisposableWeakReference)obj);

			return false;
		}

		public override int GetHashCode()
		{
			return this.hashCode;
		}

		#endregion
	}
}
#endif