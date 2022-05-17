/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Runtime.InteropServices;

using Microsoft.Extensions.Configuration;

using NMock;

using NUnit.Framework;

using WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure;
using WellEngineered.Solder.Utilities.AppSettings;

namespace WellEngineered.Solder.UnitTests.Cli.Utilities._
{
	[TestFixture]
	public class AppSettingsFacadeTests
	{
		#region Constructors/Destructors

		public AppSettingsFacadeTests()
		{
		}

		#endregion

		#region Fields/Constants

		private const string UNATTAINABLE_VALUE = "!@#$%^&*(";

		#endregion

		#region Methods/Operators

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnAppSettingsConfigurationRootCreateTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			mockConfigurationRoot = null;

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedBooleanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueBoolean";
			string __unusedString = null;
			Boolean __unusedBoolean;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Boolean>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueByte";
			string __unusedString = null;
			Byte __unusedByte;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Byte>("BadAppSettingsFacadeValueByte");
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedCharTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueChar";
			string __unusedString = null;
			Char __unusedChar;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Char>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedDateTimeTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueDateTime";
			string __unusedString = null;
			DateTime __unusedDateTime;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<DateTime>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedDecimalTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueDecimal";
			string __unusedString = null;
			Decimal __unusedDecimal;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Decimal>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedDoubleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueDouble";
			string __unusedString = null;
			Double __unusedDouble;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Double>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedEnumTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueEnum";
			string __unusedString = null;
			CharSet __unusedCharSet;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<CharSet>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedGuidTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueGuid";
			string __unusedString = null;
			Guid __unusedGuid;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Guid>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueInt16";
			string __unusedString = null;
			Int16 __unusedInt16;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Int16>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueInt32";
			string __unusedString = null;
			Int32 __unusedInt32;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Int32>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueInt64";
			string __unusedString = null;
			Int64 __unusedInt64;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Int64>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedSByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueSByte";
			string __unusedString = null;
			SByte __unusedSByte;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<SByte>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedSingleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueSingle";
			string __unusedString = null;
			Single __unusedSingle;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Single>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedTimeSpanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueTimeSpan";
			string __unusedString = null;
			TimeSpan __unusedTimeSpan;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<TimeSpan>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedUInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueUInt16";
			string __unusedString = null;
			UInt16 __unusedUInt16;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<UInt16>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedUInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueUInt32";
			string __unusedString = null;
			UInt32 __unusedUInt32;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<UInt32>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetTypedUInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueUInt64";
			string __unusedString = null;
			UInt64 __unusedUInt64;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<UInt64>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedBooleanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueBoolean";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedBoolean;
			Type valueType = typeof(Boolean);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueByte";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedByte;
			Type valueType = typeof(Byte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedCharTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueChar";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedChar;
			Type valueType = typeof(Char);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedDateTimeTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueDateTime";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedDateTime;
			Type valueType = typeof(DateTime);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedDecimalTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueDecimal";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedDecimal;
			Type valueType = typeof(Decimal);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedDoubleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueDouble";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedDouble;
			Type valueType = typeof(Double);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedEnumTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueEnum";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedCharSet;
			Type valueType = typeof(CharSet);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedGuidTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueGuid";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedGuid;
			Type valueType = typeof(Guid);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueInt16";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedInt16;
			Type valueType = typeof(Int16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueInt32";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedInt32;
			Type valueType = typeof(Int32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueInt64";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedInt64;
			Type valueType = typeof(Int64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedSByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueSByte";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedSByte;
			Type valueType = typeof(SByte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedSingleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueSingle";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedSingle;
			Type valueType = typeof(Single);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedTimeSpanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueTimeSpan";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedTimeSpan;
			Type valueType = typeof(TimeSpan);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedUInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueUInt16";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedUInt16;
			Type valueType = typeof(UInt16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedUInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueUInt32";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedUInt32;
			Type valueType = typeof(UInt32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnInvalidValueGetUntypedUInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "BadAppSettingsFacadeValueUInt64";
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedUInt64;
			Type valueType = typeof(UInt64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedBooleanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueBoolean";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Boolean>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueByte";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Byte>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedCharTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueChar";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Char>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedDateTimeTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueDateTime";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<DateTime>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedDecimalTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueDecimal";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Decimal>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedDoubleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueDouble";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Double>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedEnumTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueEnum";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<CharSet>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedGuidTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueGuid";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Guid>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueInt16";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Int16>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueInt32";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Int32>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueInt64";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Int64>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedSByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueSByte";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<SByte>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedSingleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueSingle";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Single>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedTimeSpanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueTimeSpan";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<TimeSpan>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedUInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueUInt16";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<UInt16>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedUInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueUInt32";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<UInt32>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetTypedUInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueUInt64";

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<UInt64>(KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedBooleanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueBoolean";
			Type valueType = typeof(Boolean);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueByte";
			Type valueType = typeof(Byte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedCharTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueChar";
			Type valueType = typeof(Char);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedDateTimeTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueDateTime";
			Type valueType = typeof(DateTime);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedDecimalTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueDecimal";
			Type valueType = typeof(Decimal);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedDoubleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueDouble";
			Type valueType = typeof(Double);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedEnumTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueEnum";
			Type valueType = typeof(CharSet);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedGuidTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueGuid";
			Type valueType = typeof(Guid);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueInt16";
			Type valueType = typeof(Int16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueInt32";
			Type valueType = typeof(Int32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueInt64";
			Type valueType = typeof(Int64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedSByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueSByte";
			Type valueType = typeof(SByte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedSingleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueSingle";
			Type valueType = typeof(Single);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedTimeSpanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueTimeSpan";
			Type valueType = typeof(TimeSpan);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedUInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueUInt16";
			Type valueType = typeof(UInt16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedUInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueUInt32";
			Type valueType = typeof(UInt32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(AppSettingsException))]
		public void ShouldFailOnNonExistKeyGetUntypedUInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "NotThereAppSettingsFacadeValueUInt64";
			Type valueType = typeof(UInt64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedBooleanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Boolean>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Byte>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedCharTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Char>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedDateTimeTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<DateTime>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedDecimalTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Decimal>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedDoubleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Double>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedEnumTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<CharSet>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedGuidTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Guid>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Int16>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Int32>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Int64>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedSByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<SByte>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedSingleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<Single>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedTimeSpanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<TimeSpan>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedUInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<UInt16>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedUInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<UInt32>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetTypedUInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<UInt64>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedBooleanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Boolean);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Byte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedCharTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Char);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedDateTimeTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(DateTime);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedDecimalTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Decimal);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedDoubleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Double);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedEnumTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(CharSet);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedGuidTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Guid);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Int16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Int32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Int64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedSByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(SByte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedSingleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(Single);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedTimeSpanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(TimeSpan);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedUInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(UInt16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedUInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(UInt32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyGetUntypedUInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(UInt64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyHasAppSettingTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.HasAppSetting(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullNameGetTypedAppSettingTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting<string>(KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullNameGetUntypedAppSettingTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = null;
			Type valueType = typeof(String);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeGetUntypedAppSettingTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "";
			Type valueType = null;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			appSettingsFacade.GetAppSetting(valueType, KEY);
		}

		//[Test]
		//public void Should__GetArgsParseCommandLineArgumentsTest()
		//{
		//	List<string> args;
		//	IDictionary<string, IList<string>> cmdlnargs;

		//	args = new List<string>();
		//	args.Add("arg0");
		//	args.Add(string.Empty);
		//	args.Add("-");
		//	args.Add("-arg1");
		//	args.Add("-arg2:");
		//	args.Add("-arg3:");
		//	args.Add("-arg4:value4");
		//	args.Add("-arg5:value5");
		//	args.Add("-arg4:value4");
		//	args.Add("arg6:value6");
		//	args.Add("-:value7");
		//	args.Add("-arg8:value8a");
		//	args.Add("-arg8:value8b");
		//	args.Add("-arg8:value8c");
		//	args.Add("-arg8:value8a");

		//	cmdlnargs = AppSettingsFacade.ReflectionFacade.ParseCommandLineArguments(args.ToArray());

		//	Assert.IsNotNull(cmdlnargs);
		//	Assert.AreEqual(3, cmdlnargs.Count);

		//	Assert.AreEqual(1, cmdlnargs["arg4"].Count);
		//	Assert.AreEqual("value4", cmdlnargs["arg4"][0]);

		//	Assert.AreEqual(1, cmdlnargs["arg5"].Count);
		//	Assert.AreEqual("value5", cmdlnargs["arg5"][0]);

		//	Assert.AreEqual(3, cmdlnargs["arg8"].Count);
		//	Assert.AreEqual("value8a", cmdlnargs["arg8"][0]);
		//	Assert.AreEqual("value8b", cmdlnargs["arg8"][1]);
		//	Assert.AreEqual("value8c", cmdlnargs["arg8"][2]);
		//}

		[Test]
		public void ShouldGetTypedBooleanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueBoolean";
			Boolean expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Boolean __unusedBoolean;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = true;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<Boolean>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueByte";
			Byte expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Byte __unusedByte;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<Byte>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedCharTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueChar";
			Char expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Char __unusedChar;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = '\0';

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<Char>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedDateTimeTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueDateTime";
			DateTime expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			DateTime __unusedDateTime;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = DateTime.Now;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString("O"));

			value = appSettingsFacade.GetAppSetting<DateTime>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedDecimalTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueDecimal";
			Decimal expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Decimal __unusedDecimal;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<Decimal>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedDoubleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueDouble";
			Double expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Double __unusedDouble;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<Double>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedEnumTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueEnum";
			CharSet expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			CharSet __unusedCharSet;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = CharSet.Unicode;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<CharSet>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedGuidTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueGuid";
			Guid expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Guid __unusedGuid;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = Guid.Empty;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<Guid>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueInt16";
			Int16 expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Int16 __unusedInt16;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<Int16>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueInt32";
			Int32 expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Int32 __unusedInt32;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<Int32>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueInt64";
			Int64 expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Int64 __unusedInt64;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<Int64>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedSByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueSByte";
			SByte expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			SByte __unusedSByte;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<SByte>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedSingleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueSingle";
			Single expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			Single __unusedSingle;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<Single>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedStringTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueString";
			String expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			//String __unusedString;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = "foobar";

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<String>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedTimeSpanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueTimeSpan";
			TimeSpan expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			TimeSpan __unusedTimeSpan;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = TimeSpan.Zero;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<TimeSpan>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedUInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueUInt16";
			UInt16 expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			UInt16 __unusedUInt16;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<UInt16>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedUInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueUInt32";
			UInt32 expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			UInt32 __unusedUInt32;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<UInt32>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetTypedUInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueUInt64";
			UInt64 expected, value;
			Type __unusedType = null;
			string __unusedString = null;
			UInt64 __unusedUInt64;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting<UInt64>(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedBooleanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueBoolean";
			Boolean expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedBoolean;
			Type valueType = typeof(Boolean);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = true;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueByte";
			Byte expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedByte;
			Type valueType = typeof(Byte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedCharTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueChar";
			Char expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedChar;
			Type valueType = typeof(Char);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = '\0';

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedDateTimeTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueDateTime";
			DateTime expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedDateTime;
			Type valueType = typeof(DateTime);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = DateTime.Now;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString("O"));

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedDecimalTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueDecimal";
			Decimal expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedDecimal;
			Type valueType = typeof(Decimal);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedDoubleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueDouble";
			Double expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedDouble;
			Type valueType = typeof(Double);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedEnumTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueEnum";
			CharSet expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedCharSet;
			Type valueType = typeof(CharSet);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = CharSet.Unicode;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedGuidTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueGuid";
			Guid expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedGuid;
			Type valueType = typeof(Guid);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = Guid.Empty;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueInt16";
			Int16 expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedInt16;
			Type valueType = typeof(Int16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueInt32";
			Int32 expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedInt32;
			Type valueType = typeof(Int32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueInt64";
			Int64 expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedInt64;
			Type valueType = typeof(Int64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedSByteTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueSByte";
			SByte expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedSByte;
			Type valueType = typeof(SByte);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedSingleTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueSingle";
			Single expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedSingle;
			Type valueType = typeof(Single);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedStringTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueString";
			String expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedString2;
			Type valueType = typeof(String);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = "foobar";

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedTimeSpanTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueTimeSpan";
			TimeSpan expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedTimeSpan;
			Type valueType = typeof(TimeSpan);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = TimeSpan.Zero;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedUInt16Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueUInt16";
			UInt16 expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedUInt16;
			Type valueType = typeof(UInt16);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedUInt32Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueUInt32";
			UInt32 expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedUInt32;
			Type valueType = typeof(UInt32);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldGetUntypedUInt64Test()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueUInt64";
			UInt64 expected;
			object value;
			Type __unusedType = null;
			string __unusedString = null;
			object __unusedUInt64;
			Type valueType = typeof(UInt64);

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = 0L;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(expected.ToString());

			value = appSettingsFacade.GetAppSetting(valueType, KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldHaveFalseHasAppSettingTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueBooleanFalse";
			bool expected, value;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = false;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(null);

			value = appSettingsFacade.HasAppSetting(KEY);

			Assert.AreEqual(expected, value);
		}

		[Test]
		public void ShouldHaveTrueHasAppSettingTest()
		{
			AppSettingsFacade appSettingsFacade;
			IConfigurationRoot mockConfigurationRoot;

			MockFactory mockFactory;
			const string KEY = "AppSettingsFacadeValueBoolean";
			bool expected, value;

			mockFactory = new MockFactory();
			mockConfigurationRoot = mockFactory.CreateInstance<IConfigurationRoot>();

			appSettingsFacade = new AppSettingsFacade(mockConfigurationRoot);

			expected = true;

			Expect.On(mockConfigurationRoot).One.GetProperty(p => p[KEY]).WillReturn(UNATTAINABLE_VALUE);
			value = appSettingsFacade.HasAppSetting(KEY);

			Assert.AreEqual(expected, value);
		}

		#endregion
	}
}