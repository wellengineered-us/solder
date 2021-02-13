/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Executive
{
	public abstract partial class ExecutableApplicationFascade
	{
		#region Methods/Operators

		/// <summary>
		/// The entry point method for this application.
		/// </summary>
		/// <param name="args"> The command line arguments passed from the executing environment. </param>
		/// <returns> The resulting exit code. </returns>
		public static async ValueTask<int> RunAsync<TExecutableApplicationFascade>(string[] args)
			where TExecutableApplicationFascade : ExecutableApplicationFascade
		{
			await using (TExecutableApplicationFascade program = AssemblyDomain.Default.DependencyManager.ResolveDependency<TExecutableApplicationFascade>(string.Empty, true))
			{
				await program.CreateAsync();
				return await program.EntryPointAsync(args);
			}
		}

		protected override async ValueTask CoreCreateAsync(bool creating)
		{
			if (this.IsCreated)
				return;

			if (creating)
			{
				Console.CancelKeyPress += this.ConsoleOnCancelKeyPress;
				await Task.CompletedTask;
			}
		}

		protected override async ValueTask CoreDisposeAsync(bool disposing)
		{
			if (this.IsDisposed)
				return;

			if (disposing)
			{
				await Task.CompletedTask;
				//Console.CancelKeyPress -= this.ConsoleOnCancelKeyPress;
			}
		}

		protected virtual async ValueTask DisplayArgumentErrorMessageAsync(IEnumerable<Message> argumentMessages)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;

			if ((object)argumentMessages != null)
			{
				await Console.Out.WriteLineAsync();
				foreach (Message argumentMessage in argumentMessages)
					await Console.Out.WriteLineAsync(string.Format(argumentMessage.Description));
			}

			Console.ForegroundColor = oldConsoleColor;
		}

		protected virtual async ValueTask DisplayArgumentMapMessageAsync(IDictionary<string, ArgumentSpec> argumentMap)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Magenta;

			IEnumerable<string> requiredArgumentTokens = argumentMap.Select(m => (!m.Value.Required ? "[" : string.Empty) + string.Format("-{0}:value{1}", m.Key, !m.Value.Bounded ? "(s)" : string.Empty) + (!m.Value.Required ? "]" : string.Empty));

			if ((object)requiredArgumentTokens != null)
			{
				await Console.Out.WriteLineAsync();
				// HACK
				await Console.Out.WriteLineAsync(string.Format(string.Format("USAGE: {0} ", this.AssemblyInformationFascade.ModuleName) + string.Join((string)" ", (IEnumerable<string>)requiredArgumentTokens)));
			}

			Console.ForegroundColor = oldConsoleColor;
		}

		protected virtual async ValueTask DisplayBannerMessageAsync()
		{
			await Console.Out.WriteLineAsync(string.Format(string.Format("{0} v{1} ({2}; {3})", this.AssemblyInformationFascade.ModuleName,
				this.AssemblyInformationFascade.NativeFileVersion, this.AssemblyInformationFascade.AssemblyVersion,
				this.AssemblyInformationFascade.InformationalVersion)));
		}

		protected virtual async ValueTask DisplayFailureMessageAsync(Exception exception)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;

			await Console.Out.WriteLineAsync();
			await Console.Out.WriteLineAsync(string.Format((object)exception != null ? this.ReflectionFascade.GetErrors(exception, 0) : "<unknown>"));

			Console.ForegroundColor = oldConsoleColor;

			await Console.Out.WriteLineAsync();
			await Console.Out.WriteLineAsync(string.Format("The operation failed to complete."));
		}

		protected virtual async ValueTask DisplayRawArgumentsMessageAsync(string[] args, IEnumerable<string> arguments)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Blue;

			if ((object)arguments != null)
			{
				await Console.Out.WriteLineAsync();
				await Console.Out.WriteLineAsync(string.Format("RAW CMDLN: {0}", string.Join(" ", args)));
				await Console.Out.WriteLineAsync();
				await Console.Out.WriteLineAsync(string.Format("RAW ARGS:"));

				int index = 0;
				foreach (string argument in arguments)
					await Console.Out.WriteLineAsync(string.Format("[{0}] => {1}", index++, argument));
			}

			Console.ForegroundColor = oldConsoleColor;
		}

		protected virtual async ValueTask DisplaySuccessMessageAsync(TimeSpan duration)
		{
			await Console.Out.WriteLineAsync();
			await Console.Out.WriteLineAsync(string.Format("Operation completed successfully; duration: '{0}'.", duration));
		}

		/// <summary>
		/// The indirect entry point method for this application. Code is wrapped in this method to leverage the 'TryStartup'/'Startup' pattern. This method, if used, wraps the Startup() method in an exception handler. The handler will catch all exceptions and report a full detailed stack trace to the Console.Error stream; -1 is then returned as the exit code. Otherwise, if no exception is thrown, the exit code returned is that which is returned by Startup().
		/// </summary>
		/// <param name="args"> The command line arguments passed from the executing environment. </param>
		/// <returns> The resulting exit code. </returns>
		public async ValueTask<int> EntryPointAsync(string[] args)
		{
			this.MaybeLaunchDebugger();

			if (this.HookUnhandledExceptions)
				return await this.TryStartupAsync(args);
			else
				return await this.StartupAsync(args);
		}

		protected abstract ValueTask<int> OnStartupAsync(string[] args, IDictionary<string, IList<object>> arguments);

		private async ValueTask<int> StartupAsync(string[] args)
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
				await this.DisplayArgumentErrorMessageAsync(argumentValidationMessages);
				await this.DisplayArgumentMapMessageAsync(argumentMap);
				//await this.DisplayRawArgumentsMessageAsync(args);
				returnCode = await Task.FromResult(-1);
			}
			else
				returnCode = await this.OnStartupAsync(args, finalArguments);

			end = DateTime.UtcNow;
			duration = end - start;

			await this.DisplaySuccessMessageAsync(duration);

			return returnCode;
		}

		private async ValueTask<int> TryStartupAsync(string[] args)
		{
			try
			{
				return await this.StartupAsync(args);
			}
			catch (Exception ex)
			{
				return this.ShowNestedExceptionsAndThrowBrickAtProcess(new Exception("Main", ex));
			}
		}

		#endregion
	}
}