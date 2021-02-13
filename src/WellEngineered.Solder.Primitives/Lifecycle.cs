/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Primitives
{
	public abstract partial class Lifecycle : ICreatableEx, IDisposableEx
	{
		#region Constructors/Destructors

		protected Lifecycle()
		{
		}

		#endregion

		#region Fields/Constants

		private bool isCreated;
		private bool isDisposed;

		#endregion

		#region Properties/Indexers/Events

		public bool IsCreated
		{
			get
			{
				return this.isCreated;
			}
			private set
			{
				this.isCreated = value;
			}
		}

		public bool IsDisposed
		{
			get
			{
				return this.isDisposed;
			}
			private set
			{
				this.isDisposed = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected abstract void CoreCreate(bool creating);

		protected abstract void CoreDispose(bool disposing);

		public void Create()
		{
			this.Initialize();
		}

		public void Dispose()
		{
			this.Terminate();
		}

		private void ExplicitInitialize()
		{
			if (this.IsCreated)
				return;

			this.CoreCreate(true);
			this.MaybeSetIsCreated();
		}

		private void ExplicitSetIsCreated()
		{
			//GC.ReRegisterForFinalize(this);
			this.IsCreated = true;
		}

		private void ExplicitSetIsDisposed()
		{
			this.IsDisposed = true;
			GC.SuppressFinalize(this);
		}

		private void ExplicitTerminate()
		{
			if (this.IsDisposed)
				return;

			this.CoreDispose(true);
			this.MaybeSetIsDisposed();
		}

		public void Initialize()
		{
			this.MaybeInitialize();
		}

		protected virtual void MaybeInitialize()
		{
			this.ExplicitInitialize();
		}

		protected virtual void MaybeSetIsCreated()
		{
			this.ExplicitSetIsCreated();
		}

		protected virtual void MaybeSetIsDisposed()
		{
			this.ExplicitSetIsDisposed();
		}

		protected virtual void MaybeTerminate()
		{
			this.ExplicitTerminate();
		}

		public void Terminate()
		{
			this.MaybeTerminate();
		}

		#endregion
	}
}