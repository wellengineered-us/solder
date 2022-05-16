/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Configuration
{
	/// <summary>
	/// Represents a configuration object collection.
	/// </summary>
	public partial interface ISolderConfigurationCollection
		: IList,
			IValidatable
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the site configuration object or null if this instance is unattached.
		/// </summary>
		ISolderConfiguration Site
		{
			get;
		}

		#endregion
	}
}