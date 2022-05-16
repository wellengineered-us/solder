/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Nito.AsyncEx;

using WellEngineered.Solder.Injection.Resolutions;
using WellEngineered.Solder.Primitives;
using WellEngineered.Solder.Utilities.AppSettings;

using DependencyMagicMethod = System.Action<WellEngineered.Solder.Injection.IDependencyManager>;

namespace WellEngineered.Solder.Injection
{
	/// <summary>
	/// Serves as a logical run-time boundary for assemblies.
	/// </summary>
	public sealed partial class AssemblyDomain : DualLifecycle
	{
		#region Constructors/Destructors

		private AssemblyDomain()
			: this(AppDomain.CurrentDomain)
		{
		}

		private AssemblyDomain(AppDomain appDomain)
		{
			if ((object)appDomain == null)
				throw new ArgumentNullException(nameof(appDomain));

			this.appDomain = appDomain;
		}

		#endregion

		#region Fields/Constants

		private const string APP_CONFIG_FILE_NAME = "appconfig.json";
		private readonly AppDomain appDomain;
		private readonly IDependencyManager dependencyManager = new DependencyManager();
		private readonly IDictionary<AssemblyName, Assembly> knownAssemblies = new Dictionary<AssemblyName, Assembly>(___.ByValueEquality.AssemblyName);
		private readonly AsyncReaderWriterLock readerWriterLockDual = new AsyncReaderWriterLock();
		private readonly IResourceManager resourceManager = new ResourceManager();

		#endregion

		#region Properties/Indexers/Events

		public static AssemblyDomain Default
		{
			get
			{
				AssemblyDomain instance = AssemblyDomainLazySingleton.instance;

				// the only way this is null is if this property is invoked AGAIN while the lazy singleton is being contructed
				if ((object)instance == null)
					Environment.FailFast(string.Format("Not re-entrant; property is invoked AGAIN while the lazy singleton is being contructed: '{0}.{1}'", nameof(AssemblyDomain), nameof(Default)));

				return instance;
			}
		}

		private AppDomain AppDomain
		{
			get
			{
				return this.appDomain;
			}
		}

		/// <summary>
		/// Gets the dependency manager instance associated with the current assembly domain.
		/// </summary>
		public IDependencyManager DependencyManager
		{
			get
			{
				return this.dependencyManager;
			}
		}

		private IDictionary<AssemblyName, Assembly> KnownAssemblies
		{
			get
			{
				return this.knownAssemblies;
			}
		}

		private AsyncReaderWriterLock ReaderWriterLock
		{
			get
			{
				return this.readerWriterLockDual;
			}
		}

		public IResourceManager ResourceManager
		{
			get
			{
				return this.resourceManager;
			}
		}

		#endregion

		#region Methods/Operators

		private static void AddTrustedDependencies(IDependencyManager dependencyManager)
		{
			IAppSettingsFacade appSettingsFacade;
			AssemblyInformation assemblyInformation;

			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			appSettingsFacade = AppSettingsFacade.Default;
			assemblyInformation = AssemblyInformation.Default;

			dependencyManager.AddResolution<IAppSettingsFacade>(string.Empty, false, new SingletonWrapperDependencyResolution<IAppSettingsFacade>(new InstanceDependencyResolution<IAppSettingsFacade>(appSettingsFacade)));
			dependencyManager.AddResolution<AssemblyInformation>(string.Empty, false, new SingletonWrapperDependencyResolution<AssemblyInformation>(new InstanceDependencyResolution<AssemblyInformation>(assemblyInformation)));
		}

		private void AppDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs e)
		{
			if ((object)sender == null)
				throw new ArgumentNullException(nameof(sender));

			if ((object)e == null)
				throw new ArgumentNullException(nameof(e));

			this.OnAssemblyLoaded(e.LoadedAssembly);
		}

		private void AppDomain_ProcessExit(object sender, EventArgs e)
		{
			this.OnProcessExit();
		}

