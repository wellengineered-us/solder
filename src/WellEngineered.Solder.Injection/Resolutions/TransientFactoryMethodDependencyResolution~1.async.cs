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
	/// A dependency resolution implementation that executes a
	/// factory method callback each time a dependency resolution occurs.
	/// </summary>
	public sealed partial class TransientFactoryMethodDependencyResolution<TResolution> : DependencyResolution<TResolution>
	{
		#region Fields/Constants

		private readonly Func<ValueTask<TResolution>> asyncFactoryMethod;

		#endregion

		#region Properties/Indexers/Events

		private Func<ValueTask<TResolution>> AsyncFactoryMethod
		{
			get
			{
				return this.asyncFactoryMethod;
			}
		}

		#endregion

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

		protected override async ValueTask<TResolution> CoreResolveAsync(IDependencyManager dependencyManager, string selectorKey, CancellationToken cancellationToken = default)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			return await this.AsyncFactoryMethod();
		}

		#endregion
	}
}
#endif