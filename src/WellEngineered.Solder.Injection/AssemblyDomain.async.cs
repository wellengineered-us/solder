/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Nito.AsyncEx;

using WellEngineered.Solder.Injection.Resolutions;
using WellEngineered.Solder.Primitives;
using WellEngineered.Solder.Utilities.AppSettings;

using AsyncDependencyMagicMethod = System.Func<WellEngineered.Solder.Injection.IDependencyManager, System.Threading.CancellationToken, System.Threading.Tasks.ValueTask>;

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

		private static async ValueTask AddTrustedDependenciesAsync(IDependencyManager dependencyManager, CancellationToken cancellationToken = default)
		{
			IAppSettingsFacade appSettingsFacade;
			AssemblyInformation assemblyInformation;

			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			appSettingsFacade = AppSettingsFacade.Default;
			assemblyInformation = AssemblyInformation.Default;

			await dependencyManager.AddResolutionAsync<IAppSettingsFacade>(string.Empty, false, new SingletonWrapperDependencyResolution<IAppSettingsFacade>(new InstanceDependencyResolution<IAppSettingsFacade>(appSettingsFacade)), cancellationToken);
			await dependencyManager.AddResolutionAsync<AssemblyInformation>(string.Empty, false, new SingletonWrapperDependencyResolution<AssemblyInformation>(new InstanceDependencyResolution<AssemblyInformation>(assemblyInformation)), cancellationToken);
		}

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
					await AddTrustedDependenciesAsync(this.DependencyManager, cancellationToken);

					// hook app domain events
					//this.AppDomain.ProcessExit += this.AppDomain_ProcessExit;
					this.AppDomain.AssemblyLoad += this.AppDomain_AssemblyLoad;

					// probe known assemblies at run-time
					assemblies = this.AppDomain.GetAssemblies();
					await this.ScanAssembliesAsync(assemblies, cancellationToken);

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

		private async ValueTask FireAsyncDependencyMagicMethods(Type assemblyType, CancellationToken cancellationToken = default)
		{
			MethodInfo[] methodInfos;
			AsyncDependencyMagicMethodAttribute asyncDependencyMagicMethodAttribute;
			AsyncDependencyMagicMethod asyncDependencyMagicMethod;

			methodInfos = assemblyType.GetMethods(BindingFlags.Public | BindingFlags.Static);

			if ((object)assemblyType == null)
				throw new ArgumentNullException(nameof(assemblyType));

			if ((object)methodInfos != null)
			{
				foreach (MethodInfo methodInfo in methodInfos)
				{
					asyncDependencyMagicMethodAttribute = methodInfo.GetOneAttribute<AsyncDependencyMagicMethodAttribute>();

					if ((object)asyncDependencyMagicMethodAttribute == null)
						continue;

					if (!methodInfo.IsStatic)
						continue;

					if (!methodInfo.IsPublic)
						continue;

					if (methodInfo.ReturnType != typeof(ValueTask))
						continue;

					if (methodInfo.GetParameters().Count() != 2 ||
						methodInfo.GetParameters()[0].ParameterType != typeof(IDependencyManager) ||
						methodInfo.GetParameters()[1].ParameterType != typeof(CancellationToken))
						continue;

					asyncDependencyMagicMethod = (AsyncDependencyMagicMethod)(methodInfo.CreateDelegate(typeof(AsyncDependencyMagicMethod), null /* static */));

					if ((object)asyncDependencyMagicMethod == null)
						continue;

					// notify
					await this.ResourceManager.PrintAsync(Guid.Empty, string.Format("Firing dependency magic method: '{1}::{0}'", methodInfo.Name, methodInfo.DeclaringType.FullName), cancellationToken);

					// execute (assuming under existing SRWL)
					await asyncDependencyMagicMethod(this.DependencyManager, cancellationToken);
				}
			}
		}

		/// <summary>
		/// Private method that will scan all assemblies specified to perform dependency magic.
		/// </summary>
		/// <param name="assemblies"> An enumerable of assemblies to scan for dependency magic methods. </param>
		private async ValueTask ScanAssembliesAsync(IEnumerable<Assembly> assemblies, CancellationToken cancellationToken = default)
		{
			if ((object)assemblies != null)
			{
				foreach (Assembly assembly in assemblies)
					await this.ScanAssemblyAsync(assembly, cancellationToken);
			}
		}

		private async ValueTask ScanAssemblyAsync(Assembly assembly, CancellationToken cancellationToken = default)
		{
			AssemblyName assemblyName;

			Type[] assemblyTypes;

			if ((object)assembly == null)
				throw new ArgumentNullException(nameof(assembly));

			if (assembly.IsDynamic)
				return;

			assemblyName = assembly.GetName();

			if (this.KnownAssemblies.ContainsKey(assemblyName))
				return;

			await this.ResourceManager.PrintAsync(Guid.Empty, string.Format("Scanning assembly: '{0}'", assembly.FullName), cancellationToken);

			// track which ones we have seen - not sure if AN is fully ==...
			this.KnownAssemblies.Add(assemblyName, assembly);

			try
			{
				assemblyTypes = assembly.ExportedTypes.ToArray();
			}
			catch (ReflectionTypeLoadException rtlex)
			{
				// is this even needed?
				assemblyTypes = rtlex.Types.Where(t => (object)t != null).ToArray();
			}

			if ((object)assemblyTypes != null)
			{
				foreach (Type assemblyType in assemblyTypes)
				{
					// TODO: in the future we could support real auto-wire via type probing
					await this.FireAsyncDependencyMagicMethods(assemblyType, cancellationToken);
				}
			}
		}

		#endregion
	}
}
#endif