		protected override void CoreCreate(bool creating)
		{
			IEnumerable<Assembly> assemblies;

			if (!creating)
				return;

			this.ResourceManager.Print(Guid.Empty, "Creating assembly domain");

			// cop a writer lock
			using (this.ReaderWriterLock.WriterLock())
			{
				if (this.IsCreated)
					throw new DependencyException(string.Format("This instance has already been initialized."));

				if (this.IsDisposed)
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
					this.ExplicitSetIsCreated();
				}
			}
		}

		protected override void CoreDispose(bool disposing)
		{
			if (!disposing)
				return;

			this.ResourceManager.Print(Guid.Empty, "Disposing assembly domain");

			// cop a writer lock
			using (this.ReaderWriterLock.WriterLock())
			{
				if (this.IsDisposed)
					return;

				// ...
				{
					if ((object)this.DependencyManager != null)
						this.DependencyManager.Dispose();

					if ((object)this.AppDomain != null)
					{
						// unhook app domain events
						//this.AppDomain.AssemblyLoad -= this.AppDomain_AssemblyLoad;
						this.AppDomain.ProcessExit -= this.AppDomain_ProcessExit;
					}

					// special case here since this class will execute under multi-threaded scenarios
					//this.ExplicitSetIsDisposed();
				}
			}
		}

		private void FireDependencyMagicMethods(Type assemblyType)
		{
			MethodInfo[] methodInfos;
			DependencyMagicMethodAttribute dependencyMagicMethodAttribute;
			DependencyMagicMethod dependencyMagicMethod;

			methodInfos = assemblyType.GetMethods(BindingFlags.Public | BindingFlags.Static);

			if ((object)assemblyType == null)
				throw new ArgumentNullException(nameof(assemblyType));

			if ((object)methodInfos != null)
			{
				foreach (MethodInfo methodInfo in methodInfos)
				{
					dependencyMagicMethodAttribute = methodInfo.GetOneAttribute<DependencyMagicMethodAttribute>();

					if ((object)dependencyMagicMethodAttribute == null)
						continue;

					if (!methodInfo.IsStatic)
						continue;

					if (!methodInfo.IsPublic)
						continue;

					if (methodInfo.ReturnType != typeof(void))
						continue;

					if (methodInfo.GetParameters().Count() != 1 ||
						methodInfo.GetParameters()[0].ParameterType != typeof(IDependencyManager))
						continue;

					dependencyMagicMethod = (DependencyMagicMethod)(methodInfo.CreateDelegate(typeof(DependencyMagicMethod), null /* static */));

					if ((object)dependencyMagicMethod == null)
						continue;

					// notify
					this.ResourceManager.Print(Guid.Empty, string.Format("Firing dependency magic method: '{1}::{0}'", methodInfo.Name, methodInfo.DeclaringType.FullName));

					// execute (assuming under existing SRWL)
					dependencyMagicMethod(this.DependencyManager);
				}
			}
		}

		public Assembly LoadAssembly(AssemblyName assemblyName)
		{
			Assembly assembly;

			if ((object)assemblyName == null)
				throw new ArgumentNullException(nameof(assemblyName));

			// cop a reader lock
			using (this.ReaderWriterLock.ReaderLock())
			{
				if (this.IsDisposed)
					throw new ObjectDisposedException(typeof(AssemblyDomain).FullName);

				assembly = this.AppDomain.Load(assemblyName);

				// probe explicit dynamically loaded assmblies
				if ((object)assembly != null)
					this.ScanAssembly(assembly);

				return assembly;
			}
		}

		private void OnAssemblyLoaded(Assembly assembly)
		{
			if ((object)assembly == null)
				throw new ArgumentNullException(nameof(assembly));

			// ...
			{
				if (this.IsDisposed)
					throw new ObjectDisposedException(typeof(AssemblyDomain).FullName);

				// probe implicit loaded assemblies
				if ((object)assembly != null)
					this.ScanAssembly(assembly);
			}
		}

		private void OnProcessExit()
		{
			// simply dispose
			this.Dispose();
		}

		/// <summary>
		/// Private method that will scan all assemblies specified to perform dependency magic.
		/// </summary>
		/// <param name="assemblies"> An enumerable of assemblies to scan for dependency magic methods. </param>
		private void ScanAssemblies(IEnumerable<Assembly> assemblies)
		{
			if ((object)assemblies != null)
			{
				foreach (Assembly assembly in assemblies)
					this.ScanAssembly(assembly);
			}
		}

		private void ScanAssembly(Assembly assembly)
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

			this.ResourceManager.Print(Guid.Empty, string.Format("Scanning assembly: '{0}'", assembly.FullName));

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
					this.FireDependencyMagicMethods(assemblyType);
				}
			}
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		/// <summary>
		/// From: Microsoft.VisualStudio.Composition
		/// https://github.com/Microsoft/vs-mef/tree/master/src/Microsoft.VisualStudio.Composition
		/// Copyright (c) Microsoft Corporation, All rights reserved.
		/// License: MIT License
		/// Commit: 44cf565e2f9799bcf7f33f8af76115134c074324
		/// </summary>
		private static class ___
		{
			#region Classes/Structs/Interfaces/Enums/Delegates

			internal static partial class ByValueEquality
			{
				#region Properties/Indexers/Events

				internal static IEqualityComparer<byte[]> Buffer
				{
					get
					{
						return BufferComparer.Default;
					}
				}

				#endregion

				#region Classes/Structs/Interfaces/Enums/Delegates

				private class BufferComparer : IEqualityComparer<byte[]>
				{
					#region Constructors/Destructors

					private BufferComparer()
					{
					}

					#endregion

					#region Fields/Constants

					internal static readonly BufferComparer Default = new BufferComparer();

					#endregion

					#region Methods/Operators

					public bool Equals(byte[] x, byte[] y)
					{
						if (x == y)
						{
							return true;
						}

						if (x == null ^ y == null)
						{
							return false;
						}

						if (x.Length != y.Length)
						{
							return false;
						}

						for (int i = 0; i < x.Length; i++)
						{
							if (x[i] != y[i])
							{
								return false;
							}
						}

						return true;
					}

					public int GetHashCode(byte[] obj)
					{
						throw new NotImplementedException();
					}

					#endregion
				}

				#endregion
			}

			internal static partial class ByValueEquality
			{
				#region Properties/Indexers/Events

				internal static IEqualityComparer<AssemblyName> AssemblyName
				{
					get
					{
						return AssemblyNameComparer.Default;
					}
				}

				internal static IEqualityComparer<AssemblyName> AssemblyNameNoFastCheck
				{
					get
					{
						return AssemblyNameComparer.NoFastCheck;
					}
				}

				#endregion

				#region Classes/Structs/Interfaces/Enums/Delegates

				private class AssemblyNameComparer : IEqualityComparer<AssemblyName>
				{
					#region Constructors/Destructors

					internal AssemblyNameComparer(bool fastCheck = true)
					{
						this.fastCheck = fastCheck;
					}

					#endregion

					#region Fields/Constants

					internal static readonly AssemblyNameComparer Default = new AssemblyNameComparer();
					internal static readonly AssemblyNameComparer NoFastCheck = new AssemblyNameComparer(fastCheck: false);
					private bool fastCheck;

					#endregion

					#region Methods/Operators

					public bool Equals(AssemblyName x, AssemblyName y)
					{
						if (x == null ^ y == null)
						{
							return false;
						}

						if (x == null)
						{
							return true;
						}

#if NET45 // If fast check is enabled, we can compare the code bases
                if (this.fastCheck && x.CodeBase == y.CodeBase)
                {
                    return true;
                }
#endif

						// There are some cases where two AssemblyNames who are otherwise equivalent
						// have a null PublicKey but a correct PublicKeyToken, and vice versa. We should
						// compare the PublicKeys first, but then fall back to GetPublicKeyToken(), which
						// will generate a public key token for the AssemblyName that has a public key and
						// return the public key token for the other AssemblyName.
						byte[] xPublicKey = x.GetPublicKey();
						byte[] yPublicKey = y.GetPublicKey();

						// Testing on FullName is horrifically slow.
						// So test directly on its components instead.
						if (xPublicKey != null && yPublicKey != null)
						{
							return x.Name == y.Name
									&& x.Version.Equals(y.Version)
									&& string.Equals(x.CultureName, y.CultureName)
									&& Buffer.Equals(xPublicKey, yPublicKey);
						}

						return x.Name == y.Name
								&& x.Version.Equals(y.Version)
								&& string.Equals(x.CultureName, y.CultureName)
								&& Buffer.Equals(x.GetPublicKeyToken(), y.GetPublicKeyToken());
					}

					public int GetHashCode(AssemblyName obj)
					{
						return obj.Name.GetHashCode();
					}

					#endregion
				}

				#endregion
			}

			#endregion
		}

		/// <summary>
		/// http://www.yoda.arachsys.com/csharp/singleton.html
		/// </summary>
		private class AssemblyDomainLazySingleton
		{
			#region Constructors/Destructors

			/// <summary>
			/// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
			/// </summary>
			static AssemblyDomainLazySingleton()
			{
			}

			#endregion

			#region Fields/Constants

			internal static readonly AssemblyDomain instance = new AssemblyDomain();

			#endregion
		}

		#endregion
	}
}