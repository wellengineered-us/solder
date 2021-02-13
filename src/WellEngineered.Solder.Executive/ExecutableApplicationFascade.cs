﻿/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Primitives;
using WellEngineered.Solder.Utilities;

namespace WellEngineered.Solder.Executive
{
	public abstract partial class ExecutableApplicationFascade : Lifecycle, IExecutableApplicationFascade
	{
		#region Constructors/Destructors

		protected ExecutableApplicationFascade(IDataTypeFascade dataTypeFascade,
			IAppConfigFascade appConfigFascade,
			IReflectionFascade reflectionFascade,
			IAssemblyInformationFascade assemblyInformationFascade)
		{
			if ((object)dataTypeFascade == null)
				throw new ArgumentNullException(nameof(dataTypeFascade));

			if ((object)appConfigFascade == null)
				throw new ArgumentNullException(nameof(appConfigFascade));

			if ((object)reflectionFascade == null)
				throw new ArgumentNullException(nameof(reflectionFascade));

			if ((object)assemblyInformationFascade == null)
				throw new ArgumentNullException(nameof(assemblyInformationFascade));

			this.dataTypeFascade = dataTypeFascade;
			this.appConfigFascade = appConfigFascade;
			this.reflectionFascade = reflectionFascade;
			this.assemblyInformationFascade = assemblyInformationFascade;
		}

		#endregion

		#region Fields/Constants

		private const string APPCONFIG_ARGS_REGEX = @"-(" + APPCONFIG_ID_REGEX_UNBOUNDED + @"{0,63}):(.{0,})";
		private const string APPCONFIG_ID_REGEX_UNBOUNDED = @"[a-zA-Z_\.][a-zA-Z_\.0-9]";
		private const string APPCONFIG_PROPS_REGEX = @"(" + APPCONFIG_ID_REGEX_UNBOUNDED + @"{0,63})=(.{0,})";
		private const string SOLDER_HOOK_UNHANDLED_EXCEPTIONS = "SOLDER_HOOK_UNHANDLED_EXCEPTIONS";
		private const string SOLDER_LAUNCH_DEBUGGER_ON_ENTRY_POINT = "SOLDER_LAUNCH_DEBUGGER_ON_ENTRY_POINT";

		private readonly IAppConfigFascade appConfigFascade;
		private readonly IAssemblyInformationFascade assemblyInformationFascade;
		private readonly IDataTypeFascade dataTypeFascade;
		private readonly IReflectionFascade reflectionFascade;

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

		protected IAppConfigFascade AppConfigFascade
		{
			get
			{
				return this.appConfigFascade;
			}
		}

		protected IAssemblyInformationFascade AssemblyInformationFascade
		{
			get
			{
				return this.assemblyInformationFascade;
			}
		}

		protected IDataTypeFascade DataTypeFascade
		{
			get
			{
				return this.dataTypeFascade;
			}
		}

		protected bool HookUnhandledExceptions
		{
			get
			{
				string svalue;
				bool ovalue;

				svalue = Environment.GetEnvironmentVariable(SOLDER_HOOK_UNHANDLED_EXCEPTIONS);

				if ((object)svalue == null)
					return false;

				if (!this.DataTypeFascade.TryParse<bool>(svalue, out ovalue))
					return false;

				return !Debugger.IsAttached && ovalue;
			}
		}

		protected bool LaunchDebuggerOnEntryPoint
		{
			get
			{
				string svalue;
				bool ovalue;

				svalue = Environment.GetEnvironmentVariable(SOLDER_LAUNCH_DEBUGGER_ON_ENTRY_POINT);

				if ((object)svalue == null)
					return false;

				if (!this.DataTypeFascade.TryParse<bool>(svalue, out ovalue))
					return false;

				return !Debugger.IsAttached && ovalue;
			}
		}

		protected IReflectionFascade ReflectionFascade
		{
			get
			{
				return this.reflectionFascade;
			}
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// The entry point method for this application.
		/// </summary>
		/// <param name="args"> The command line arguments passed from the executing environment. </param>
		/// <returns> The resulting exit code. </returns>
		public static int Run<TExecutableApplicationFascade>(string[] args)
			where TExecutableApplicationFascade : ExecutableApplicationFascade
		{
			using (TExecutableApplicationFascade program = AssemblyDomain.Default.DependencyManager.ResolveDependency<TExecutableApplicationFascade>(string.Empty, true))
			{
				program.Create();
				return program.EntryPoint(args);
			}
		}

		/// <summary>
		/// The event handler for this event is executed on a thread pool thread.
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="args"> </param>
		private void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs args)
		{
			//if ((object)sender == null)
			//throw new ArgumentNullException(nameof(sender));

			if ((object)args == null)
				throw new ArgumentNullException(nameof(args));

			args.Cancel = this.OnCancelKeySignal(args.SpecialKey);
		}

