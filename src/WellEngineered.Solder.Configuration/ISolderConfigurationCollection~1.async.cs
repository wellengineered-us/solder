/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Collections.Generic;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Configuration
{
	/// <summary>
	/// Represents an configuration object collection.
	/// NOTE: This interface is invariant due to its use of IList`1,
	/// thus it should NOT derive/implement the non-generic version.
	/// This will be left to the class implementation for which to solve.
	/// </summary>
	/// <typeparam name="TItemConfigurationObject"> </typeparam>
	public partial interface ISolderConfigurationCollection<TItemConfigurationObject>
		: IList<TItemConfigurationObject>,
			IAsyncValidatable
		where TItemConfigurationObject : ISolderConfiguration
	{
	}
}
#endif