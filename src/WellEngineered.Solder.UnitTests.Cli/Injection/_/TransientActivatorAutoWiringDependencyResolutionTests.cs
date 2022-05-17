/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Reflection;

using NMock;

using NUnit.Framework;

using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Injection.Resolutions;
using WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure;

namespace WellEngineered.Solder.UnitTests.Cli.Injection._
{
	[TestFixture]
	public class TransientActivatorAutoWiringDependencyResolutionTests
	{
		#region Constructors/Destructors

		public TransientActivatorAutoWiringDependencyResolutionTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateAndEvaluateSelectorKeyTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			Type activatorType;
			object result;
			MockFactory mockFactory;
			string _unusedString = null;
			bool _unusedBoolean = false;
			Type _unusedType = null;
			ConstructorInfo _unusedConstructorInfo = null;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			activatorType = typeof(MockDependantObject);

			//Expect.On().Exactly(4).Method(x => x.GetOneAttribute<DependencyInjectionAttribute>(_unusedConstructorInfo)).WithAnyArguments().Will(Return.Value<DependencyInjectionAttribute>((DependencyInjectionAttribute)null));

			Expect.On(mockDependencyManager).One.Method(m => m.ResolveDependency(_unusedType, _unusedString, _unusedBoolean)).With(typeof(MockDependantObject), string.Empty, true).Will(Return.Value(new MockDependantObject("both")));

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(activatorType);

			Assert.AreEqual(DependencyLifetime.Transient, transientActivatorAutoWiringDependencyResolution.DependencyLifetime);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(object), "named_dep_obj");

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<MockDependantObject>(result);
			Assert.IsNotNull(((MockDependantObject)result).Text);
			Assert.AreEqual(string.Empty, ((MockDependantObject)result).Text);
			Assert.IsNotNull(((MockDependantObject)result).Left);
			Assert.IsNotNull(((MockDependantObject)result).Right);
			Assert.AreEqual("both", ((MockDependantObject)result).Left.Text);
			Assert.AreEqual("both", ((MockDependantObject)result).Right.Text);

			transientActivatorAutoWiringDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldCreateAndEvaluateTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			Type activatorType;
			object result;
			MockFactory mockFactory;
			string _unusedString = null;
			bool _unusedBoolean = false;
			Type _unusedType = null;
			ConstructorInfo _unusedConstructorInfo = null;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			activatorType = typeof(MockDependantObject);

			//Expect.On().Exactly(4).Method(x => x.GetOneAttribute<DependencyInjectionAttribute>(_unusedConstructorInfo)).WithAnyArguments().Will(Return.Value<DependencyInjectionAttribute>((DependencyInjectionAttribute)null));

			Expect.On(mockDependencyManager).One.Method(m => m.ResolveDependency(_unusedType, _unusedString, _unusedBoolean)).With(typeof(MockDependantObject), "named_dep_obj", true).Will(Return.Value(new MockDependantObject("left")));
			Expect.On(mockDependencyManager).One.Method(m => m.ResolveDependency(_unusedType, _unusedString, _unusedBoolean)).With(typeof(MockDependantObject), string.Empty, true).Will(Return.Value(new MockDependantObject("right")));

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(activatorType);

			Assert.AreEqual(DependencyLifetime.Transient, transientActivatorAutoWiringDependencyResolution.DependencyLifetime);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(object), string.Empty);

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<MockDependantObject>(result);
			Assert.IsNotNull(((MockDependantObject)result).Text);
			Assert.AreEqual(string.Empty, ((MockDependantObject)result).Text);
			Assert.IsNotNull(((MockDependantObject)result).Left);
			Assert.IsNotNull(((MockDependantObject)result).Right);
			Assert.AreEqual("left", ((MockDependantObject)result).Left.Text);
			Assert.AreEqual("right", ((MockDependantObject)result).Right.Text);

			transientActivatorAutoWiringDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		[ExpectedException(typeof(DependencyException))]
		public void ShouldFailOnMockAmbiguousConstructorMatchTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			Type activatorType;
			object result;
			MockFactory mockFactory;
			string _unusedString = null;
			bool _unusedBoolean = false;
			Type _unusedType = null;
			ConstructorInfo _unusedConstructorInfo = null;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			activatorType = typeof(MockAmbiguousCtorMatchDependantObject);

			//Expect.On().Exactly(4).Method(x => x.GetOneAttribute<DependencyInjectionAttribute>(_unusedConstructorInfo)).WithAnyArguments().Will(Return.Value<DependencyInjectionAttribute>((DependencyInjectionAttribute)null));

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(activatorType);

			Assert.AreEqual(DependencyLifetime.Transient, transientActivatorAutoWiringDependencyResolution.DependencyLifetime);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(object), "named_dep_obj");
		}

		[Test]
		[ExpectedException(typeof(DependencyException))]
		public void ShouldFailOnMockUnmarkedParametersMatchTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			Type activatorType;
			object result;
			MockFactory mockFactory;
			string _unusedString = null;
			bool _unusedBoolean = false;
			Type _unusedType = null;
			ConstructorInfo _unusedConstructorInfo = null;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			activatorType = typeof(MockAmbiguousCtorMatchDependantObject);

			//Expect.On().Exactly(4).Method(x => x.GetOneAttribute<DependencyInjectionAttribute>(_unusedConstructorInfo)).WithAnyArguments().Will(Return.Value<DependencyInjectionAttribute>((DependencyInjectionAttribute)null));

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(activatorType);

			Assert.AreEqual(DependencyLifetime.Transient, transientActivatorAutoWiringDependencyResolution.DependencyLifetime);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(object), "i_haz_no_marked_params");
		}

		[Test]
		[ExpectedException(typeof(DependencyException))]
		public void ShouldFailOnNoMatchingConstructorTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			Type activatorType;
			object result;
			MockFactory mockFactory;
			string _unusedString = null;
			bool _unusedBoolean = false;
			Type _unusedType = null;
			ConstructorInfo _unusedConstructorInfo = null;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			activatorType = typeof(MockAmbiguousCtorMatchDependantObject);

			//Expect.On().Exactly(4).Method(x => x.GetOneAttribute<DependencyInjectionAttribute>(_unusedConstructorInfo)).WithAnyArguments().Will(Return.Value<DependencyInjectionAttribute>((DependencyInjectionAttribute)null));

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(activatorType);

			Assert.AreEqual(DependencyLifetime.Transient, transientActivatorAutoWiringDependencyResolution.DependencyLifetime);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(object), "boaty_mcboatface_NO_MATCH_right?");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullActualTypeCreateTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			Type activatorType;
			MockFactory mockFactory;

			mockFactory = new MockFactory();

			activatorType = null;

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(activatorType);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			object result;
			MockFactory mockFactory;
			Type activatorType;

			mockFactory = new MockFactory();
			mockDependencyManager = null;
			activatorType = typeof(int);

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(activatorType);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(object), string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			object result;
			MockFactory mockFactory;
			Type activatorType;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			activatorType = typeof(int);

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(activatorType);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(object), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeResolveTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			object result;
			MockFactory mockFactory;
			Type activatorType;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			activatorType = typeof(int);

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(activatorType);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, null, string.Empty);
		}

		#endregion
	}
}