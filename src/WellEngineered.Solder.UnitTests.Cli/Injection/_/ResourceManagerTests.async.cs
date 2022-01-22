/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.UnitTests.Cli.Injection._
{
	[TestFixture]
	public partial class ResourceManagerTests
	{
		#region Methods/Operators

		[Test]
		public async ValueTask ShouldCreateTestAsync()
		{
			await using (ResourceManager resourceManager = new ResourceManager())
			{
				Guid? __ = await resourceManager.EnterAsync();
				IAsyncDisposable asyncDisposable;

				asyncDisposable = new MockDualLifecycle();
				await resourceManager.WatchingAsync(__, asyncDisposable);

				asyncDisposable = new MockDualLifecycle();
				//resourceManager.UsingAsync(__, disposable);
				await using (await resourceManager.UsingAsync(__, asyncDisposable))
				{
				}

				await resourceManager.LeaveAsync(__);

				await resourceManager.CheckAsync();
			}
		}

		#endregion

		internal partial class MockDualLifecycle
			: DualLifecycle
		{
			#region Methods/Operators

			protected override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
			{
				return default;
			}

			protected override ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
			{
				return default;
			}

			protected override void CoreCreate(bool creating)
			{
			}

			protected override void CoreDispose(bool disposing)
			{
			}

			#endregion
		}
	}
}
#endif