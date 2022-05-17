/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Serialization
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects using binary reader/writer semantics.
	/// </summary>
	public partial interface IBinaryReaderWriterSerializationStrategy
	{
		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified binary reader.
		/// </summary>
		/// <param name="binaryReader"> The binary reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		ValueTask<object> DeserializeObjectFromBinaryReaderAsync(BinaryReader binaryReader, Type targetType, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deserializes an object from the specified binary reader. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="binaryReader"> The binary reader to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		ValueTask<TObject> DeserializeObjectFromBinaryReaderAsync<TObject>(BinaryReader binaryReader, CancellationToken cancellationToken = default);

		/// <summary>
		/// Serializes an object to the specified binary writer.
		/// </summary>
		/// <param name="binaryWriter"> The binary writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		ValueTask SerializeObjectToBinaryWriterAsync(BinaryWriter binaryWriter, Type targetType, object obj, CancellationToken cancellationToken = default);

		/// <summary>
		/// Serializes an object to the specified binary writer. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="binaryWriter"> The binary writer to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		ValueTask SerializeObjectToBinaryWriterAsync<TObject>(BinaryWriter binaryWriter, TObject obj, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif