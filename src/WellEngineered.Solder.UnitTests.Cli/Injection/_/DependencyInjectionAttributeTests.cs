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
	public class DependencyInjectionAttributeTests
	{
		#region Constructors/Destructors

		public DependencyInjectionAttributeTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			DependencyInjectionAttribute attribute;
			string expected;

			expected = "selid";
			attribute = new DependencyInjectionAttribute();
			Assert.IsNotNull(attribute);

			attribute.SelectorKey = expected;
			Assert.AreEqual(expected, attribute.SelectorKey);
		}

		#endregion
	}
}