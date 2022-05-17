/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	public partial interface IResourceManager
		: IAsyncLifecycle
	{
		#region Methods/Operators

		ValueTask CheckAsync(CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask DisposeAsync(Guid? slotId, IAsyncDisposable asyncDisposable, CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask<Guid?> EnterAsync(CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask<TValue> LeaveAsync<TValue>(Guid? slotId, TValue value, CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask LeaveAsync(Guid? slotId, CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask PrintAsync(Guid? slotId, string message, CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask ResetAsync(CancellationToken cancellationToken = default);

		ValueTask<IAsyncDisposableDispatch<TAsyncDisposable>> UsingAsync<TAsyncDisposable>(Guid? slotId, TAsyncDisposable asyncDisposable, CancellationToken cancellationToken = default, Action onAsyncDisposal = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
			where TAsyncDisposable : IAsyncDisposable;

		ValueTask WatchingAsync(Guid? slotId, IAsyncDisposable asyncDisposable, CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask<TAsyncDisposable> WatchingAsync<TAsyncDisposable>(Guid? slotId, TAsyncDisposable asyncDisposable, CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
			where TAsyncDisposable : IAsyncDisposable;

		#endregion
	}
}
#endif