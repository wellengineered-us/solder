/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Serialization.Xyzl
{
	public interface IXyzlName
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets or sets the local name of the configuration object.
		/// </summary>
		string LocalName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the namespace URI of the configuration object.
		/// </summary>
		string NamespaceUri
		{
			get;
			set;
		}

		#endregion
	}
}