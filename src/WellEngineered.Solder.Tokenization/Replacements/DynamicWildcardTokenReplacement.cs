﻿/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Linq;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Tokenization.Replacements
{
	/// <summary>
	/// Provides a wildcard token replacement strategy which returns the data using reflection or dictionary semantics against an object property token.
	/// </summary>
	public partial class DynamicWildcardTokenReplacement
		: TokenReplacement
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the DynamicWildcardTokenReplacement class. This overload overrides the default strict setting (true).
		/// </summary>
		/// <param name="targets"> The target object instances to evaluate (in linear order) during wildcard token replacement. </param>
		/// <param name="strict"> A value indicating if exceptions are thrown for bad token matches. </param>
		public DynamicWildcardTokenReplacement(object[] targets, bool strict)
		{
			this.targets = targets;
			this.strict = strict;
		}

		#endregion

		#region Fields/Constants

		private readonly bool strict;
		private readonly object[] targets;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets a value indicating whether strict matching semantics are enabled.
		/// </summary>
		public bool Strict
		{
			get
			{
				return this.strict;
			}
		}

		/// <summary>
		/// Gets the target object instance value to evaluate (in linear order) during wildcard token replacement.
		/// </summary>
		public object[] Targets
		{
			get
			{
				return this.targets;
			}
		}

		#endregion

		#region Methods/Operators

		protected override object CoreEvaluate(string[] parameters)
		{
			object value;

			this.GetByToken(null, out value); // or fail if strict

			return value;
		}

		/// <summary>
		/// Evaluate a token using any parameters specified.
		/// </summary>
		/// <param name="token"> The wildcard token to evaludate. </param>
		/// <param name="parameters"> Should be null for value semantics; or a valid object array for function semantics. </param>
		/// <returns> An appropriate token replacement value. </returns>
		protected override object CoreEvaluate(string token, object[] parameters)
		{
			object value;

			this.GetByToken(token, out value); // or fail if strict

			return value;
		}

		/// <summary>
		/// Gets a value by token from the array of target objects. This method obeys the strict matching semantics flag in effect and if enabled, will throw an exception on token lookup failure.
		/// </summary>
		/// <param name="token"> The logical token (i.e. property name, dictionary key, etc.) to lookup. </param>
		/// <param name="value"> The output value or null if the token was not found. </param>
		/// <returns> A value indicating whether the token was found in the array of target objects. </returns>
		public bool GetByToken(string _token, out object value)
		{
			string firstToken, rawToken = null;
			string[] tokens;
			object tokenLogicalValue, tokenReplacementValue = null;

			value = null;
			rawToken = _token;

			if (string.IsNullOrWhiteSpace(rawToken))
			{
				if (this.Strict)
					throw new InvalidOperationException(string.Format("Failed to get value for empty token"));
				else
					return false;
			}

			// break any token paths into token list
			tokens = rawToken.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

			if ((object)tokens == null ||
				tokens.Length <= 0)
			{
				if (this.Strict)
					throw new InvalidOperationException(string.Format("Failed to get value for empty token"));
				else
					return false;
			}

			firstToken = tokens[0];
			tokens = tokens.Skip(1).ToArray();

			if ((object)this.Targets != null)
			{
				foreach (object target in this.Targets)
				{
					if (target.GetLogicalPropertyValue(firstToken, out tokenReplacementValue))
					{
						if ((object)tokens == null ||
							tokens.Length <= 0)
						{
							value = tokenReplacementValue;
							return true;
						}

						tokenLogicalValue = tokenReplacementValue;
						foreach (string token in tokens)
						{
							// only do logical lookup here
							if (!target.GetLogicalPropertyValue(token, out tokenLogicalValue))
							{
								if (this.Strict)
									throw new InvalidOperationException(string.Format("Failed to get value for token '{0}'", _token));
								else
									return false;
							}
						}

						value = tokenLogicalValue;
						return true;
					}
				}
			}

			if (this.Strict)
				throw new InvalidOperationException(string.Format("Failed to get value for token '{0}'", _token));

			return false;
		}

		/// <summary>
		/// Sets a value by token to the array of target objects. This method obeys the strict matching semantics flag in effect and if enabled, will throw an exception on token lookup failure.
		/// </summary>
		/// <param name="token"> The logical token (i.e. property name, dictionary key, etc.) to lookup. </param>
		/// <param name="value"> The value to set or null. </param>
		/// <returns> A value indicating whether the token was found in the array of target objects. </returns>
		public bool SetByToken(string token, object value)
		{
			object unused;

			this.GetByToken(token, out unused); // or fail if strict

			if ((object)this.Targets != null)
			{
				foreach (object target in this.Targets)
				{
					if (target.SetLogicalPropertyValue(token, value, false, false))
						return true;
				}
			}

			if (this.Strict)
				throw new InvalidOperationException(string.Format("Failed to set value for token '{0}", token));

			return false;
		}

		#endregion
	}
}