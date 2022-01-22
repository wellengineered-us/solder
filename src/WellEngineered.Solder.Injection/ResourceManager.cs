/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

using WellEngineered.Solder.Primitives;

using static WellEngineered.Solder.Primitives.Telemetry;

namespace WellEngineered.Solder.Injection
{
	public sealed partial class ResourceManager
		: DualLifecycle,
			IResourceManager
	{
		#region Constructors/Destructors

#if ASYNC_ALL_THE_WAY_DOWN

		public ResourceManager()
			: this(new ObjectIDGenerator(),
				new ConcurrentDictionary<Guid?, IList<WeakReference<IDisposable>>>(),
				new ConcurrentDictionary<Guid?, IList<WeakReference<IAsyncDisposable>>>())
		{
		}

		public ResourceManager(ObjectIDGenerator objectIdGenerator,
			ConcurrentDictionary<Guid?, IList<WeakReference<IDisposable>>> trackedResources,
			ConcurrentDictionary<Guid?, IList<WeakReference<IAsyncDisposable>>> asyncTrackedResources)
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

#else
		public ResourceManager()
			: this(new ObjectIDGenerator(),
				new ConcurrentDictionary<Guid?, IList<WeakReference<IDisposable>>>())
		{
		}

		public ResourceManager(ObjectIDGenerator objectIdGenerator,
			ConcurrentDictionary<Guid?, IList<WeakReference<IDisposable>>> trackedResources)
		{
			if ((object)objectIdGenerator == null)
				throw new ArgumentNullException(nameof(objectIdGenerator));

			if ((object)trackedResources == null)
				throw new ArgumentNullException(nameof(trackedResources));

			this.objectIdGenerator = objectIdGenerator;
			this.trackedResources = trackedResources;
		}

#endif

		#endregion

		#region Fields/Constants

		private readonly ObjectIDGenerator objectIdGenerator;
		private readonly ConcurrentDictionary<Guid?, IList<WeakReference<IDisposable>>> trackedResources;

		#endregion

		#region Properties/Indexers/Events

		private ObjectIDGenerator ObjectIdGenerator
		{
			get
			{
				return this.objectIdGenerator;
			}
		}

		private ConcurrentDictionary<Guid?, IList<WeakReference<IDisposable>>> TrackedResources
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

			foreach (KeyValuePair<Guid?, IList<WeakReference<IDisposable>>> trackedResource in this.TrackedResources)
			{
				if ((object)trackedResource.Key == null ||
					(object)trackedResource.Value == null)
					throw NewExceptionWithCallerInfo<ResourceException>((value) => new ResourceException(value));

				Guid? slotId = trackedResource.Key;
				IList<WeakReference<IDisposable>> disposables = trackedResource.Value;
				int size = disposables.Count;
				int ezis = 0;

				foreach (WeakReference<IDisposable> disposable in disposables)
				{
					IDisposableEx disposableEx;

					disposable.TryGetTarget(out IDisposable _disposable);

					if ((object)(disposableEx = _disposable as IDisposableEx) != null &&
						disposableEx.IsDisposed)
					{
						ezis--;
						continue;
					}

					sb.Append(Environment.NewLine);
					sb.Append(string.Format("\t\t{0}", FormatObjectInfo(this.ObjectIdGenerator, disposable)));
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

			this.Print(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), string.Format("check!{0}", sb.ToString()));

			if (slots > 0)
			{
				string message = string.Format("Resource manager tracked resource check FAILED with {0} slots having {1} leaked disposable objects", slots, objs);
				this.Print(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), message);
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
			IList<WeakReference<IDisposable>> disposables;
			WeakReference<IDisposable> _disposable;

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

			_disposable = new WeakReference<IDisposable>(disposable);

			if (!this.TrackedResources.TryGetValue(slotId, out disposables) || !disposables.Contains(_disposable))
				throw new ResourceException(FormatOperation(this.ObjectIdGenerator, "error", slotId, disposable));

			disposables.Remove(_disposable);

			this.Print(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), FormatOperation(this.ObjectIdGenerator, "dispose", slotId, disposable));
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

			this.Print(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), FormatOperation("enter", slotId));

			return slotId;
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

			this.Leave(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId);

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

			this.Leave(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId);
		}

		private void Leave(string caller, Guid? slotId)
		{
			if ((object)caller == null)
				throw new ArgumentNullException(nameof(caller));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			this.Print(caller, FormatOperation("leave", slotId));
		}

		public void Print(Guid? slotId, string message, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
		{
			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)message == null)
				throw new ArgumentNullException(nameof(message));

			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			this.Print(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), string.Format("{0}\t{1}", FormatGuid(slotId), message));
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
#if ASYNC_ALL_THE_WAY_DOWN
			this.AsyncTrackedResources.Clear();
#endif
		}

		private void Slot(string caller, Guid? slotId, IDisposable disposable, string message)
		{
			IList<WeakReference<IDisposable>> disposables;
			WeakReference<IDisposable> _disposable;

			if ((object)caller == null)
				throw new ArgumentNullException(nameof(caller));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)disposable == null)
				throw new ArgumentNullException(nameof(disposable));

			if ((object)message == null)
				throw new ArgumentNullException(nameof(message));

			_disposable = new WeakReference<IDisposable>(disposable);

			if (this.TrackedResources.TryGetValue(slotId, out disposables))
			{
				if (disposables.Contains(_disposable))
					throw new ResourceException(FormatOperation(this.ObjectIdGenerator, "error", slotId, disposable));
			}
			else
			{
				disposables = new List<WeakReference<IDisposable>>();

				if (!this.TrackedResources.TryAdd(slotId, disposables))
					throw new ResourceException(FormatOperation(this.ObjectIdGenerator, "error", slotId, disposable));
			}

			disposables.Add(_disposable);

			this.Print(caller, message);
		}

		public IDisposableDispatch<TDisposable> Using<TDisposable>(Guid? slotId, TDisposable disposable, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int? callerLineNumber = null, [CallerMemberName] string callerMemberName = null)
			where TDisposable : IDisposable
		{
			IDisposable ldlProxyDisposable;
			IDisposableDispatch<TDisposable> disposableDispatch;

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

			disposableDispatch = new DisposableDispatch<TDisposable>(ldlProxyDisposable, disposable);

			this.Slot(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, disposable, FormatOperation(this.ObjectIdGenerator, "using", slotId, disposable));

			return disposableDispatch;
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

			this.Slot(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, disposable, FormatOperation(this.ObjectIdGenerator, "watching", slotId, disposable));
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

			this.Slot(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName), slotId, disposable, FormatOperation(this.ObjectIdGenerator, "watching", slotId, disposable));

			return disposable;
		}

		#endregion
	}
}