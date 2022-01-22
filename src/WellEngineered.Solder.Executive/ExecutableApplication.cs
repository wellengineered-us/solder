/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Primitives;
using WellEngineered.Solder.Utilities.AppSettings;

/* SEE:

https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Hosting/docs/HostShutdown.md

--

A) The ExecutableApplication.Run()/RunAsync() exits normally; therefore, Program.Main()/MainAsync() returns as expected. **

B) An unhandled exception occurs. [AppDomain.CurrentDomain.UnhandledException] **

C) The app is forcefully shut down, e.g. SIGKILL (i.e. CTRL+Z on Unix only) **

D) Signals:
	SIGINT (i.e. CTRL+C om Windows and Unix) [Console.CancelKeyPress]
	SIGQUIT (i.e. CTRL+BREAK on Windows, CTRL+\ on Unix) [Console.CancelKeyPress]
	SIGTERM (sent by other apps, e.g. docker stop) [AppDomain.CurrentDomain.ProcessExit]

E) The ExecutableApplication calls Environment.Exit(); Program.Main()/MainAsync() do not return. [AppDomain.CurrentDomain.ProcessExit]

--

A, B, and C are not handled by ExecutableApplication; these are implementation-specific.

E should be avoided as in pre-.NET Core 6.0 which will support POSIX signals,
	ExecutableApplication blocks ProcessExit waiting for the ExecutableApplication to shutdown.
	This can lead to deadlocks due to Environment.Exit() also blocking waiting for ProcessExit to return.
	Additionally, since the SIGTERM handling was attempting to gracefully shut down the process,
	ExecutableApplication would set the ExitCode to 0, which overrode the exit code passed to Environment.Exit().

*/

