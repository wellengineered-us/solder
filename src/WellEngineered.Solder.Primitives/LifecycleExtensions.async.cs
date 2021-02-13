/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Primitives
{
	public static partial class LifecycleExtensions
	{
		#region Methods/Operators

		public static async ValueTask DisposeAllAsync<TAsyncDisposable>(this IList<TAsyncDisposable> asyncDisposables)
			where TAsyncDisposable : IAsyncDisposable
		{
			if ((object)asyncDisposables != null)
			{
				foreach (TAsyncDisposable asyncDisposable in asyncDisposables)
				{
					if ((object)asyncDisposable != null)
						await asyncDisposable.DisposeAsync();
				}

				asyncDisposables.Clear();
			}
		}

		public static bool? GetSafeIsAsyncCreated(this object obj)
		{
			if ((object)obj != null)
			{
				if (obj is IAsyncCreatableEx)
				{
					return ((IAsyncCreatableEx)obj).IsAsyncCreated;
				}
			}

			return null;
		}

		public static bool? GetSafeIsAsyncDisposed(this object obj)
		{
			if ((object)obj != null)
			{
				if (obj is IAsyncDisposableEx)
				{
					return ((IAsyncDisposableEx)obj).IsAsyncDisposed;
				}
			}

			return null;
		}

		public static async Task<bool> SafeCreateAsync(this object obj)
		{
			bool result = false;
			if ((object)obj != null)
			{
				if (obj is IAsyncCreatableEx)
				{
					await ((IAsyncCreatableEx)obj).CreateAsync();
					result = true;
				}
			}

			return result;
		}

		public static async Task<bool> SafeDisposeAsync(this object obj)
		{
			bool result = false;
			if ((object)obj != null)
			{
				if (obj is IAsyncDisposable)
				{
					await ((IAsyncDisposable)obj).DisposeAsync();
					result = true;
				}
			}

			return result;
		}

		#endregion
	}
}