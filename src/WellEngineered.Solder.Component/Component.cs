/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Component
{
	public abstract partial class Component : Lifecycle, IComponent
	{
		#region Constructors/Destructors

		protected Component()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly Guid componentId = Guid.NewGuid();
		private readonly bool isReusable = false;

		#endregion

		#region Properties/Indexers/Events

		public Guid ComponentId
		{
			get
			{
				return this.componentId;
			}
		}

		public bool IsReusable
		{
			get
			{
				return this.isReusable;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreCreate(bool creating)
		{
			if (creating)
			{
				// do nothing
			}
		}

		protected override void CoreDispose(bool disposing)
		{
			if (disposing)
			{
				// do nothing
			}
		}

		#endregion
	}
}