/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Nito.AsyncEx;

using WellEngineered.Solder.Primitives;

using DependencyMagicMethod = System.Action<WellEngineered.Solder.Injection.IDependencyManager>;

namespace WellEngineered.Solder.Injection
{
	/// <summary>
	/// Serves as a logical run-time boundary for assemblies.
	/// </summary>
	public sealed partial class AssemblyDomain : DualLifecycle
	{
		#region Properties/Indexers/Events

		private AsyncReaderWriterLock AsyncReaderWriterLock
		{
			get
			{
				return this.readerWriterLockDual;
			}
		}

		#endregion

		#region Methods/Operators

		protected override async ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			IEnumerable<Assembly> assemblies;

			if (!creating)
				return;

			await this.ResourceManager.PrintAsync(Guid.Empty, "Creating assembly domain", cancellationToken);

			// cop a writer lock
			using (await this.AsyncReaderWriterLock.WriterLockAsync(cancellationToken))
			{
				if (this.IsAsyncCreated)
					throw new DependencyException(string.Format("This instance has already been initialized."));

				if (this.IsAsyncDisposed)
					throw new ObjectDisposedException(typeof(AssemblyDomain).FullName);

				// ...
				{
					// add trusted dependencies
					AddTrustedDependencies(this.DependencyManager);

					// hook app domain events
					//this.AppDomain.ProcessExit += this.AppDomain_ProcessExit;
					this.AppDomain.AssemblyLoad += this.AppDomain_AssemblyLoad;

					// probe known assemblies at run-time
					assemblies = this.AppDomain.GetAssemblies();
					this.ScanAssemblies(assemblies);

					// special case here since this class wil execute under multi-threaded scenarios
					await this.ExplicitSetIsAsyncCreatedAsync(cancellationToken);
				}
			}
		}

		protected override async ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			if (!disposing)
				return;

			await this.ResourceManager.PrintAsync(Guid.Empty, "Disposing assembly domain", cancellationToken);

			// cop a writer lock
			using (await this.AsyncReaderWriterLock.WriterLockAsync(cancellationToken))
			{
				if (this.IsAsyncDisposed)
					return;

				// ...
				{
					if ((object)this.DependencyManager != null)
						await this.DependencyManager.DisposeAsync();

					if ((object)this.AppDomain != null)
					{
						// unhook app domain events
						//this.AppDomain.AssemblyLoad -= this.AppDomain_AssemblyLoad;
						this.AppDomain.ProcessExit -= this.AppDomain_ProcessExit;
					}
				}
			}
		}

		#endregion
	}
}
#endif