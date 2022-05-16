/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Reflection;

using NMock;

using NUnit.Framework;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.UnitTests.Cli.Primitives._
{
	[TestFixture]
	public class AssemblyInformationTests
	{
		#region Constructors/Destructors

		public AssemblyInformationTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			AssemblyInformation assemblyInformation;
			MockFactory mockFactory;
			Assembly mockAssembly, _unusedAssembly = null;

			const string ASSEMBLY_MODULE_NAME = "ainioinnnn";
			const string ASSEMBLY_VERSION = "sfweff";
			const string ASSEMBLY_TITLE = "sdfgsfdg";
			const string ASSEMBLY_DESCRIPTION = "rthrs";
			const string ASSEMBLY_PRODUCT = "hnfgnhfghn";
			const string ASSEMBLY_COPYRIGHT = "bxcbx";
			const string ASSEMBLY_COMPANY = "swergwerg";
			const string ASSEMBLY_CONFIGURATION = "webxvxn";
			const string ASSEMBLY_FILE_VERSION = "wegwegr";
			const string ASSEMBLY_INFORMATIONAL_VERSION = "dnxnn";
			const string ASSEMBLY_TRADEMARK = "yjtmrum";

			mockFactory = new MockFactory();
			mockAssembly = Assembly.GetEntryAssembly(); // dummy

			assemblyInformation = new AssemblyInformation(ASSEMBLY_VERSION, ASSEMBLY_COMPANY,
				ASSEMBLY_CONFIGURATION, ASSEMBLY_COPYRIGHT, ASSEMBLY_DESCRIPTION,
				ASSEMBLY_INFORMATIONAL_VERSION, ASSEMBLY_MODULE_NAME, ASSEMBLY_FILE_VERSION,
				ASSEMBLY_PRODUCT, ASSEMBLY_TITLE, ASSEMBLY_TRADEMARK);

			Assert.AreEqual(ASSEMBLY_TITLE, assemblyInformation.Title);
			Assert.AreEqual(ASSEMBLY_DESCRIPTION, assemblyInformation.Description);
			Assert.AreEqual(ASSEMBLY_PRODUCT, assemblyInformation.Product);
			Assert.AreEqual(ASSEMBLY_COPYRIGHT, assemblyInformation.Copyright);
			Assert.AreEqual(ASSEMBLY_COMPANY, assemblyInformation.Company);
			Assert.AreEqual(ASSEMBLY_CONFIGURATION, assemblyInformation.Configuration);
			Assert.AreEqual(ASSEMBLY_FILE_VERSION, assemblyInformation.NativeFileVersion);
			Assert.AreEqual(ASSEMBLY_INFORMATIONAL_VERSION, assemblyInformation.InformationalVersion);
			Assert.AreEqual(ASSEMBLY_TRADEMARK, assemblyInformation.Trademark);
			Assert.AreEqual(ASSEMBLY_VERSION, assemblyInformation.AssemblyVersion);
			Assert.AreEqual(ASSEMBLY_MODULE_NAME, assemblyInformation.ModuleName);
		}

		[Test]
		public void ShouldNotFailOnNullAssemblyCreateTest()
		{
			AssemblyInformation assemblyInformation;

			assemblyInformation = new AssemblyInformation(null, null, null, null, null, null, null, null, null, null, null);

			Assert.IsNull(assemblyInformation.Company);
			Assert.IsNull(assemblyInformation.Configuration);
			Assert.IsNull(assemblyInformation.Copyright);
			Assert.IsNull(assemblyInformation.Description);
			Assert.IsNull(assemblyInformation.Product);
			Assert.IsNull(assemblyInformation.Title);
			Assert.IsNull(assemblyInformation.Trademark);
			Assert.IsNull(assemblyInformation.AssemblyVersion);
			Assert.IsNull(assemblyInformation.InformationalVersion);
			Assert.IsNull(assemblyInformation.ModuleName);
			Assert.IsNull(assemblyInformation.NativeFileVersion);
		}

		#endregion
	}
}