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
	/// A composing dependency resolution implementation that allows only a specific instance
	/// to be lazy created on demand/first use, cached, and reused; the specific instance is
	/// retrieved from the inner dependency resolution once per the lifetime of the specified context scope.
	/// NOTE: no explicit synchronization is assumed.
	/// </summary>
	public sealed partial class ContextWrapperDependencyResolution
		: DependencyResolution
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

		protected override async ValueTask<object> CoreResolveAsync(IDependencyManager dependencyManager, Type resolutionType, string selectorKey, CancellationToken cancellationToken = default)
		{
			string key;
			object value;

			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			key = string.Format("{0}_{1}", resolutionType.FullName, selectorKey);

			if (!this.ContextualStorageStrategy.HasValue(key))
			{
				value = await this.InnerDependencyResolution.ResolveAsync(dependencyManager, resolutionType, selectorKey, cancellationToken);
				this.ContextualStorageStrategy.SetValue<object>(key, value);

				this.TrackedContextKeys.Add(key);
			}
			else
				value = this.ContextualStorageStrategy.GetValue<object>(key);

			await Task.CompletedTask;
			return value;
		}

		#endregion
	}
}
#endif