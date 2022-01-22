/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

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
		#region Constructors/Destructors

		public ResourceManagerTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			using (ResourceManager resourceManager = new ResourceManager())
			{
				Guid? __ = resourceManager.Enter();
				IDisposable disposable;

				disposable = new MockDualLifecycle();
				resourceManager.Watching(__, disposable);

				disposable = new MockDualLifecycle();
				//resourceManager.Using(__, disposable);
				using (resourceManager.Using(__, disposable))
				{
				}

				resourceManager.Leave(__);

				resourceManager.Check();
			}
		}

		#endregion
	}

	internal partial class MockDualLifecycle
		: DualLifecycle
	{
		#region Methods/Operators
#if ASYNC_ALL_THE_WAY_DOWN
		
protected override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			return default;
		}
		
#endif
		protected override void CoreCreate(bool creating)
		{
		}
		
		protected override void CoreDispose(bool disposing)
		{
		}
		
		#endregion
	}
}