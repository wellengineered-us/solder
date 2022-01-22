/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Text;

namespace WellEngineered.Solder.Tokenization.Namings
{
	public abstract class SymbolNaming : ISymbolNaming
	{
		#region Constructors/Destructors

		protected SymbolNaming()
		{
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Gets a valid C# identifier from the specified name (symbol).
		/// </summary>
		/// <param name="value"> The value to which to derive the C# identifier. </param>
		/// <returns> The valid C# identifier form of the specified value. </returns>
		public static string GetValidCSharpIdentifier(string value)
		{
			bool first = true;
			StringBuilder sb;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			sb = new StringBuilder();

			foreach (char curr in value)
			{
				if (!(first && char.IsDigit(curr)) && (char.IsLetterOrDigit(curr) || curr == '_'))
					sb.Append(curr);
				else if ((first && char.IsDigit(curr)) || curr == ' ')
					sb.Append('_');
				else
				{
					// skip
				}

				first = false;
			}

			return sb.ToString();
		}

		/// <summary>
		/// Gets a value indicating whether the specified value is a valid C# identifier.
		/// </summary>
		/// <param name="value"> The value to test as a C# identifier. </param>
		/// <returns> True if the specified value is a valid C# identifier; otherwise false. </returns>
		public static bool IsValidCSharpIdentifier(string value)
		{
			throw new NotImplementedException(nameof(IsValidCSharpIdentifier));
		}

		protected abstract string CoreGetCamelCase(string value);

		protected abstract string CoreGetConstantCase(string value);

		protected abstract string CoreGetPascalCase(string value);

		protected abstract string CoreGetPluralForm(string value);

		protected abstract string CoreGetSingularForm(string value);

		public string GetCamelCase(string value)
		{
			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			try
			{
				return this.CoreGetCamelCase(value);
			}
			catch (Exception ex)
			{
				throw new TokenizationException(string.Format("The symbol naming failed (see inner exception)."), ex);
			}
		}

		public string GetConstantCase(string value)
		{
			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			try
			{
				return this.CoreGetConstantCase(value);
			}
			catch (Exception ex)
			{
				throw new TokenizationException(string.Format("The symbol naming failed (see inner exception)."), ex);
			}
		}

		public string GetPascalCase(string value)
		{
			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			try
			{
				return this.CoreGetPascalCase(value);
			}
			catch (Exception ex)
			{
				throw new TokenizationException(string.Format("The symbol naming failed (see inner exception)."), ex);
			}
		}

		public string GetPluralForm(string value)
		{
			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			try
			{
				return this.CoreGetPluralForm(value);
			}
			catch (Exception ex)
			{
				throw new TokenizationException(string.Format("The symbol naming failed (see inner exception)."), ex);
			}
		}

		public string GetSingularForm(string value)
		{
			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			try
			{
				return this.CoreGetSingularForm(value);
			}
			catch (Exception ex)
			{
				throw new TokenizationException(string.Format("The symbol naming failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}