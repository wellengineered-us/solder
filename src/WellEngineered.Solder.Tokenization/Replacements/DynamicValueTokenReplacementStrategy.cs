﻿/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Reflection;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Tokenization.Replacements
{
	/// <summary>
	/// Provides a dynamic token replacement strategy which executes an on-demand callback method to obtain a replacement value.
	/// </summary>
	public partial class DynamicValueTokenReplacement
		: TokenReplacement
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the DynamicValueTokenReplacement class.
		/// </summary>
		/// <param name="method"> The callback method to evaluate during token replacement. </param>
		public DynamicValueTokenReplacement(Func<string[], object> method)
		{
			if ((object)method == null)
				throw new ArgumentNullException(nameof(method));

			this.method = method;
		}

		#endregion

		#region Fields/Constants

		private readonly Func<string[], object> method;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the callback method to evaluate during token replacement.
		/// </summary>
		public Func<string[], object> Method
		{
			get
			{
				return this.method;
			}
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// FOR TESTING USE ONLY
		/// </summary>
		/// <returns> A value. </returns>
		public static object StaticMethodResolverPing()
		{
			return DateTime.UtcNow.Ticks;
		}

		/// <summary>
		/// FOR TESTING USE ONLY
		/// </summary>
		/// <returns> A value. </returns>
		public static object StaticMethodResolverPing(int? value)
		{
			return DateTime.UtcNow.Ticks / value;
		}

		/// <summary>
		/// FOR TESTING USE ONLY
		/// </summary>
		/// <returns> A value. </returns>
		public static object StaticMethodResolverPing(string value)
		{
			return (value ?? string.Empty).ToUpper();
		}

		/// <summary>
		/// Used by the token model to get the value of public, static properties with zero or more parameters in a dynamic manner.
		/// </summary>
		/// <param name="parameters"> An array of parameters in the form: assembly-qualified-type-name, property-name, [parameter-assembly-qualified-type-name, indexer-value, ...] </param>
		/// <returns> The return value of the property getter. </returns>
		public static object StaticPropertyResolver(string[] parameters)
		{
			Type targetType;
			PropertyInfo propertyInfo;
			object propertyValue = null;

			MethodInfo getter, setter;

			if ((object)parameters == null)
				throw new ArgumentNullException(nameof(parameters));

			if (parameters.Length != 2)
				throw new InvalidOperationException(string.Format("StaticPropertyResolver requires at most two parameters but was invoked with '{0}'. USAGE: StaticPropertyResolver(`assembly-qualified-type-name`, `static-property-name`)", parameters.Length));

			// assembly-qualified-type-name
			targetType = Type.GetType(parameters[0], false);

			if ((object)targetType == null)
				throw new InvalidOperationException(string.Format("StaticPropertyResolver parameter at index '{0}' with value '{1}' was not a valid, loadable CLR type.", 0, parameters[0]));

			// property-name
			propertyInfo = targetType.GetProperty(parameters[1], null, new Type[] { });

			if ((object)propertyInfo == null)
				throw new InvalidOperationException(string.Format("StaticPropertyResolver parameter at index '{0}' with value '{1}' was not a valid, executable property name of type '{2}'.", 1, parameters[1], targetType.FullName));

			// BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static
			getter = propertyInfo.GetGetMethod();
			setter = propertyInfo.GetGetMethod();
			if ((object)propertyInfo.DeclaringType != targetType ||
				((object)getter != null && (!getter.IsPublic || !getter.IsStatic)) ||
				((object)setter != null && (!setter.IsPublic || !setter.IsStatic)))
				throw new InvalidOperationException(string.Format("StaticPropertyResolver parameter at index '{0}' with value '{1}' was not a valid, executable property name of type '{2}'.", 1, parameters[1], targetType.FullName));

			if (!propertyInfo.CanRead)
				throw new InvalidOperationException(string.Format("StaticPropertyResolver parameter at index '{0}' with value '{1}' was not a valid, readable property name of type '{2}'.", 1, parameters[1], targetType.FullName));

			propertyValue = propertyInfo.GetValue(null, new object[] { });

			return propertyValue;
		}

		/// <summary>
		/// Evaluate a token using any parameters specified.
		/// </summary>
		/// <param name="parameters"> Should be null for value semantics; or a string object array for function semantics. </param>
		/// <returns> An appropriate token replacement value. </returns>
		protected override object CoreEvaluate(string[] parameters)
		{
			return this.Method(parameters);
		}

		protected override object CoreEvaluate(string token, object[] parameters)
		{
			string[] parameterz;

			if ((object)parameters != null)
			{
				parameterz = new string[parameters.Length];

				for (int i = 0; i < parameterz.Length; i++)
					parameterz[i] = parameters[i] is string ? (string)parameters[i] : parameters[i].ToStringEx();
			}
			else
			{
				parameterz = new string[] { };
			}

			return this.Method(parameterz);
		}

		/// <summary>
		/// Used by the token model to execute public, static methods with zero or more parameters in a dynamic manner.
		/// </summary>
		/// <param name="parameters"> An array of parameters in the form: assembly-qualified-type-name, method-name, [parameter-assembly-qualified-type-name, argument-value, ...] </param>
		/// <returns> The return value of the executed method. </returns>
		public object StaticMethodResolver(string[] parameters)
		{
			Type targetType, parameterType = null;
			List<Type> parameterTypes;
			MethodInfo methodInfo;
			ParameterInfo[] parameterInfos;
			object returnValue, argumentValue;
			List<object> argumentValues;

			if ((object)parameters == null)
				throw new ArgumentNullException(nameof(parameters));

			if (parameters.Length < 2)
				throw new InvalidOperationException(string.Format("StaticMethodResolver requires at least two parameters but was invoked with '{0}'. USAGE: StaticMethodResolver(`assembly-qualified-type-name`, `static-method-name`, [`type0`, `arg0`...])", parameters.Length));

			if ((parameters.Length % 2) != 0)
				throw new InvalidOperationException(string.Format("StaticMethodResolver was invoked with an odd number '{0}' of parameters.", parameters.Length));

			parameterTypes = new List<Type>();
			argumentValues = new List<object>();

			// assembly-qualified-type-name
			targetType = Type.GetType(parameters[0], false);

			if ((object)targetType == null)
				throw new InvalidOperationException(string.Format("StaticMethodResolver parameter at index '{0}' with value '{1}' was not a valid, loadable CLR type.", 0, parameters[0]));

			// skip first two parameters
			for (int i = 2; i < parameters.Length; i++)
			{
				if ((i % 2) == 0) // parameter type
				{
					// assembly-qualified-type-name
					parameterType = Type.GetType(parameters[i], false);

					if ((object)parameterType == null)
						throw new InvalidOperationException(string.Format("StaticMethodResolver parameter at index '{0}' with value '{1}' was not a valid, loadable CLR type (for method call parameter).", i, parameters[i]));

					parameterTypes.Add(parameterType);
				}
				else // argument value
				{
					if (!parameters[i].TryParse(parameterType, out argumentValue))
						throw new InvalidOperationException(string.Format("StaticMethodResolver parameter at index '{0}' with value '{1}' was not a valid '{2}'.", i, parameters[i], parameterType.FullName));

					argumentValues.Add(argumentValue);
				}
			}

			// method-name
			methodInfo = targetType.GetMethod(parameters[1], parameterTypes.ToArray());

			if ((object)methodInfo == null)
				throw new InvalidOperationException(string.Format("StaticMethodResolver parameter at index '{0}' with value '{1}' was not a valid, executable method name of type '{2}'.", 1, parameters[1], targetType.FullName));

			// BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static
			if (methodInfo.DeclaringType != targetType || !methodInfo.IsPublic || !methodInfo.IsStatic)
				throw new InvalidOperationException(string.Format("StaticMethodResolver parameter at index '{0}' with value '{1}' was not a valid, executable method name of type '{2}'.", 1, parameters[1], targetType.FullName));

			parameterInfos = methodInfo.GetParameters();

			if (parameterInfos.Length != ((parameters.Length - 2) / 2))
				throw new InvalidOperationException(string.Format("StaticMethodResolver was invoked with '{0}' but the static method '{1}' of type '{2}' requires '{3}' parameters.", ((parameters.Length - 2) / 2), methodInfo.Name, targetType.FullName, parameterInfos.Length));

			returnValue = methodInfo.Invoke(null, argumentValues.ToArray());

			return returnValue;
		}

		#endregion
	}
}