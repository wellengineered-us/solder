/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

namespace WellEngineered.Solder.Serialization
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects using text reader/writer semantics.
	/// </summary>
	public partial interface ITextReaderWriterSerializationStrategy : ISerializationStrategy
	{
		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified text reader.
		/// </summary>
		/// <param name="textReader"> The text reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		object DeserializeObjectFromTextReader(TextReader textReader, Type targetType);

		/// <summary>
		/// Deserializes an object from the specified text reader. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="textReader"> The text reader to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		TObject DeserializeObjectFromTextReader<TObject>(TextReader textReader);

		/// <summary>
		/// Serializes an object to the specified text writer.
		/// </summary>
		/// <param name="textWriter"> The text writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		void SerializeObjectToTextWriter(TextWriter textWriter, Type targetType, object obj);

		/// <summary>
		/// Serializes an object to the specified text writer. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="textWriter"> The text writer to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		void SerializeObjectToTextWriter<TObject>(TextWriter textWriter, TObject obj);

		#endregion
	}
}