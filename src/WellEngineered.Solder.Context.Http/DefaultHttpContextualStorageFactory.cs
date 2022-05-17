/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Context.Http
{
	/// <summary>
	/// Manages execution path storage of objects in a manner which is safe in standard executables, libraries, ASP.NET, and WCF code.
	/// </summary>
	public class DefaultHttpContextualStorageFactory : DefaultContextualStorageFactory
	{
		#region Constructors/Destructors

		public DefaultHttpContextualStorageFactory(ContextScope contextScope)
			: base(contextScope)
		{
		}

		#endregion

		#region Methods/Operators

		public override IContextualStorageStrategy GetContextualStorage()
		{
			switch (this.ContextScope)
			{
				case ContextScope.LocalRequestSafe:
					return new HttpContextAccessorContextualStorageStrategy(null);
				default:
					return base.GetContextualStorage();
			}
		}

		#endregion
	}
}