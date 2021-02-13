/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

namespace WellEngineered.Solder.Configuration
{
	/// <summary>
	/// Represents a configuration object collection.
	/// </summary>
	public interface IConfigurationObjectCollection : IList<IConfigurationObject>
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the site configuration object or null if this instance is unattached.
		/// </summary>
		IConfigurationObject Site
		{
			get;
		}

		#endregion
	}
}