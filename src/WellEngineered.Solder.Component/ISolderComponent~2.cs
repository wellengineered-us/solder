/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Component
{
	public interface ISolderComponent<TComponentConfiguration, TComponentSpecification>
		: ISolderComponent2,
			ISolderComponent<TComponentConfiguration>,
			ISpecifiable<TComponentSpecification>
		where TComponentConfiguration : IUnknownSolderConfiguration // *** DO NOT USE ARITY 1: <TComponentSpecification>
		where TComponentSpecification : ISolderSpecification, new()
	{
	}
}