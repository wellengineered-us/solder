﻿/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using WellEngineered.Solder.Extensions;
using WellEngineered.Solder.Utilities;

namespace WellEngineered.Solder.Tokenization
{
	/// <summary>
	/// Replaces a tokenized input string with replacement values.
	/// A token is in the following format: ${token(`arg0`, `arg1`, ...)}
	/// token: a required 'key' into a 'dictionary' of token replacement strategies.
	/// A missing token is considered invalid and no replacement will be made.
	/// An unknown token considered invalid and no replacement will be made.
	/// The minimum length of a token is 1; the maximum length of a token is 1024.
	/// Tokens are case insensative.
	/// An token may be proceded by an optional function call operator with zero or more arguments.
	/// Each function call argument must be enclosed in tick marks e.g. `some value`.
	/// Recursion/nested token expressions is not supported.
	/// </summary>
	public sealed class Tokenizer : ITokenizer
	{
		#region Constructors/Destructors

		public Tokenizer(IDataTypeFascade dataTypeFascade, IReflectionFascade reflectionFascade,
			IDictionary<string, ITokenReplacementStrategy> tokenReplacementStrategies, bool strictMatching)
		{
			if ((object)dataTypeFascade == null)
				throw new ArgumentNullException(nameof(dataTypeFascade));

			if ((object)reflectionFascade == null)
				throw new ArgumentNullException(nameof(reflectionFascade));

			if ((object)tokenReplacementStrategies == null)
				throw new ArgumentNullException(nameof(tokenReplacementStrategies));

			this.dataTypeFascade = dataTypeFascade;
			this.reflectionFascade = reflectionFascade;
			this.tokenReplacementStrategies = tokenReplacementStrategies;
			this.strictMatching = strictMatching;
		}

		#endregion

		#region Fields/Constants

		private const string TOKEN_ID_REGEX = TOKEN_ID_REGEX_UNBOUNDED + @"{0,1023}";
		private const string TOKEN_ID_REGEX_UNBOUNDED = @"[a-zA-Z_\.][a-zA-Z_\.0-9]";
		private const char TOKENIZER_LOGICAL_PROPERTY_PATH_CHAR = '.';

		private const string TOKENIZER_REGEX =
			@"\$ \{" +
			@"(?: [ ]* ( " + TOKEN_ID_REGEX + " ){1,1} )" +
			@"(?: [ ]* \( ( [ ]* (?: ` [^`]* ` [ ]* (?: , [ ]* ` [^`]* ` [ ]* )* ){0,1} ){0,1} \) ){0,1}" +
			@"[ ]* \}";

		private readonly IDataTypeFascade dataTypeFascade;
		private readonly List<string> previousExpansionTokens = new List<string>();
		private readonly IReflectionFascade reflectionFascade;
		private readonly bool strictMatching;
		private readonly IDictionary<string, ITokenReplacementStrategy> tokenReplacementStrategies;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the token ID regular expression.
		/// </summary>
		public static string TokenIdRegEx
		{
			get
			{
				return TOKEN_ID_REGEX;
			}
		}

		/// <summary>
		/// Gets the tokenizer regular expression.
		/// </summary>
		public static string TokenizerRegEx
		{
			get
			{
				return TOKENIZER_REGEX;
			}
		}

		private IDataTypeFascade DataTypeFascade
		{
			get
			{
				return this.dataTypeFascade;
			}
		}

		/// <summary>
		/// Gets an ordered array of the previous execution of expansion tokens encountered.
		/// </summary>
		public string[] OrderedPreviousExpansionTokens
		{
			get
			{
				return this.PreviousExpansionTokens.Distinct().OrderBy(x => x).ToArray();
			}
		}

		private List<string> PreviousExpansionTokens
		{
			get
			{
				return this.previousExpansionTokens;
			}
		}

		private IReflectionFascade ReflectionFascade
		{
			get
			{
				return this.reflectionFascade;
			}
		}

		/// <summary>
		/// Gets a value indicating if exceptions are thrown for bad token matches.
		/// </summary>
		public bool StrictMatching
		{
			get
			{
				return this.strictMatching;
			}
		}

