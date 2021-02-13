/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	public abstract class DependencyResolution : Lifecycle, IDependencyResolution
	{
		#region Constructors/Destructors

		protected DependencyResolution(DependencyLifetime dependencyLifetime)
		{
			this.dependencyLifetime = dependencyLifetime;
		}

		#endregion

		#region Fields/Constants

		private readonly DependencyLifetime dependencyLifetime;

		#endregion

		#region Properties/Indexers/Events

		public DependencyLifetime DependencyLifetime
		{
			get
			{
				return this.dependencyLifetime;
			}
		}

		#endregion

		#region Methods/Operators

		protected abstract object CoreResolve(IDependencyManager dependencyManager, Type resolutionType, string selectorKey);

		protected sealed override ValueTask CoreCreateAsync(bool creating)
		{
			throw new NotImplementedException();
		}

		protected sealed override ValueTask CoreDisposeAsync(bool disposing)
		{
			throw new NotImplementedException();
		}

		public object Resolve(IDependencyManager dependencyManager, Type resolutionType, string selectorKey)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			return this.CoreResolve(dependencyManager, resolutionType, selectorKey);
		}

		#endregion
	}
}