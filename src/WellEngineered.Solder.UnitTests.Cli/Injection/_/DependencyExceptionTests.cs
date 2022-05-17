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
	public class DependencyExceptionTests
	{
		#region Constructors/Destructors

		public DependencyExceptionTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			DependencyException exception;
			string expected;

			expected = string.Format("Exception of type '{0}' was thrown.", typeof(DependencyException).FullName);
			exception = new DependencyException();

			Assert.IsNotNull(exception);
			Assert.IsNotNull(exception.Message);
			Assert.IsNull(exception.InnerException);
			Assert.AreEqual(expected, exception.Message);

			expected = "msg";
			exception = new DependencyException(expected);

			Assert.IsNotNull(exception);
			Assert.IsNotNull(exception.Message);
			Assert.IsNull(exception.InnerException);
			Assert.AreEqual(expected, exception.Message);

			expected = "msg";
			exception = new DependencyException(expected, exception);

			Assert.IsNotNull(exception);
			Assert.IsNotNull(exception.Message);
			Assert.IsNotNull(exception.InnerException);
			Assert.AreEqual(expected, exception.Message);
		}

		#endregion
	}
}