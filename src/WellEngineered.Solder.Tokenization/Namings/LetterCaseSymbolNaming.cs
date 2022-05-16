/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Tokenization.Namings
{
	public class LetterCaseSymbolNaming : SymbolNaming
	{
		#region Constructors/Destructors

		protected LetterCaseSymbolNaming()
			: this(false)
		{
		}

		protected LetterCaseSymbolNaming(bool upperCase)
		{
			this.upperCase = upperCase;
		}

		#endregion

		#region Fields/Constants

		private static readonly LetterCaseSymbolNaming instanceLowerCase = new LetterCaseSymbolNaming(false);
		private static readonly LetterCaseSymbolNaming instanceUpperCase = new LetterCaseSymbolNaming(true);
		private bool upperCase;

		#endregion

		#region Properties/Indexers/Events

		public static LetterCaseSymbolNaming InstanceLowerCase
		{
			get
			{
				return instanceLowerCase;
			}
		}

		public static LetterCaseSymbolNaming InstanceUpperCase
		{
			get
			{
				return instanceUpperCase;
			}
		}

		private bool UpperCase
		{
			get
			{
				return this.upperCase;
			}
		}

		#endregion

		#region Methods/Operators

		protected override string CoreGetCamelCase(string value)
		{
			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			value = GetValidCSharpIdentifier(value)
				.ToStringEx(null, string.Empty);

			return this.UpperCase ? value.ToUpper() : value.ToLower();
		}

		protected override string CoreGetConstantCase(string value)
		{
			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			value = GetValidCSharpIdentifier(value)
				.ToStringEx(null, string.Empty);

			return this.UpperCase ? value.ToUpper() : value.ToLower();
		}

		protected override string CoreGetPascalCase(string value)
		{
			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			value = GetValidCSharpIdentifier(value)
				.ToStringEx(null, string.Empty);

			return this.UpperCase ? value.ToUpper() : value.ToLower();
		}

		protected override string CoreGetPluralForm(string value)
		{
			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			value = GetValidCSharpIdentifier(value)
				.ToStringEx(null, string.Empty);

			return this.UpperCase ? value.ToUpper() : value.ToLower();
		}

		protected override string CoreGetSingularForm(string value)
		{
			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			value = GetValidCSharpIdentifier(value)
				.ToStringEx(null, string.Empty);

			return this.UpperCase ? value.ToUpper() : value.ToLower();
		}

		#endregion
	}
}