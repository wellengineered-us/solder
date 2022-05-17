/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Injection
{
	/// <summary>
	/// Marks a static void (IDependencyManager) method as a dependency magic method.
	/// These methods are used to register dependencies when an assembly is loaded.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public sealed class DependencyMagicMethodAttribute : Attribute
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the DependencyMagicMethodAttribute class.
		/// </summary>
		public DependencyMagicMethodAttribute()
		{
		}

		#endregion
	}
}