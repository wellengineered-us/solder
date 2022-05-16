/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Injection
{
	/// <summary>
	/// Provides a unified strategy pattern for dependency resolution.
	/// </summary>
	/// <typeparam name="TResolution"> The resolution type of resolution. </typeparam>
	public partial interface IDependencyResolution<TResolution>
		: IDependencyResolution
	{
		#region Methods/Operators

		/// <summary>
		/// Resolves a dependency.
		/// </summary>
		/// <param name="dependencyManager"> The in-effect dependency manager requesting this dependency resolution. </param>
		/// <param name="selectorKey"> The contextual selector key for this dependency resolution; a non-null, zero or greater length string is required. </param>
		/// <returns> An instance of an object assignable to the resolution type or an appropriate default value (e.g. null for reference types, logical 'zero' for value types). </returns>
		ValueTask<TResolution> ResolveAsync(IDependencyManager dependencyManager, string selectorKey, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif