/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using NUnit.Framework;

using WellEngineered.Solder.Primitives;
using WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure;

namespace WellEngineered.Solder.UnitTests.Cli.Primitives._
{
	/// <summary>
	/// Unit tests.
	/// </summary>
	[TestFixture]
	public class StringExtensionsTests
	{
		#region Constructors/Destructors

		public StringExtensionsTests()
		{
		}

		#endregion

		#region Methods/Operators

		

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ShouldFailOnInvalidGenericTypeTryParseTest()
		{
			KeyValuePair<int, int> ovalue;
			bool result;
			
			result = string.Empty.TryParse<KeyValuePair<int, int>>(out ovalue);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ShouldFailOnInvalidTypeTryParseTest()
		{
			object ovalue;
			bool result;

			result = string.Empty.TryParse(typeof(KeyValuePair<int, int>), out ovalue);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeTryParseTest()
		{
			object ovalue;
			bool result;

			result = string.Empty.TryParse(null, out ovalue);
		}

		[Test]
		public void ShouldGetBooleanTest()
		{
			Boolean ovalue;
			bool result;
			
			result = "true".TryParse<Boolean>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(true, ovalue);

			result = "false".TryParse<Boolean>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(false, ovalue);
		}

		[Test]
		public void ShouldGetByteTest()
		{
			Byte ovalue;
			bool result;

			result = "0".TryParse<Byte>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetCharTest()
		{
			Char ovalue;
			bool result;

			result = "0".TryParse<Char>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual('0', ovalue);
		}

		[Test]
		public void ShouldGetDateTimeOffsetTest()
		{
			DateTimeOffset ovalue;
			bool result;

			result = "8/24/2019".TryParse<DateTimeOffset>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(new DateTimeOffset(new DateTime(2019, 8, 24)), ovalue);
		}

		[Test]
		public void ShouldGetDateTimeTest()
		{
			DateTime ovalue;
			bool result;

			result = "8/24/2019".TryParse<DateTime>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(new DateTime(2019, 8, 24), ovalue);
		}

		[Test]
		public void ShouldGetDBNullTest()
		{
			DBNull ovalue;
			bool result;

			result = "".TryParse<DBNull>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(DBNull.Value, ovalue);

			result = "___".TryParse<DBNull>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(DBNull.Value, ovalue);
		}

		[Test]
		public void ShouldGetDecimalTest()
		{
			Decimal ovalue;
			bool result;

			result = "0".TryParse<Decimal>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetDoubleTest()
		{
			Double ovalue;
			bool result;

			result = "0".TryParse<Double>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetEnumTest()
		{
			CharSet ovalue;
			bool result;

			result = "Unicode".TryParse<CharSet>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(CharSet.Unicode, ovalue);
		}

		[Test]
		public void ShouldGetGuidTest()
		{
			Guid ovalue;
			bool result;

			result = "{00000000-0000-0000-0000-000000000000}".TryParse<Guid>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(Guid.Empty, ovalue);
		}

		[Test]
		public void ShouldGetInt16Test()
		{
			Int16 ovalue;
			bool result;

			result = "0".TryParse<Int16>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetInt32Test()
		{
			Int32 ovalue;
			bool result;

			result = "0".TryParse<Int32>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetInt64Test()
		{
			Int64 ovalue;
			bool result;

			result = "0".TryParse<Int64>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetSByteTest()
		{
			SByte ovalue;
			bool result;

			result = "0".TryParse<SByte>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetSingleTest()
		{
			Single ovalue;
			bool result;

			result = "0".TryParse<Single>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetStringTest()
		{
			String ovalue;
			bool result;

			result = "0-8-8-8-8-8-8-8-c".TryParse<String>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual("0-8-8-8-8-8-8-8-c", ovalue);
		}

		[Test]
		public void ShouldGetTimeSpanTest()
		{
			TimeSpan ovalue;
			bool result;

			result = "0:0:0".TryParse<TimeSpan>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(TimeSpan.Zero, ovalue);
		}

		[Test]
		public void ShouldGetUInt16Test()
		{
			UInt16 ovalue;
			bool result;

			result = "0".TryParse<UInt16>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetUInt32Test()
		{
			UInt32 ovalue;
			bool result;

			result = "0".TryParse<UInt32>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetUInt64Test()
		{
			UInt64 ovalue;
			bool result;

			result = "0".TryParse<UInt64>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(0, ovalue);
		}

		[Test]
		public void ShouldGetVersionTest()
		{
			Version ovalue;
			bool result;

			result = "1.2.3.4".TryParse<Version>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(new Version(1, 2, 3, 4), ovalue);

			result = "0.0.0.0".TryParse<Version>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual(new Version(0, 0, 0, 0), ovalue);
		}

		[Test]
		public void ShouldNotGetBooleanTest()
		{
			Boolean ovalue;
			bool result;

			Assert.IsFalse(result = "gibberish".TryParse<Boolean>(out ovalue));
		}

		[Test]
		public void ShouldNotGetByteTest()
		{
			Byte ovalue;
			bool result;

			Assert.IsFalse(result = "gibberish".TryParse<Byte>(out ovalue));
			Assert.IsFalse(result = "1111111111111111111".TryParse<Byte>(out ovalue));
		}

		[Test]
		public void ShouldNotGetCharTest()
		{
			Char ovalue;
			bool result;

			Assert.IsFalse(result = "gibberish".TryParse<Char>(out ovalue));
		}

		[Test]
		public void ShouldNotGetDateTimeOffsetTest()
		{
			DateTimeOffset ovalue;
			bool result;

			Assert.IsFalse(result = "gibberish".TryParse<DateTimeOffset>(out ovalue));
		}

		[Test]
		public void ShouldNotGetDateTimeTest()
		{
			DateTime ovalue;
			bool result;

			Assert.IsFalse(result = "gibberish".TryParse<DateTime>(out ovalue));
		}

		[Test]
		public void ShouldNotGetDecimalTest()
		{
			Decimal ovalue;
			bool result;

			Assert.IsFalse(result = StringExtensions.TryParse<Decimal>("gibberish", out ovalue));
			Assert.IsFalse(result = StringExtensions.TryParse<Decimal>("11111111111111111111111111111111111111", out ovalue));
		}

		[Test]
		public void ShouldNotGetDoubleTest()
		{
			Double ovalue;
			bool result;

			Assert.IsFalse(result = StringExtensions.TryParse<Double>("gibberish", out ovalue));
			Assert.IsFalse(result = StringExtensions.TryParse<Double>("999,769,313,486,232,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000.00", out ovalue));
		}

		[Test]
		public void ShouldNotGetEnumTest()
		{
			CharSet ovalue;
			bool result;

			Assert.IsFalse(result = StringExtensions.TryParse<CharSet>("gibberish", out ovalue));
		}

		[Test]
		public void ShouldNotGetGuidTest()
		{
			Guid ovalue;
			bool result;

			Assert.IsFalse(result = StringExtensions.TryParse<Guid>("gibberish", out ovalue));
		}

		[Test]
		public void ShouldNotGetInt16Test()
		{
			Int16 ovalue;
			bool result;

			Assert.IsFalse(result = StringExtensions.TryParse<Int16>("gibberish", out ovalue));
			Assert.IsFalse(result = StringExtensions.TryParse<Int16>("1111111111111111111", out ovalue));
		}

		[Test]
		public void ShouldNotGetInt32Test()
		{
			Int32 ovalue;
			bool result;

			Assert.IsFalse(result = StringExtensions.TryParse<Int32>("gibberish", out ovalue));
			Assert.IsFalse(result = StringExtensions.TryParse<Int32>("1111111111111111111", out ovalue));
		}

		[Test]
		public void ShouldNotGetInt64Test()
		{
			Int64 ovalue;
			bool result;

			Assert.IsFalse(result = StringExtensions.TryParse<Int64>("gibberish", out ovalue));
			Assert.IsFalse(result = StringExtensions.TryParse<Int64>("9999999999999999999", out ovalue));
		}

		[Test]
		public void ShouldNotGetSByteTest()
		{
			SByte ovalue;
			bool result;

			Assert.IsFalse(result = StringExtensions.TryParse<SByte>("gibberish", out ovalue));
			Assert.IsFalse(result = StringExtensions.TryParse<SByte>("1111111111111111111", out ovalue));
		}

		[Test]
		public void ShouldNotGetSingleTest()
		{
			Single ovalue;
			bool result;

			Assert.IsFalse(result = StringExtensions.TryParse<Single>("gibberish", out ovalue));
			Assert.IsFalse(result = StringExtensions.TryParse<Single>("999,282,300,000,000,000,000,000,000,000,000,000,000.00", out ovalue));
		}

		[Test]
		public void ShouldNotGetTimeSpanTest()
		{
			TimeSpan ovalue;
			bool result;

			Assert.IsFalse(result = StringExtensions.TryParse<TimeSpan>("gibberish", out ovalue));
			Assert.IsFalse(result = StringExtensions.TryParse<TimeSpan>("99999999.02:48:05.4775807", out ovalue));
		}

		[Test]
		public void ShouldNotGetUInt16Test()
		{
			UInt16 ovalue;
			bool result;

			Assert.IsFalse(result = StringExtensions.TryParse<UInt16>("gibberish", out ovalue));
			Assert.IsFalse(result = StringExtensions.TryParse<UInt16>("1111111111111111111", out ovalue));
		}

		[Test]
		public void ShouldNotGetUInt32Test()
		{
			UInt32 ovalue;
			bool result;

			Assert.IsFalse(result = StringExtensions.TryParse<UInt32>("gibberish", out ovalue));
			Assert.IsFalse(result = StringExtensions.TryParse<UInt32>("1111111111111111111", out ovalue));
		}

		[Test]
		public void ShouldNotGetUInt64Test()
		{
			UInt64 ovalue;
			bool result;

			Assert.IsFalse(result = StringExtensions.TryParse<UInt64>("gibberish", out ovalue));
			Assert.IsFalse(result = StringExtensions.TryParse<UInt64>("99999999999999999999", out ovalue));
		}

		[Test]
		public void ShouldToStringExTest()
		{
			Assert.AreEqual("123.456", 123.456.ToStringEx());
			Assert.AreEqual("123.46", 123.456.ToStringEx("n"));
			Assert.AreEqual("urn:foo", new Uri("urn:foo").ToStringEx("n"));

			Assert.AreEqual(string.Empty, ((object)null).ToStringEx(null));
			Assert.AreEqual(string.Empty, ((object)null).ToStringEx(null, null));
			Assert.AreEqual("1", string.Empty.ToStringEx(null, "1"));
		}

		[Test]
		public void ShouldSpecialGetValueOnNonNullNullableGenericTryParseTest()
		{
			int? ovalue;
			bool result;

			result = "100".TryParse<int?>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual((int?)100, ovalue);
		}

		[Test]
		public void ShouldSpecialGetValueOnNonNullNullableTryParseTest()
		{
			object ovalue;
			bool result;

			result = "100".TryParse(typeof(int?), out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual((int?)100, ovalue);
		}

		[Test]
		public void ShouldSpecialGetValueOnNullNullableGenericTryParseTest()
		{
			int? ovalue;
			bool result;

			result = ((string)null).TryParse<int?>(out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual((int?)null, ovalue);
		}

		[Test]
		public void ShouldSpecialGetValueOnNullNullableTryParseTest()
		{
			object ovalue;
			bool result;

			result = ((string)null).TryParse(typeof(int?), out ovalue);
			Assert.IsTrue(result);
			Assert.AreEqual((int?)null, ovalue);
		}

		[Test]
		public void ShouldWithNullCanGetDBNullTest()
		{
			DBNull ovalue;
			bool result;

			Assert.IsTrue(result = ((string)null).TryParse<DBNull>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetBooleanTest()
		{
			Boolean ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<Boolean>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetByteTest()
		{
			Byte ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<Byte>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetCharTest()
		{
			Char ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<Char>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetDateTimeOffsetTest()
		{
			DateTimeOffset ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<DateTimeOffset>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetDateTimeTest()
		{
			DateTime ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<DateTime>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetDecimalTest()
		{
			Decimal ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<Decimal>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetDoubleTest()
		{
			Double ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<Double>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetEnumTest()
		{
			CharSet ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<CharSet>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetGuidTest()
		{
			Guid ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<Guid>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetInt16Test()
		{
			Int16 ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<Int16>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetInt32Test()
		{
			Int32 ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<Int32>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetInt64Test()
		{
			Int64 ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<Int64>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetSByteTest()
		{
			SByte ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<SByte>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetSingleTest()
		{
			Single ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<Single>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetTimeSpanTest()
		{
			TimeSpan ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<TimeSpan>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetUInt16Test()
		{
			UInt16 ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<UInt16>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetUInt32Test()
		{
			UInt32 ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<UInt32>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetUInt64Test()
		{
			UInt64 ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<UInt64>(out ovalue));
		}

		[Test]
		public void ShouldWithNullNotGetVersionTest()
		{
			Version ovalue;
			bool result;

			Assert.IsFalse(result = ((string)null).TryParse<Version>(out ovalue));
		}

		#endregion
	}
}