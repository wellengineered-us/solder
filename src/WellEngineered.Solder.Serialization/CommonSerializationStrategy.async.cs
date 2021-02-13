/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Serialization
{
	public abstract partial class CommonSerializationStrategy
	{
		#region Methods/Operators

		public abstract ValueTask<object> DeserializeObjectFromBinaryReaderAsync(BinaryReader binaryReader, Type targetType);

		public async ValueTask<TObject> DeserializeObjectFromBinaryReaderAsync<TObject>(BinaryReader binaryReader)
		{
			TObject obj;
			Type targetType;

			if ((object)binaryReader == null)
				throw new ArgumentNullException(nameof(binaryReader));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromBinaryReaderAsync(binaryReader, targetType);

			return obj;
		}

		public async ValueTask<object> DeserializeObjectFromBufferAsync(Memory<byte> value, Type targetType)
		{
			object obj;
			BinaryReader binaryReader;

			using (Stream stream = new MemoryStream(value.ToArray()))
			{
				using (binaryReader = new BinaryReader(stream))
				{
					obj = await this.DeserializeObjectFromBinaryReaderAsync(binaryReader, targetType);
					return obj;
				}
			}
		}

		public async ValueTask<TObject> DeserializeObjectFromBufferAsync<TObject>(Memory<byte> value)
		{
			TObject obj;
			Type targetType;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromBufferAsync(value, targetType);

			return obj;
		}

		public async ValueTask<object> DeserializeObjectFromByteArrayAsync(byte[] value, Type targetType)
		{
			object obj;
			BinaryReader binaryReader;

			using (Stream stream = new MemoryStream(value))
			{
				using (binaryReader = new BinaryReader(stream))
				{
					obj = await this.DeserializeObjectFromBinaryReaderAsync(binaryReader, targetType);
					return obj;
				}
			}
		}

		public async ValueTask<TObject> DeserializeObjectFromByteArrayAsync<TObject>(byte[] value)
		{
			TObject obj;
			Type targetType;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromByteArrayAsync(value, targetType);

			return obj;
		}

		public ValueTask<object> DeserializeObjectFromCharArrayAsync(char[] value, Type targetType)
		{
			throw new NotImplementedException();
		}

		public ValueTask<TObject> DeserializeObjectFromCharArrayAsync<TObject>(char[] value)
		{
			throw new NotImplementedException();
		}

		public async ValueTask<object> DeserializeObjectFromFileAsync(string inputFilePath, Type targetType)
		{
			object obj;

			if ((object)inputFilePath == null)
				throw new ArgumentNullException(nameof(inputFilePath));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (Stream stream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				obj = await this.DeserializeObjectFromStreamAsync(stream, targetType);

			return obj;
		}

		public async ValueTask<TObject> DeserializeObjectFromFileAsync<TObject>(string inputFilePath)
		{
			TObject obj;
			Type targetType;

			if ((object)inputFilePath == null)
				throw new ArgumentNullException(nameof(inputFilePath));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromFileAsync(inputFilePath, targetType);

			return obj;
		}

		public abstract ValueTask<object> DeserializeObjectFromStreamAsync(Stream stream, Type targetType);

		public async ValueTask<TObject> DeserializeObjectFromStreamAsync<TObject>(Stream stream)
		{
			TObject obj;
			Type targetType;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromStreamAsync(stream, targetType);

			return obj;
		}

		public async ValueTask<object> DeserializeObjectFromStringAsync(string value, Type targetType)
		{
			object obj;
			StringReader stringReader;

			using (stringReader = new StringReader(value))
			{
				obj = await this.DeserializeObjectFromTextReaderAsync(stringReader, targetType);
				return obj;
			}
		}

		public async ValueTask<TObject> DeserializeObjectFromStringAsync<TObject>(string value)
		{
			TObject obj;
			Type targetType;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromStringAsync(value, targetType);

			return obj;
		}

		public abstract ValueTask<object> DeserializeObjectFromTextReaderAsync(TextReader textReader, Type targetType);

		public async ValueTask<TObject> DeserializeObjectFromTextReaderAsync<TObject>(TextReader textReader)
		{
			TObject obj;
			Type targetType;

			if ((object)textReader == null)
				throw new ArgumentNullException(nameof(textReader));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromTextReaderAsync(textReader, targetType);

			return obj;
		}

		public abstract ValueTask SerializeObjectToBinaryWriterAsync(BinaryWriter binaryWriter, Type targetType, object obj);

		public async ValueTask SerializeObjectToBinaryWriterAsync<TObject>(BinaryWriter binaryWriter, TObject obj)
		{
			Type targetType;

			if ((object)binaryWriter == null)
				throw new ArgumentNullException(nameof(binaryWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			await this.SerializeObjectToBinaryWriterAsync(binaryWriter, targetType, (object)obj);
		}

		public async ValueTask<Memory<byte>> SerializeObjectToBufferAsync(Type targetType, object obj)
		{
			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					await this.SerializeObjectToBinaryWriterAsync(binaryWriter, obj);
					return memoryStream.ToArray();
				}
			}
		}

		public async ValueTask<Memory<byte>> SerializeObjectToBufferAsync<TObject>(TObject obj)
		{
			Type targetType;

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			return await this.SerializeObjectToByteArrayAsync(targetType, (object)obj);
		}

		public async ValueTask<byte[]> SerializeObjectToByteArrayAsync(Type targetType, object obj)
		{
			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					await this.SerializeObjectToBinaryWriterAsync(binaryWriter, obj);
					return memoryStream.ToArray();
				}
			}
		}

		public async ValueTask<byte[]> SerializeObjectToByteArrayAsync<TObject>(TObject obj)
		{
			Type targetType;

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			return await this.SerializeObjectToByteArrayAsync(targetType, (object)obj);
		}

		public ValueTask<char[]> SerializeObjectToCharArrayAsync(Type targetType, object obj)
		{
			throw new NotImplementedException();
		}

		public ValueTask<char[]> SerializeObjectToCharArrayAsync<TObject>(TObject obj)
		{
			throw new NotImplementedException();
		}

		public async ValueTask SerializeObjectToFileAsync<TObject>(string outputFilePath, TObject obj)
		{
			Type targetType;

			if ((object)outputFilePath == null)
				throw new ArgumentNullException(nameof(outputFilePath));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			await this.SerializeObjectToFileAsync(outputFilePath, targetType, (object)obj);
		}

		public async ValueTask SerializeObjectToFileAsync(string outputFilePath, Type targetType, object obj)
		{
			if ((object)outputFilePath == null)
				throw new ArgumentNullException(nameof(outputFilePath));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (Stream stream = File.Open(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
				await this.SerializeObjectToStreamAsync(stream, targetType, obj);
		}

		public abstract ValueTask SerializeObjectToStreamAsync(Stream stream, Type targetType, object obj);

		public async ValueTask SerializeObjectToStreamAsync<TObject>(Stream stream, TObject obj)
		{
			Type targetType;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			await this.SerializeObjectToStreamAsync(stream, targetType, (object)obj);
		}

		public async ValueTask<string> SerializeObjectToStringAsync(Type targetType, object obj)
		{
			StringWriter stringWriter;

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (stringWriter = new StringWriter())
			{
				await this.SerializeObjectToTextWriterAsync(stringWriter, targetType, obj);
				return stringWriter.ToString();
			}
		}

		public async ValueTask<string> SerializeObjectToStringAsync<TObject>(TObject obj)
		{
			Type targetType;

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			return await this.SerializeObjectToStringAsync(targetType, (object)obj);
		}

		public abstract ValueTask SerializeObjectToTextWriterAsync(TextWriter textWriter, Type targetType, object obj);

		public async ValueTask SerializeObjectToTextWriterAsync<TObject>(TextWriter textWriter, TObject obj)
		{
			Type targetType;

			if ((object)textWriter == null)
				throw new ArgumentNullException(nameof(textWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			await this.SerializeObjectToTextWriterAsync(textWriter, targetType, (object)obj);
		}

		#endregion
	}
}