namespace WellEngineered.Solder.Executive
{
	public abstract partial class ExecutableApplication
		: DualLifecycle,
			IExecutableApplication
	{
		#region Constructors/Destructors

		protected ExecutableApplication(IAppSettingsFacade appSettingsFacade,
			AssemblyInformation assemblyInformation)
		{
			if ((object)appSettingsFacade == null)
				throw new ArgumentNullException(nameof(appSettingsFacade));

			if ((object)assemblyInformation == null)
				throw new ArgumentNullException(nameof(assemblyInformation));

			this.appSettingsFacade = appSettingsFacade;
			this.assemblyInformation = assemblyInformation;
		}

		#endregion

		#region Fields/Constants

		private const string APPCONFIG_ARGS_REGEX = @"-(" + APPCONFIG_ID_REGEX_UNBOUNDED + @"{0,63}):(.{0,})";
		private const string APPCONFIG_ID_REGEX_UNBOUNDED = @"[a-zA-Z_\.][a-zA-Z_\.0-9]";
		private const string APPCONFIG_PROPS_REGEX = @"(" + APPCONFIG_ID_REGEX_UNBOUNDED + @"{0,63})=(.{0,})";
		private const string SOLDER_HOOK_UNHANDLED_EXCEPTIONS = "SOLDER_HOOK_UNHANDLED_EXCEPTIONS";
		private const string SOLDER_LAUNCH_DEBUGGER_ON_ENTRY_POINT = "SOLDER_LAUNCH_DEBUGGER_ON_ENTRY_POINT";

		private readonly IAppSettingsFacade appSettingsFacade;
		private readonly AssemblyInformation assemblyInformation;
		private readonly ManualResetEvent processExitBlock = new ManualResetEvent(false);
		private bool isAsyncMode;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the regular expression pattern for arguments.
		/// </summary>
		public static string ArgsRegEx
		{
			get
			{
				return APPCONFIG_ARGS_REGEX;
			}
		}

		/// <summary>
		/// Gets the regular expression pattern for properties.
		/// </summary>
		public static string PropsRegEx
		{
			get
			{
				return APPCONFIG_PROPS_REGEX;
			}
		}

		protected IAppSettingsFacade AppSettingsFacade
		{
			get
			{
				return this.appSettingsFacade;
			}
		}

		protected AssemblyInformation AssemblyInformation
		{
			get
			{
				return this.assemblyInformation;
			}
		}

		protected bool HookUnhandledExceptionsOnRun
		{
			get
			{
				string svalue;
				bool ovalue;

				svalue = Environment.GetEnvironmentVariable(SOLDER_HOOK_UNHANDLED_EXCEPTIONS);

				if ((object)svalue == null)
					return false;

				if (!svalue.TryParse<bool>(out ovalue))
					return false;

				return !Debugger.IsAttached && ovalue;
			}
		}

		protected bool LaunchDebuggerOnRun
		{
			get
			{
				string svalue;
				bool ovalue;

				svalue = Environment.GetEnvironmentVariable(SOLDER_LAUNCH_DEBUGGER_ON_ENTRY_POINT);

				if ((object)svalue == null)
					return false;

				if (!svalue.TryParse<bool>(out ovalue))
					return false;

				return !Debugger.IsAttached && ovalue;
			}
		}

		private ManualResetEvent ProcessExitBlock
		{
			get
			{
				return this.processExitBlock;
			}
		}

		protected abstract TimeSpan ProcessExitDelayTimeSpan
		{
			get;
		}

		protected bool IsAsyncMode
		{
			get
			{
				return this.isAsyncMode;
			}
			private set
			{
				this.isAsyncMode = value;
			}
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Given a string array of command line arguments, this method will parse the arguments using a well know pattern match to obtain a loosely typed dictionary of key/multi-value pairs for use by applications.
		/// </summary>
		/// <param name="args"> The command line argument array to parse. </param>
		/// <returns> A loosely typed dictionary of key/multi-value pairs. </returns>
		protected static IDictionary<string, IList<string>> ParseCommandLineArguments(string[] args)
		{
			IDictionary<string, IList<string>> arguments;
			Match match;
			string key, value;
			IList<string> argumentValues;

			if ((object)args == null)
				throw new ArgumentNullException(nameof(args));

			arguments = new Dictionary<string, IList<string>>(StringComparer.CurrentCultureIgnoreCase);

			foreach (string arg in args)
			{
				match = Regex.Match(arg, ArgsRegEx, RegexOptions.IgnorePatternWhitespace);

				if ((object)match == null)
					continue;

				if (!match.Success)
					continue;

				if (match.Groups.Count != 3)
					continue;

				key = match.Groups[1].Value;
				value = match.Groups[2].Value;

				// key is required
				if (string.IsNullOrWhiteSpace(key))
					continue;

				// val is required
				if (string.IsNullOrWhiteSpace(value))
					continue;

				if (!arguments.ContainsKey(key))
					arguments.Add(key, new List<string>());

				argumentValues = arguments[key];

				// duplicate values are ignored
				if (argumentValues.Contains(value))
					continue;

				argumentValues.Add(value);
			}

			return arguments;
		}

		public static int ResolveRun<TExecutableApplication>(string[] args)
			where TExecutableApplication : IExecutableApplication
		{
			using (AssemblyDomain assemblyDomain = AssemblyDomain.Default)
			{
				using (TExecutableApplication executableApplication = assemblyDomain
							.DependencyManager
							.ResolveDependency<TExecutableApplication>(string.Empty, true))
				{
					executableApplication.Create();
					return executableApplication.Run(args);
				}
			}
		}

		/// <summary>
		/// Given a string property, this method will parse the property using a well know pattern match to obtain an output key/value pair for use by applications.
		/// </summary>
		/// <param name="arg"> The property to parse. </param>
		/// <param name="key"> The output property key. </param>
		/// <param name="value"> The output property value. </param>
		/// <returns> A value indicating if the parse was successful or not. </returns>
		public static bool TryParseCommandLineArgumentProperty(string arg, out string key, out string value)
		{
			Match match;
			string k, v;

			key = null;
			value = null;

			if ((object)arg == null)
				throw new ArgumentNullException(nameof(arg));

			match = Regex.Match(arg, PropsRegEx, RegexOptions.IgnorePatternWhitespace);

			if ((object)match == null)
				return false;

			if (!match.Success)
				return false;

			if (match.Groups.Count != 3)
				return false;

			k = match.Groups[1].Value;
			v = match.Groups[2].Value;

			// key is required
			if (string.IsNullOrWhiteSpace(k))
				return false;

			// val is required
			if (string.IsNullOrWhiteSpace(v))
				return false;

			key = k;
			value = v;
			return true;
		}

		private void AppDomainProcessExit(object sender, EventArgs e)
		{
			Signal signal;

			if (Environment.ExitCode == 143)
				signal = Signal.DockerStop;
			else
				signal = Signal.EnvExit;

			this.OnSignal(signal);

			if (!this.ProcessExitBlock.WaitOne(this.ProcessExitDelayTimeSpan))
			{
				// waiting for disposals ???
			}

			// wait one more time after the above log message, but only for ProcessExitDelayTimeSpan, so it doesn't hang forever
			this.ProcessExitBlock.WaitOne(this.ProcessExitDelayTimeSpan);

			// on Linux if the shutdown is triggered by SIGTERM then that's signaled with the 143 exit code.
			// suppress that since we shut down gracefully. https://github.com/dotnet/aspnetcore/issues/6526
			Environment.ExitCode = 0;
		}

		private void ConsoleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			Signal signal;

			if ((object)e == null)
				throw new ArgumentNullException(nameof(e));

			e.Cancel = true;

			switch (e.SpecialKey)
			{
				case ConsoleSpecialKey.ControlBreak:
					signal = Signal.ControlBreak;
					break;
				case ConsoleSpecialKey.ControlC:
					signal = Signal.ControlC;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(e.SpecialKey));
			}

			this.OnSignal(signal);

			// don't block in process shutdown for CTRL+C/SIGINT since we can set e.Cancel to true
			// we assume that application code will unwind once __HaltAndCatchFire signals the token
			this.ProcessExitBlock.Set();
		}

