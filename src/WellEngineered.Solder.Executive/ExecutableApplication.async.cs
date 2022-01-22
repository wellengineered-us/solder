/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Executive
{
	public abstract partial class ExecutableApplication
	{
		#region Methods/Operators

		public static async ValueTask<int> ResolveRunAsync<TExecutableApplication>(string[] args)
			where TExecutableApplication : IExecutableApplication
		{
			await using (AssemblyDomain assemblyDomain = AssemblyDomain.Default)
			{
				await using (TExecutableApplication executableApplication = assemblyDomain
								.DependencyManager
								.ResolveDependency<TExecutableApplication>(string.Empty, true))
				{
					await executableApplication.CreateAsync();
					return await executableApplication.RunAsync(args);
				}
			}
		}

		protected override async ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			if (creating)
			{
				this.RegisterShutdownHandlers();
				await Task.CompletedTask;
			}
		}

		protected override async ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			if (disposing)
			{
				this.UnregisterShutdownHandlers();
				await Task.CompletedTask;
			}
		}

		protected abstract ValueTask<int> CoreRunAsync(IDictionary<string, IList<object>> arguments, CancellationToken cancellationToken = default);

		protected abstract ValueTask CoreSignalAsync(Signal signal, CancellationToken cancellationToken = default);

		protected virtual async ValueTask CoreUxDisplayArgumentErrorMessageAsync(IEnumerable<Message> argumentMessages, CancellationToken cancellationToken = default)
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

		protected virtual async ValueTask CoreUxDisplayArgumentMapMessageAsync(IDictionary<string, ArgumentSpec> argumentMap, CancellationToken cancellationToken = default)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Magenta;

			IEnumerable<string> requiredArgumentTokens = argumentMap.Select(m => (!m.Value.Required ? "[" : string.Empty) + string.Format("-{0}:value{1}", m.Key, !m.Value.Bounded ? "(s)" : string.Empty) + (!m.Value.Required ? "]" : string.Empty));

			if ((object)requiredArgumentTokens != null)
			{
				await Console.Out.WriteLineAsync();
				// HACK
				await Console.Out.WriteLineAsync(string.Format(string.Format("USAGE: {0} ", this.AssemblyInformation.ModuleName) + string.Join((string)" ", (IEnumerable<string>)requiredArgumentTokens)));
			}

			Console.ForegroundColor = oldConsoleColor;
		}

		protected virtual async ValueTask CoreUxDisplayBannerMessageAsync(CancellationToken cancellationToken = default)
		{
			await Console.Out.WriteLineAsync(string.Format(string.Format("{0} v{1} ({2}; {3})", this.AssemblyInformation.ModuleName,
				this.AssemblyInformation.NativeFileVersion, this.AssemblyInformation.AssemblyVersion,
				this.AssemblyInformation.InformationalVersion)));
		}

		protected virtual async ValueTask CoreUxDisplayFailureMessageAsync(Exception exception, CancellationToken cancellationToken = default)
		{
			ConsoleColor oldConsoleColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;

			await Console.Out.WriteLineAsync();
			await Console.Out.WriteLineAsync(string.Format((object)exception != null ? exception.GetErrors(0) : "<unknown>"));

			Console.ForegroundColor = oldConsoleColor;

			await Console.Out.WriteLineAsync();
			await Console.Out.WriteLineAsync(string.Format("The operation failed to complete."));
		}

		protected virtual async ValueTask CoreUxDisplaySuccessMessageAsync(TimeSpan duration, CancellationToken cancellationToken = default)
		{
			await Console.Out.WriteLineAsync();
			await Console.Out.WriteLineAsync(string.Format("Operation completed successfully; duration: '{0}'.", duration));
		}

		public async ValueTask<int> RunAsync(string[] args, CancellationToken cancellationToken = default)
		{
			this.IsAsyncMode = true;
			this.MaybeLaunchDebugger();

			if (this.HookUnhandledExceptionsOnRun)
				return await this.TryRunUxAsync(args, cancellationToken);
			else
				return await this.RunUxAsync(args, cancellationToken);
		}

		private async ValueTask<int> RunUxAsync(string[] args, CancellationToken cancellationToken = default)
		{
			int returnCode;
			DateTime start, end;
			TimeSpan duration;

			IDictionary<string, ArgumentSpec> argumentMap;
			IList<Message> argumentValidationMessages;
			IDictionary<string, IList<object>> finalArguments;

			await this.CoreUxDisplayBannerMessageAsync(cancellationToken);
			start = DateTime.UtcNow;

			argumentValidationMessages = new List<Message>();
			argumentMap = this.CoreGetArgumentMap();
			finalArguments = this.CoreParseArguments(argumentMap, args, ref argumentValidationMessages);

			if (argumentValidationMessages.Any())
			{
				await this.CoreUxDisplayArgumentErrorMessageAsync(argumentValidationMessages, cancellationToken);
				await this.CoreUxDisplayArgumentMapMessageAsync(argumentMap, cancellationToken);
				returnCode = -1;
			}
			else
				returnCode = await this.CoreRunAsync(finalArguments, cancellationToken);

			end = DateTime.UtcNow;
			duration = end - start;

			await this.CoreUxDisplaySuccessMessageAsync(duration, cancellationToken);

			return returnCode;
		}

		private async ValueTask<int> TryRunUxAsync(string[] args, CancellationToken cancellationToken = default)
		{
			try
			{
				return await this.RunUxAsync(args, cancellationToken);
			}
			catch (Exception ex)
			{
				await this.CoreUxDisplayFailureMessageAsync(ex, cancellationToken);
				return -1;
			}
		}

		#endregion
	}
}
#endif