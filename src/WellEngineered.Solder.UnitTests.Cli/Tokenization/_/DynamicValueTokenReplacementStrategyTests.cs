/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NUnit.Framework;

using WellEngineered.Solder.Tokenization;
using WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure;
using WellEngineered.Solder.Utilities;

namespace WellEngineered.Solder.UnitTests.Cli.Tokenization._
{
	/// <summary>
	/// Unit tests.
	/// </summary>
	[TestFixture]
	public class DynamicValueTokenReplacementStrategyTests
	{
		#region Constructors/Destructors

		public DynamicValueTokenReplacementStrategyTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateAndEvaluateTest()
		{
			DynamicValueTokenReplacementStrategy tokenReplacementStrategy;
			Func<string[], object> value;
			object result;

			value = p => int.Parse(p[0]) + 1;

			// TODO - MOCK THIS
			IDataTypeFascade mockDataTypeFascade = new DataTypeFascade();
			IReflectionFascade mockReflectionFascade = new ReflectionFascade(mockDataTypeFascade);

			tokenReplacementStrategy = new DynamicValueTokenReplacementStrategy(mockDataTypeFascade, mockReflectionFascade, value);

			Assert.IsNotNull(tokenReplacementStrategy);
			Assert.IsNotNull(tokenReplacementStrategy.Method);

			result = tokenReplacementStrategy.Evaluate(new string[] { "10" });

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullValueCreateTest()
		{
			DynamicValueTokenReplacementStrategy tokenReplacementStrategy;
			Func<string[], object> value;

			value = null;

			// TODO - MOCK THIS
			IDataTypeFascade mockDataTypeFascade = new DataTypeFascade();
			IReflectionFascade mockReflectionFascade = new ReflectionFascade(mockDataTypeFascade);

			tokenReplacementStrategy = new DynamicValueTokenReplacementStrategy(mockDataTypeFascade, mockReflectionFascade, value);
		}

		#endregion
	}
}