/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	public sealed partial class ResourceManager : Lifecycle, IResourceManager
	{
		#region Constructors/Destructors

		public ResourceManager()
			: this(new ObjectIDGenerator(),
				new ConcurrentDictionary<Guid?, IList<IDisposable>>(),
				new ConcurrentDictionary<Guid?, IList<IAsyncDisposable>>())
		{
		}

		public ResourceManager(ObjectIDGenerator objectIdGenerator,
			ConcurrentDictionary<Guid?, IList<IDisposable>> trackedResources,
			ConcurrentDictionary<Guid?, IList<IAsyncDisposable>> asyncTrackedResources)
		{
			if ((object)objectIdGenerator == null)
				throw new ArgumentNullException(nameof(objectIdGenerator));

			if ((object)trackedResources == null)
				throw new ArgumentNullException(nameof(trackedResources));

			if ((object)asyncTrackedResources == null)
				throw new ArgumentNullException(nameof(asyncTrackedResources));

			this.objectIdGenerator = objectIdGenerator;
			this.trackedResources = trackedResources;
			this.asyncTrackedResources = asyncTrackedResources;
		}

		#endregion

		#region Fields/Constants

		private readonly ObjectIDGenerator objectIdGenerator;
		private readonly ConcurrentDictionary<Guid?, IList<IDisposable>> trackedResources;

		#endregion

		#region Properties/Indexers/Events

		private ObjectIDGenerator ObjectIdGenerator
		{
			get
			{
				return this.objectIdGenerator;
			}
		}

		private ConcurrentDictionary<Guid?, IList<IDisposable>> TrackedResources
		{
			get
			{
				return this.trackedResources;
			}
		}

		#endregion

		#region Methods/Operators

		public void Check([CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
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

			foreach (KeyValuePair<Guid?, IList<IDisposable>> trackedResource in this.TrackedResources)
			{
				if ((object)trackedResource.Key == null ||
					(object)trackedResource.Value == null)
					throw new ResourceException(string.Format(""));

				Guid? slotId = trackedResource.Key;
				IList<IDisposable> disposables = trackedResource.Value;
				int size = disposables.Count;
				int ezis = 0;

				foreach (IDisposable disposable in disposables)
				{
					if (disposable.GetSafeIsDisposed() ?? false)
					{
						ezis--;
						continue;
					}

					sb.Append(Environment.NewLine);
					sb.Append(string.Format("\t\t{0}", this.FormatObjectInfo(disposable)));
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

			this.Print(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), string.Format("check!{0}", sb.ToString()));

			if (slots > 0)
			{
				string message = string.Format("Resource manager tracked resource check FAILED with {0} slots having {1} leaked disposable objects", slots, objs);
				this.Print(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), message);
			}
		}

		protected override void CoreCreate(bool creating)
		{
			// do nothing
		}

		protected override void CoreDispose(bool disposing)
		{
			if (this.IsDisposed)
				return;

			if (disposing)
			{
				this.Reset();
			}
		}

		public void Dispose(Guid? slotId, IDisposable disposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			IList<IDisposable> disposables;

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)disposable == null)
				throw new ArgumentNullException(nameof(disposable));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			if (!this.TrackedResources.TryGetValue(slotId, out disposables) || !disposables.Contains(disposable))
				throw new ResourceException(string.Format("Leak detector does not contain a record of slot '{0} | {1}'.", this.FormatGuid(slotId), this.FormatObjectInfo(disposable)));

			disposables.Remove(disposable);

			this.Print(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), this.FormatOperation("dispose", slotId, disposable));
		}

		public Guid? Enter([CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			Guid slotId;

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			slotId = Guid.NewGuid();

			this.Print(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), this.FormatOperation("enter", slotId));

			return slotId;
		}

		private string FormatCallerInfo(string callerFilePath, int? callerLineNumber, string callerMemberName)
		{
			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			return string.Format("{0}${1}::{2}(),{3}", Path.GetFileName(callerFilePath), callerLineNumber, callerMemberName, this.FormatCurrentThreadId());
		}

		private string FormatCurrentThreadId()
		{
			return string.Format("{0}", Thread.CurrentThread.ManagedThreadId);
		}

		private string FormatGuid(Guid? guid)
		{
			if ((object)guid == null)
				throw new ArgumentNullException(nameof(guid));

			return guid.Value.ToString("N");
		}

		private string FormatObjectInfo(object obj)
		{
			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			return string.Format("{0}@{1}", obj.GetType().FullName, this.ObjectIdGenerator.GetId(obj, out bool firstTime));
		}

		private string FormatOperation(String operation, Guid? slotId, object obj)
		{
			if ((object)operation == null)
				throw new ArgumentNullException(nameof(operation));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			return string.Format("{0} => {1}", this.FormatOperation(operation, slotId), this.FormatObjectInfo(obj));
		}

		private string FormatOperation(String operation, Guid? slotId)
		{
			if ((object)operation == null)
				throw new ArgumentNullException(nameof(operation));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			return string.Format("{0}#{1}", operation, this.FormatGuid(slotId));
		}

		public TValue Leave<TValue>(Guid? slotId, TValue value, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			this.Leave(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId);

			return value;
		}

		public void Leave(Guid? slotId, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			this.Leave(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId);
		}

		private void Leave(string caller, Guid? slotId)
		{
			if ((object)caller == null)
				throw new ArgumentNullException(nameof(caller));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			this.Print(caller, this.FormatOperation("leave", slotId));
		}

		/*public TException NewExceptionWithCallerInfo<TException>(Func<string, TException> factory,
			[CallerFilePath] string callerFilePath = null,
			[CallerLineNumber] int? callerLineNumber = null,
			[CallerMemberName] string callerMemberName = null)
			where TException : Exception, new()
		{
			TException ex;

			if ((object)factory == null)
				throw new ArgumentNullException(nameof(factory));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			ex = factory(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName));

			return ex;
		}*/

		public void Print(string message, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			if ((object)message == null)
				throw new ArgumentNullException(nameof(message));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			this.Print(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), message);
		}

		private void Print(string caller, string message)
		{
			if ((object)caller == null)
				throw new ArgumentNullException(nameof(caller));

			if ((object)message == null)
				throw new ArgumentNullException(nameof(message));

			/* THIS METHOD SHOULD NOT BE DEFINED IN RELEASE/PRODUCTION BUILDS */
			string temp = string.Format("[{0}: {1}]", caller, message);

			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine(temp);
			Console.ForegroundColor = oldConsoleColor;
		}

		public void Reset()
		{
			this.TrackedResources.Clear();
		}

		private void Slot(string caller, Guid? slotId, IDisposable disposable, string message)
		{
			IList<IDisposable> disposables;

			if ((object)caller == null)
				throw new ArgumentNullException(nameof(caller));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)disposable == null)
				throw new ArgumentNullException(nameof(disposable));

			if ((object)message == null)
				throw new ArgumentNullException(nameof(message));

			if (this.TrackedResources.TryGetValue(slotId, out disposables))
			{
				if (disposables.Contains(disposable))
					throw new ResourceException(string.Format("{0}", slotId));
			}
			else
			{
				disposables = new List<IDisposable>();

				if (!this.TrackedResources.TryAdd(slotId, disposables))
					throw new ResourceException(string.Format("{0}", slotId));
			}

			disposables.Add(disposable);

			this.Print(caller, message);
		}

		public IDisposable Using(Guid? slotId, IDisposable disposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			IDisposable ldlProxyDisposable;

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)disposable == null)
				throw new ArgumentNullException(nameof(disposable));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			ldlProxyDisposable = new UsingBlockProxy(this, slotId, disposable); // forward call to .Dispose()

			this.Slot(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, disposable, this.FormatOperation("using", slotId, disposable));

			return ldlProxyDisposable;
		}

		public void Watching(Guid? slotId, IDisposable disposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)disposable == null)
				throw new ArgumentNullException(nameof(disposable));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			this.Slot(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, disposable, this.FormatOperation("watching", slotId, disposable));
		}

		public TDisposable Watching<TDisposable>(Guid? slotId, TDisposable disposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
			where TDisposable : IDisposable
		{
			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)disposable == null)
				throw new ArgumentNullException(nameof(disposable));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			this.Slot(this.FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, disposable, this.FormatOperation("watching", slotId, disposable));

			return disposable;
		}

		#endregion
	}
}