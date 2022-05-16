/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Runtime.InteropServices;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	public sealed class DisposableWeakReference
		: Lifecycle,
			IEquatable<DisposableWeakReference>
	{
		#region Constructors/Destructors

		public DisposableWeakReference(IDisposable target)
		{
			if ((object)target == null)
				throw new ArgumentNullException(nameof(target));

			this.hashCode = target.GetHashCode();
			this.handle = GCHandle.Alloc(target, GCHandleType.Weak);
		}

		~DisposableWeakReference()
		{
			// special case
			this.Dispose();
		}

		#endregion

		#region Fields/Constants

		readonly GCHandle handle;
		readonly int hashCode;

		#endregion

		#region Properties/Indexers/Events

		public bool IsAlive
		{
			get
			{
				return (this.handle.Target != null);
			}
		}

		public IDisposable Target
		{
			get
			{
				return this.handle.Target as IDisposable;
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
			if (disposing)
			{
				this.handle.Free();
			}
		}

		public bool Equals(DisposableWeakReference other)
		{
			return ReferenceEquals(other.Target, this.Target);
		}

		public override bool Equals(object obj)
		{
			if (obj is DisposableWeakReference)
				return this.Equals((DisposableWeakReference)obj);

			return false;
		}

		public override int GetHashCode()
		{
			return this.hashCode;
		}

		#endregion
	}
}