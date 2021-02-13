/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace WellEngineered.Solder.Primitives
{
	public static partial class LifecycleExtensions
	{
		#region Methods/Operators

		public static void DisposeAll<TDisposable>(this IList<TDisposable> disposables)
			where TDisposable : IDisposable
		{
			if ((object)disposables != null)
			{
				foreach (TDisposable disposable in disposables)
				{
					if ((object)disposable != null)
						disposable.Dispose();
				}

				disposables.Clear();
			}
		}

		public static bool? GetSafeIsCreated(this object obj)
		{
			if ((object)obj != null)
			{
				if (obj is ICreatableEx)
				{
					return ((ICreatableEx)obj).IsCreated;
				}
			}

			return null;
		}

		public static bool? GetSafeIsDisposed(this object obj)
		{
			if ((object)obj != null)
			{
				if (obj is IDisposableEx)
				{
					return ((IDisposableEx)obj).IsDisposed;
				}
			}

			return null;
		}

		public static bool SafeCreate(this object obj)
		{
			bool result = false;
			if ((object)obj != null)
			{
				if (obj is ICreatableEx)
				{
					((ICreatableEx)obj).Create();
					result = true;
				}
			}

			return result;
		}

		public static bool SafeDispose(this object obj)
		{
			bool result = false;
			if ((object)obj != null)
			{
				if (obj is IDisposable)
				{
					((IDisposable)obj).Dispose();
					result = true;
				}
			}

			return result;
		}

		#endregion
	}
}