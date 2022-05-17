/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NUnit.Framework;

using WellEngineered.Solder.Context;
using WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure;

namespace WellEngineered.Solder.UnitTests.Cli.Context._
{
	[TestFixture]
	public class DefaultContextualStorageFactoryTests
	{
		#region Constructors/Destructors

		public DefaultContextualStorageFactoryTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			DefaultContextualStorageFactory defaultContextualStorageFactory;
			IContextualStorageStrategy contextualStorageStrategy;

			defaultContextualStorageFactory = new DefaultContextualStorageFactory(ContextScope.GlobalStaticUnsafe);
			contextualStorageStrategy = defaultContextualStorageFactory.GetContextualStorage();

			Assert.IsNotNull(contextualStorageStrategy);

			defaultContextualStorageFactory = new DefaultContextualStorageFactory(ContextScope.LocalThreadSafe);
			contextualStorageStrategy = defaultContextualStorageFactory.GetContextualStorage();

			Assert.IsNotNull(contextualStorageStrategy);

			defaultContextualStorageFactory = new DefaultContextualStorageFactory(ContextScope.LocalAsyncSafe);
			contextualStorageStrategy = defaultContextualStorageFactory.GetContextualStorage();

			Assert.IsNotNull(contextualStorageStrategy);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ShouldFailOnInvalidContextScopeCreateTest()
		{
			DefaultContextualStorageFactory defaultContextualStorageFactory;

			defaultContextualStorageFactory = new DefaultContextualStorageFactory(ContextScope.Unknown);

			defaultContextualStorageFactory.GetContextualStorage();
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ShouldFailOnLocalRequestSafeScopeCreateTest()
		{
			DefaultContextualStorageFactory defaultContextualStorageFactory;

			defaultContextualStorageFactory = new DefaultContextualStorageFactory(ContextScope.LocalRequestSafe);

			defaultContextualStorageFactory.GetContextualStorage();
		}

		#endregion
	}
}