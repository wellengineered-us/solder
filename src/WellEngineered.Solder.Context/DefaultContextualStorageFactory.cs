/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Context
{
	/// <summary>
	/// Manages execution path storage of objects in a manner which is safe in standard executables, libraries, ASP.NET, and WCF code.
	/// </summary>
	public class DefaultContextualStorageFactory : IContextualStorageFactory
	{
		#region Constructors/Destructors

		public DefaultContextualStorageFactory(ContextScope contextScope)
		{
			this.contextScope = contextScope;
		}

		#endregion

		#region Fields/Constants

		private readonly ContextScope contextScope;

		#endregion

		#region Properties/Indexers/Events

		public ContextScope ContextScope
		{
			get
			{
				return this.contextScope;
			}
		}

		#endregion

		#region Methods/Operators

		public virtual IContextualStorageStrategy GetContextualStorage()
		{
			switch (this.ContextScope)
			{
				case ContextScope.GlobalStaticUnsafe:
					return new GlobalStaticContextualStorageStrategy();
				case ContextScope.LocalThreadSafe:
					return new ThreadLocalContextualStorageStrategy();
#if ASYNC_ALL_THE_WAY_DOWN
				case ContextScope.LocalAsyncSafe:
					return new AsyncLocalContextualStorageStrategy();
#endif
				default:
					throw new ArgumentOutOfRangeException(nameof(this.ContextScope));
			}
		}

		#endregion
	}
}