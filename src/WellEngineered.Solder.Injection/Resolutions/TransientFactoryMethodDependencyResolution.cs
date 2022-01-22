﻿/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Injection.Resolutions
{
	/// <summary>
	/// A dependency resolution implementation that executes a
	/// factory method callback each time a dependency resolution occurs.
	/// </summary>
	public sealed partial class TransientFactoryMethodDependencyResolution : DependencyResolution
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TransientFactoryMethodDependencyResolution class.
		/// </summary>
		/// <param name="factoryMethod"> The callback method to execute during resolution. </param>
		public TransientFactoryMethodDependencyResolution(Delegate factoryMethod)
			: base(DependencyLifetime.Transient)
		{
			if ((object)factoryMethod == null)
				throw new ArgumentNullException(nameof(factoryMethod));

			this.factoryMethod = factoryMethod;
		}

		#endregion

		#region Fields/Constants

		private readonly Delegate factoryMethod;

		#endregion

		#region Properties/Indexers/Events

		private Delegate FactoryMethod
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

		protected override object CoreResolve(IDependencyManager dependencyManager, Type resolutionType, string selectorKey)
		{
			Type activatorType;

			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			activatorType = this.FactoryMethod.Method.ReturnType;
			if (!resolutionType.IsAssignableFrom(activatorType))
				throw new DependencyException(string.Format("Resolution type '{1}' is not assignable from activator type '{0}'; selector key '{2}'.", activatorType.FullName, resolutionType.FullName, selectorKey));

			return this.FactoryMethod.DynamicInvoke(null);
		}

		#endregion
	}
}