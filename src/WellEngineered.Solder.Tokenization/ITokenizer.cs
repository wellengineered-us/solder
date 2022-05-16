/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

namespace WellEngineered.Solder.Tokenization
{
	public partial interface ITokenizer
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets an ordered array of the previous execution of expansion tokens encountered.
		/// </summary>
		string[] OrderedPreviousExpansionTokens
		{
			get;
		}

		/// <summary>
		/// Gets a value indicating if exceptions are thrown for bad token matches.
		/// </summary>
		bool StrictMatching
		{
			get;
		}

		/// <summary>
		/// Gets a dictionary of token replacement strategies.
		/// </summary>
		IDictionary<string, ITokenReplacement> TokenReplacementStrategies
		{
			get;
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Replaces a tokenized input string with replacement values. No wildcard support is assumed.
		/// </summary>
		/// <param name="tokenizedValue"> The input string containing tokenized values. </param>
		/// <returns> A string value with all possible replacements made. </returns>
		string ExpandTokens(string tokenizedValue);

		/// <summary>
		/// Replaces a tokenized input string with replacement values. Wildcard support is optional.
		/// </summary>
		/// <param name="tokenizedValue"> The input string containing tokenized values. </param>
		/// <param name="optionalWildcardTokenReplacement"> An optional wildcard token replacement strategy. </param>
		/// <returns> A string value with all possible replacements made. </returns>
		string ExpandTokens(string tokenizedValue, ITokenReplacement optionalWildcardTokenReplacement);

		#endregion
	}
}