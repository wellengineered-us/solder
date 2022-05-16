﻿/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Serialization
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects using native semantics.
	/// </summary>
	public partial interface INativeSerializationStrategy<in TNativeInput, in TNativeOutput>
	{
		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified native.
		/// </summary>
		/// <param name="nativeInput"> The native input to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		ValueTask<object> DeserializeObjectFromNativeAsync(TNativeInput nativeInput, Type targetType, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deserializes an object from the specified native. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="nativeInput"> The native to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		ValueTask<TObject> DeserializeObjectFromNativeAsync<TObject>(TNativeInput nativeInput, CancellationToken cancellationToken = default);

		/// <summary>
		/// Serializes an object to the specified native.
		/// </summary>
		/// <param name="nativeOutput"> The native output to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		ValueTask SerializeObjectToNativeAsync(TNativeOutput nativeOutput, Type targetType, object obj, CancellationToken cancellationToken = default);

		/// <summary>
		/// Serializes an object to the specified native. This is the generic overload.
		/// </summary>
		/// <param name="nativeOutput"> The native output to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		ValueTask SerializeObjectToNativeAsync<TObject>(TNativeOutput nativeOutput, TObject obj, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif