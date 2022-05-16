/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Component
{
	public abstract partial class SolderComponent0
		: DualLifecycle,
			ISolderComponent0

	{
		#region Constructors/Destructors

		protected SolderComponent0()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly Guid componentIdDual = Guid.NewGuid();
		private readonly bool isReusableDual = false;

		#endregion

		#region Properties/Indexers/Events

		public Guid ComponentId
		{
			get
			{
				return this.componentIdDual;
			}
		}

		public bool IsReusable
		{
			get
			{
				return this.isReusableDual;
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

		#endregion
	}
}