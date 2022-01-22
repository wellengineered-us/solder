/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Serialization.Xyzl
{
	/// <summary>
	/// Represents a XYZL object and it's "name".
	/// </summary>
	public interface IXyzlModel : ISolderConfiguration
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets or sets the configuration object name.
		/// </summary>
		IXyzlName Name
		{
			get;
			set;
		}

		#endregion
	}
}