		protected override void CoreCreate(bool creating)
		{
			if (this.IsCreated)
				return;

			if (creating)
			{
				Console.CancelKeyPress += this.ConsoleOnCancelKeyPress;
			}
		}

		protected override void CoreDispose(bool disposing)
		{
			if (this.IsDisposed)
				return;

			if (disposing)
			{
				//Console.CancelKeyPress -= this.ConsoleOnCancelKeyPress;
			}
		}

		protected virtual void DisplayArgumentErrorMessage(IEnumerable<Message> argumentMessages)
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

		protected virtual void DisplayArgumentMapMessage(IDictionary<string, ArgumentSpec> argumentMap)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Magenta;

			IEnumerable<string> requiredArgumentTokens = argumentMap.Select(m => (!m.Value.Required ? "[" : string.Empty) + string.Format("-{0}:value{1}", m.Key, !m.Value.Bounded ? "(s)" : string.Empty) + (!m.Value.Required ? "]" : string.Empty));

			if ((object)requiredArgumentTokens != null)
			{
				Console.WriteLine();
				// HACK
				Console.WriteLine(string.Format("USAGE: {0} ", this.AssemblyInformationFascade.ModuleName) + string.Join((string)" ", (IEnumerable<string>)requiredArgumentTokens));
			}

			Console.ForegroundColor = oldConsoleColor;
		}

		protected virtual void DisplayBannerMessage()
		{
			Console.WriteLine(string.Format("{0} v{1} ({2}; {3})", this.AssemblyInformationFascade.ModuleName,
				this.AssemblyInformationFascade.NativeFileVersion, this.AssemblyInformationFascade.AssemblyVersion, this.AssemblyInformationFascade.InformationalVersion));
		}

		protected virtual void DisplayFailureMessage(Exception exception)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine();
			Console.WriteLine((object)exception != null ? this.ReflectionFascade.GetErrors(exception, 0) : "<unknown>");
			Console.ForegroundColor = oldConsoleColor;

