/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Runtime.CompilerServices;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	public partial interface IResourceManager : ICreatableEx, IDisposableEx
	{
		#region Methods/Operators

		/*TException NewExceptionWithCallerInfo<TException>(Func<string, TException> factory,
			[CallerFilePath] string callerFilePath = null,
			[CallerLineNumber] int? callerLineNumber = null,
			[CallerMemberName] string callerMemberName = null)
			where TException : Exception, new();*/

		void Check([CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		void Dispose(Guid? slotId, IDisposable disposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		Guid? Enter([CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		TValue Leave<TValue>(Guid? slotId, TValue value, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		void Leave(Guid? slotId, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		void Print(string message, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		void Reset();

		IDisposable Using(Guid? slotId, IDisposable disposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		void Watching(Guid? slotId, IDisposable disposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null);

		TDisposable Watching<TDisposable>(Guid? slotId, TDisposable disposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
			where TDisposable : IDisposable;

		#endregion
	}
}