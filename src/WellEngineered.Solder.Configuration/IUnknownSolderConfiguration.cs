/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace WellEngineered.Solder.Configuration
{
	public interface IUnknownSolderConfiguration
		: ISolderConfiguration
	{
		#region Properties/Indexers/Events

		IDictionary<string, object> Specification
		{
			get;
		}

		Type SpecificationType
		{
			get;
		}

		string AssemblyQualifiedTypeName
		{
			get;
			set;
		}

		#endregion
	}
}