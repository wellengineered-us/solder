/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NUnit.Framework;

using WellEngineered.Solder.Primitives;
using WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure;

namespace WellEngineered.Solder.UnitTests.Cli.Primitives._
{
	[TestFixture]
	public class MessageTests
	{
		#region Constructors/Destructors

		public MessageTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			Message message;
			const string CATEGORY = "myCategory";
			const string DESCRIPTION = "myDescription";
			const Severity SEVERITY = Severity.Information;

			message = new Message(CATEGORY, DESCRIPTION, SEVERITY);

			Assert.AreEqual(CATEGORY, message.Category);
			Assert.AreEqual(DESCRIPTION, message.Description);
			Assert.AreEqual(SEVERITY, message.Severity);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullCategoryCreateTest()
		{
			Message message;
			const string CATEGORY = null;
			const string DESCRIPTION = "myDescription";
			const Severity SEVERITY = Severity.Information;

			message = new Message(CATEGORY, DESCRIPTION, SEVERITY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDescriptionCreateTest()
		{
			Message message;
			const string CATEGORY = "myCategory";
			const string DESCRIPTION = null;
			const Severity SEVERITY = Severity.Information;

			message = new Message(CATEGORY, DESCRIPTION, SEVERITY);
		}

		#endregion
	}
}