/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Injection.Resolutions
{
	/// <summary>
	/// A dependency resolution implementation that executes a
	/// factory method callback each time a dependency resolution occurs.
	/// </summary>
	public sealed partial class TransientFactoryMethodDependencyResolution<TResolution> : DependencyResolution<TResolution>
	{
		#region Constructors/Destructors

#if ASYNC_ALL_THE_WAY_DOWN
		/// <summary>
		/// Initializes a new instance of the TransientFactoryMethodDependencyResolution`1 class.
		/// </summary>
		/// <param name="factoryMethod"> The callback method to execute during resolution. </param>
		/// <param name="asyncFactoryMethod"> The asynchronous callback method to execute during resolution. </param>
		public TransientFactoryMethodDependencyResolution(Func<TResolution> factoryMethod, Func<ValueTask<TResolution>> asyncFactoryMethod)
			: base(DependencyLifetime.Transient)
		{
			if ((object)factoryMethod == null)
				throw new ArgumentNullException(nameof(factoryMethod));

			if ((object)asyncFactoryMethod == null)
				throw new ArgumentNullException(nameof(asyncFactoryMethod));

			this.factoryMethod = factoryMethod;
			this.asyncFactoryMethod = asyncFactoryMethod;
		}
#else
		public TransientFactoryMethodDependencyResolution(Func<TResolution> factoryMethod, object unused = null)
			: base(DependencyLifetime.Transient)
		{
			if ((object)factoryMethod == null)
				throw new ArgumentNullException(nameof(factoryMethod));

			this.factoryMethod = factoryMethod;
		}
#endif

		#endregion

		#region Fields/Constants

		private readonly Func<TResolution> factoryMethod;

		#endregion

		#region Properties/Indexers/Events

		private Func<TResolution> FactoryMethod
		{
			get
			{
				return this.factoryMethod;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreCreate(bool creating)
		{
			// do nothing
		}

		protected override void CoreDispose(bool disposing)
		{
			// do nothing
		}

		protected override TResolution CoreResolve(IDependencyManager dependencyManager, string selectorKey)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			return this.FactoryMethod();
		}

		#endregion
	}
}