		/// <summary>
		/// Gets a dictionary of token replacement strategies.
		/// </summary>
		public IDictionary<string, ITokenReplacementStrategy> TokenReplacementStrategies
		{
			get
			{
				return this.tokenReplacementStrategies;
			}
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// A private method that obeys the strict matching semantics flag in effect and if enabled, will throw an exception. Otherwise, returns the original unmatched value without alteration.
		/// </summary>
		/// <param name="strictMatching"> A value indicating whether strict matching semantics are in effect. </param>
		/// <param name="originalValue"> The original unmatched value. </param>
		/// <param name="matchPoint"> A short description of where the match failure occured. </param>
		/// <returns> The original value if strict matching semantics are disabled. </returns>
		private static string GetOriginalValueOrThrowExecption(bool strictMatching, string originalValue, string matchPoint)
		{
			if (strictMatching)
				throw new InvalidOperationException(string.Format("Failed to recognize '{0}' due to '{1}' match error; strict matching enabled.", originalValue, matchPoint));
			else
				return originalValue;
		}

		public static bool IsValidTokenId(string token)
		{
			return Regex.IsMatch(token, TokenIdRegEx, RegexOptions.IgnorePatternWhitespace);
		}

		/// <summary>
		/// Replaces a tokenized input string with replacement values. No wildcard support is assumed.
		/// </summary>
		/// <param name="tokenizedValue"> The input string containing tokenized values. </param>
		/// <returns> A string value with all possible replacements made. </returns>
		public string ExpandTokens(string tokenizedValue)
		{
			return this.ExpandTokens(tokenizedValue, null);
		}

		/// <summary>
		/// Replaces a tokenized input string with replacement values. Wildcard support is optional.
		/// </summary>
		/// <param name="tokenizedValue"> The input string containing tokenized values. </param>
		/// <param name="optionalWildcardTokenReplacementStrategy"> An optional wildcard token replacement strategy. </param>
		/// <returns> A string value with all possible replacements made. </returns>
		public string ExpandTokens(string tokenizedValue, IWildcardTokenReplacementStrategy optionalWildcardTokenReplacementStrategy)
		{
			if (this.DataTypeFascade.IsNullOrWhiteSpace(tokenizedValue))
				return tokenizedValue;

			// clean token collection
			this.PreviousExpansionTokens.Clear();

			tokenizedValue = Regex.Replace(tokenizedValue, TokenizerRegEx, (m) => this.ReplacementMatcherEx(m, optionalWildcardTokenReplacementStrategy), RegexOptions.IgnorePatternWhitespace);

			return tokenizedValue;
		}

		/// <summary>
		/// A private method to parse an argument array from a tokenized call list.
		/// </summary>
		/// <param name="call"> The call list from a tokenized call site. </param>
		/// <returns> A string array of call site arguments. </returns>
		private string[] GetArgs(string call)
		{
			string[] args;

			if (this.DataTypeFascade.IsNullOrWhiteSpace((call ?? string.Empty).Trim()))
				return new string[] { };

			// fixup argument list
			call = Regex.Replace(call, "^ [ ]* `", string.Empty, RegexOptions.IgnorePatternWhitespace);
			call = Regex.Replace(call, "` [ ]* $", string.Empty, RegexOptions.IgnorePatternWhitespace);
			call = Regex.Replace(call, "` [ ]* , [ ]* `", "`", RegexOptions.IgnorePatternWhitespace);

			args = call.Split('`');

			// fix-up escaped backtick
			for (int i = 0; i < args.Length; i++)
				args[i] = args[i].Replace("\\'", "`");

			return args;
		}

		/// <summary>
		/// Private method used to match and process tokenized regular expressions.
		/// </summary>
		/// <param name="match"> The regular express match object. </param>
		/// <param name="wildcardTokenReplacementStrategy"> The wildcard token replacement strategy to use in the event a predefined token replacement strategy lookup failed. </param>
		/// <returns> The token-resolved string value. </returns>
		private string ReplacementMatcherEx(Match match, IWildcardTokenReplacementStrategy wildcardTokenReplacementStrategy)
		{
			// ${ .token.token.token_end (`arg0`, ..) }

			string firstToken, rawToken = null;
			string[] tokens;
			string[] argumentList = null;
			object tokenLogicalValue, tokenReplacementValue = null;
			ITokenReplacementStrategy tokenReplacementStrategy;
			bool keyNotFound, tryWildcard;

			rawToken = match.Groups[1].Success ? match.Groups[1].Value : null;

			argumentList = match.Groups[2].Success ? this.GetArgs(match.Groups[2].Value) : null;

			if (this.DataTypeFascade.IsNullOrWhiteSpace(rawToken))
				return GetOriginalValueOrThrowExecption(this.StrictMatching, match.Value, "token missing");

			// break any token paths into token list
			tokens = rawToken.Split(new char[] { TOKENIZER_LOGICAL_PROPERTY_PATH_CHAR }, StringSplitOptions.RemoveEmptyEntries);

			if ((object)tokens == null ||
				tokens.Length <= 0)
				return null;

			firstToken = tokens[0];
			tokens = tokens.Skip(1).ToArray();

			// add to token collection
			this.PreviousExpansionTokens.Add(firstToken);

			keyNotFound = !this.TokenReplacementStrategies.TryGetValue(firstToken, out tokenReplacementStrategy);
			tryWildcard = keyNotFound && (object)wildcardTokenReplacementStrategy != null;

			if (keyNotFound && !tryWildcard)
				return GetOriginalValueOrThrowExecption(this.StrictMatching, match.Value, "token unknown");

			try
			{
				if (!tryWildcard)
					tokenReplacementValue = tokenReplacementStrategy.Evaluate(argumentList);
				else
					tokenReplacementValue = wildcardTokenReplacementStrategy.Evaluate(firstToken, argumentList);
			}
			catch (Exception ex)
			{
				return GetOriginalValueOrThrowExecption(this.StrictMatching, match.Value, string.Format("function exception {{" + Environment.NewLine + "{0}" + Environment.NewLine + "}}", this.ReflectionFascade.GetErrors(ex, 0)));
			}

			if ((object)tokens == null ||
				tokens.Length <= 0)
				return tokenReplacementValue.SafeToString();

			tokenLogicalValue = tokenReplacementValue;
			foreach (string token in tokens)
			{
				// only do logical lookup here
				if (!this.ReflectionFascade.GetLogicalPropertyValue(tokenLogicalValue, token, out tokenLogicalValue))
					return GetOriginalValueOrThrowExecption(this.StrictMatching, match.Value, string.Format("logical property expansion failed {{{0}}}", token));
			}

			return tokenLogicalValue.SafeToString();
		}

		#endregion
	}
}