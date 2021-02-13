/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	public partial interface IResourceManager : IAsyncCreatableEx, IAsyncDisposableEx
	{
		#region Methods/Operators

		ValueTask CheckAsync([CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask DisposeAsync(Guid? slotId, IAsyncDisposable asyncDisposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask<Guid?> EnterAsync([CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask<TValue> LeaveAsync<TValue>(Guid? slotId, TValue value, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask LeaveAsync(Guid? slotId, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask PrintAsync(string message, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask ResetAsync();

		ValueTask<IAsyncDisposable> UsingAsync(Guid? slotId, IAsyncDisposable asyncDisposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask WatchingAsync(Guid? slotId, IAsyncDisposable asyncDisposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		ValueTask<TAsyncDisposable> WatchingAsync<TAsyncDisposable>(Guid? slotId, TAsyncDisposable asyncDisposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
			where TAsyncDisposable : IAsyncDisposable;

		#endregion
	}
}