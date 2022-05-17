/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

namespace WellEngineered.Solder.Serialization
{
	public abstract partial class CommonSerializationStrategy : ICommonSerializationStrategy
	{
		#region Constructors/Destructors

		protected CommonSerializationStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified text reader.
		/// </summary>
		/// <param name="binaryReader"> The text reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public abstract object DeserializeObjectFromBinaryReader(BinaryReader binaryReader, Type targetType);

		/// <summary>
		/// Deserializes an object from the specified text reader. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="binaryReader"> The text reader to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject DeserializeObjectFromBinaryReader<TObject>(BinaryReader binaryReader)
		{
			TObject obj;
			Type targetType;

			if ((object)binaryReader == null)
				throw new ArgumentNullException(nameof(binaryReader));

			targetType = typeof(TObject);
			obj = (TObject)this.DeserializeObjectFromBinaryReader(binaryReader, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified buffer value.
		/// </summary>
		/// <param name="value"> The buffer value to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object DeserializeObjectFromBuffer(Memory<byte> value, Type targetType)
		{
			object obj;
			BinaryReader binaryReader;

			using (Stream stream = new MemoryStream(value.ToArray()))
			{
				using (binaryReader = new BinaryReader(stream))
				{
					obj = this.DeserializeObjectFromBinaryReader(binaryReader, targetType);
					return obj;
				}
			}
		}

		/// <summary>
		/// Deserializes an object from the specified text value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="value"> The buffer value to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject DeserializeObjectFromBuffer<TObject>(Memory<byte> value)
		{
			TObject obj;
			Type targetType;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			targetType = typeof(TObject);
			obj = (TObject)this.DeserializeObjectFromBuffer(value, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified byte array value.
		/// </summary>
		/// <param name="value"> The byte array value to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object DeserializeObjectFromByteArray(byte[] value, Type targetType)
		{
			object obj;
			BinaryReader binaryReader;

			using (Stream stream = new MemoryStream(value))
			{
				using (binaryReader = new BinaryReader(stream))
				{
					obj = this.DeserializeObjectFromBinaryReader(binaryReader, targetType);
					return obj;
				}
			}
		}

		/// <summary>
		/// Deserializes an object from the specified byte array value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="value"> The byte array value to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject DeserializeObjectFromByteArray<TObject>(byte[] value)
		{
			TObject obj;
			Type targetType;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			targetType = typeof(TObject);
			obj = (TObject)this.DeserializeObjectFromByteArray(value, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified char array value.
		/// </summary>
		/// <param name="value"> The char array value to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object DeserializeObjectFromCharArray(char[] value, Type targetType)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Deserializes an object from the specified char array value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="value"> The char array value to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject DeserializeObjectFromCharArray<TObject>(char[] value)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Deserializes an object from the specified input file.
		/// </summary>
		/// <param name="inputFilePath"> The input file path to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object DeserializeObjectFromFile(string inputFilePath, Type targetType)
		{
			object obj;

			if ((object)inputFilePath == null)
				throw new ArgumentNullException(nameof(inputFilePath));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (Stream stream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				obj = this.DeserializeObjectFromStream(stream, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified input file. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="inputFilePath"> The input file path to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject DeserializeObjectFromFile<TObject>(string inputFilePath)
		{
			TObject obj;
			Type targetType;

			if ((object)inputFilePath == null)
				throw new ArgumentNullException(nameof(inputFilePath));

			targetType = typeof(TObject);
			obj = (TObject)this.DeserializeObjectFromFile(inputFilePath, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified readable stream.
		/// </summary>
		/// <param name="stream"> The readable stream to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public abstract object DeserializeObjectFromStream(Stream stream, Type targetType);

		/// <summary>
		/// Deserializes an object from the specified readable stream. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="stream"> The readable stream to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject DeserializeObjectFromStream<TObject>(Stream stream)
		{
			TObject obj;
			Type targetType;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			targetType = typeof(TObject);
			obj = (TObject)this.DeserializeObjectFromStream(stream, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified string value.
		/// </summary>
		/// <param name="value"> The string value to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object DeserializeObjectFromString(string value, Type targetType)
		{
			object obj;
			StringReader stringReader;

			using (stringReader = new StringReader(value))
			{
				obj = this.DeserializeObjectFromTextReader(stringReader, targetType);
				return obj;
			}
		}

		/// <summary>
		/// Deserializes an object from the specified text value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="value"> The string value to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject DeserializeObjectFromString<TObject>(string value)
		{
			TObject obj;
			Type targetType;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			targetType = typeof(TObject);
			obj = (TObject)this.DeserializeObjectFromString(value, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified text reader.
		/// </summary>
		/// <param name="textReader"> The text reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public abstract object DeserializeObjectFromTextReader(TextReader textReader, Type targetType);

		/// <summary>
		/// Deserializes an object from the specified text reader. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="textReader"> The text reader to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject DeserializeObjectFromTextReader<TObject>(TextReader textReader)
		{
			TObject obj;
			Type targetType;

			if ((object)textReader == null)
				throw new ArgumentNullException(nameof(textReader));

			targetType = typeof(TObject);
			obj = (TObject)this.DeserializeObjectFromTextReader(textReader, targetType);

			return obj;
		}

		/// <summary>
		/// Serializes an object to the specified binary writer.
		/// </summary>
		/// <param name="binaryWriter"> The binary writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public abstract void SerializeObjectToBinaryWriter(BinaryWriter binaryWriter, Type targetType, object obj);

		/// <summary>
		/// Serializes an object to the specified binary writer. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="binaryWriter"> The binary writer to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SerializeObjectToBinaryWriter<TObject>(BinaryWriter binaryWriter, TObject obj)
		{
			Type targetType;

			if ((object)binaryWriter == null)
				throw new ArgumentNullException(nameof(binaryWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			this.SerializeObjectToBinaryWriter(binaryWriter, targetType, (object)obj);
		}

		/// <summary>
		/// Serializes an object to a buffer value.
		/// </summary>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A buffer representation of the object graph. </returns>
		public Memory<byte> SerializeObjectToBuffer(Type targetType, object obj)
		{
			BinaryWriter binaryWriter;

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (binaryWriter = new BinaryWriter(memoryStream))
				{
					this.SerializeObjectToBinaryWriter(binaryWriter, obj);
					return memoryStream.ToArray();
				}
			}
		}

		/// <summary>
		/// Serializes an object to a buffer value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A buffer representation of the object graph. </returns>
		public Memory<byte> SerializeObjectToBuffer<TObject>(TObject obj)
		{
			Type targetType;

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			return this.SerializeObjectToByteArray(targetType, (object)obj);
		}

		/// <summary>
		/// Serializes an object to a byte array value.
		/// </summary>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A byte array representation of the object graph. </returns>
		public byte[] SerializeObjectToByteArray(Type targetType, object obj)
		{
			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					this.SerializeObjectToBinaryWriter(binaryWriter, obj);
					return memoryStream.ToArray();
				}
			}
		}

		/// <summary>
		/// Serializes an object to a byte array value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A byte array representation of the object graph. </returns>
		public byte[] SerializeObjectToByteArray<TObject>(TObject obj)
		{
			Type targetType;

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			return this.SerializeObjectToByteArray(targetType, (object)obj);
		}

		/// <summary>
		/// Serializes an object to a char array value.
		/// </summary>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A char array representation of the object graph. </returns>
		public char[] SerializeObjectToCharArray(Type targetType, object obj)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Serializes an object to a char array value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A char array representation of the object graph. </returns>
		public char[] SerializeObjectToCharArray<TObject>(TObject obj)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Serializes an object to the specified output file.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="outputFilePath"> The output file path to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SerializeObjectToFile<TObject>(string outputFilePath, TObject obj)
		{
			Type targetType;

			if ((object)outputFilePath == null)
				throw new ArgumentNullException(nameof(outputFilePath));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			this.SerializeObjectToFile(outputFilePath, targetType, (object)obj);
		}

		/// <summary>
		/// Serializes an object to the specified output file.
		/// </summary>
		/// <param name="outputFilePath"> The output file path to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SerializeObjectToFile(string outputFilePath, Type targetType, object obj)
		{
			if ((object)outputFilePath == null)
				throw new ArgumentNullException(nameof(outputFilePath));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (Stream stream = File.Open(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
				this.SerializeObjectToStream(stream, targetType, obj);
		}

		/// <summary>
		/// Serializes an object to the specified writable stream.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="stream"> The writable stream to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SerializeObjectToStream<TObject>(Stream stream, TObject obj)
		{
			Type targetType;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			this.SerializeObjectToStream(stream, targetType, (object)obj);
		}

		/// <summary>
		/// Serializes an object to the specified writable stream.
		/// </summary>
		/// <param name="stream"> The writable stream to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public abstract void SerializeObjectToStream(Stream stream, Type targetType, object obj);

		/// <summary>
		/// Serializes an object to a string value.
		/// </summary>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A string representation of the object graph. </returns>
		public string SerializeObjectToString(Type targetType, object obj)
		{
			StringWriter stringWriter;

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (stringWriter = new StringWriter())
			{
				this.SerializeObjectToTextWriter(stringWriter, targetType, obj);
				return stringWriter.ToString();
			}
		}

		/// <summary>
		/// Serializes an object to a string value. This is the generic overload.
		/// </summary>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A string representation of the object graph. </returns>
		public string SerializeObjectToString<TObject>(TObject obj)
		{
			Type targetType;

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			return this.SerializeObjectToString(targetType, (object)obj);
		}

		/// <summary>
		/// Serializes an object to the specified text writer.
		/// </summary>
		/// <param name="textWriter"> The text writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public abstract void SerializeObjectToTextWriter(TextWriter textWriter, Type targetType, object obj);

		/// <summary>
		/// Serializes an object to the specified text writer. This is the generic overload.
		/// </summary>
		/// <param name="textWriter"> The text writer to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SerializeObjectToTextWriter<TObject>(TextWriter textWriter, TObject obj)
		{
			Type targetType;

			if ((object)textWriter == null)
				throw new ArgumentNullException(nameof(textWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			this.SerializeObjectToTextWriter(textWriter, targetType, (object)obj);
		}

		#endregion
	}
}