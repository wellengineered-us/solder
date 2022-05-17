/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Injection.Resolutions
{
	/// <summary>
	/// A dependency resolution implementation that executes a public, default constructor
	/// on the activation type each time a dependency resolution occurs.
	/// </summary>
	public partial class TransientDefaultConstructorDependencyResolution : DependencyResolution

	{
		#region Methods/Operators

		protected override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		protected override ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		protected override async ValueTask<object> CoreResolveAsync(IDependencyManager dependencyManager, Type resolutionType, string selectorKey, CancellationToken cancellationToken = default)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			if (!resolutionType.IsAssignableFrom(this.activatorType))
				throw new DependencyException(string.Format("Resolution type '{1}' is not assignable from activator type '{0}'; selector key '{2}'.", this.activatorType.FullName, resolutionType.FullName, selectorKey));

			await Task.CompletedTask;
			return Activator.CreateInstance(this.ActivatorType);
		}

		#endregion
	}
}
#endif