/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NMock;

using NUnit.Framework;

using WellEngineered.Solder.Primitives;
using WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure;

namespace WellEngineered.Solder.UnitTests.Cli.Primitives._
{
	[TestFixture]
	public class ReflectionExtensionsTests
	{
		#region Constructors/Destructors

		public ReflectionExtensionsTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCheckEqualByValueTest()
		{
			bool result;
			object objA, objB;
			
			// both null
			objA = null;
			objB = null;
			result = ReflectionExtensions.EqualByValue(objA, objB);
			Assert.IsTrue(result);

			// objA null, objB not null
			objA = null;
			objB = "not null string";
			result = ReflectionExtensions.EqualByValue(objA, objB);
			Assert.IsFalse(result);

			// objA not null, objB null
			objA = "not null string";
			objB = null;
			result = ReflectionExtensions.EqualByValue(objA, objB);
			Assert.IsFalse(result);

			// objA == objB
			objA = 100;
			objB = 100;
			result = ReflectionExtensions.EqualByValue(objA, objB);
			Assert.IsTrue(result);

			// objA != objB
			objA = 100;
			objB = -100;
			result = ReflectionExtensions.EqualByValue(objA, objB);
			Assert.IsFalse(result);
		}
		
		[Test]
		public void ShouldPreventNullCoaleseTest()
		{
			bool result;
			object objA, objB;

			// both null
			objA = null;
			objB = (int)0;
			result = ReflectionExtensions.EqualByValue(objA, objB);
			Assert.IsFalse(result);
		}
		
		[Test]
		public void ShouldGetNonNullOnNonNullValueChangeTypeTest()
		{
			object value;

			value = ReflectionExtensions.ChangeType((byte)1, typeof(int));

			Assert.IsNotNull(value);
			Assert.IsInstanceOf<int>(value);
			Assert.AreEqual((int)1, value);
		}

		[Test]
		public void ShouldGetNonNullOnNonNullValueChangeTypeTick1Test()
		{
			int value;

			value = ReflectionExtensions.ChangeType<int>((byte)1);

			Assert.IsNotNull(value);
			Assert.IsInstanceOf<int>(value);
			Assert.AreEqual((int)1, value);
		}

		[Test]
		public void ShouldGetNonNullOnNonNullValueNullableChangeTypeTest()
		{
			object value;

			value = ReflectionExtensions.ChangeType((byte)1, typeof(int?));

			Assert.IsNotNull(value);
			Assert.IsInstanceOf<int?>(value);
			Assert.AreEqual((int?)1, value);
		}

		[Test]
		public void ShouldGetNonNullOnNonNullValueNullableChangeTypeTick1Test()
		{
			int? value;

			value = ReflectionExtensions.ChangeType<int?>((byte)1);

			Assert.IsNotNull(value);
			Assert.IsInstanceOf<int?>(value);
			Assert.AreEqual((int?)1, value);
		}
		
		[Test]
		public void ShouldGetDefaultOnNullValueChangeTypeTest()
		{
			object value;

			value = ReflectionExtensions.ChangeType(null, typeof(int));

			Assert.AreEqual(default(int), value);
		}

		[Test]
		public void ShouldGetDefaultOnNullValueChangeTypeTick1Test()
		{
			int value;

			value = ReflectionExtensions.ChangeType<int>(null);

			Assert.AreEqual(default(int), value);
		}

