/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Component
{
	public interface ISolderComponent<TComponentConfiguration>
		: ISolderComponent1,
			IConfigurable<TComponentConfiguration>
		where TComponentConfiguration : ISolderConfiguration
	{
	}
}