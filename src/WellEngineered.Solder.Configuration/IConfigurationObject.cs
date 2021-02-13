/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace WellEngineered.Solder.Configuration
{
	/// <summary>
	/// Represents a configuration object and it's "schema".
	/// </summary>
	public interface IConfigurationObject
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets an enumerable of allowed child configuration object types.
		/// </summary>
		IEnumerable<Type> AllowedChildTypes
		{
			get;
		}

		/// <summary>
		/// Gets a list of configuration object items.
		/// </summary>
		IConfigurationObjectCollection Items
		{
			get;
		}

		/// <summary>
		/// Gets or sets the optional single configuration content object.
		/// </summary>
		IConfigurationObject Content
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the parent configuration object or null if this is the configuration root.
		/// </summary>
		IConfigurationObject Parent
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the surrounding configuration object or null if this is not surrounded (in a collection).
		/// </summary>
		IConfigurationObjectCollection Surround
		{
			get;
			set;
		}

		#endregion
	}
}