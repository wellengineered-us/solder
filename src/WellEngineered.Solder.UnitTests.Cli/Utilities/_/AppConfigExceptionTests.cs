/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NUnit.Framework;

using WellEngineered.Solder.Utilities;

namespace WellEngineered.Solder.UnitTests.Cli.Utilities._
{
	[TestFixture]
	public class AppConfigExceptionTests
	{
		#region Constructors/Destructors

		public AppConfigExceptionTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			AppConfigException exception;
			string expected;

			expected = string.Format("Exception of type '{0}' was thrown.", typeof(AppConfigException).FullName);
			exception = new AppConfigException();

			Assert.IsNotNull(exception);
			Assert.IsNotNull(exception.Message);
			Assert.IsNull(exception.InnerException);
			Assert.AreEqual(expected, exception.Message);

			expected = "msg";
			exception = new AppConfigException(expected);

			Assert.IsNotNull(exception);
			Assert.IsNotNull(exception.Message);
			Assert.IsNull(exception.InnerException);
			Assert.AreEqual(expected, exception.Message);

			expected = "msg";
			exception = new AppConfigException(expected, exception);

			Assert.IsNotNull(exception);
			Assert.IsNotNull(exception.Message);
			Assert.IsNotNull(exception.InnerException);
			Assert.AreEqual(expected, exception.Message);
		}

		#endregion
	}
}