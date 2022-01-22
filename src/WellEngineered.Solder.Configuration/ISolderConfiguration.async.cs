/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Configuration
{
	/// <summary>
	/// Represents a configuration object and it's "schema".
	/// </summary>
	public partial interface ISolderConfiguration
		: IAsyncValidatable
	{
	}
}
#endif