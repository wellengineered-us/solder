/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Executive
{
	public partial interface IExecutableApplicationFascade : IAsyncCreatableEx, IAsyncDisposableEx
	{
		#region Methods/Operators

		ValueTask<int> EntryPointAsync(string[] args);

		#endregion
	}
}