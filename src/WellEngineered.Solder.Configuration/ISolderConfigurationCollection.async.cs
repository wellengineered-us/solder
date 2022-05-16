﻿/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Collections;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Configuration
{
	/// <summary>
	/// Represents a configuration object collection.
	/// </summary>
	public partial interface ISolderConfigurationCollection
		: IList,
			IAsyncValidatable
	{
	}
}
#endif