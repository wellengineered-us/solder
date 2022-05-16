/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

using Newtonsoft.Json;

namespace WellEngineered.Solder.Serialization.JsonNet
{
	public partial class NativeJsonSerializationStrategy : CommonSerializationStrategy, INativeJsonSerializationStrategy
	{
		#region Constructors/Destructors

		public NativeJsonSerializationStrategy()
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

			using (StreamReader streamReader = new StreamReader(binaryReader.BaseStream))
			{
				using (JsonReader jsonReader = new JsonTextReader(streamReader))
					obj = this.DeserializeObjectFromNative(jsonReader, targetType);
			}

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified JSON reader.
		/// </summary>
		/// <param name="jsonReader"> The JSON reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object DeserializeObjectFromNative(JsonReader jsonReader, Type targetType)
		{
			JsonSerializer jsonSerializer;
			object obj;

			if ((object)jsonReader == null)
				throw new ArgumentNullException(nameof(jsonReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			jsonSerializer = GetJsonSerializer();
			obj = jsonSerializer.Deserialize(jsonReader, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified JSON reader. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="jsonReader"> The JSON reader to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject DeserializeObjectFromNative<TObject>(JsonReader jsonReader)
		{
			TObject obj;
			Type targetType;

			if ((object)jsonReader == null)
				throw new ArgumentNullException(nameof(jsonReader));

			targetType = typeof(TObject);
			obj = (TObject)this.DeserializeObjectFromNative(jsonReader, targetType);

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

			using (StreamReader streamReader = new StreamReader(stream))
			{
				using (JsonReader jsonReader = new JsonTextReader(streamReader))
					obj = this.DeserializeObjectFromNative(jsonReader, targetType);
			}

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
			object obj;

			if ((object)textReader == null)
				throw new ArgumentNullException(nameof(textReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (JsonReader jsonReader = new JsonTextReader(textReader))
				obj = this.DeserializeObjectFromNative(jsonReader, targetType);

			return obj;
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

			using (StreamWriter streamWriter = new StreamWriter(binaryWriter.BaseStream))
			{
				using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
					this.SerializeObjectToNative(jsonWriter, targetType, obj);
			}
		}

		/// <summary>
		/// Serializes an object to the specified JSON writer.
		/// </summary>
		/// <param name="jsonWriter"> The JSON writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SerializeObjectToNative(JsonWriter jsonWriter, Type targetType, object obj)
		{
			JsonSerializer jsonSerializer;

			if ((object)jsonWriter == null)
				throw new ArgumentNullException(nameof(jsonWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			jsonSerializer = GetJsonSerializer();
			jsonSerializer.Serialize(jsonWriter, obj, targetType);
		}

		/// <summary>
		/// Serializes an object to the specified JSON writer. This is the generic overload.
		/// </summary>
		/// <param name="jsonWriter"> The JSON writer to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SerializeObjectToNative<TObject>(JsonWriter jsonWriter, TObject obj)
		{
			Type targetType;

			if ((object)jsonWriter == null)
				throw new ArgumentNullException(nameof(jsonWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			this.SerializeObjectToNative(jsonWriter, targetType, (object)obj);
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

			using (StreamWriter streamWriter = new StreamWriter(stream))
			{
				using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
					this.SerializeObjectToNative(jsonWriter, targetType, obj);
			}
		}

		/// <summary>
		/// Serializes an object to the specified text writer.
		/// </summary>
		/// <param name="textWriter"> The text writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public override void SerializeObjectToTextWriter(TextWriter textWriter, Type targetType, object obj)
		{
			if ((object)textWriter == null)
				throw new ArgumentNullException(nameof(textWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (JsonWriter jsonWriter = new JsonTextWriter(textWriter))
				this.SerializeObjectToNative(jsonWriter, targetType, obj);
		}

		#endregion
	}
}