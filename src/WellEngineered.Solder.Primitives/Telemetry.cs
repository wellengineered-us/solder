/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;

namespace WellEngineered.Solder.Primitives
{
	public static class Telemetry
	{
		#region Methods/Operators

		public static string FormatCallerInfo(string callerFilePath, int? callerLineNumber, string callerMemberName)
		{
			if ((object)callerFilePath == null)
				throw new ArgumentNullException(nameof(callerFilePath));

			if ((object)callerLineNumber == null)
				throw new ArgumentNullException(nameof(callerLineNumber));

			if ((object)callerMemberName == null)
				throw new ArgumentNullException(nameof(callerMemberName));

			return string.Format("{0}${1}::{2}(),{3}", Path.GetFileName(callerFilePath), callerLineNumber, callerMemberName, FormatCurrentThreadId());
		}

		public static string FormatCurrentThreadId()
		{
			return string.Format("{0}", Thread.CurrentThread.ManagedThreadId);
		}

		public static string FormatGuid(Guid? guid)
		{
			if ((object)guid == null)
				throw new ArgumentNullException(nameof(guid));

			return guid.Value.ToString("N");
		}

		public static string FormatObjectInfo(ObjectIDGenerator objectIdGenerator, object obj)
		{
			if ((object)objectIdGenerator == null)
				throw new ArgumentNullException(nameof(objectIdGenerator));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			return string.Format("{0}@{1}{2}", obj.GetType().FullName, objectIdGenerator.GetId(obj, out bool firstTime), firstTime ? "*" : "+");
		}

		public static string FormatOperation(ObjectIDGenerator objectIdGenerator, String operation, Guid? slotId, object obj)
		{
			if ((object)objectIdGenerator == null)
				throw new ArgumentNullException(nameof(objectIdGenerator));

			if ((object)operation == null)
				throw new ArgumentNullException(nameof(operation));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			return string.Format("{0} => {1}", FormatOperation(operation, slotId), FormatObjectInfo(objectIdGenerator, obj));
		}

		public static string FormatOperation(String operation, Guid? slotId)
		{
			if ((object)operation == null)
				throw new ArgumentNullException(nameof(operation));

			if ((object)slotId == null)
				throw new ArgumentNullException(nameof(slotId));

			return string.Format("{0}#{1}", operation, FormatGuid(slotId));
		}

		public static TException NewExceptionWithCallerInfo<TException>(Func<string, TException> factory,
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

			ex = factory(FormatCallerInfo(callerFilePath, callerLineNumber, callerMemberName));

			return ex;
		}

		#endregion
	}
}