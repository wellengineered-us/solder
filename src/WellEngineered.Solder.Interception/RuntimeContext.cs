/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Interception
{
	public sealed class RuntimeContext : Lifecycle, IRuntimeContext
	{
		#region Constructors/Destructors

		public RuntimeContext()
		{
		}

		#endregion

		#region Fields/Constants

		private bool continueInterception;
		private int interceptionCount;
		private int interceptionIndex;

		#endregion

		#region Properties/Indexers/Events

		public bool ContinueInterception
		{
			get
			{
				return this.continueInterception;
			}
			set
			{
				this.continueInterception = value;
			}
		}

		public int InterceptionCount
		{
			get
			{
				return this.interceptionCount;
			}
			set
			{
				this.interceptionCount = value;
			}
		}

		public int InterceptionIndex
		{
			get
			{
				return this.interceptionIndex;
			}
			set
			{
				this.interceptionIndex = value;
			}
		}

		#endregion

		#region Methods/Operators

		public void AbortInterceptionChain()
		{
			if (!this.ContinueInterception)
				throw new InvalidOperationException(string.Format("Cannot abort interception chain; the chain has was either previously aborted or pending graceful completion."));

			this.continueInterception = false;
		}

		protected override void CoreCreate(bool creating)
		{
			// do nothing
		}

		protected override void CoreDispose(bool disposing)
		{
			// do nothing
		}

		protected override ValueTask CoreCreateAsync(bool creating)
		{
			throw new NotSupportedException();
		}

		protected override ValueTask CoreDisposeAsync(bool disposing)
		{
			throw new NotSupportedException();
		}

		#endregion
	}
}