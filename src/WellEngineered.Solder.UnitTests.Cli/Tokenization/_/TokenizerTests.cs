/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using NMock;
using NMock.Actions;
using NMock.Matchers;

using NUnit.Framework;

using WellEngineered.Solder.Tokenization;
using WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure;

namespace WellEngineered.Solder.UnitTests.Cli.Tokenization._
{
	/// <summary>
	/// Unit tests.
	/// </summary>
	[TestFixture]
	public class TokenizerTests
	{
		#region Constructors/Destructors

		public TokenizerTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			Tokenizer tokenizer;
			MockFactory mockFactory;
			IDictionary<string, ITokenReplacement> mockTokenReplacementStrategies;

			mockFactory = new MockFactory();
			mockTokenReplacementStrategies = mockFactory.CreateInstance<IDictionary<string, ITokenReplacement>>();

			tokenizer = new Tokenizer(mockTokenReplacementStrategies, true);

			Assert.IsNotNull(tokenizer);
			Assert.IsNotNull(tokenizer.TokenReplacementStrategies);
			Assert.IsTrue(tokenizer.StrictMatching);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldExpandTokensLooseMatchingTest()
		{
			Tokenizer tokenizer;
			MockFactory mockFactory;
			IDictionary<string, ITokenReplacement> mockTokenReplacementStrategies;
			ITokenReplacement mockTokenReplacement;

			ITokenReplacement unusedTokenReplacement = null;
			string _unusedString = null;
			string[] _unusedStrings = null;

			string tokenizedValue;
			string expandedValue;
			string expectedValue;

			mockFactory = new MockFactory();
			mockTokenReplacementStrategies = mockFactory.CreateInstance<IDictionary<string, ITokenReplacement>>();
			mockTokenReplacement = mockFactory.CreateInstance<ITokenReplacement>();

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myValueSemanticToken"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(null)).WillReturn("testValue");

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myFunctionSemanticToken0"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(new string[] { })).WillReturn("testValue");

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myFunctionSemanticToken1"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(new string[] { "a", })).WillReturn("testValue");

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myFunctionSemanticToken2"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(new string[] { "a", "b" })).WillReturn("testValue");

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myUnkSemanticToken"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", null), Return.Value(false));

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myErrSemanticToken"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(null)).Will(Throw.Exception(new Exception()));

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("a"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(null)).WillReturn(string.Empty);

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("b"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(null)).WillReturn(string.Empty);

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("c"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(null)).WillReturn(string.Empty);

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("d"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(null)).Will(Throw.Exception(new Exception()));

			tokenizer = new Tokenizer(mockTokenReplacementStrategies, false);

