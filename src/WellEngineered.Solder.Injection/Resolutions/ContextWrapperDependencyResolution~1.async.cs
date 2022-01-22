/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Injection.Resolutions
{
	/// <summary>
	/// A composing dependency resolution implementation that allows only a specific instance
	/// to be lazy created on demand/first use, cached, and reused; the specific instance is
	/// retrieved from the inner dependency resolution once per the lifetime of the specified context scope.
	/// NOTE: no explicit synchronization is assumed.
	/// </summary>
	public sealed partial class ContextWrapperDependencyResolution<TResolution>
		: DependencyResolution<TResolution>
	{
		#region Methods/Operators

		protected override async ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			if (creating)
			{
				if ((object)this.InnerDependencyResolution != null)
					await this.InnerDependencyResolution.CreateAsync();
			}
		}

		protected override async ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			if (disposing)
			{
				foreach (string trackedContextKey in this.TrackedContextKeys)
				{
					if (this.ContextualStorageStrategy.HasValue(trackedContextKey))
						this.ContextualStorageStrategy.RemoveValue(trackedContextKey);
				}

				if ((object)this.InnerDependencyResolution != null)
					await this.InnerDependencyResolution.DisposeAsync();
			}
		}

		protected override async ValueTask<TResolution> CoreResolveAsync(IDependencyManager dependencyManager, string selectorKey, CancellationToken cancellationToken = default)
		{
			string contextKey;
			Type resolutionType;
			TResolution value;

			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			resolutionType = typeof(TResolution);
			contextKey = string.Format("{0}_{1}", resolutionType.FullName, selectorKey);

			if (!this.ContextualStorageStrategy.HasValue(contextKey))
			{
				value = await this.InnerDependencyResolution.ResolveAsync(dependencyManager, selectorKey, cancellationToken);
				this.ContextualStorageStrategy.SetValue<TResolution>(contextKey, value);

				this.TrackedContextKeys.Add(contextKey);
			}
			else
				value = this.ContextualStorageStrategy.GetValue<TResolution>(contextKey);

			return value;
		}

		#endregion
	}
}
#endif