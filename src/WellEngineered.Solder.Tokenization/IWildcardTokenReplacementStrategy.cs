/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Tokenization
{
	/// <summary>
	/// Represents a token replacement strategy.
	/// </summary>
	public interface IWildcardTokenReplacementStrategy
	{
		#region Methods/Operators

		/// <summary>
		/// Evaluate a token using any parameters specified.
		/// </summary>
		/// <param name="token"> The wildcard token to evaludate. </param>
		/// <param name="parameters"> Should be null for value semantics; or a valid object array for function semantics. </param>
		/// <returns> An approapriate token replacement value. </returns>
		object Evaluate(string token, object[] parameters);

		#endregion
	}
}