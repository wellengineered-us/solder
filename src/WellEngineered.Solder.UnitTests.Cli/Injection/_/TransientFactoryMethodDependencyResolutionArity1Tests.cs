/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

using NMock;

using NUnit.Framework;

using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Injection.Resolutions;
using WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure;

namespace WellEngineered.Solder.UnitTests.Cli.Injection._
{
	[TestFixture]
	public class TransientFactoryMethodDependencyResolutionArity1Tests
	{
		#region Constructors/Destructors

		public TransientFactoryMethodDependencyResolutionArity1Tests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateAndEvaluateTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			IDependencyManager mockDependencyManager;
			Func<int> value;
			Func<ValueTask<int>> asyncValue;
			int result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = () => 11;
			asyncValue = async () => await Task.FromResult<int>(7);

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value, asyncValue);

			Assert.AreEqual(DependencyLifetime.Transient, transientFactoryMethodDependencyResolution.DependencyLifetime);

			result = transientFactoryMethodDependencyResolution.Resolve(mockDependencyManager, string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);

			transientFactoryMethodDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldCreateAndEvaluateUntypedTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			IDependencyManager mockDependencyManager;
			Func<int> value;
			Func<ValueTask<int>> asyncValue;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = () => 11;
			asyncValue = async () => await Task.FromResult<int>(7);

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value, asyncValue);

			Assert.AreEqual(DependencyLifetime.Transient, transientFactoryMethodDependencyResolution.DependencyLifetime);

			result = transientFactoryMethodDependencyResolution.Resolve(mockDependencyManager, typeof(int), string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);

			transientFactoryMethodDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			IDependencyManager mockDependencyManager;
			Func<int> value;
			Func<ValueTask<int>> asyncValue;
			int result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;

			value = () => 11;
			asyncValue = async () => await Task.FromResult<int>(7);

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value, asyncValue);

			result = transientFactoryMethodDependencyResolution.Resolve(mockDependencyManager, string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveUntypedTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			IDependencyManager mockDependencyManager;
			Func<int> value;
			Func<ValueTask<int>> asyncValue;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;

			value = () => 11;
			asyncValue = async () => await Task.FromResult<int>(7);

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value, asyncValue);

			result = transientFactoryMethodDependencyResolution.Resolve(mockDependencyManager, typeof(int), string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullFactoryMethodCreateTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			Func<int> value;
			Func<ValueTask<int>> asyncValue;

			value = null;
			asyncValue = async () => await Task.FromResult<int>(7);

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value, asyncValue);
		}
		
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullFactoryMethodCreateAsyncTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			Func<int> value;
			Func<ValueTask<int>> asyncValue;

			value = () => 11;
			asyncValue = null;

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value, asyncValue);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			IDependencyManager mockDependencyManager;
			Func<int> value;
			Func<ValueTask<int>> asyncValue;
			int result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = () => 11;
			asyncValue = async () => await Task.FromResult<int>(7);

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value, asyncValue);

			result = transientFactoryMethodDependencyResolution.Resolve(mockDependencyManager, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveUntypedTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			IDependencyManager mockDependencyManager;
			Func<int> value;
			Func<ValueTask<int>> asyncValue;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = () => 11;
			asyncValue = async () => await Task.FromResult<int>(7);

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value, asyncValue);

			result = transientFactoryMethodDependencyResolution.Resolve(mockDependencyManager, typeof(int), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeResolveUntypedTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			IDependencyManager mockDependencyManager;
			Func<int> value;
			Func<ValueTask<int>> asyncValue;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = () => 11;
			asyncValue = async () => await Task.FromResult<int>(7);

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value, asyncValue);

			result = transientFactoryMethodDependencyResolution.Resolve(mockDependencyManager, null, string.Empty);
		}

		#endregion
	}
}