/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NUnit.Framework;

using WellEngineered.Solder.Utilities.AppSettings;

namespace WellEngineered.Solder.UnitTests.Cli.Utilities._
{
	[TestFixture]
	public class AppSettingsExceptionTests
	{
		#region Constructors/Destructors

		public AppSettingsExceptionTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			AppSettingsException exception;
			string expected;

			expected = string.Format("Exception of type '{0}' was thrown.", typeof(AppSettingsException).FullName);
			exception = new AppSettingsException();

			Assert.IsNotNull(exception);
			Assert.IsNotNull(exception.Message);
			Assert.IsNull(exception.InnerException);
			Assert.AreEqual(expected, exception.Message);

			expected = "msg";
			exception = new AppSettingsException(expected);

			Assert.IsNotNull(exception);
			Assert.IsNotNull(exception.Message);
			Assert.IsNull(exception.InnerException);
			Assert.AreEqual(expected, exception.Message);

			expected = "msg";
			exception = new AppSettingsException(expected, exception);

			Assert.IsNotNull(exception);
			Assert.IsNotNull(exception.Message);
			Assert.IsNotNull(exception.InnerException);
			Assert.AreEqual(expected, exception.Message);
		}

		#endregion
	}
}