			tokenizedValue = string.Empty;
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = string.Empty;
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...{myNoSemanticToken}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = "...{myNoSemanticToken}...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myValueSemanticToken}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = "...testValue...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myFunctionSemanticToken0()}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = "...testValue...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myFunctionSemanticToken1(`a`)}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = "...testValue...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myFunctionSemanticToken2(`a`,  `b`)}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = "...testValue...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myUnkSemanticToken}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = "...${myUnkSemanticToken}...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myErrSemanticToken}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = "...${myErrSemanticToken}...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${a}...${c}...${b}...${d}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = "............${d}...";
			Assert.AreEqual(expectedValue, expandedValue);

			Assert.IsNotNull(tokenizer.OrderedPreviousExpansionTokens);
			Assert.AreEqual("a,b,c,d", string.Join(",", tokenizer.OrderedPreviousExpansionTokens));

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldExpandTokensStrictMatchingTest()
		{
			Tokenizer tokenizer;
			MockFactory mockFactory;
			IDictionary<string, ITokenReplacement> mockTokenReplacementStrategies;
			ITokenReplacement mockTokenReplacement;

			ITokenReplacement unusedTokenReplacement = null;
			string _unusedString = null;
			string[] _unusedStrings = null;

			string tokenizedValue;
			string expandedValue;
			string expectedValue;
			Exception capturedException;

			mockFactory = new MockFactory();
			mockTokenReplacementStrategies = mockFactory.CreateInstance<IDictionary<string, ITokenReplacement>>();
			mockTokenReplacement = mockFactory.CreateInstance<ITokenReplacement>();

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myValueSemanticToken"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(null)).WillReturn("testValue");

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myFunctionSemanticToken0"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(new string[] { })).WillReturn("testValue");

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myFunctionSemanticToken1"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(new string[] { "a", })).WillReturn("testValue");

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myFunctionSemanticToken2"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(new string[] { "a", "b" })).WillReturn("testValue");

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myUnkSemanticToken"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", null), Return.Value(false));

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myErrSemanticToken"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(null)).Will(Throw.Exception(new Exception()));

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("a"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(null)).WillReturn(string.Empty);

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("b"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(null)).WillReturn(string.Empty);

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("c"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(null)).WillReturn(string.Empty);

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("d"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", mockTokenReplacement), Return.Value(true));
			Expect.On(mockTokenReplacement).One.Method(x => x.Evaluate(_unusedStrings)).With(new EqualMatcher(null)).Will(Throw.Exception(new Exception()));

			tokenizer = new Tokenizer(mockTokenReplacementStrategies, true);

			tokenizedValue = string.Empty;
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = string.Empty;
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...{myNoSemanticToken}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = "...{myNoSemanticToken}...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myValueSemanticToken}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = "...testValue...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myFunctionSemanticToken0()}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = "...testValue...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myFunctionSemanticToken1(`a`)}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = "...testValue...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myFunctionSemanticToken2(`a`,  `b`)}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue);
			expectedValue = "...testValue...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myUnkSemanticToken}...";
			capturedException = Assert.Throws<TokenizationException>(delegate
																	{
																		expandedValue = tokenizer.ExpandTokens(tokenizedValue);
																	});
			Assert.IsNotNull(capturedException);

			tokenizedValue = "...${myErrSemanticToken}...";
			capturedException = Assert.Throws<TokenizationException>(delegate
																	{
																		expandedValue = tokenizer.ExpandTokens(tokenizedValue);
																	});
			Assert.IsNotNull(capturedException);

			tokenizedValue = "...${a}...${c}...${b}...${d}...";
			capturedException = Assert.Throws<TokenizationException>(delegate
																	{
																		expandedValue = tokenizer.ExpandTokens(tokenizedValue);
																	});
			Assert.IsNotNull(capturedException);

			Assert.IsNotNull(tokenizer.OrderedPreviousExpansionTokens);
			Assert.AreEqual("a,b,c,d", string.Join(",", tokenizer.OrderedPreviousExpansionTokens));

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldExpandTokensWildcardLooseMatchingTest()
		{
			Tokenizer tokenizer;
			MockFactory mockFactory;
			IDictionary<string, ITokenReplacement> mockTokenReplacementStrategies;
			ITokenReplacement mockWildcardTokenReplacement;

			ITokenReplacement unusedTokenReplacement = null;
			string _unusedString = null;
			string[] _unusedStrings = null;

			string tokenizedValue;
			string expandedValue;
			string expectedValue;

			mockFactory = new MockFactory();
			mockTokenReplacementStrategies = mockFactory.CreateInstance<IDictionary<string, ITokenReplacement>>();
			mockWildcardTokenReplacement = mockFactory.CreateInstance<ITokenReplacement>();

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myValueSemanticToken"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", null), Return.Value(false));
			Expect.On(mockWildcardTokenReplacement).One.Method(x => x.Evaluate(_unusedString, _unusedStrings)).With(new EqualMatcher("myValueSemanticToken"), new EqualMatcher(null)).WillReturn("testValue");

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myFunctionSemanticToken0"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", null), Return.Value(false));
			Expect.On(mockWildcardTokenReplacement).One.Method(x => x.Evaluate(_unusedString, _unusedStrings)).With(new EqualMatcher("myFunctionSemanticToken0"), new EqualMatcher(new string[] { })).WillReturn("testValue");

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myFunctionSemanticToken1"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", null), Return.Value(false));
			Expect.On(mockWildcardTokenReplacement).One.Method(x => x.Evaluate(_unusedString, _unusedStrings)).With(new EqualMatcher("myFunctionSemanticToken1"), new EqualMatcher(new string[] { "a", })).WillReturn("testValue");

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myFunctionSemanticToken2"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", null), Return.Value(false));
			Expect.On(mockWildcardTokenReplacement).One.Method(x => x.Evaluate(_unusedString, _unusedStrings)).With(new EqualMatcher("myFunctionSemanticToken2"), new EqualMatcher(new string[] { "a", "b" })).WillReturn("testValue");

			//Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myUnkSemanticToken"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", null), Return.Value(false));

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("myErrSemanticToken"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", null), Return.Value(false));
			Expect.On(mockWildcardTokenReplacement).One.Method(x => x.Evaluate(_unusedString, _unusedStrings)).With(new EqualMatcher("myErrSemanticToken"), new EqualMatcher(null)).Will(Throw.Exception(new Exception()));

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("a"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", null), Return.Value(false));
			Expect.On(mockWildcardTokenReplacement).One.Method(x => x.Evaluate(_unusedString, _unusedStrings)).With(new EqualMatcher("a"), new EqualMatcher(null)).WillReturn(string.Empty);

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("b"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", null), Return.Value(false));
			Expect.On(mockWildcardTokenReplacement).One.Method(x => x.Evaluate(_unusedString, _unusedStrings)).With(new EqualMatcher("b"), new EqualMatcher(null)).WillReturn(string.Empty);

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("c"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", null), Return.Value(false));
			Expect.On(mockWildcardTokenReplacement).One.Method(x => x.Evaluate(_unusedString, _unusedStrings)).With(new EqualMatcher("c"), new EqualMatcher(null)).WillReturn(string.Empty);

			Expect.On(mockTokenReplacementStrategies).One.Method(x => x.TryGetValue(_unusedString, out unusedTokenReplacement)).With(new EqualMatcher("d"), new AndMatcher(new ArgumentsMatcher.OutMatcher(), new AlwaysMatcher(true, string.Empty))).Will(new SetNamedParameterAction("value", null), Return.Value(false));
			Expect.On(mockWildcardTokenReplacement).One.Method(x => x.Evaluate(_unusedString, _unusedStrings)).With(new EqualMatcher("d"), new EqualMatcher(null)).Will(Throw.Exception(new Exception()));

			tokenizer = new Tokenizer(mockTokenReplacementStrategies, false);

			tokenizedValue = string.Empty;
			expandedValue = tokenizer.ExpandTokens(tokenizedValue, mockWildcardTokenReplacement);
			expectedValue = string.Empty;
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...{myNoSemanticToken}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue, mockWildcardTokenReplacement);
			expectedValue = "...{myNoSemanticToken}...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myValueSemanticToken}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue, mockWildcardTokenReplacement);
			expectedValue = "...testValue...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myFunctionSemanticToken0()}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue, mockWildcardTokenReplacement);
			expectedValue = "...testValue...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myFunctionSemanticToken1(`a`)}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue, mockWildcardTokenReplacement);
			expectedValue = "...testValue...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myFunctionSemanticToken2(`a`,  `b`)}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue, mockWildcardTokenReplacement);
			expectedValue = "...testValue...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${myErrSemanticToken}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue, mockWildcardTokenReplacement);
			expectedValue = "...${myErrSemanticToken}...";
			Assert.AreEqual(expectedValue, expandedValue);

			tokenizedValue = "...${a}...${c}...${b}...${d}...";
			expandedValue = tokenizer.ExpandTokens(tokenizedValue, mockWildcardTokenReplacement);
			expectedValue = "............${d}...";
			Assert.AreEqual(expectedValue, expandedValue);

			Assert.IsNotNull(tokenizer.OrderedPreviousExpansionTokens);
			Assert.AreEqual("a,b,c,d", string.Join(",", tokenizer.OrderedPreviousExpansionTokens));

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTokenReplStratsCreateTest()
		{
			Tokenizer tokenizer;
			MockFactory mockFactory;
			IDictionary<string, ITokenReplacement> mockTokenReplacementStrategies;

			mockFactory = new MockFactory();
			mockTokenReplacementStrategies = null;

			tokenizer = new Tokenizer(mockTokenReplacementStrategies, true);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldValidateTokenizerRegExTest()
		{
			Match match;

			match = Regex.Match("${myToken(``,``,``,``}", Tokenizer.TokenizerRegEx);
			Assert.IsNotNull(match);
			Assert.IsFalse(match.Success);
		}

		#endregion
	}
}