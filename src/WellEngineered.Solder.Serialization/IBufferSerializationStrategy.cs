/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using Buffer = System.Memory<byte>;

namespace WellEngineered.Solder.Serialization
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects using buffer semantics.
	/// </summary>
	public partial interface IBufferSerializationStrategy : ISerializationStrategy
	{
		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified buffer value.
		/// </summary>
		/// <param name="value"> The buffer value to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		object DeserializeObjectFromBuffer(Buffer value, Type targetType);

		/// <summary>
		/// Deserializes an object from the specified text value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="value"> The buffer value to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		TObject DeserializeObjectFromBuffer<TObject>(Buffer value);

		/// <summary>
		/// Serializes an object to a buffer value.
		/// </summary>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A buffer representation of the object graph. </returns>
		Buffer SerializeObjectToBuffer(Type targetType, object obj);

		/// <summary>
		/// Serializes an object to a buffer value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A buffer representation of the object graph. </returns>
		Buffer SerializeObjectToBuffer<TObject>(TObject obj);

		#endregion
	}
}