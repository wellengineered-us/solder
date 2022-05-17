/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Globalization;

namespace WellEngineered.Solder.Primitives
{
	public static class StringExtensions
	{
		#region Methods/Operators

		/// <summary>
		/// Gets a value indicating whether the specified character is a vowel (US English).
		/// </summary>
		/// <param name="ch"> The value to test as a vowel (US English). </param>
		/// <returns> True if the specified value is a vowel (US English); otherwise false. </returns>
		public static bool IsVowel(this char ch)
		{
			switch (ch)
			{
				case 'A':
				case 'E':
				case 'I':
				case 'O':
				case 'U':
				case 'Y':
					return true;
				case 'a':
				case 'e':
				case 'i':
				case 'o':
				case 'u':
				case 'y':
					return true;
				default:
					return false;
			}
		}

		public static string ToStringEx<TValue>(this TValue value, string format = null, string @default = null)
		{
			string retval;

			@default = @default ?? string.Empty;

			if ((object)value == null)
				retval = @default;
			else if (value is IFormattable formattable)
				retval = formattable.ToString(format, null);
			else
				retval = value.ToString();

			if (string.IsNullOrEmpty(retval))
				retval = @default;

			return retval;
		}

		/// <summary>
		/// Converts the specified string representation to its valueType equivalent and returns a value that indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="valueType"> The type to convert the string value. </param>
		/// <param name="value"> A string containing a valueType to convert. </param>
		/// <param name="result"> When this method returns, contains the valueType value equivalent contained in value, if the conversion succeeded, or null if the conversion failed. The conversion fails if the value parameter is null, is an empty string, or does not contain a valid string representation of an valueType. This parameter is passed uninitialized. </param>
		/// <returns> A boolean value of true if the value was converted successfully; otherwise, false. </returns>
		public static bool TryParse(this string value, Type valueType, out object result)
		{
			bool retval;
			Type openNullableType;

			if ((object)valueType == null)
				throw new ArgumentNullException(nameof(valueType));

			openNullableType = typeof(Nullable<>);
			result = null;

			if (valueType.IsGenericType &&
				!valueType.IsGenericTypeDefinition &&
				valueType.GetGenericTypeDefinition().Equals(openNullableType))
			{
				if ((object)value == null)
					return true;
				else
					return TryParse(value, valueType.GetGenericArguments()[0], out result);
			}
			else if (valueType == typeof(DBNull))
			{
				// all values become DBNull
				retval = true;
				result = DBNull.Value;
			}
			else if (valueType == typeof(String))
			{
				retval = true;
				result = value;
			}
			else if (valueType == typeof(Boolean))
			{
				Boolean zresult;
				retval = Boolean.TryParse(value, out zresult);
				result = zresult;
				result = zresult;
			}
			else if (valueType == typeof(Byte))
			{
				Byte zresult;
				retval = Byte.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Char))
			{
				Char zresult;
				retval = Char.TryParse(value, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(DateTime))
			{
				DateTime zresult;
				retval = DateTime.TryParse(value, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(DateTimeOffset))
			{
				DateTimeOffset zresult;
				retval = DateTimeOffset.TryParse(value, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Decimal))
			{
				Decimal zresult;
				retval = Decimal.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Double))
			{
				Double zresult;
				retval = Double.TryParse(value, NumberStyles.Any, null, out zresult) &&
						Double.IsFinite(zresult);
				result = zresult;
			}
			else if (valueType == typeof(Guid))
			{
				Guid zresult;
				retval = Guid.TryParse(value, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Int16))
			{
				Int16 zresult;
				retval = Int16.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Int32))
			{
				Int32 zresult;
				retval = Int32.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Int64))
			{
				Int64 zresult;
				retval = Int64.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(SByte))
			{
				SByte zresult;
				retval = SByte.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Single))
			{
				Single zresult;
				retval = Single.TryParse(value, NumberStyles.Any, null, out zresult) &&
						Single.IsFinite(zresult);
				result = zresult;
			}
			else if (valueType == typeof(TimeSpan))
			{
				TimeSpan zresult;
				retval = TimeSpan.TryParse(value, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(UInt16))
			{
				UInt16 zresult;
				retval = UInt16.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(UInt32))
			{
				UInt32 zresult;
				retval = UInt32.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(UInt64))
			{
				UInt64 zresult;
				retval = UInt64.TryParse(value, NumberStyles.Any, null, out zresult);
				result = zresult;
			}
			else if (valueType == typeof(Version))
			{
				Version zresult;
				retval = Version.TryParse(value, out zresult);
				result = zresult;
			}
			else if (valueType.IsEnum) // special case
			{
				object zresult = null;
				retval = false;

				if ((object)value != null)
				{
					if (Enum.IsDefined(valueType, value))
					{
						zresult = Enum.Parse(valueType, value);
						retval = true;
					}
				}

				result = zresult;
			}
			else
				throw new ArgumentOutOfRangeException(nameof(valueType), string.Format("The value type '{0}' is not supported.", valueType.FullName));

			return retval;
		}

		/// <summary>
		/// Converts the specified string representation to its TValue equivalent and returns a value that indicates whether the conversion succeeded.
		/// </summary>
		/// <typeparam name="TValue"> The type to parse the string value. </typeparam>
		/// <param name="value"> A string containing a TValue to convert. </param>
		/// <param name="result"> When this method returns, contains the TValue value equivalent contained in value, if the conversion succeeded, or default(TValue) if the conversion failed. The conversion fails if the value parameter is null, is an empty string, or does not contain a valid string representation of a TValue. This parameter is passed uninitialized. </param>
		/// <returns> A boolean value of true if the value was converted successfully; otherwise, false. </returns>
		public static bool TryParse<TValue>(this string value, out TValue result)
		{
			Type valueType;
			object oresult;
			bool retval;

			valueType = typeof(TValue);

			// no null check is intentional
			retval = TryParse(value, valueType, out oresult);

			if ((object)oresult == null)
				result = default(TValue);
			else
				result = (TValue)oresult;

			return retval;
		}

		#endregion
	}
}