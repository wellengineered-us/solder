/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace WellEngineered.Solder.Serialization.JsonNet
{
	public partial class NativeBsonSerializationStrategy : CommonSerializationStrategy, INativeBsonSerializationStrategy
	{
		#region Constructors/Destructors

		public NativeBsonSerializationStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		private static JsonSerializer GetJsonSerializer()
		{
			return JsonSerializer.Create(new JsonSerializerSettings()
										{
											Formatting = Formatting.Indented,
											TypeNameHandling = TypeNameHandling.None,
											ReferenceLoopHandling = ReferenceLoopHandling.Ignore
										});
		}

		/// <summary>
		/// Deserializes an object from the specified text reader.
		/// </summary>
		/// <param name="binaryReader"> The text reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public override object DeserializeObjectFromBinaryReader(BinaryReader binaryReader, Type targetType)
		{
			object obj;

			if ((object)binaryReader == null)
				throw new ArgumentNullException(nameof(binaryReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (BsonReader bsonReader = new BsonReader(binaryReader))
				obj = this.DeserializeObjectFromNative(bsonReader, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified JSON reader.
		/// </summary>
		/// <param name="bsonReader"> The JSON reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object DeserializeObjectFromNative(BsonReader bsonReader, Type targetType)
		{
			JsonSerializer jsonSerializer;
			object obj;

			if ((object)bsonReader == null)
				throw new ArgumentNullException(nameof(bsonReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			jsonSerializer = GetJsonSerializer();
			obj = jsonSerializer.Deserialize(bsonReader, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified JSON reader. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="bsonReader"> The JSON reader to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject DeserializeObjectFromNative<TObject>(BsonReader bsonReader)
		{
			TObject obj;
			Type targetType;

			if ((object)bsonReader == null)
				throw new ArgumentNullException(nameof(bsonReader));

			targetType = typeof(TObject);
			obj = (TObject)this.DeserializeObjectFromNative(bsonReader, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified readable stream.
		/// </summary>
		/// <param name="stream"> The readable stream to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public override object DeserializeObjectFromStream(Stream stream, Type targetType)
		{
			object obj;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (BsonReader bsonReader = new BsonReader(stream))
				obj = this.DeserializeObjectFromNative(bsonReader, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified text reader.
		/// </summary>
		/// <param name="textReader"> The text reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public override object DeserializeObjectFromTextReader(TextReader textReader, Type targetType)
		{
			throw new NotSupportedException("BSON is binary, NOT textual.");
		}

		/// <summary>
		/// Serializes an object to the specified binary writer.
		/// </summary>
		/// <param name="binaryWriter"> The binary writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public override void SerializeObjectToBinaryWriter(BinaryWriter binaryWriter, Type targetType, object obj)
		{
			if ((object)binaryWriter == null)
				throw new ArgumentNullException(nameof(binaryWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (BsonWriter bsonWriter = new BsonWriter(binaryWriter))
				this.SerializeObjectToNative(bsonWriter, targetType, obj);
		}

		/// <summary>
		/// Serializes an object to the specified JSON writer.
		/// </summary>
		/// <param name="bsonWriter"> The JSON writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SerializeObjectToNative(BsonWriter bsonWriter, Type targetType, object obj)
		{
			JsonSerializer jsonSerializer;

			if ((object)bsonWriter == null)
				throw new ArgumentNullException(nameof(bsonWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			jsonSerializer = GetJsonSerializer();
			jsonSerializer.Serialize(bsonWriter, obj, targetType);
		}

		/// <summary>
		/// Serializes an object to the specified JSON writer. This is the generic overload.
		/// </summary>
		/// <param name="bsonWriter"> The JSON writer to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SerializeObjectToNative<TObject>(BsonWriter bsonWriter, TObject obj)
		{
			Type targetType;

			if ((object)bsonWriter == null)
				throw new ArgumentNullException(nameof(bsonWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			this.SerializeObjectToNative(bsonWriter, targetType, (object)obj);
		}

		/// <summary>
		/// Serializes an object to the specified writable stream.
		/// </summary>
		/// <param name="stream"> The writable stream to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public override void SerializeObjectToStream(Stream stream, Type targetType, object obj)
		{
			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (BsonWriter bsonWriter = new BsonWriter(stream))
				this.SerializeObjectToNative(bsonWriter, targetType, obj);
		}

		/// <summary>
		/// Serializes an object to the specified text writer.
		/// </summary>
		/// <param name="textWriter"> The text writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public override void SerializeObjectToTextWriter(TextWriter textWriter, Type targetType, object obj)
		{
			throw new NotSupportedException("BSON is binary, NOT textual.");
		}

		#endregion
	}
}