﻿/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Executive
{
	public partial interface IExecutableApplicationFascade : ICreatableEx, IDisposableEx
	{
		#region Methods/Operators

		int EntryPoint(string[] args);

		/// <summary>
		/// Given a string array of command line arguments, this method will parse the arguments using a well know pattern match to obtain a loosely typed dictionary of key/multi-value pairs for use by applications.
		/// </summary>
		/// <param name="args"> The command line argument array to parse. </param>
		/// <returns> A loosely typed dictionary of key/multi-value pairs. </returns>
		IDictionary<string, IList<string>> ParseCommandLineArguments(string[] args);

		int ShowNestedExceptionsAndThrowBrickAtProcess(Exception e);

		/// <summary>
		/// Given a string property, this method will parse the property using a well know pattern match to obtain an output key/value pair for use by applications.
		/// </summary>
		/// <param name="arg"> The property to parse. </param>
		/// <param name="key"> The output property key. </param>
		/// <param name="value"> The output property value. </param>
		/// <returns> A value indicating if the parse was successful or not. </returns>
		bool TryParseCommandLineArgumentProperty(string arg, out string key, out string value);

		#endregion
	}
}