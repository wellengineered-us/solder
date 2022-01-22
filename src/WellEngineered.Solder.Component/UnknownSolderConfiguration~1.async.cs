/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Component
{
	public abstract partial class UnknownSolderConfiguration<TComponentSpecification>
		: UnknownSolderConfiguration,
			IUnknownSolderConfiguration<TComponentSpecification>
		where TComponentSpecification : class, ISolderSpecification, new()
	{
	}
}
#endif