		[Test]
		public void ShouldGetDefaultValueTest()
		{
			object defaultValue;

			defaultValue = ReflectionExtensions.DefaultValue(typeof(int));

			Assert.AreEqual(0, defaultValue);

			defaultValue = ReflectionExtensions.DefaultValue(typeof(int?));

			Assert.IsNull(defaultValue);
		}
		
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeChangeTypeTest()
		{
			ReflectionExtensions.ChangeType(1, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeDefaultValueTest()
		{
			ReflectionExtensions.DefaultValue(null);
		}
		
		[Test]
		public void ShouldAssociativeOnlyGetLogicalPropertyTypeTest()
		{
			Dictionary<string, object> mockObject;
			string propertyName;
			Type propertyType;
			bool result;

			string _unusedString = null;

			// null, null
			mockObject = null;
			propertyName = null;

			result = ReflectionExtensions.GetLogicalPropertyType(mockObject, propertyName, out propertyType);

			Assert.IsFalse(result);
			Assert.IsNull(propertyType);

			// null, ""
			mockObject = null;
			propertyName = string.Empty;

			result = ReflectionExtensions.GetLogicalPropertyType(mockObject, propertyName, out propertyType);

			Assert.IsFalse(result);
			Assert.IsNull(propertyType);

			// null, "PropName"
			mockObject = null;
			propertyName = "FirstName";

			result = ReflectionExtensions.GetLogicalPropertyType(mockObject, propertyName, out propertyType);

			Assert.IsFalse(result);
			Assert.IsNull(propertyType);

			// !null, null
			mockObject = new Dictionary<string, object>();
			propertyName = null;

			result = ReflectionExtensions.GetLogicalPropertyType(mockObject, propertyName, out propertyType);

			Assert.IsFalse(result);
			Assert.IsNull(propertyType);

			// !null, ""
			mockObject = new Dictionary<string, object>();
			propertyName = string.Empty;

			result = ReflectionExtensions.GetLogicalPropertyType(mockObject, propertyName, out propertyType);

			Assert.IsFalse(result);
			Assert.IsNull(propertyType);

			// !null, "PropName"
			mockObject = new Dictionary<string, object>();
			mockObject["FirstName"] = "john";
			propertyName = "FirstName";

			result = ReflectionExtensions.GetLogicalPropertyType(mockObject, propertyName, out propertyType);

			Assert.IsTrue(result);
			Assert.IsNotNull(propertyType);
			Assert.AreEqual(typeof(string), propertyType);
		}

		[Test]
		public void ShouldAssociativeOnlyGetLogicalPropertyValueTest()
		{
			Dictionary<string, object> mockObject;
			string propertyName;
			object propertyValue;
			bool result;

			string _unusedString = null;

			// null, null
			mockObject = null;
			propertyName = null;

			result = ReflectionExtensions.GetLogicalPropertyValue(mockObject, propertyName, out propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// null, ""
			mockObject = null;
			propertyName = string.Empty;

			result = ReflectionExtensions.GetLogicalPropertyValue(mockObject, propertyName, out propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// null, "PropName"
			mockObject = null;
			propertyName = "FirstName";

			result = ReflectionExtensions.GetLogicalPropertyValue(mockObject, propertyName, out propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// !null, null
			mockObject = new Dictionary<string, object>();
			propertyName = null;

			result = ReflectionExtensions.GetLogicalPropertyValue(mockObject, propertyName, out propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// !null, ""
			mockObject = new Dictionary<string, object>();
			propertyName = string.Empty;

			result = ReflectionExtensions.GetLogicalPropertyValue(mockObject, propertyName, out propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// !null, "PropName"
			mockObject = new Dictionary<string, object>();
			mockObject["FirstName"] = "john";
			propertyName = "FirstName";

			result = ReflectionExtensions.GetLogicalPropertyValue(mockObject, propertyName, out propertyValue);

			Assert.IsTrue(result);
			Assert.IsNotNull(propertyValue);
			Assert.AreEqual("john", propertyValue);
		}

		[Test]
		public void ShouldAssociativeOnlySetLogicalPropertyValueTest()
		{
			Dictionary<string, object> mockObject;
			string propertyName;
			object propertyValue;
			bool result;

			string _unusedString = null;

			propertyValue = null;

			// null, null
			mockObject = null;
			propertyName = null;

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// null, ""
			mockObject = null;
			propertyName = string.Empty;

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// null, "PropName"
			mockObject = null;
			propertyName = "FirstName";

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// !null, null
			mockObject = new Dictionary<string, object>();
			propertyName = null;

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// !null, ""
			mockObject = new Dictionary<string, object>();
			propertyName = string.Empty;

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// !null, "PropName"
			mockObject = new Dictionary<string, object>();
			mockObject["FirstName"] = null;
			propertyName = "FirstName";
			propertyValue = "john";

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue);

			Assert.IsTrue(result);
			Assert.IsNotNull(propertyValue);
			Assert.AreEqual("john", propertyValue);

			// !null, "PropName" - !staySoft
			mockObject = new Dictionary<string, object>();
			propertyName = "FirstName";
			propertyValue = null;

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue, false, false);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShouldFailOnDefinedGetNoAttributesTest()
		{
			ReflectionExtensions.GetZeroAttributes<MockMultipleTestAttibute>(typeof(MockTestAttributedClass));
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShouldFailOnMultiDefinedGetAttributeTest()
		{
			ReflectionExtensions.GetOneAttribute<MockMultipleTestAttibute>(typeof(MockTestAttributedClass));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullConversionTypeMakeNonNullableTypeTest()
		{
			ReflectionExtensions.MakeNonNullableType(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullConversionTypeMakeNullableTypeTest()
		{
			ReflectionExtensions.MakeNullableType(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTargetGetAttributesTest()
		{
			ReflectionExtensions.GetAllAttributes<MockMultipleTestAttibute>(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTargetGetAttributeTest()
		{
			ReflectionExtensions.GetOneAttribute<MockMultipleTestAttibute>(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTargetGetZeroAttributesTest()
		{
			ReflectionExtensions.GetZeroAttributes<MockMultipleTestAttibute>(null);
		}

		[Test]
		public void ShouldGetAttributesTest()
		{
			MockMultipleTestAttibute[] tas;

			tas = ReflectionExtensions.GetAllAttributes<MockMultipleTestAttibute>(typeof(MockTestAttributedClass));

			Assert.IsNotNull(tas);
			Assert.AreEqual(2, tas.Length);
		}

		[Test]
		public void ShouldGetAttributeTest()
		{
			MockSingleTestAttibute sta;
			Type targetType;
			MethodInfo methodInfo;
			ParameterInfo parameterInfo;

			targetType = typeof(MockTestAttributedClass);
			var _targetType = targetType.GetTypeInfo();

			sta = ReflectionExtensions.GetOneAttribute<MockSingleTestAttibute>(_targetType.Module);

			Assert.IsNotNull(sta);
			Assert.AreEqual(int.MinValue, sta.Value);

			sta = ReflectionExtensions.GetOneAttribute<MockSingleTestAttibute>(_targetType.Assembly);

			Assert.IsNotNull(sta);
			Assert.AreEqual(int.MaxValue, sta.Value);

			sta = ReflectionExtensions.GetOneAttribute<MockSingleTestAttibute>(targetType);

			Assert.IsNotNull(sta);
			Assert.AreEqual(1, sta.Value);

			methodInfo = targetType.GetMethod(nameof(MockTestAttributedClass.MyMethod));
			Assert.IsNotNull(methodInfo);

			sta = ReflectionExtensions.GetOneAttribute<MockSingleTestAttibute>(methodInfo);

			Assert.IsNotNull(sta);
			Assert.AreEqual(2, sta.Value);

			parameterInfo = methodInfo.GetParameters().Single(p => p.Name == "obj");
			Assert.IsNotNull(parameterInfo);

			sta = ReflectionExtensions.GetOneAttribute<MockSingleTestAttibute>(parameterInfo);

			Assert.IsNotNull(sta);
			Assert.AreEqual(4, sta.Value);

			parameterInfo = methodInfo.ReturnParameter;
			Assert.IsNotNull(parameterInfo);

			sta = ReflectionExtensions.GetOneAttribute<MockSingleTestAttibute>(parameterInfo);

			Assert.IsNotNull(sta);
			Assert.AreEqual(8, sta.Value);
		}

		[Test]
		public void ShouldGetEmptyAttributesTest()
		{
			MockMultipleTestAttibute[] tas;

			tas = ReflectionExtensions.GetAllAttributes<MockMultipleTestAttibute>(typeof(Exception));

			Assert.IsNotNull(tas);
			Assert.IsEmpty(tas);
		}

		[Test]
		public void ShouldGetErrors()
		{
			MockException mockException;
			string message;

			try
			{
				try
				{
					throw new InvalidOperationException("ioe.collected.outer", new DivideByZeroException("dbze.collected.inner"));
				}
				catch (Exception ex)
				{
					mockException = new MockException("me.outer", new BadImageFormatException("bie.inner"));
					mockException.CollectedExceptions.Add(ex);

					throw mockException;
				}
			}
			catch (Exception ex)
			{
				message = ReflectionExtensions.GetErrors(ex, 0);

				Console.WriteLine(message);
			}
		}

		[Test]
		public void ShouldGetNoAttributesTest()
		{
			ReflectionExtensions.GetZeroAttributes<AssemblyDescriptionAttribute>(typeof(MockTestAttributedClass));
		}

		[Test]
		public void ShouldGetNullAttributeTest()
		{
			MockMultipleTestAttibute ta;

			ta = ReflectionExtensions.GetOneAttribute<MockMultipleTestAttibute>(typeof(Exception));

			Assert.IsNull(ta);
		}

		[Test]
		public void ShouldMakeNonNullableTypeTest()
		{
			Type conversionType;
			Type nonNullableType;

			conversionType = typeof(int);
			nonNullableType = ReflectionExtensions.MakeNonNullableType(conversionType);
			Assert.AreEqual(typeof(int), nonNullableType);

			conversionType = typeof(int?);
			nonNullableType = ReflectionExtensions.MakeNonNullableType(conversionType);
			Assert.AreEqual(typeof(int), nonNullableType);

			conversionType = typeof(IDisposable);
			nonNullableType = ReflectionExtensions.MakeNonNullableType(conversionType);
			Assert.AreEqual(typeof(IDisposable), nonNullableType);
		}

		[Test]
		public void ShouldMakeNullableTypeTest()
		{
			Type conversionType;
			Type nullableType;

			conversionType = typeof(int);
			nullableType = ReflectionExtensions.MakeNullableType(conversionType);
			Assert.AreEqual(typeof(int?), nullableType);

			conversionType = typeof(int?);
			nullableType = ReflectionExtensions.MakeNullableType(conversionType);
			Assert.AreEqual(typeof(int?), nullableType);

			conversionType = typeof(IDisposable);
			nullableType = ReflectionExtensions.MakeNullableType(conversionType);
			Assert.AreEqual(typeof(IDisposable), nullableType);
		}

		[Test]
		public void ShouldReflectionOnlyGetLogicalPropertyTypeTest()
		{
			MockObject mockObject;
			string propertyName;
			Type propertyType;
			bool result;

			string _unusedString = null;

			// null, null
			mockObject = null;
			propertyName = null;

			result = ReflectionExtensions.GetLogicalPropertyType(mockObject, propertyName, out propertyType);

			Assert.IsFalse(result);
			Assert.IsNull(propertyType);

			// null, ""
			mockObject = null;
			propertyName = string.Empty;

			result = ReflectionExtensions.GetLogicalPropertyType(mockObject, propertyName, out propertyType);

			Assert.IsFalse(result);
			Assert.IsNull(propertyType);

			// null, "PropName"
			mockObject = null;
			propertyName = "FirstName";

			result = ReflectionExtensions.GetLogicalPropertyType(mockObject, propertyName, out propertyType);

			Assert.IsFalse(result);
			Assert.IsNull(propertyType);

			// !null, null
			mockObject = new MockObject();
			propertyName = null;

			result = ReflectionExtensions.GetLogicalPropertyType(mockObject, propertyName, out propertyType);

			Assert.IsFalse(result);
			Assert.IsNull(propertyType);

			// !null, ""
			mockObject = new MockObject();
			propertyName = string.Empty;

			result = ReflectionExtensions.GetLogicalPropertyType(mockObject, propertyName, out propertyType);

			Assert.IsFalse(result);
			Assert.IsNull(propertyType);

			// !null, "PropName"
			mockObject = new MockObject();
			mockObject.FirstName = "john";
			propertyName = "FirstName";

			result = ReflectionExtensions.GetLogicalPropertyType(mockObject, propertyName, out propertyType);

			Assert.IsTrue(result);
			Assert.IsNotNull(propertyType);
			Assert.AreEqual(typeof(string), propertyType);

			// !null, "PropName:PropName!!!getter"
			mockObject = new MockObject();
			mockObject.FirstName = "john";
			propertyName = "NoGetter";

			result = ReflectionExtensions.GetLogicalPropertyType(mockObject, propertyName, out propertyType);

			Assert.IsTrue(result);
			Assert.IsNotNull(propertyType);
			Assert.AreEqual(typeof(object), propertyType);

			// !null, "PropName:PropName!!!setter"
			mockObject = new MockObject();
			mockObject.FirstName = "john";
			propertyName = "NoSetter";

			result = ReflectionExtensions.GetLogicalPropertyType(mockObject, propertyName, out propertyType);

			Assert.IsTrue(result);
			Assert.IsNotNull(propertyType);
			Assert.AreEqual(typeof(object), propertyType);
		}

		[Test]
		public void ShouldReflectionOnlyGetLogicalPropertyValueTest()
		{
			MockObject mockObject;
			string propertyName;
			object propertyValue;
			bool result;

			string _unusedString = null;

			// null, null
			mockObject = null;
			propertyName = null;

			result = ReflectionExtensions.GetLogicalPropertyValue(mockObject, propertyName, out propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// null, ""
			mockObject = null;
			propertyName = string.Empty;

			result = ReflectionExtensions.GetLogicalPropertyValue(mockObject, propertyName, out propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// null, "PropName"
			mockObject = null;
			propertyName = "FirstName";

			result = ReflectionExtensions.GetLogicalPropertyValue(mockObject, propertyName, out propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// !null, null
			mockObject = new MockObject();
			propertyName = null;

			result = ReflectionExtensions.GetLogicalPropertyValue(mockObject, propertyName, out propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// !null, ""
			mockObject = new MockObject();
			propertyName = string.Empty;

			result = ReflectionExtensions.GetLogicalPropertyValue(mockObject, propertyName, out propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// !null, "PropName"
			mockObject = new MockObject();
			mockObject.FirstName = "john";
			propertyName = "FirstName";

			result = ReflectionExtensions.GetLogicalPropertyValue(mockObject, propertyName, out propertyValue);

			Assert.IsTrue(result);
			Assert.IsNotNull(propertyValue);
			Assert.AreEqual("john", propertyValue);

			// !null, "PropName:PropName!!!getter"
			mockObject = new MockObject();
			mockObject.FirstName = "john";
			propertyName = "NoGetter";

			result = ReflectionExtensions.GetLogicalPropertyValue(mockObject, propertyName, out propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// !null, "PropName:PropName!!!setter"
			mockObject = new MockObject();
			mockObject.FirstName = "john";
			propertyName = "NoSetter";

			result = ReflectionExtensions.GetLogicalPropertyValue(mockObject, propertyName, out propertyValue);

			Assert.IsTrue(result);
			Assert.IsNotNull(propertyValue);
			Assert.AreEqual(1, propertyValue);
		}

		[Test]
		public void ShouldReflectionOnlySetLogicalPropertyValueTest()
		{
			MockObject mockObject;
			string propertyName;
			object propertyValue;
			bool result;

			string _unusedString = null;
			object _unusedObject = null;
			Type _unusedType = null;

			propertyValue = null;

			// null, null
			mockObject = null;
			propertyName = null;

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// null, ""
			mockObject = null;
			propertyName = string.Empty;

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// null, "PropName"
			mockObject = null;
			propertyName = "FirstName";

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// !null, null
			mockObject = new MockObject();
			propertyName = null;

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// !null, ""
			mockObject = new MockObject();
			propertyName = string.Empty;

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue);

			Assert.IsFalse(result);
			Assert.IsNull(propertyValue);

			// !null, "PropName"
			mockObject = new MockObject();
			propertyName = "FirstName";
			propertyValue = "john";

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue);

			Assert.IsTrue(result);
			Assert.IsNotNull(propertyValue);
			Assert.AreEqual("john", propertyValue);

			// !null, "PropName:PropName!!!getter"
			mockObject = new MockObject();
			propertyName = "NoGetter";
			propertyValue = "john";

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue);

			Assert.IsTrue(result);
			Assert.IsNotNull(propertyValue);
			Assert.AreEqual("john", propertyValue);

			// !null, "PropName:PropName!!!setter"
			mockObject = new MockObject();
			mockObject.FirstName = "john";
			propertyName = "NoSetter";

			result = ReflectionExtensions.SetLogicalPropertyValue(mockObject, propertyName, propertyValue);

			Assert.IsFalse(result);
		}

		#endregion
	}
}