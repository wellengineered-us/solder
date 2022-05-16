/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Tokenization
{
	/// <summary>
	/// Methods used to format a name (symbol) in US English.
	/// </summary>
	public interface ISymbolNaming
	{
		#region Methods/Operators

		/// <summary>
		/// Gets the camel (e.g. 'myVariableName') form of a name. This method strips underscores.
		/// </summary>
		/// <param name="value"> The value to which to get the camel case form. </param>
		/// <returns> The camel case, valid C# identifier form of the specified value. </returns>
		string GetCamelCase(string value);

		/// <summary>
		/// Gets the contant (e.g. 'MY_VARIABLE_NAME') form of a name. This method adds underscores at case change boundaries.
		/// </summary>
		/// <param name="value"> The value to which to get the constant case form. </param>
		/// <returns> The constant case, valid C# identifier form of the specified value. </returns>
		string GetConstantCase(string value);

		/// <summary>
		/// Gets the Pascal (e.g. 'MyVariableName') form of a name. This method strips underscores.
		/// </summary>
		/// <param name="value"> The value to which to get the Pascal case form. </param>
		/// <returns> The Pascal case, valid C# identifier form of the specified value. </returns>
		string GetPascalCase(string value);

		/// <summary>
		/// Gets the plural (e.g. 'myVariableNames') form of a name. This method uses basic stemming.
		/// </summary>
		/// <param name="value"> The value to which to get the plural form. </param>
		/// <returns> The plural, valid C# identifier form of the specified value. </returns>
		string GetPluralForm(string value);

		/// <summary>
		/// Gets the singular (e.g. 'myVariableName') form of a name. This method uses basic stemming.
		/// </summary>
		/// <param name="value"> The value to which to get the singular form. </param>
		/// <returns> The singular, valid C# identifier form of the specified value. </returns>
		string GetSingularForm(string value);

		#endregion
	}
}