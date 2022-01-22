/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Text;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Tokenization.Namings
{
	public class CSharpStyleSymbolNaming : SymbolNaming
	{
		#region Constructors/Destructors

		protected CSharpStyleSymbolNaming()
			: this(false)
		{
		}

		protected CSharpStyleSymbolNaming(bool disableNameMangling)
		{
			this.disableNameMangling = disableNameMangling;
		}

		#endregion

		#region Fields/Constants

		private static readonly CSharpStyleSymbolNaming instance = new CSharpStyleSymbolNaming();
		private static readonly CSharpStyleSymbolNaming instanceDisableNameMangling = new CSharpStyleSymbolNaming(true);
		private bool disableNameMangling;

		#endregion

		#region Properties/Indexers/Events

		public static CSharpStyleSymbolNaming Instance
		{
			get
			{
				return instance;
			}
		}

		public static CSharpStyleSymbolNaming InstanceDisableNameMangling
		{
			get
			{
				return instanceDisableNameMangling;
			}
		}

		private bool DisableNameMangling
		{
			get
			{
				return this.disableNameMangling;
			}
		}

		#endregion

		#region Methods/Operators

		protected override string CoreGetCamelCase(string value)
		{
			StringBuilder sb;
			char prev;
			int i = 0;
			bool toupper = false;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			value = GetValidCSharpIdentifier(value)
				.ToStringEx(null, string.Empty);

			if (value.Length < 1)
				return value;

			sb = new StringBuilder();
			prev = value[0];

			foreach (char curr in value)
			{
				if (curr == '_')
				{
					toupper = true;

					if (this.DisableNameMangling)
						sb.Append(curr);

					continue; // ignore setting prev=curr
				}

				toupper = toupper || char.IsDigit(prev) || (char.IsLower(prev) && char.IsUpper(curr));

				if (toupper)
					sb.Append(char.ToUpper(curr));
				else
					sb.Append(char.ToLower(curr));

				prev = curr;
				i++;
				toupper = false;
			}

			return sb.ToString();
		}

		protected override string CoreGetConstantCase(string value)
		{
			StringBuilder sb;
			char prev;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			value = GetValidCSharpIdentifier(value)
				.ToStringEx(null, string.Empty);

			if (value.Length < 1)
				return value;

			sb = new StringBuilder();
			prev = value[0];

			foreach (char curr in value)
			{
				if (!this.DisableNameMangling && curr == '_')
					continue;

				if (char.IsLower(prev) && char.IsUpper(curr))
					sb.Append('_');

				sb.Append(char.ToUpper(curr));
				prev = curr;
			}

			return sb.ToString();
		}

		protected override string CoreGetPascalCase(string value)
		{
			StringBuilder sb;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			value = GetValidCSharpIdentifier(value)
				.ToStringEx(null, string.Empty);

			if (value.Length < 1)
				return value;

			value = this.GetCamelCase(value);
			sb = new StringBuilder(value);

			sb[0] = char.ToUpper(sb[0]);

			return sb.ToString();
		}

		protected override string CoreGetPluralForm(string value)
		{
			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			value = GetValidCSharpIdentifier(value)
				.ToStringEx(null, string.Empty);

			if (this.DisableNameMangling)
				return value;

			if (value.Length < 1)
				return value;

			if (value.EndsWith("x", StringComparison.OrdinalIgnoreCase) ||
				value.EndsWith("ch", StringComparison.OrdinalIgnoreCase) ||
				(value.EndsWith("ss", StringComparison.OrdinalIgnoreCase) ||
				value.EndsWith("sh", StringComparison.OrdinalIgnoreCase)))
				value = value + "es";
			else if (value.EndsWith("y", StringComparison.OrdinalIgnoreCase) && value.Length > 1 &&
					!value[value.Length - 2].IsVowel())
			{
				value = value.Remove(value.Length - 1, 1);
				value = value + "ies";
			}
			else if (!value.EndsWith("s", StringComparison.OrdinalIgnoreCase))
				value = value + "s";

			return value;
		}

		protected override string CoreGetSingularForm(string value)
		{
			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			value = GetValidCSharpIdentifier(value)
				.ToStringEx(null, string.Empty);

			if (this.DisableNameMangling)
				return value;

			if (value.Length < 1)
				return value;

			if (string.Compare(value, "series", StringComparison.OrdinalIgnoreCase) == 0)
				return value;

			if (string.Compare(value, "wines", StringComparison.OrdinalIgnoreCase) == 0)
				return value.Remove(value.Length - 1, 1);

			if (value.Length > 3 && value.EndsWith("ies", StringComparison.OrdinalIgnoreCase))
			{
				if (!value[value.Length - 4].IsVowel())
				{
					value = value.Remove(value.Length - 3, 3);
					value = value + 'y';
				}
			}
			else if (value.EndsWith("ees", StringComparison.OrdinalIgnoreCase))
				value = value.Remove(value.Length - 1, 1);
			else if (value.EndsWith("ches", StringComparison.OrdinalIgnoreCase) || value.EndsWith("xes", StringComparison.OrdinalIgnoreCase) || value.EndsWith("sses", StringComparison.OrdinalIgnoreCase))
				value = value.Remove(value.Length - 2, 2);
			else
			{
				if (string.Compare(value, "gas", StringComparison.OrdinalIgnoreCase) == 0 || value.Length <= 1 || (!value.EndsWith("s", StringComparison.OrdinalIgnoreCase) || value.EndsWith("ss", StringComparison.OrdinalIgnoreCase)) || value.EndsWith("us", StringComparison.OrdinalIgnoreCase))
					return value;
				value = value.Remove(value.Length - 1, 1);
			}

			return value;
		}

		#endregion
	}
}