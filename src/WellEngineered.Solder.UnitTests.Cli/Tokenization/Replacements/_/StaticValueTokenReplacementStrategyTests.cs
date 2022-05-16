/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using NUnit.Framework;

using WellEngineered.Solder.Tokenization.Replacements;

namespace WellEngineered.Solder.UnitTests.Cli.Tokenization.Replacements._
{
	/// <summary>
	/// Unit tests.
	/// </summary>
	[TestFixture]
	public class StaticValueTokenReplacementStrategyTests
	{
		#region Constructors/Destructors

		public StaticValueTokenReplacementStrategyTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateAndEvaluateTest()
		{
			StaticValueTokenReplacement tokenReplacement;
			int value;
			object result;

			value = 10;

			tokenReplacement = new StaticValueTokenReplacement(value);

			Assert.IsNotNull(tokenReplacement);
			Assert.IsNotNull(tokenReplacement.Value);

			result = tokenReplacement.Evaluate(new string[] { "10" });

			Assert.IsNotNull(result);
			Assert.AreEqual(10, result);
		}

		#endregion
	}
}