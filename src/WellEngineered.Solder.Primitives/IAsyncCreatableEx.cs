/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Threading;
using System.Threading.Tasks;

#if ASYNC_ALL_THE_WAY_DOWN
namespace WellEngineered.Solder.Primitives
{
	public interface IAsyncCreatableEx : IAsyncCreatable
	{
		#region Properties/Indexers/Events

		bool IsAsyncCreated
		{
			get;
		}

		#endregion

		#region Methods/Operators

		ValueTask CreateAsync(CancellationToken cancellationToken);

		#endregion
	}
}
#endif