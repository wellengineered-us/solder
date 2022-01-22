/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection.Resolutions
{
	public abstract partial class DependencyResolution
		: DualLifecycle,
			IDependencyResolution
	{
		#region Methods/Operators

		protected abstract ValueTask<object> CoreResolveAsync(IDependencyManager dependencyManager, Type resolutionType, string selectorKey, CancellationToken cancellationToken = default);

		public ValueTask<object> ResolveAsync(IDependencyManager dependencyManager, Type resolutionType, string selectorKey, CancellationToken cancellationToken = default)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			try
			{
				return this.CoreResolveAsync(dependencyManager, resolutionType, selectorKey, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new DependencyException(string.Format("The dependency resolution failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}
#endif