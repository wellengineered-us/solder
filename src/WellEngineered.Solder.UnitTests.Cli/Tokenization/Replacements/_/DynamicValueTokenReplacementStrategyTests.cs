/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NUnit.Framework;

using WellEngineered.Solder.Tokenization.Replacements;
using WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure;

namespace WellEngineered.Solder.UnitTests.Cli.Tokenization.Replacements._
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
			DynamicValueTokenReplacement tokenReplacement;
			Func<string[], object> value;
			object result;

			value = p => int.Parse(p[0]) + 1;

			tokenReplacement = new DynamicValueTokenReplacement(value);

			Assert.IsNotNull(tokenReplacement);
			Assert.IsNotNull(tokenReplacement.Method);

			result = tokenReplacement.Evaluate(new string[] { "10" });

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullValueCreateTest()
		{
			DynamicValueTokenReplacement tokenReplacement;
			Func<string[], object> value;

			value = null;

			tokenReplacement = new DynamicValueTokenReplacement(value);
		}

		#endregion
	}
}