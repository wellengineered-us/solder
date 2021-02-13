/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Configuration
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class ConfigurationIgnoreAttribute : Attribute
	{
		#region Constructors/Destructors

		public ConfigurationIgnoreAttribute()
		{
		}

		#endregion
	}
}