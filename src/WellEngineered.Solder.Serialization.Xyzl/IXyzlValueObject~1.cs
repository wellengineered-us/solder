/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Serialization.Xyzl
{
	/// <summary>
	/// Represents a value-based configuration object and it's "name".
	/// </summary>
	public interface IXyzlValue<TValue> : IXyzlValue
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		new TValue Value
		{
			get;
			set;
		}

		#endregion
	}
}