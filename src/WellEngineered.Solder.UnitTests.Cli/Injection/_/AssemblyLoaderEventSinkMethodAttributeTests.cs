/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NUnit.Framework;

using WellEngineered.Solder.Injection;

namespace WellEngineered.Solder.UnitTests.Cli.Injection._
{
	[TestFixture]
	public class AssemblyLoaderEventSinkMethodAttributeTests
	{
		#region Constructors/Destructors

		public AssemblyLoaderEventSinkMethodAttributeTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			DependencyMagicMethodAttribute attribute;

			attribute = new DependencyMagicMethodAttribute();
			Assert.IsNotNull(attribute);
		}

		#endregion
	}
}