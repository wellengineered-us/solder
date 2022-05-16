/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace WellEngineered.Solder.Primitives
{
	public static class ReflectionExtensions
	{
		#region Methods/Operators

		/// <summary>
		/// Performs a run-time type change on a given value.
		/// </summary>
		/// <typeparam name="T"> The type to change value to. </typeparam>
		/// <param name="value"> The value to change type. </param>
		/// <returns> A value changed to the given type. </returns>
		public static T ChangeType<T>(this object value)
		{
			return (T)value.ChangeType(typeof(T));
		}

		/// <summary>
		/// Performs a run-time type change on a given value.
		/// </summary>
		/// <param name="value"> The value to change type. </param>
		/// <param name="conversionType"> The type to change value to. </param>
		/// <returns> A value changed to the given type. </returns>
		public static object ChangeType(this object value, Type conversionType)
		{
			if ((object)conversionType == null)
				throw new ArgumentNullException(nameof(conversionType));

			if ((object)value == null || value == DBNull.Value)
				return conversionType.DefaultValue();

			if (conversionType.IsAssignableFrom(value.GetType()))
				return value;

			if (conversionType.IsGenericType &&
				!conversionType.IsGenericTypeDefinition &&
				conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
				conversionType = Nullable.GetUnderlyingType(conversionType);

			return Convert.ChangeType(value, conversionType);
		}

		public static TTarget CreateInstanceAssignableToTargetType<TTarget>(this Type instantiateType, bool throwOnError = true)
		{
			Type targetType;
			object obj;

			if ((object)instantiateType == null)
				throw new ArgumentNullException(nameof(instantiateType));

			targetType = typeof(TTarget);

			// TTarget obj = new [instantiateType]() ???
			if (!targetType.IsAssignableFrom(instantiateType))
			{
				if (throwOnError)
					throw new InvalidOperationException(string.Format("Target type '{0}' is not assignable from instantiate type '{1}'.", targetType, instantiateType));
				else
					return default;
			}

			obj = Activator.CreateInstance(instantiateType);

			return (TTarget)obj;
		}

		/// <summary>
		/// Obtains the default value for a given type using reflection.
		/// </summary>
		/// <param name="targetType"> The target type. </param>
		/// <returns> The default value for the target type. </returns>
		public static object DefaultValue(this Type targetType)
		{
			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
		}

		/// <summary>
		/// Checks whether two object instances are equal using the Object.Equals() method. Value coercion is performed.
		/// </summary>
		/// <param name="objA"> An object instance or null. </param>
		/// <param name="objB"> Another object instance or null. </param>
		/// <returns> A value indicating whether the two object instances are equal. </returns>
		public static bool EqualByValue(this object objA, object objB)
		{
			Type typeOfA = null, typeOfB = null;

			if (((object)objA != null && (object)objB == null) ||
				((object)objA == null && (object)objB != null))
				return false; // prevent null coalescence

			if ((object)objA != null)
				typeOfA = objA.GetType();

			if ((object)objB != null)
				typeOfB = objB.GetType();

			return ((object)objA != null ? objA.Equals(ChangeType(objB, typeOfA)) : ((object)objB != null ? objB.Equals(ChangeType(objA, typeOfB)) : true /* both null */));
		}

		/// <summary>
		/// Gets all custom attributes of the specified type. If no custom attributes of the specified type are defined, then null is returned.
		/// </summary>
		/// <typeparam name="TAttribute"> The target object (Assembly, Type, MemberInfo, etc.) </typeparam>
		/// <param name="target"> The target object. </param>
		/// <returns> The custom attributes array or null. </returns>
		public static TAttribute[] GetAllAttributes<TAttribute>(this ICustomAttributeProvider target)
			where TAttribute : Attribute
		{
			Type attributeType;
			IEnumerable<TAttribute> attributes;

			if ((object)target == null)
				throw new ArgumentNullException(nameof(target));

			attributeType = typeof(TAttribute);
			attributes = target.GetCustomAttributes(attributeType, true).Cast<TAttribute>();

			if ((object)attributes != null)
				return attributes.ToArray();
			else
				return null;
		}

		public static AssemblyInformation GetAssemblyInformation(this Assembly assembly)
		{
			AssemblyTitleAttribute aTiA;
			AssemblyVersionAttribute ava;
			AssemblyDescriptionAttribute ada;
			AssemblyProductAttribute apa;
			AssemblyCopyrightAttribute aCopA;
			AssemblyCompanyAttribute aComA;
			AssemblyConfigurationAttribute aConA;
			AssemblyFileVersionAttribute afva;
			AssemblyInformationalVersionAttribute aiva;
			AssemblyTrademarkAttribute aTrA;

			if ((object)assembly == null)
				throw new ArgumentNullException(nameof(assembly));

			aTiA = assembly.GetOneAttribute<AssemblyTitleAttribute>();
			string title = aTiA?.Title;

			ava = assembly.GetOneAttribute<AssemblyVersionAttribute>();
			string assemblyVersion = ava?.Version;

			ada = assembly.GetOneAttribute<AssemblyDescriptionAttribute>();
			string description = ada?.Description;

			apa = assembly.GetOneAttribute<AssemblyProductAttribute>();
			string product = apa?.Product;

			aCopA = assembly.GetOneAttribute<AssemblyCopyrightAttribute>();
			string copyright = aCopA?.Copyright;

			aComA = assembly.GetOneAttribute<AssemblyCompanyAttribute>();
			string company = aComA?.Company;

			aConA = assembly.GetOneAttribute<AssemblyConfigurationAttribute>();
			string configuration = aConA?.Configuration;

			afva = assembly.GetOneAttribute<AssemblyFileVersionAttribute>();
			string nativeFileVersion = afva?.Version;

			aiva = assembly.GetOneAttribute<AssemblyInformationalVersionAttribute>();
			string informationalVersion = aiva?.InformationalVersion;

			aTrA = assembly.GetOneAttribute<AssemblyTrademarkAttribute>();
			string trademark = aTrA?.Trademark;

			string moduleName = assembly.ManifestModule?.Name;

			return new AssemblyInformation(assemblyVersion, company, configuration,
				copyright, description, informationalVersion, moduleName,
				nativeFileVersion, product, title, trademark);
		}

		/// <summary>
		/// Returns the concatenation of error messages from an exception object. All inner exceptions and collected exceptions (public properties implementing IEnumerable&lt;Exception&gt;) are returned.
		/// </summary>
		/// <param name="exception"> The root exception to get errors. </param>
		/// <param name="indent"> The indent level count. </param>
		/// <returns> A string concatenation of error messages delimited by newlines. </returns>
		public static string GetErrors(this Exception exception, int indent)
		{
			PropertyInfo[] propertyInfos;
			Type exceptionType, exceptionEnumerableType;
			string message = string.Empty;
			object propertyValue;
			bool first = true;
			const char INDENT_CHAR = '\t';

			exceptionEnumerableType = typeof(IEnumerable<Exception>);

			while ((object)exception != null)
			{
				if (first)
				{
					message +=
						new string(INDENT_CHAR, indent) + "+++ BEGIN ROOT EXECPTION +++" + Environment.NewLine +
						new string(INDENT_CHAR, indent) + "Type: " + exception.GetType().FullName + Environment.NewLine +
						new string(INDENT_CHAR, indent) + "Message: " + exception.Message + Environment.NewLine +
						new string(INDENT_CHAR, indent) + "Stack:" + Environment.NewLine + new string(INDENT_CHAR, indent) + exception.StackTrace + Environment.NewLine + Environment.NewLine;
				}
				else
				{
					message += Environment.NewLine +
								new string(INDENT_CHAR, indent) + "+++ BEGIN INNER EXECPTION +++" + Environment.NewLine +
								new string(INDENT_CHAR, indent) + "Type: " + exception.GetType().FullName + Environment.NewLine +
								new string(INDENT_CHAR, indent) + "Message: " + exception.Message + Environment.NewLine +
								new string(INDENT_CHAR, indent) + "Stack:" + Environment.NewLine + new string(INDENT_CHAR, indent) + exception.StackTrace + Environment.NewLine + Environment.NewLine;
				}

				exceptionType = exception.GetType();
				propertyInfos = exceptionType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

				if ((object)propertyInfos != null)
				{
					foreach (PropertyInfo propertyInfo in propertyInfos)
					{
						if (propertyInfo.CanRead &&
							exceptionEnumerableType.IsAssignableFrom(propertyInfo.PropertyType))
						{
							propertyValue = propertyInfo.GetValue(exception, null);

							message += new string(INDENT_CHAR, indent + 1) + "+++ BEGIN ENUMERABLE EXECPTIONS +++" + Environment.NewLine;

							if ((object)propertyValue != null)
							{
								foreach (Exception subException in (IEnumerable<Exception>)propertyValue)
									message += Environment.NewLine + subException.GetErrors(indent + 1);
							}

							message += Environment.NewLine + new string(INDENT_CHAR, indent + 1) + "+++ END ENUMERABLE EXECPTIONS +++" + Environment.NewLine + Environment.NewLine;
						}
					}
				}

				if (first)
				{
					message +=
						new string(INDENT_CHAR, indent) + "+++ END ROOT EXECPTION +++" + Environment.NewLine;
				}
				else
				{
					message +=
						new string(INDENT_CHAR, indent) + "+++ END INNER EXECPTION +++" + Environment.NewLine;
				}

				first = false;
				exception = exception.InnerException;
			}

			return message;
		}

		/// <summary>
		/// Attempts to get the property type for a logical property (CLR, associative, etc.).
		/// </summary>
		/// <param name="targetInstance"> The target instance to search for a logical property. </param>
		/// <param name="propertyName"> The logical property name to get the type for. </param>
		/// <param name="propertyType"> An output run-time type of the logical property or null if the logical property lookup failed. </param>
		/// <returns> A value indicating whether the logical property name lookup was successful or not. </returns>
		public static bool GetLogicalPropertyType(this object targetInstance, string propertyName, out Type propertyType)
		{
			bool trigger = false;
			Type targetType;
			PropertyInfo propertyInfo;
			IDictionary targetDictionary;
			object propertyValue;

			propertyType = null;

			if ((object)targetInstance == null)
				return false;

			if (string.IsNullOrWhiteSpace(propertyName))
				return false;

			// STRATEGY: attempt reflection
			if (!trigger)
			{
				targetType = targetInstance.GetType();

				propertyInfo = targetType.GetLowestProperty(propertyName);

				if ((object)propertyInfo != null)
				{
					propertyType = propertyInfo.PropertyType;
					trigger = true;
				}
			}

			// STRATEGY: attempt association
			if (!trigger)
			{
				targetDictionary = targetInstance as IDictionary;

				if ((object)targetDictionary != null &&
					targetDictionary.Contains(propertyName))
				{
					propertyValue = targetDictionary[propertyName];

					if ((object)propertyValue != null)
					{
						propertyType = propertyValue.GetType();
						trigger = true;
					}
				}
			}

			return trigger;
		}

		/// <summary>
		/// Attempts to get the property value for a logical property (CLR, associative, etc.).
		/// </summary>
		/// <param name="targetInstance"> The target instance to search for a logical property. </param>
		/// <param name="propertyName"> The logical property name to get the value for. </param>
		/// <param name="propertyValue"> An output run-time value of the logical property or null if the logical property lookup failed. </param>
		/// <returns> A value indicating whether the logical property name lookup was successful or not. </returns>
		public static bool GetLogicalPropertyValue(this object targetInstance, string propertyName, out object propertyValue)
		{
			bool trigger = false;
			Type targetType;
			PropertyInfo propertyInfo;
			IDictionary targetDictionary;

			propertyValue = null;

			if ((object)targetInstance == null)
				return false;

			if (string.IsNullOrWhiteSpace(propertyName))
				return false;

			// STRATEGY: attempt reflection
			if (!trigger)
			{
				targetType = targetInstance.GetType();

				propertyInfo = targetType.GetLowestProperty(propertyName);

				if ((object)propertyInfo != null &&
					propertyInfo.CanRead)
				{
					propertyValue = propertyInfo.GetValue(targetInstance, null);
					trigger = true;
				}
			}

			// STRATEGY: attempt association
			if (!trigger)
			{
				targetDictionary = targetInstance as IDictionary;

				if ((object)targetDictionary != null &&
					targetDictionary.Contains(propertyName))
				{
					propertyValue = targetDictionary[propertyName];
					trigger = true;
				}
			}

			return trigger;
		}

		/// <summary>
		/// Used to obtain the least-derived public, instance property of a given name.
		/// </summary>
		/// <param name="propertyType"> The property type to interrogate. </param>
		/// <param name="propertyName"> The property name to lookup. </param>
		/// <returns> A PropertyInfo for the least-derived public, instance property by the given name or null if none were found. </returns>
		public static PropertyInfo GetLowestProperty(this Type propertyType, string propertyName)
		{
			PropertyInfo property;

			while ((object)propertyType != null)
			{
				TypeInfo _propertyTypeInfo = propertyType.GetTypeInfo();

				property = propertyType.GetProperty(propertyName, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

				if ((object)property != null)
					return property;

				propertyType = _propertyTypeInfo.BaseType;
			}

			return null;
		}

		/// <summary>
		/// Get the single custom attribute of the attribute specified type. If more than one custom attribute exists for the requested type, an InvalidOperationException is thrown. If no custom attributes of the specified type are defined, then null is returned.
		/// </summary>
		/// <typeparam name="TAttribute"> The custom attribute type. </typeparam>
		/// <param name="target"> The target object (Assembly, Type, MemberInfo, etc.) </param>
		/// <returns> The single custom attribute or null if none are defined. </returns>
		public static TAttribute GetOneAttribute<TAttribute>(this ICustomAttributeProvider target)
			where TAttribute : Attribute
		{
			TAttribute[] attributes;
			Type attributeType, targetType;

			if ((object)target == null)
				throw new ArgumentNullException(nameof(target));

			attributeType = typeof(TAttribute);
			targetType = target.GetType();

			attributes = target.GetAllAttributes<TAttribute>();

			if ((object)attributes == null || attributes.Length == 0)
				return null;
			else if (attributes.Length > 1)
				throw new InvalidOperationException(string.Format("Multiple custom attributes of type '{0}' are defined on type '{1}'.", attributeType.FullName, targetType.FullName));
			else
				return attributes[0];
		}

		public static Type GetTypeFromAssemblyQualifiedTypeName(string assemblyQualifiedTypeName, bool throwOnError = false, bool ignoreCase = false)
		{
			Type type;

			if (string.IsNullOrWhiteSpace(assemblyQualifiedTypeName))
			{
				if (throwOnError)
					throw new InvalidOperationException(string.Format("Assembly qualified type name is required."));
				else
					return null;
			}

			type = Type.GetType(assemblyQualifiedTypeName, throwOnError, ignoreCase);

			return type;
		}

		/// <summary>
		/// Asserts that the custom attribute is not defined on the target. If more than zero custom attributes exist for the requested type, an InvalidOperationException is thrown.
		/// </summary>
		/// <typeparam name="TAttribute"> The custom attribute type. </typeparam>
		/// <param name="target"> The target object (Assembly, Type, MemberInfo, etc.) </param>
		public static void GetZeroAttributes<TAttribute>(this ICustomAttributeProvider target)
			where TAttribute : Attribute
		{
			TAttribute[] attributes;
			Type attributeType, targetType;

			if ((object)target == null)
				throw new ArgumentNullException(nameof(target));

			attributeType = typeof(TAttribute);
			targetType = target.GetType();

			attributes = target.GetAllAttributes<TAttribute>();

			if ((object)attributes == null || attributes.Length == 0)
				return;
			else
				throw new InvalidOperationException(string.Format("Some custom attributes of type '{0}' are defined on type '{1}'.", attributeType.FullName, targetType.FullName));
		}

		/// <summary>
		/// Returns a DbType mapping for a Type.
		/// An InvalidOperationException is thrown for unmappable types.
		/// </summary>
		/// <param name="clrType"> The CLR type to map to a DbType. </param>
		/// <returns> The mapped DbType. </returns>
		public static DbType InferDbTypeForClrType(this Type clrType)
		{
			if ((object)clrType == null)
				throw new ArgumentNullException(nameof(clrType));

			if (clrType.IsByRef /* || type.IsPointer || type.IsArray */)
				return clrType.GetElementType().InferDbTypeForClrType();
			else if (clrType.IsGenericType &&
					!clrType.IsGenericTypeDefinition &&
					clrType.GetGenericTypeDefinition() == typeof(Nullable<>))
				return Nullable.GetUnderlyingType(clrType).InferDbTypeForClrType();
			else if (clrType.IsEnum)
				return Enum.GetUnderlyingType(clrType).InferDbTypeForClrType();
			else if (clrType == typeof(Boolean))
				return DbType.Boolean;
			else if (clrType == typeof(Byte))
				return DbType.Byte;
			else if (clrType == typeof(DateTime))
				return DbType.DateTime;
			else if (clrType == typeof(DateTimeOffset))
				return DbType.DateTimeOffset;
			else if (clrType == typeof(Decimal))
				return DbType.Decimal;
			else if (clrType == typeof(Double))
				return DbType.Double;
			else if (clrType == typeof(Guid))
				return DbType.Guid;
			else if (clrType == typeof(Int16))
				return DbType.Int16;
			else if (clrType == typeof(Int32))
				return DbType.Int32;
			else if (clrType == typeof(Int64))
				return DbType.Int64;
			else if (clrType == typeof(SByte))
				return DbType.SByte;
			else if (clrType == typeof(Single))
				return DbType.Single;
			else if (clrType == typeof(TimeSpan))
				return DbType.Time;
			else if (clrType == typeof(UInt16))
				return DbType.UInt16;
			else if (clrType == typeof(UInt32))
				return DbType.UInt32;
			else if (clrType == typeof(UInt64))
				return DbType.UInt64;
			else if (clrType == typeof(Byte[]))
				return DbType.Binary;
			else if (clrType == typeof(Boolean[]))
				return DbType.Byte;
			else if (clrType == typeof(String))
				return DbType.String;
			else if (clrType == typeof(XmlDocument))
				return DbType.Xml;
			else if (clrType == typeof(Object))
				return DbType.Object;
			else
				throw new InvalidOperationException(string.Format("Cannot infer parameter type from unsupported CLR type '{0}'.", clrType.FullName));
		}

		/// <summary>
		/// Transforms a nullable type to its underlying non-nullable equivalent.
		/// Simply returns an existing reference type
		/// </summary>
		/// <param name="conversionType"> The nullable run-time type to transform. </param>
		/// <returns> The non-nullbale run-time type. </returns>
		public static Type MakeNonNullableType(this Type conversionType)
		{
			Type openNullableType;

			if ((object)conversionType == null)
				throw new ArgumentNullException(nameof(conversionType));

			TypeInfo _conversionTypeInfo = conversionType.GetTypeInfo();

			openNullableType = typeof(Nullable<>);

			if (_conversionTypeInfo.IsGenericType &&
				!_conversionTypeInfo.IsGenericTypeDefinition &&
				conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
				return conversionType.GetGenericArguments()[0];

			if (_conversionTypeInfo.IsValueType)
				return conversionType;

			return conversionType; // DPB (2014-04-09: change this behavior.)
		}

		/// <summary>
		/// Transforms a nullable type to its underlying non-nullable equivalent.
		/// </summary>
		/// <param name="conversionType"> The nullable run-time type to transform. </param>
		/// <returns> The non-nullbale run-time type. </returns>
		public static Type MakeNullableType(this Type conversionType)
		{
			Type openNullableType, closedNullableType;

			if ((object)conversionType == null)
				throw new ArgumentNullException(nameof(conversionType));

			TypeInfo _conversionTypeInfo = conversionType.GetTypeInfo();

			openNullableType = typeof(Nullable<>);

			if (!_conversionTypeInfo.IsValueType)
				return conversionType;

			if (_conversionTypeInfo.IsGenericType &&
				!_conversionTypeInfo.IsGenericTypeDefinition &&
				conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
				return conversionType;

			closedNullableType = openNullableType.MakeGenericType(conversionType);

			return closedNullableType;
		}

		/// <summary>
		/// Attempts to set the property value for a logical property (CLR, associative, etc.). This overload assume stayHard=false and makeSoft=true semantics.
		/// </summary>
		/// <param name="targetInstance"> The target instance to search for a logical property. </param>
		/// <param name="propertyName"> The logical property name to set the value for. </param>
		/// <param name="propertyValue"> The value of the logical property to set or null. </param>
		/// <returns> A value indicating whether the logical property name lookup was successful or not. </returns>
		public static bool SetLogicalPropertyValue(this object targetInstance, string propertyName, object propertyValue)
		{
			return targetInstance.SetLogicalPropertyValue(propertyName, propertyValue, false, true);
		}

		/// <summary>
		/// Attempts to set the property value for a logical property (CLR, associative, etc.).
		/// </summary>
		/// <param name="targetInstance"> The target instance to search for a logical property. </param>
		/// <param name="propertyName"> The logical property name to set the value for. </param>
		/// <param name="propertyValue"> The value of the logical property to set or null. </param>
		/// <param name="stayHard"> Force only 'hard' object semantics and not use associative lookup (i.e. the target instance must be a real CLR object). </param>
		/// <param name="makeSoft"> Allow making 'soft' object semantics (i.e. the target instance could be an associative object). </param>
		/// <returns> A value indicating whether the logical property name lookup was successful or not; lookup respects the 'stayHard' and 'makeSoft' flags. </returns>
		public static bool SetLogicalPropertyValue(this object targetInstance, string propertyName, object propertyValue, bool stayHard, bool makeSoft)
		{
			bool trigger = false;
			Type targetType;
			PropertyInfo propertyInfo;
			IDictionary targetDictionary;

			if ((object)targetInstance == null)
				return false;

			if (string.IsNullOrWhiteSpace(propertyName))
				return false;

			// STRATEGY: attempt reflection
			if (!trigger)
			{
				targetType = targetInstance.GetType();

				propertyInfo = targetType.GetLowestProperty(propertyName);

				if ((object)propertyInfo != null &&
					propertyInfo.CanWrite)
				{
					propertyValue = propertyValue.ChangeType(propertyInfo.PropertyType);
					propertyInfo.SetValue(targetInstance, propertyValue, null);
					trigger = true;
				}
			}

			// STRATEGY: attempt association
			if (!trigger && !stayHard)
			{
				targetDictionary = targetInstance as IDictionary;

				if ((object)targetDictionary != null)
				{
					if (!makeSoft && !targetDictionary.Contains(propertyName))
						return false;

					if (targetDictionary.Contains(propertyName))
						targetDictionary.Remove(propertyName);

					targetDictionary.Add(propertyName, propertyValue);
					trigger = true;
				}
			}

			return trigger;
		}

		#endregion
	}
}