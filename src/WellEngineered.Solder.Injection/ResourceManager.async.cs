/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

using static WellEngineered.Solder.Primitives.Telemetry;

namespace WellEngineered.Solder.Injection
{
	public sealed partial class ResourceManager
	{
		#region Fields/Constants

		private readonly ConcurrentDictionary<Guid?, IList<AsyncDisposableWeakReference>> asyncTrackedResources;

		#endregion

		#region Properties/Indexers/Events

		private ConcurrentDictionary<Guid?, IList<AsyncDisposableWeakReference>> AsyncTrackedResources
		{
			get
			{
				return this.asyncTrackedResources;
			}
		}

		#endregion

		#region Methods/Operators

		public async ValueTask CheckAsync(CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			StringBuilder sb;
			int slots = 0, objs = 0;

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			sb = new StringBuilder();

			foreach (KeyValuePair<Guid?, IList<AsyncDisposableWeakReference>> trackedResource in this.AsyncTrackedResources)
			{
				if ((object)trackedResource.Key == null ||
					(object)trackedResource.Value == null)
					throw NewExceptionWithCallerInfo<ResourceException>((value) => new ResourceException(value));

				Guid? slotId = trackedResource.Key;
				IList<AsyncDisposableWeakReference> asyncDisposables = trackedResource.Value;
				int size = asyncDisposables.Count;
				int ezis = 0;

				foreach (AsyncDisposableWeakReference asyncDisposable in asyncDisposables)
				{
					IAsyncDisposableEx asyncDisposableEx;
					IAsyncDisposable _asynDisposable = asyncDisposable.Target;

					if ((object)(asyncDisposableEx = _asynDisposable as IAsyncDisposableEx) != null &&
						asyncDisposableEx.IsAsyncDisposed)
					{
						ezis--;
						continue;
					}

					sb.Append(Environment.NewLine);
					sb.Append(string.Format("\t\t{0}", FormatObjectInfo(this.ObjectIdGenerator, asyncDisposable)));
				}

				int offset = size + ezis;

				if (offset > 0)
				{
					sb.Append(Environment.NewLine);
					sb.Append(string.Format("\t{0} ~ {1}#", FormatGuid(slotId), offset));

					slots++;
					objs += offset;
				}
			}

			if (sb.Length > 0)
				sb.Append(Environment.NewLine);

			await this.PrintAsync(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), string.Format("check_async!{0}", sb.ToString()), cancellationToken);

