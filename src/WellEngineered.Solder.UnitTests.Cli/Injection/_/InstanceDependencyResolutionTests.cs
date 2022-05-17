/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NMock;

using NUnit.Framework;

using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Injection.Resolutions;
using WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure;

namespace WellEngineered.Solder.UnitTests.Cli.Injection._
{
	[TestFixture]
	public class InstanceDependencyResolutionTests
	{
		#region Constructors/Destructors

		public InstanceDependencyResolutionTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateAndEvaluateTest()
		{
			InstanceDependencyResolution instanceDependencyResolution;
			IDependencyManager mockDependencyManager;
			int value;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = 11;

			instanceDependencyResolution = new InstanceDependencyResolution(value);

			Assert.AreEqual(DependencyLifetime.Instance, instanceDependencyResolution.DependencyLifetime);

			result = instanceDependencyResolution.Resolve(mockDependencyManager, typeof(object), string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);

			instanceDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveTest()
		{
			InstanceDependencyResolution instanceDependencyResolution;
			IDependencyManager mockDependencyManager;
			int value;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;

			value = 11;

			instanceDependencyResolution = new InstanceDependencyResolution(value);

			result = instanceDependencyResolution.Resolve(mockDependencyManager, typeof(object), string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveTest()
		{
			InstanceDependencyResolution instanceDependencyResolution;
			IDependencyManager mockDependencyManager;
			int value;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = 11;

			instanceDependencyResolution = new InstanceDependencyResolution(value);

			result = instanceDependencyResolution.Resolve(mockDependencyManager, typeof(object), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeResolveTest()
		{
			InstanceDependencyResolution instanceDependencyResolution;
			IDependencyManager mockDependencyManager;
			int value;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = 11;

			instanceDependencyResolution = new InstanceDependencyResolution(value);

			result = instanceDependencyResolution.Resolve(mockDependencyManager, null, string.Empty);
		}

		#endregion
	}
}