/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Solder.Context;

namespace WellEngineered.Solder.Injection.Resolutions
{
	/// <summary>
	/// A composing dependency resolution implementation that allows only a specific instance
	/// to be lazy created on demand/first use, cached, and reused; the specific instance is
	/// retrieved from the inner dependency resolution once per the lifetime of the specified context scope.
	/// NOTE: no explicit synchronization is assumed.
	/// </summary>
	public sealed class ContextWrapperDependencyResolution : DependencyResolution
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the ContextWrapperDependencyResolution class.
		/// </summary>
		/// <param name="contextScope"> The context scope being requested. </param>
		/// <param name="innerDependencyResolution"> The chained dependency resolution which is called only once. </param>
		public ContextWrapperDependencyResolution(ContextScope contextScope, IDependencyResolution innerDependencyResolution)
			: this(new DefaultContextualStorageFactory(contextScope).GetContextualStorage(), innerDependencyResolution)
		{
		}

		public ContextWrapperDependencyResolution(IContextualStorageStrategy contextualStorageStrategy, IDependencyResolution innerDependencyResolution)
			: base(DependencyLifetime.Singleton)
		{
			if ((object)contextualStorageStrategy == null)
				throw new ArgumentNullException(nameof(contextualStorageStrategy));

			if ((object)innerDependencyResolution == null)
				throw new ArgumentNullException(nameof(innerDependencyResolution));

			this.contextualStorageStrategy = contextualStorageStrategy;
			this.innerDependencyResolution = innerDependencyResolution;
		}

		#endregion

		#region Fields/Constants

		private readonly IContextualStorageStrategy contextualStorageStrategy;
		private readonly IDependencyResolution innerDependencyResolution;
		private readonly IList<string> trackedContextKeys = new List<string>();

		#endregion

		#region Properties/Indexers/Events

		private IContextualStorageStrategy ContextualStorageStrategy
		{
			get
			{
				return this.contextualStorageStrategy;
			}
		}

		private IDependencyResolution InnerDependencyResolution
		{
			get
			{
				return this.innerDependencyResolution;
			}
		}

		private IList<string> TrackedContextKeys
		{
			get
			{
				return this.trackedContextKeys;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreCreate(bool creating)
		{
			if (creating)
			{
				if ((object)this.InnerDependencyResolution != null)
					this.InnerDependencyResolution.Create();
			}
		}

		protected override void CoreDispose(bool disposing)
		{
			if (disposing)
			{
				foreach (string trackedContextKey in this.TrackedContextKeys)
				{
					if (this.ContextualStorageStrategy.HasValue(trackedContextKey))
						this.ContextualStorageStrategy.RemoveValue(trackedContextKey);
				}

				if ((object)this.InnerDependencyResolution != null)
					this.InnerDependencyResolution.Dispose();
			}
		}

		protected override object CoreResolve(IDependencyManager dependencyManager, Type resolutionType, string selectorKey)
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
				value = this.InnerDependencyResolution.Resolve(dependencyManager, resolutionType, selectorKey);
				this.ContextualStorageStrategy.SetValue<object>(key, value);

				this.TrackedContextKeys.Add(key);
			}
			else
				value = this.ContextualStorageStrategy.GetValue<object>(key);

			return value;
		}

		#endregion
	}
}