			if (slots > 0)
			{
				string message = string.Format("Resource manager tracked resource check FAILED with {0} slots having {1} leaked AsyncDisposable objects", slots, objs);
				await this.PrintAsync(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), message, cancellationToken);
			}
		}

		protected override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		protected override async ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			if (this.IsDisposed)
				return;

			if (disposing)
			{
				await this.ResetAsync(cancellationToken);
			}
		}

		public async ValueTask DisposeAsync(Guid? slotId, IAsyncDisposable asyncDisposable, CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			await this.DisposeAsync(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, asyncDisposable, cancellationToken);
		}

		private async ValueTask DisposeAsync(string caller, Guid? slotId, IAsyncDisposable asyncDisposable, CancellationToken cancellationToken = default)
		{
			IList<AsyncDisposableWeakReference> asyncDisposables;
			AsyncDisposableWeakReference _asyncDisposable;

			if ((object)caller == null)
				throw new ArgumentNullException(nameof(caller));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)asyncDisposable == null)
				throw new ArgumentNullException(nameof(asyncDisposable));

			_asyncDisposable = new AsyncDisposableWeakReference(asyncDisposable);

			if (!this.AsyncTrackedResources.TryGetValue(slotId, out asyncDisposables) || !asyncDisposables.Contains(_asyncDisposable))
				throw new ResourceException(FormatOperation(this.ObjectIdGenerator, "error", slotId, asyncDisposable));

			asyncDisposables.Remove(_asyncDisposable);

			await this.PrintAsync(caller, FormatOperation(this.ObjectIdGenerator, "dispose_async", slotId, asyncDisposable), cancellationToken);
		}

		public async ValueTask<Guid?> EnterAsync(CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			Guid slotId;

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			slotId = Guid.NewGuid();

			await this.PrintAsync(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), FormatOperation("enter_async", slotId), cancellationToken);

			return slotId;
		}

		public async ValueTask<TValue> LeaveAsync<TValue>(Guid? slotId, TValue value, CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			await this.LeaveAsync(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, cancellationToken);

			return value;
		}

		public async ValueTask LeaveAsync(Guid? slotId, CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			await this.LeaveAsync(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, cancellationToken);
		}

		private async ValueTask LeaveAsync(string caller, Guid? slotId, CancellationToken cancellationToken = default)
		{
			if ((object)caller == null)
				throw new ArgumentNullException(nameof(caller));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			await this.PrintAsync(caller, FormatOperation("leave_async", slotId), cancellationToken);
		}

		public async ValueTask OnlyWhenDebugPrintAsync(string message, CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			if ((object)message == null)
				throw new ArgumentNullException(nameof(message));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			await this.PrintAsync(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), message, cancellationToken);
		}

		public async ValueTask PrintAsync(Guid? slotId, string message, CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			if ((object)message == null)
				throw new ArgumentNullException(nameof(message));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			await this.PrintAsync(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), string.Format("{0}\t{1}", FormatGuid(slotId), message), cancellationToken);
		}

		private async ValueTask PrintAsync(string caller, string message, CancellationToken cancellationToken = default)
		{
			if ((object)caller == null)
				throw new ArgumentNullException(nameof(caller));

			if ((object)message == null)
				throw new ArgumentNullException(nameof(message));

			/* THIS METHOD SHOULD NOT BE DEFINED IN RELEASE/PRODUCTION BUILDS */
			string temp = string.Format("[{0}: {1}]", caller, message);

			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkGray;
			await Console.Out.WriteLineAsync(temp);
			Console.ForegroundColor = oldConsoleColor;
		}

		public async ValueTask ResetAsync(CancellationToken cancellationToken = default)
		{
			this.TrackedResources.Clear();
			this.AsyncTrackedResources.Clear();
			await Task.CompletedTask;
		}

		private async ValueTask SlotAsync(string caller, Guid? slotId, IAsyncDisposable asyncDisposable, string message, CancellationToken cancellationToken = default)
		{
			IList<AsyncDisposableWeakReference> asyncDisposables;
			AsyncDisposableWeakReference _asyncDisposable;

			if ((object)caller == null)
				throw new ArgumentNullException(nameof(caller));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)asyncDisposable == null)
				throw new ArgumentNullException(nameof(asyncDisposable));

			if ((object)message == null)
				throw new ArgumentNullException(nameof(message));

			_asyncDisposable = new AsyncDisposableWeakReference(asyncDisposable);

			if (this.AsyncTrackedResources.TryGetValue(slotId, out asyncDisposables))
			{
				if (asyncDisposables.Contains(_asyncDisposable))
					throw new ResourceException(FormatOperation(this.ObjectIdGenerator, "error", slotId, asyncDisposable));
			}
			else
			{
				asyncDisposables = new List<AsyncDisposableWeakReference>();

				if (!this.AsyncTrackedResources.TryAdd(slotId, asyncDisposables))
					throw new ResourceException(FormatOperation(this.ObjectIdGenerator, "error", slotId, asyncDisposable));
			}

			asyncDisposables.Add(_asyncDisposable);

			await this.PrintAsync(caller, message, cancellationToken);
		}

		public async ValueTask<IAsyncDisposableDispatch<TAsyncDisposable>> UsingAsync<TAsyncDisposable>(Guid? slotId, TAsyncDisposable asyncDisposable, CancellationToken cancellationToken = default, Action onAsyncDisposal = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
			where TAsyncDisposable : IAsyncDisposable
		{
			IAsyncDisposableDispatch<TAsyncDisposable> asyncDisposableDispatch;

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)asyncDisposable == null)
				throw new ArgumentNullException(nameof(asyncDisposable));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			asyncDisposableDispatch = new AsyncUsingBlockProxy<TAsyncDisposable>(this, slotId, asyncDisposable); // forward call to .AsyncDispose()
			await asyncDisposableDispatch.SafeCreateAsync(cancellationToken);

			await this.SlotAsync(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, asyncDisposable, FormatOperation(this.ObjectIdGenerator, "using_async", slotId, asyncDisposable), cancellationToken);

			return asyncDisposableDispatch;
		}

		public async ValueTask WatchingAsync(Guid? slotId, IAsyncDisposable asyncDisposable, CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)asyncDisposable == null)
				throw new ArgumentNullException(nameof(asyncDisposable));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			await this.SlotAsync(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, asyncDisposable, FormatOperation(this.ObjectIdGenerator, "watching_async", slotId, asyncDisposable), cancellationToken);
		}

		public async ValueTask<TAsyncDisposable> WatchingAsync<TAsyncDisposable>(Guid? slotId, TAsyncDisposable asyncDisposable, CancellationToken cancellationToken = default, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
			where TAsyncDisposable : IAsyncDisposable
		{
			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)asyncDisposable == null)
				throw new ArgumentNullException(nameof(asyncDisposable));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			await this.SlotAsync(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, asyncDisposable, FormatOperation(this.ObjectIdGenerator, "watching_async", slotId, asyncDisposable), cancellationToken);

			return asyncDisposable;
		}

		#endregion
	}
}
#endif