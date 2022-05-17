/*
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
	/// Provides a strategy pattern around serializing and deserializing objects using byte array semantics.
	/// </summary>
	public partial interface IByteArraySerializationStrategy
	{
		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified byte array value.
		/// </summary>
		/// <param name="value"> The byte array value to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		ValueTask<object> DeserializeObjectFromByteArrayAsync(byte[] value, Type targetType, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deserializes an object from the specified byte array value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="value"> The byte array value to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		ValueTask<TObject> DeserializeObjectFromByteArrayAsync<TObject>(byte[] value, CancellationToken cancellationToken = default);

		/// <summary>
		/// Serializes an object to a byte array value.
		/// </summary>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A byte array representation of the object graph. </returns>
		ValueTask<byte[]> SerializeObjectToByteArrayAsync(Type targetType, object obj, CancellationToken cancellationToken = default);

		/// <summary>
		/// Serializes an object to a byte array value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A byte array representation of the object graph. </returns>
		ValueTask<byte[]> SerializeObjectToByteArrayAsync<TObject>(TObject obj, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif