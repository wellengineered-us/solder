/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Injection
{
	/// <summary>
	/// Provides dependency registration and resolution services.
	/// </summary>
	public partial interface IDependencyManager : IAsyncLifecycle
	{
		#region Methods/Operators

		/// <summary>
		/// Adds a new dependency resolution for a given resolution type and selector key. Throws a DependencyException if the resolution type and selector key combination has been previously registered in this instance. This is the generic overload.
		/// </summary>
		/// <typeparam name="TResolution"> The resolution type of resolution. </typeparam>
		/// <param name="selectorKey"> An non-null, zero or greater length string selector key. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution existence check. </param>
		/// <param name="dependencyResolution"> The dependency resolution. </param>
		ValueTask AddResolutionAsync<TResolution>(string selectorKey, bool includeAssignableTypes, IDependencyResolution<TResolution> dependencyResolution, CancellationToken cancellationToken = default);

		/// <summary>
		/// Adds a new dependency resolution for a given resolution type and selector key. Throws a DependencyException if the resolution type and selector key combination has been previously registered in this instance. This is the non-generic overload.
		/// </summary>
		/// <param name="resolutionType"> The resolution type of resolution. </param>
		/// <param name="selectorKey"> An non-null, zero or greater length string selector key. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution existence check. </param>
		/// <param name="dependencyResolution"> The dependency resolution. </param>
		ValueTask AddResolutionAsync(Type resolutionType, string selectorKey, bool includeAssignableTypes, IDependencyResolution dependencyResolution, CancellationToken cancellationToken = default);

		/// <summary>
		/// Clears all registered dependency resolutions from this instance.
		/// </summary>
		/// <returns> A value indicating if at least one dependency resolution was removed. </returns>
		ValueTask<bool> ClearAllResolutionsAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Clears all registered dependency resolutions of the specified resolution type from this instance. This is the generic overload.
		/// </summary>
		/// <typeparam name="TResolution"> The resolution type of removal. </typeparam>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution removal list. </param>
		/// <returns> A value indicating if at least one dependency resolution was removed. </returns>
		ValueTask<bool> ClearTypeResolutionsAsync<TResolution>(bool includeAssignableTypes, CancellationToken cancellationToken = default);

		/// <summary>
		/// Clears all registered dependency resolutions of the specified resolution type from this instance. This is the non-generic overload.
		/// </summary>
		/// <param name="resolutionType"> The resolution type of removal. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution removal list. </param>
		/// <returns> A value indicating if at least one dependency resolution was removed. </returns>
		ValueTask<bool> ClearTypeResolutionsAsync(Type resolutionType, bool includeAssignableTypes, CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets a value indicating whether there are any registered dependency resolutions of the specified resolution type in this instance. If selector key is a null value, then this method will return true if any resolution exists for the specified resolution type, regardless of selector key; otherwise, this method will return true only if a resolution exists for the specified resolution type and selector key. This is the generic overload.
		/// </summary>
		/// <param name="selectorKey"> An null or zero or greater length string selector key. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution existence check. </param>
		/// <typeparam name="TResolution"> The resolution type of the check. </typeparam>
		/// <returns> A value indicating if at least one dependency resolution is present. </returns>
		ValueTask<bool> HasTypeResolutionAsync<TResolution>(string selectorKey, bool includeAssignableTypes, CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets a value indicating whether there are any registered dependency resolutions of the specified resolution type in this instance. If selector key is a null value, then this method will return true if any resolution exists for the specified resolution type, regardless of selector key; otherwise, this method will return true only if a resolution exists for the specified resolution type and selector key. This is the non-generic overload.
		/// </summary>
		/// <param name="resolutionType"> The resolution type of the check. </param>
		/// <param name="selectorKey"> An null or zero or greater length string selector key. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution existence check. </param>
		/// <returns> A value indicating if at least one dependency resolution is present. </returns>
		ValueTask<bool> HasTypeResolutionAsync(Type resolutionType, string selectorKey, bool includeAssignableTypes, CancellationToken cancellationToken = default);

		/// <summary>
		/// Removes the registered dependency resolution of the specified resolution type and selector key from this instance. Throws a DependencyException if the resolution type and selector key combination has not been previously registered in this instance. This is the generic overload.
		/// </summary>
		/// <typeparam name="TResolution"> The resolution type of removal. </typeparam>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution removal list. </param>
		/// <param name="selectorKey"> An non-null, zero or greater length string selector key. </param>
		ValueTask RemoveResolutionAsync<TResolution>(string selectorKey, bool includeAssignableTypes, CancellationToken cancellationToken = default);

		/// <summary>
		/// Removes the registered dependency resolution of the specified resolution type and selector key from this instance. Throws a DependencyException if the resolution type and selector key combination has not been previously registered in this instance. This is the non-generic overload.
		/// </summary>
		/// <param name="resolutionType"> The resolution type of removal. </param>
		/// <param name="selectorKey"> An non-null, zero or greater length string selector key. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution removal list. </param>
		ValueTask RemoveResolutionAsync(Type resolutionType, string selectorKey, bool includeAssignableTypes, CancellationToken cancellationToken = default);

		/// <summary>
		/// Resolves a dependency for the given resolution type and selector key combination. Throws a DependencyException if the resolution type and selector key combination has not been previously registered in this instance. Throws a DependencyException if the resolved value cannot be assigned to the resolution type. This is the non-generic overload.
		/// </summary>
		/// <typeparam name="TResolution"> The resolution type of resolution. </typeparam>
		/// <param name="selectorKey"> An non-null, zero or greater length string selector key. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution lookup list. </param>
		/// <returns> An object instance of assignable to the resolution type. </returns>
		ValueTask<TResolution> ResolveDependencyAsync<TResolution>(string selectorKey, bool includeAssignableTypes, CancellationToken cancellationToken = default);

		/// <summary>
		/// Resolves a dependency for the given resolution type and selector key combination. Throws a DependencyException if the resolution type and selector key combination has not been previously registered in this instance. Throws a DependencyException if the resolved value cannot be assigned to the resolution type. This is the non-generic overload.
		/// </summary>
		/// <param name="resolutionType"> The resolution type of resolution. </param>
		/// <param name="selectorKey"> An non-null, zero or greater length string selector key. </param>
		/// <param name="includeAssignableTypes"> A boolean value indicating whether to include assignable types in the candidate resolution lookup list. </param>
		/// <returns> An object instance of assignable to the resolution type. </returns>
		ValueTask<object> ResolveDependencyAsync(Type resolutionType, string selectorKey, bool includeAssignableTypes, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif