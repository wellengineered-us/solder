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
	public abstract partial class CommonSerializationStrategy
	{
		#region Methods/Operators

		public abstract ValueTask<object> DeserializeObjectFromBinaryReaderAsync(BinaryReader binaryReader, Type targetType, CancellationToken cancellationToken = default);

		public async ValueTask<TObject> DeserializeObjectFromBinaryReaderAsync<TObject>(BinaryReader binaryReader, CancellationToken cancellationToken = default)
		{
			TObject obj;
			Type targetType;

			if ((object)binaryReader == null)
				throw new ArgumentNullException(nameof(binaryReader));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromBinaryReaderAsync(binaryReader, targetType);

			return obj;
		}

		public async ValueTask<object> DeserializeObjectFromBufferAsync(Memory<byte> value, Type targetType, CancellationToken cancellationToken = default)
		{
			object obj;
			BinaryReader binaryReader;

			await using (Stream stream = new MemoryStream(value.ToArray()))
			{
				using (binaryReader = new BinaryReader(stream))
				{
					obj = await this.DeserializeObjectFromBinaryReaderAsync(binaryReader, targetType);
					return obj;
				}
			}
		}

		public async ValueTask<TObject> DeserializeObjectFromBufferAsync<TObject>(Memory<byte> value, CancellationToken cancellationToken = default)
		{
			TObject obj;
			Type targetType;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromBufferAsync(value, targetType);

			return obj;
		}

		public async ValueTask<object> DeserializeObjectFromByteArrayAsync(byte[] value, Type targetType, CancellationToken cancellationToken = default)
		{
			object obj;
			BinaryReader binaryReader;

			await using (Stream stream = new MemoryStream(value))
			{
				using (binaryReader = new BinaryReader(stream))
				{
					obj = await this.DeserializeObjectFromBinaryReaderAsync(binaryReader, targetType);
					return obj;
				}
			}
		}

		public async ValueTask<TObject> DeserializeObjectFromByteArrayAsync<TObject>(byte[] value, CancellationToken cancellationToken = default)
		{
			TObject obj;
			Type targetType;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromByteArrayAsync(value, targetType);

			return obj;
		}

		public ValueTask<object> DeserializeObjectFromCharArrayAsync(char[] value, Type targetType, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public ValueTask<TObject> DeserializeObjectFromCharArrayAsync<TObject>(char[] value, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public async ValueTask<object> DeserializeObjectFromFileAsync(string inputFilePath, Type targetType, CancellationToken cancellationToken = default)
		{
			object obj;

			if ((object)inputFilePath == null)
				throw new ArgumentNullException(nameof(inputFilePath));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			await using (Stream stream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				obj = await this.DeserializeObjectFromStreamAsync(stream, targetType);

			return obj;
		}

		public async ValueTask<TObject> DeserializeObjectFromFileAsync<TObject>(string inputFilePath, CancellationToken cancellationToken = default)
		{
			TObject obj;
			Type targetType;

			if ((object)inputFilePath == null)
				throw new ArgumentNullException(nameof(inputFilePath));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromFileAsync(inputFilePath, targetType);

			return obj;
		}

		public abstract ValueTask<object> DeserializeObjectFromStreamAsync(Stream stream, Type targetType, CancellationToken cancellationToken = default);

		public async ValueTask<TObject> DeserializeObjectFromStreamAsync<TObject>(Stream stream, CancellationToken cancellationToken = default)
		{
			TObject obj;
			Type targetType;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromStreamAsync(stream, targetType);

			return obj;
		}

		public async ValueTask<object> DeserializeObjectFromStringAsync(string value, Type targetType, CancellationToken cancellationToken = default)
		{
			object obj;
			StringReader stringReader;

			using (stringReader = new StringReader(value))
			{
				obj = await this.DeserializeObjectFromTextReaderAsync(stringReader, targetType);
				return obj;
			}
		}

		public async ValueTask<TObject> DeserializeObjectFromStringAsync<TObject>(string value, CancellationToken cancellationToken = default)
		{
			TObject obj;
			Type targetType;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromStringAsync(value, targetType);

			return obj;
		}

		public abstract ValueTask<object> DeserializeObjectFromTextReaderAsync(TextReader textReader, Type targetType, CancellationToken cancellationToken = default);

		public async ValueTask<TObject> DeserializeObjectFromTextReaderAsync<TObject>(TextReader textReader, CancellationToken cancellationToken = default)
		{
			TObject obj;
			Type targetType;

			if ((object)textReader == null)
				throw new ArgumentNullException(nameof(textReader));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromTextReaderAsync(textReader, targetType);

			return obj;
		}

		public abstract ValueTask SerializeObjectToBinaryWriterAsync(BinaryWriter binaryWriter, Type targetType, object obj, CancellationToken cancellationToken = default);

		public async ValueTask SerializeObjectToBinaryWriterAsync<TObject>(BinaryWriter binaryWriter, TObject obj, CancellationToken cancellationToken = default)
		{
			Type targetType;

			if ((object)binaryWriter == null)
				throw new ArgumentNullException(nameof(binaryWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			await this.SerializeObjectToBinaryWriterAsync(binaryWriter, targetType, (object)obj);
		}

		public async ValueTask<Memory<byte>> SerializeObjectToBufferAsync(Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			await using (MemoryStream memoryStream = new MemoryStream())
			{
				await using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					await this.SerializeObjectToBinaryWriterAsync(binaryWriter, obj);
					return memoryStream.ToArray();
				}
			}
		}

		public async ValueTask<Memory<byte>> SerializeObjectToBufferAsync<TObject>(TObject obj, CancellationToken cancellationToken = default)
		{
			Type targetType;

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			return await this.SerializeObjectToByteArrayAsync(targetType, (object)obj);
		}

		public async ValueTask<byte[]> SerializeObjectToByteArrayAsync(Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			await using (MemoryStream memoryStream = new MemoryStream())
			{
				await using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					await this.SerializeObjectToBinaryWriterAsync(binaryWriter, obj);
					return memoryStream.ToArray();
				}
			}
		}

		public async ValueTask<byte[]> SerializeObjectToByteArrayAsync<TObject>(TObject obj, CancellationToken cancellationToken = default)
		{
			Type targetType;

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			return await this.SerializeObjectToByteArrayAsync(targetType, (object)obj);
		}

		public ValueTask<char[]> SerializeObjectToCharArrayAsync(Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public ValueTask<char[]> SerializeObjectToCharArrayAsync<TObject>(TObject obj, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public async ValueTask SerializeObjectToFileAsync<TObject>(string outputFilePath, TObject obj, CancellationToken cancellationToken = default)
		{
			Type targetType;

			if ((object)outputFilePath == null)
				throw new ArgumentNullException(nameof(outputFilePath));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			await this.SerializeObjectToFileAsync(outputFilePath, targetType, (object)obj);
		}

		public async ValueTask SerializeObjectToFileAsync(string outputFilePath, Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			if ((object)outputFilePath == null)
				throw new ArgumentNullException(nameof(outputFilePath));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			await using (Stream stream = File.Open(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
				await this.SerializeObjectToStreamAsync(stream, targetType, obj);
		}

		public abstract ValueTask SerializeObjectToStreamAsync(Stream stream, Type targetType, object obj, CancellationToken cancellationToken = default);

		public async ValueTask SerializeObjectToStreamAsync<TObject>(Stream stream, TObject obj, CancellationToken cancellationToken = default)
		{
			Type targetType;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			await this.SerializeObjectToStreamAsync(stream, targetType, (object)obj);
		}

		public async ValueTask<string> SerializeObjectToStringAsync(Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			StringWriter stringWriter;

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			await using (stringWriter = new StringWriter())
			{
				await this.SerializeObjectToTextWriterAsync(stringWriter, targetType, obj);
				return stringWriter.ToString();
			}
		}

		public async ValueTask<string> SerializeObjectToStringAsync<TObject>(TObject obj, CancellationToken cancellationToken = default)
		{
			Type targetType;

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			return await this.SerializeObjectToStringAsync(targetType, (object)obj);
		}

		public abstract ValueTask SerializeObjectToTextWriterAsync(TextWriter textWriter, Type targetType, object obj, CancellationToken cancellationToken = default);

		public async ValueTask SerializeObjectToTextWriterAsync<TObject>(TextWriter textWriter, TObject obj, CancellationToken cancellationToken = default)
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
#endif