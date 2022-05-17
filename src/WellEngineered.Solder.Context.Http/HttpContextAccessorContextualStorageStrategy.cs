/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using Microsoft.AspNetCore.Http;

namespace WellEngineered.Solder.Context.Http
{
	/*
			// https://github.com/aspnet/Announcements/issues/190
			public void ConfigureServices(IServiceCollection services)
			{
			    services.AddHttpContextAccessor();
			    // if < .NET Core 2.2 use this
			    //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			    // Other code...
			}
	 */
	public sealed class HttpContextAccessorContextualStorageStrategy : IContextualStorageStrategy
	{
		#region Constructors/Destructors

		public HttpContextAccessorContextualStorageStrategy(IHttpContextAccessor httpContextAccessor)
		{
			this.httpContextAccessor = httpContextAccessor;
		}

		#endregion

		#region Fields/Constants

		private readonly IHttpContextAccessor httpContextAccessor;

		#endregion

		#region Properties/Indexers/Events

		private IHttpContextAccessor HttpContextAccessor
		{
			get
			{
				return this.httpContextAccessor;
			}
		}

		public bool IsValidHttpContext
		{
			get
			{
				return (object)this.HttpContextAccessor != null;
			}
		}

		#endregion

		#region Methods/Operators

		private void AssertValidHttpContext()
		{
			if (!this.IsValidHttpContext)
				throw new InvalidOperationException(string.Format("The HTTP context accessor is invalid."));
		}

		public T GetValue<T>(string key)
		{
			this.AssertValidHttpContext();
			return (T)this.HttpContextAccessor.HttpContext.Items[key];
		}

		public bool HasValue(string key)
		{
			this.AssertValidHttpContext();
			return this.HttpContextAccessor.HttpContext.Items.ContainsKey(key);
		}

		public void RemoveValue(string key)
		{
			this.AssertValidHttpContext();
			this.HttpContextAccessor.HttpContext.Items.Remove(key);
		}

		public void SetValue<T>(string key, T value)
		{
			this.AssertValidHttpContext();
			this.HttpContextAccessor.HttpContext.Items[key] = value;
		}

		#endregion
	}
}