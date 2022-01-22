/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Tokenization
{
	/// <summary>
	/// Represents a token replacement strategy pattern.
	/// </summary>
	public partial interface ITokenReplacement
	{
		#region Methods/Operators

		/// <summary>
		/// Evaluate a token using any parameters specified.
		/// </summary>
		/// <param name="parameters"> Should be null for value semantics; or a valid string array for function semantics. </param>
		/// <returns> An appropriate token replacement value. </returns>
		object Evaluate(string[] parameters);

		/// <summary>
		/// Evaluate a token using any parameters specified.
		/// </summary>
		/// <param name="token"> The wildcard token to evaluate. </param>
		/// <param name="parameters"> Should be null for value semantics; or a valid object array for function semantics. </param>
		/// <returns> An appropriate token replacement value. </returns>
		object Evaluate(string token, object[] parameters);

		#endregion
	}
}