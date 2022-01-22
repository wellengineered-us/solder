/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Reflection;

namespace WellEngineered.Solder.Primitives
{
	/// <summary>
	/// Provides easy access assembly related attribute data.
	/// </summary>
	public readonly struct AssemblyInformation
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the AssemblyInformationFascade class.
		/// </summary>
		public AssemblyInformation(string assemblyVersion, string company, string configuration, string copyright, string description, string informationalVersion, string moduleName, string nativeFileVersion, string product, string title, string trademark)
		{
			this.assemblyVersion = assemblyVersion;
			this.company = company;
			this.configuration = configuration;
			this.copyright = copyright;
			this.description = description;
			this.informationalVersion = informationalVersion;
			this.moduleName = moduleName;
			this.nativeFileVersion = nativeFileVersion;
			this.product = product;
			this.title = title;
			this.trademark = trademark;
		}

		#endregion

		#region Fields/Constants

		private static AssemblyInformation @default = Assembly.GetEntryAssembly().GetAssemblyInformation();

		private readonly string assemblyVersion;
		private readonly string company;
		private readonly string configuration;
		private readonly string copyright;
		private readonly string description;
		private readonly string informationalVersion;
		private readonly string moduleName;
		private readonly string nativeFileVersion;
		private readonly string product;
		private readonly string title;
		private readonly string trademark;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the (default) entry assembly information.
		/// </summary>
		public static AssemblyInformation Default
		{
			get
			{
				return @default;
			}
		}

		/// <summary>
		/// Gets the assembly version.
		/// </summary>
		public string AssemblyVersion
		{
			get
			{
				return this.assemblyVersion;
			}
		}

		/// <summary>
		/// Gets the assembly company.
		/// </summary>
		public string Company
		{
			get
			{
				return this.company;
			}
		}

		/// <summary>
		/// Gets the assembly configuration.
		/// </summary>
		public string Configuration
		{
			get
			{
				return this.configuration;
			}
		}

		/// <summary>
		/// Gets the assembly copyright.
		/// </summary>
		public string Copyright
		{
			get
			{
				return this.copyright;
			}
		}

		/// <summary>
		/// Gets the assembly description.
		/// </summary>
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		/// <summary>
		/// Gets the assembly informational version.
		/// </summary>
		public string InformationalVersion
		{
			get
			{
				return this.informationalVersion;
			}
		}

		/// <summary>
		/// Gets the assembly module name.
		/// </summary>
		public string ModuleName
		{
			get
			{
				return this.moduleName;
			}
		}

		/// <summary>
		/// Gets the assembly (native) file version.
		/// </summary>
		public string NativeFileVersion
		{
			get
			{
				return this.nativeFileVersion;
			}
		}

		/// <summary>
		/// Gets the assembly product.
		/// </summary>
		public string Product
		{
			get
			{
				return this.product;
			}
		}

		/// <summary>
		/// Gets the assembly title.
		/// </summary>
		public string Title
		{
			get
			{
				return this.title;
			}
		}

		/// <summary>
		/// Gets the assembly trademark.
		/// </summary>
		public string Trademark
		{
			get
			{
				return this.trademark;
			}
		}

		#endregion
	}
}