		protected override void CoreCreate(bool creating)
		{
			if (this.IsCreated)
				return;

			if (creating)
			{
				this.RegisterShutdownHandlers();
			}
		}

		protected override void CoreDispose(bool disposing)
		{
			if (this.IsDisposed)
				return;

			if (disposing)
			{
				this.UnregisterShutdownHandlers();
			}
		}

		protected abstract IDictionary<string, ArgumentSpec> CoreGetArgumentMap();

		protected virtual IDictionary<string, IList<object>> CoreParseArguments(IDictionary<string, ArgumentSpec> argumentMap, string[] args, ref IList<Message> argumentValidationMessages)
		{
			IList<string> untypedArgumentValues;
			IDictionary<string, IList<string>> untypedArguments;

			IList<object> typedArgumentValues;
			IDictionary<string, IList<object>> typedArguments;
			object typedArgumentValue;

			if ((object)argumentMap == null)
				throw new ArgumentNullException(nameof(argumentMap));

			if ((object)args == null)
				throw new ArgumentNullException(nameof(args));

			if ((object)argumentValidationMessages == null)
				throw new ArgumentNullException(nameof(argumentValidationMessages));

			untypedArguments = ParseCommandLineArguments(args);

			if ((object)untypedArguments == null)
				return null;

			typedArguments = new Dictionary<string, IList<object>>();

			argumentValidationMessages = new List<Message>();

			foreach (string argumentToken in argumentMap.Keys)
			{
				bool argumentExists;
				int argumentValueCount = 0;
				ArgumentSpec argumentSpec;

				if (string.IsNullOrEmpty(argumentToken))
					continue;

				if (argumentExists = untypedArguments.TryGetValue(argumentToken, out untypedArgumentValues))
					argumentValueCount = (object)untypedArgumentValues == null ? -1 : untypedArgumentValues.Count;

				if (!argumentMap.TryGetValue(argumentToken, out argumentSpec))
					continue;

				if ((object)argumentSpec == null)
					continue;

				if (argumentSpec.Required && !argumentExists)
				{
					argumentValidationMessages.Add(new Message(string.Empty, string.Format("A required argument was not specified: '{0}'.", argumentToken), Severity.Error));
					continue;
				}

				if (argumentSpec.Bounded && argumentValueCount > 1)
				{
					argumentValidationMessages.Add(new Message(string.Empty, string.Format("A bounded argument was specified more than once: '{0}'.", argumentToken), Severity.Error));
					continue;
				}

				if ((object)untypedArgumentValues != null)
				{
					typedArgumentValues = new List<object>();

					if ((object)argumentSpec.Type != null)
					{
						foreach (string argumentValue in untypedArgumentValues)
						{
							if (!argumentValue.TryParse(argumentSpec.Type, out typedArgumentValue))
								argumentValidationMessages.Add(new Message(string.Empty, string.Format("An argument '{0}' value '{1}' was specified that failed to parse to the target type '{2}'.", argumentToken, argumentValue, argumentSpec.Type.FullName), Severity.Error));
							else
								typedArgumentValues.Add(typedArgumentValue);
						}
					}
					else
					{
						foreach (string argumentValue in untypedArgumentValues)
							typedArgumentValues.Add(argumentValue);
					}

					typedArguments.Add(argumentToken, typedArgumentValues);
				}
			}

			return typedArguments;
		}

		protected abstract int CoreRun(IDictionary<string, IList<object>> arguments);

		protected abstract void CoreSignal(Signal signal);

		protected virtual void CoreUxDisplayArgumentErrorMessage(IEnumerable<Message> argumentMessages)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;

			if ((object)argumentMessages != null)
			{
				Console.WriteLine();
				foreach (Message argumentMessage in argumentMessages)
					Console.WriteLine(argumentMessage.Description);
			}

