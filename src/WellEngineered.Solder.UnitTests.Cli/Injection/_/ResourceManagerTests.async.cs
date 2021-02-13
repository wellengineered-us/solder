/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

using NUnit.Framework;

using WellEngineered.Solder.Injection;

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

				asyncDisposable = new MockLifecycle();
				await resourceManager.WatchingAsync(__, asyncDisposable);

				asyncDisposable = new MockLifecycle();
				//resourceManager.UsingAsync(__, disposable);
				await using (await resourceManager.UsingAsync(__, asyncDisposable))
				{
				}

				await resourceManager.LeaveAsync(__);

				await resourceManager.CheckAsync();
			}
		}

		#endregion
	}
}