			Console.WriteLine();
			Console.WriteLine("The operation failed to complete.");
		}

		protected virtual void DisplayRawArgumentsMessage(string[] args, IEnumerable<string> arguments)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Blue;

			if ((object)arguments != null)
			{
				Console.WriteLine();
				Console.WriteLine("RAW CMDLN: {0}", string.Join(" ", args));
				Console.WriteLine();
				Console.WriteLine("RAW ARGS:");

				int index = 0;
				foreach (string argument in arguments)
					Console.WriteLine("[{0}] => {1}", index++, argument);
			}

			Console.ForegroundColor = oldConsoleColor;
		}

		protected virtual void DisplaySuccessMessage(TimeSpan duration)
		{
			Console.WriteLine();
			Console.WriteLine("Operation completed successfully; duration: '{0}'.", duration);
		}

		/// <summary>
		/// The indirect entry point method for this application. Code is wrapped in this method to leverage the 'TryStartup'/'Startup' pattern. This method, if used, wraps the Startup() method in an exception handler. The handler will catch all exceptions and report a full detailed stack trace to the Console.Error stream; -1 is then returned as the exit code. Otherwise, if no exception is thrown, the exit code returned is that which is returned by Startup().
		/// </summary>
		/// <param name="args"> The command line arguments passed from the executing environment. </param>
		/// <returns> The resulting exit code. </returns>
		public int EntryPoint(string[] args)
		{
			this.MaybeLaunchDebugger();

			if (this.HookUnhandledExceptions)
				return this.TryStartup(args);
			else
				return this.Startup(args);
		}

		protected abstract IDictionary<string, ArgumentSpec> GetArgumentMap();

		protected void MaybeLaunchDebugger()
		{
			if (this.LaunchDebuggerOnEntryPoint)
				Debugger.Launch();
		}

		/// <summary>
		/// The event handler for this event is executed on a thread pool thread.
		/// </summary>
		/// <param name="consoleSpecialKey"> </param>
		/// <returns> </returns>
		protected virtual bool OnCancelKeySignal(ConsoleSpecialKey consoleSpecialKey)
		{
			return false;
		}

		protected abstract int OnStartup(string[] args, IDictionary<string, IList<object>> arguments);

		/// <summary>
		/// Given a string array of command line arguments, this method will parse the arguments using a well know pattern match to obtain a loosely typed dictionary of key/multi-value pairs for use by applications.
		/// </summary>
		/// <param name="args"> The command line argument array to parse. </param>
		/// <returns> A loosely typed dictionary of key/multi-value pairs. </returns>
		public IDictionary<string, IList<string>> ParseCommandLineArguments(string[] args)
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
				if (this.DataTypeFascade.IsNullOrWhiteSpace(key))
					continue;

				// val is required
				if (this.DataTypeFascade.IsNullOrWhiteSpace(value))
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

		public int ShowNestedExceptionsAndThrowBrickAtProcess(Exception e)
		{
			this.DisplayFailureMessage(e);

			Environment.FailFast(string.Empty, e);

			return -1;
		}

		private int Startup(string[] args)
		{
			int returnCode;
			DateTime start, end;
			TimeSpan duration;
			IDictionary<string, ArgumentSpec> argumentMap;
			IList<Message> argumentValidationMessages;

			IList<string> argumentValues;
			IDictionary<string, IList<string>> arguments;

			IDictionary<string, IList<object>> finalArguments;
			IList<object> finalArgumentValues;
			object finalArgumentValue;

			this.DisplayBannerMessage();
			start = DateTime.UtcNow;

			arguments = this.ParseCommandLineArguments(args);
			argumentMap = this.GetArgumentMap();

			finalArguments = new Dictionary<string, IList<object>>();
			argumentValidationMessages = new List<Message>();

			if ((object)argumentMap != null)
			{
				foreach (string argumentToken in argumentMap.Keys)
				{
					bool argumentExists;
					int argumentValueCount = 0;
					ArgumentSpec argumentSpec;

					if (argumentExists = arguments.TryGetValue(argumentToken, out argumentValues))
						argumentValueCount = argumentValues.Count;

					if (!argumentMap.TryGetValue(argumentToken, out argumentSpec))
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

					if ((object)argumentValues != null)
					{
						finalArgumentValues = new List<object>();

						if ((object)argumentSpec.Type != null)
						{
							foreach (string argumentValue in argumentValues)
							{
								if (!this.DataTypeFascade.TryParse(argumentSpec.Type, argumentValue, out finalArgumentValue))
									argumentValidationMessages.Add(new Message(string.Empty, string.Format("An argument '{0}' value '{1}' was specified that failed to parse to the target type '{2}'.", argumentToken, argumentValue, argumentSpec.Type.FullName), Severity.Error));
								else
									finalArgumentValues.Add(finalArgumentValue);
							}
						}
						else
						{
							foreach (string argumentValue in argumentValues)
								finalArgumentValues.Add(argumentValue);
						}

						finalArguments.Add(argumentToken, finalArgumentValues);
					}
				}
			}

			if (argumentValidationMessages.Any())
			{
				this.DisplayArgumentErrorMessage(argumentValidationMessages);
				this.DisplayArgumentMapMessage(argumentMap);
				//this.DisplayRawArgumentsMessage(args);
				returnCode = -1;
			}
			else
				returnCode = this.OnStartup(args, finalArguments);

			end = DateTime.UtcNow;
			duration = end - start;

			this.DisplaySuccessMessage(duration);

			return returnCode;
		}

		/// <summary>
		/// Given a string property, this method will parse the property using a well know pattern match to obtain an output key/value pair for use by applications.
		/// </summary>
		/// <param name="arg"> The property to parse. </param>
		/// <param name="key"> The output property key. </param>
		/// <param name="value"> The output property value. </param>
		/// <returns> A value indicating if the parse was successful or not. </returns>
		public bool TryParseCommandLineArgumentProperty(string arg, out string key, out string value)
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
			if (this.DataTypeFascade.IsNullOrWhiteSpace(k))
				return false;

			// val is required
			if (this.DataTypeFascade.IsNullOrWhiteSpace(v))
				return false;

			key = k;
			value = v;
			return true;
		}

		private int TryStartup(string[] args)
		{
			try
			{
				return this.Startup(args);
			}
			catch (Exception ex)
			{
				return this.ShowNestedExceptionsAndThrowBrickAtProcess(new Exception("Main", ex));
			}
		}

		#endregion
	}
}