			Console.ForegroundColor = oldConsoleColor;
		}

		protected virtual void CoreUxDisplayArgumentMapMessage(IDictionary<string, ArgumentSpec> argumentMap)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Magenta;

			IEnumerable<string> requiredArgumentTokens = argumentMap.Select(m => (!m.Value.Required ? "[" : string.Empty) + string.Format("-{0}:value{1}", m.Key, !m.Value.Bounded ? "(s)" : string.Empty) + (!m.Value.Required ? "]" : string.Empty));

			if ((object)requiredArgumentTokens != null)
			{
				Console.WriteLine();
				// HACK
				Console.WriteLine(string.Format("USAGE: {0} ", this.AssemblyInformation.ModuleName) + string.Join((string)" ", (IEnumerable<string>)requiredArgumentTokens));
			}

			Console.ForegroundColor = oldConsoleColor;
		}

		protected virtual void CoreUxDisplayBannerMessage()
		{
			Console.WriteLine(string.Format("{0} v{1} ({2}; {3})", this.AssemblyInformation.ModuleName,
				this.AssemblyInformation.NativeFileVersion, this.AssemblyInformation.AssemblyVersion, this.AssemblyInformation.InformationalVersion));
		}

		protected virtual void CoreUxDisplayFailureMessage(Exception exception)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine();
			Console.WriteLine((object)exception != null ? exception.GetErrors(0) : "<unknown>");
			Console.ForegroundColor = oldConsoleColor;

			Console.WriteLine();
			Console.WriteLine("The operation failed to complete.");
		}

		protected virtual void CoreUxDisplaySuccessMessage(TimeSpan duration)
		{
			Console.WriteLine();
			Console.WriteLine("Operation completed successfully; duration: '{0}'.", duration);
		}

		protected void MaybeLaunchDebugger()
		{
			if (this.LaunchDebuggerOnRun)
				Debugger.Launch();
		}

		private void OnSignal(Signal signal)
		{
#if ASYNC_ALL_THE_WAY_DOWN
			// the only place where we do this slight of hand...
			if (!this.IsAsyncMode)
				this.CoreSignal(signal);
			else
				this.CoreSignalAsync(signal)
					.GetAwaiter()
					.GetResult();
#else
			this.CoreSignal(signal);
#endif
		}

		private void RegisterShutdownHandlers()
		{
			AppDomain.CurrentDomain.ProcessExit += this.AppDomainProcessExit;
			Console.CancelKeyPress += this.ConsoleCancelKeyPress;
		}

		/// <summary>
		/// The indirect entry point method for this application. Code is wrapped in this method to leverage the 'TryRunUx'/'RunUx' pattern. This method, if used, wraps the RunUx() method in an exception handler. The handler will catch all exceptions and report a full detailed stack trace to the Console.Error stream; -1 is then returned as the exit code. Otherwise, if no exception is thrown, the exit code returned is that which is returned by RunUx().
		/// </summary>
		/// <param name="args"> The command line arguments passed from the executing environment. </param>
		/// <returns> The resulting exit code. </returns>
		public int Run(string[] args)
		{
			this.IsAsyncMode = false;
			this.MaybeLaunchDebugger();

			if (this.HookUnhandledExceptionsOnRun)
				return this.TryRunUx(args);
			else
				return this.RunUx(args);
		}

		private int RunUx(string[] args)
		{
			int returnCode;
			DateTime start, end;
			TimeSpan duration;

			IDictionary<string, ArgumentSpec> argumentMap;
			IList<Message> argumentValidationMessages;
			IDictionary<string, IList<object>> finalArguments;

			this.CoreUxDisplayBannerMessage();
			start = DateTime.UtcNow;

			argumentValidationMessages = new List<Message>();
			argumentMap = this.CoreGetArgumentMap();
			finalArguments = this.CoreParseArguments(argumentMap, args, ref argumentValidationMessages);

			if (argumentValidationMessages.Any())
			{
				this.CoreUxDisplayArgumentErrorMessage(argumentValidationMessages);
				this.CoreUxDisplayArgumentMapMessage(argumentMap);
				returnCode = -1;
			}
			else
				returnCode = this.CoreRun(finalArguments);

			end = DateTime.UtcNow;
			duration = end - start;

			this.CoreUxDisplaySuccessMessage(duration);

			return returnCode;
		}

		private int TryRunUx(string[] args)
		{
			try
			{
				return this.RunUx(args);
			}
			catch (Exception ex)
			{
				this.CoreUxDisplayFailureMessage(ex);
				return -1;
			}
		}

		private void UnregisterShutdownHandlers()
		{
			this.ProcessExitBlock.Set();

			Console.CancelKeyPress -= this.ConsoleCancelKeyPress;
			AppDomain.CurrentDomain.ProcessExit -= this.AppDomainProcessExit;
		}

		#endregion
	}
}