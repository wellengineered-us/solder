/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Executive
{
	public partial interface IExecutableApplication : ILifecycle
	{
		#region Methods/Operators

		int Run(string[] args);

		#endregion
	}
}