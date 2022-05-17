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
	public class InstanceDependencyResolutionArity1Tests
	{
		#region Constructors/Destructors

		public InstanceDependencyResolutionArity1Tests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateAndEvaluateTest()
		{
			InstanceDependencyResolution<int> instanceDependencyResolution;
			IDependencyManager mockDependencyManager;
			int value;
			int result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = 11;

			instanceDependencyResolution = new InstanceDependencyResolution<int>(value);

			Assert.AreEqual(DependencyLifetime.Instance, instanceDependencyResolution.DependencyLifetime);

			result = instanceDependencyResolution.Resolve(mockDependencyManager, string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);

			instanceDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldCreateAndEvaluateUntypedTest()
		{
			InstanceDependencyResolution<int> instanceDependencyResolution;
			IDependencyManager mockDependencyManager;
			int value;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = 11;

			instanceDependencyResolution = new InstanceDependencyResolution<int>(value);

			Assert.AreEqual(DependencyLifetime.Instance, instanceDependencyResolution.DependencyLifetime);

			result = instanceDependencyResolution.Resolve(mockDependencyManager, typeof(int), string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);

			instanceDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveTest()
		{
			InstanceDependencyResolution<int> instanceDependencyResolution;
			IDependencyManager mockDependencyManager;
			int value;
			int result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;

			value = 11;

			instanceDependencyResolution = new InstanceDependencyResolution<int>(value);

			result = instanceDependencyResolution.Resolve(mockDependencyManager, string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveUntypedTest()
		{
			InstanceDependencyResolution<int> instanceDependencyResolution;
			IDependencyManager mockDependencyManager;
			int value;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;

			value = 11;

			instanceDependencyResolution = new InstanceDependencyResolution<int>(value);

			result = instanceDependencyResolution.Resolve(mockDependencyManager, typeof(int), string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveTest()
		{
			InstanceDependencyResolution<int> instanceDependencyResolution;
			IDependencyManager mockDependencyManager;
			int value;
			int result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = 11;

			instanceDependencyResolution = new InstanceDependencyResolution<int>(value);

			result = instanceDependencyResolution.Resolve(mockDependencyManager, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveUntypedTest()
		{
			InstanceDependencyResolution<int> instanceDependencyResolution;
			IDependencyManager mockDependencyManager;
			int value;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = 11;

			instanceDependencyResolution = new InstanceDependencyResolution<int>(value);

			result = instanceDependencyResolution.Resolve(mockDependencyManager, typeof(int), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeResolveUntypedTest()
		{
			InstanceDependencyResolution<int> instanceDependencyResolution;
			IDependencyManager mockDependencyManager;
			int value;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = 11;

			instanceDependencyResolution = new InstanceDependencyResolution<int>(value);

			result = instanceDependencyResolution.Resolve(mockDependencyManager, null, string.Empty);
		}

		#endregion
	}
}