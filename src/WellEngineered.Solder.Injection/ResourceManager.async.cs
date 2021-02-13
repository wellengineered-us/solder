/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	public sealed partial class ResourceManager
	{
		#region Fields/Constants

		private readonly ConcurrentDictionary<Guid?, IList<IAsyncDisposable>> asyncTrackedResources;

		#endregion

		#region Properties/Indexers/Events

		private ConcurrentDictionary<Guid?, IList<IAsyncDisposable>> AsyncTrackedResources
		{
			get
			{
				return this.asyncTrackedResources;
			}
		}

		#endregion

		#region Methods/Operators

		public async ValueTask CheckAsync([CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
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

			foreach (KeyValuePair<Guid?, IList<IAsyncDisposable>> trackedResource in this.AsyncTrackedResources)
			{
				if ((object)trackedResource.Key == null ||
					(object)trackedResource.Value == null)
					throw new ResourceException(string.Format(""));

				Guid? slotId = trackedResource.Key;
				IList<IAsyncDisposable> asyncDisposables = trackedResource.Value;
				int size = asyncDisposables.Count;
				int ezis = 0;

				foreach (IAsyncDisposable asyncDisposable in asyncDisposables)
				{
					if (asyncDisposable.GetSafeIsAsyncDisposed() ?? false)
					{
						ezis--;
						continue;
					}

					sb.Append(Environment.NewLine);
					sb.Append(string.Format("\t\t{0}", this.FormatObjectInfo(asyncDisposable)));
				}

				int offset = size + ezis;

				if (offset > 0)
				{
					sb.Append(Environment.NewLine);
					sb.Append(string.Format("\t{0} ~ {1}#", this.FormatGuid(slotId), offset));

					slots++;
					objs += offset;
				}
			}

			if (sb.Length > 0)
				sb.Append(Environment.NewLine);

			await this.PrintAsync(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), string.Format("check_async!{0}", sb.ToString()));

			if (slots > 0)
			{
				string message = string.Format("Resource manager tracked resource check FAILED with {0} slots having {1} leaked AsyncDisposable objects", slots, objs);
				await this.PrintAsync(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), message);
			}
		}

		protected override ValueTask CoreCreateAsync(bool creating)
		{
			// do nothing
			return default;
		}

		public async ValueTask DisposeAsync(Guid? slotId, IAsyncDisposable asyncDisposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			await this.DisposeAsync(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, asyncDisposable);
		}

		private async ValueTask DisposeAsync(string caller, Guid? slotId, IAsyncDisposable asyncDisposable)
		{
			IList<IAsyncDisposable> asyncDisposables;

			if ((object)caller == null)
				throw new ArgumentNullException(nameof(caller));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)asyncDisposable == null)
				throw new ArgumentNullException(nameof(asyncDisposable));

			if (!this.AsyncTrackedResources.TryGetValue(slotId, out asyncDisposables) || !asyncDisposables.Contains(asyncDisposable))
				throw new ResourceException(string.Format("Leak detector does not contain a record of slot '{0} | {1}'.", this.FormatGuid(slotId), this.FormatObjectInfo(asyncDisposable)));

			asyncDisposables.Remove(asyncDisposable);

			await this.PrintAsync(caller, this.FormatOperation("dispose_async", slotId, asyncDisposable));
		}

		protected override async ValueTask CoreDisposeAsync(bool disposing)
		{
			if (this.IsDisposed)
				return;

			if (disposing)
			{
				await this.ResetAsync();
			}
		}

		public async ValueTask<Guid?> EnterAsync([CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			Guid slotId;

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			slotId = Guid.NewGuid();

			await this.PrintAsync(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), this.FormatOperation("enter_async", slotId));

			return slotId;
		}

		public async ValueTask<TValue> LeaveAsync<TValue>(Guid? slotId, TValue value, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			await this.LeaveAsync(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId);

			return value;
		}

		public async ValueTask LeaveAsync(Guid? slotId, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			await this.LeaveAsync(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId);
		}

		private async ValueTask LeaveAsync(string caller, Guid? slotId)
		{
			if ((object)caller == null)
				throw new ArgumentNullException(nameof(caller));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			await this.PrintAsync(caller, this.FormatOperation("leave_async", slotId));
		}

		public async ValueTask OnlyWhenDebugPrintAsync(string message, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			if ((object)message == null)
				throw new ArgumentNullException(nameof(message));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			await this.PrintAsync(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), message);
		}

		public async ValueTask PrintAsync(string message, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			if ((object)message == null)
				throw new ArgumentNullException(nameof(message));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			await this.PrintAsync(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), message);
		}

		private async ValueTask PrintAsync(string caller, string message)
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

		public async ValueTask ResetAsync()
		{
			this.AsyncTrackedResources.Clear();
			await Task.CompletedTask;
		}

		private async ValueTask SlotAsync(string caller, Guid? slotId, IAsyncDisposable asyncDisposable, string message)
		{
			IList<IAsyncDisposable> asyncDisposables;

			if ((object)caller == null)
				throw new ArgumentNullException(nameof(caller));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)asyncDisposable == null)
				throw new ArgumentNullException(nameof(asyncDisposable));

			if ((object)message == null)
				throw new ArgumentNullException(nameof(message));

			if (this.AsyncTrackedResources.TryGetValue(slotId, out asyncDisposables))
			{
				if (asyncDisposables.Contains(asyncDisposable))
					throw new ResourceException(string.Format("{0}", slotId));
			}
			else
			{
				asyncDisposables = new List<IAsyncDisposable>();

				if (!this.AsyncTrackedResources.TryAdd(slotId, asyncDisposables))
					throw new ResourceException(string.Format("{0}", slotId));
			}

			asyncDisposables.Add(asyncDisposable);

			await this.PrintAsync(caller, message);
		}

		public async ValueTask<IAsyncDisposable> UsingAsync(Guid? slotId, IAsyncDisposable asyncDisposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			IAsyncDisposable ldlProxyDisposable;

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

			ldlProxyDisposable = new UsingBlockProxy(this, slotId, asyncDisposable); // forward call to .AsyncDispose()

			await this.SlotAsync(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, asyncDisposable, this.FormatOperation("using_async", slotId, asyncDisposable));

			return ldlProxyDisposable;
		}

		public async ValueTask WatchingAsync(Guid? slotId, IAsyncDisposable asyncDisposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
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

			await this.SlotAsync(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, asyncDisposable, this.FormatOperation("watching_async", slotId, asyncDisposable));
		}

		public async ValueTask<TAsyncDisposable> WatchingAsync<TAsyncDisposable>(Guid? slotId, TAsyncDisposable asyncDisposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
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

			await this.SlotAsync(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, asyncDisposable, this.FormatOperation("watching_async", slotId, asyncDisposable));

			return asyncDisposable;
		}

		#endregion
	}
}