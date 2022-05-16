/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

namespace WellEngineered.Solder.Serialization
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects using binary reader/writer semantics.
	/// </summary>
	public partial interface IBinaryReaderWriterSerializationStrategy : ISerializationStrategy
	{
		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified binary reader.
		/// </summary>
		/// <param name="binaryReader"> The binary reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		object DeserializeObjectFromBinaryReader(BinaryReader binaryReader, Type targetType);

		/// <summary>
		/// Deserializes an object from the specified binary reader. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="binaryReader"> The binary reader to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		TObject DeserializeObjectFromBinaryReader<TObject>(BinaryReader binaryReader);

		/// <summary>
		/// Serializes an object to the specified binary writer.
		/// </summary>
		/// <param name="binaryWriter"> The binary writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		void SerializeObjectToBinaryWriter(BinaryWriter binaryWriter, Type targetType, object obj);

		/// <summary>
		/// Serializes an object to the specified binary writer. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="binaryWriter"> The binary writer to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		void SerializeObjectToBinaryWriter<TObject>(BinaryWriter binaryWriter, TObject obj);

		#endregion
	}
}