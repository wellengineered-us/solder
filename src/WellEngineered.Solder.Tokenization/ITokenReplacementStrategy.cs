/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Tokenization
{
	/// <summary>
	/// Represents a token replacement strategy.
	/// </summary>
	public interface ITokenReplacementStrategy
	{
		#region Methods/Operators

		/// <summary>
		/// Evaluate a token using any parameters specified.
		/// </summary>
		/// <param name="parameters"> Should be null for value semantics; or a valid string array for function semantics. </param>
		/// <returns> An appropriate token replacement value. </returns>
		object Evaluate(string[] parameters);

		#endregion
	}
}