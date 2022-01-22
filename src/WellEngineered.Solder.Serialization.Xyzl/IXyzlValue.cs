/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Serialization.Xyzl
{
	/// <summary>
	/// Represents a value-based XYZL object and it's "name".
	/// </summary>
	public interface IXyzlValue : IXyzlModel
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		object Value
		{
			get;
			set;
		}

		#endregion
	}
}