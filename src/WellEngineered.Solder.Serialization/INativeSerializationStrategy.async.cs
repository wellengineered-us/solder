/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
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
		ValueTask<object> DeserializeObjectFromNativeAsync(TNativeInput nativeInput, Type targetType);

		/// <summary>
		/// Deserializes an object from the specified native. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="nativeInput"> The native to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		ValueTask<TObject> DeserializeObjectFromNativeAsync<TObject>(TNativeInput nativeInput);

		/// <summary>
		/// Serializes an object to the specified native.
		/// </summary>
		/// <param name="nativeOutput"> The native output to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		ValueTask SerializeObjectToNativeAsync(TNativeOutput nativeOutput, Type targetType, object obj);

		/// <summary>
		/// Serializes an object to the specified native. This is the generic overload.
		/// </summary>
		/// <param name="nativeOutput"> The native output to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		ValueTask SerializeObjectToNativeAsync<TObject>(TNativeOutput nativeOutput, TObject obj);

		#endregion
	}
}