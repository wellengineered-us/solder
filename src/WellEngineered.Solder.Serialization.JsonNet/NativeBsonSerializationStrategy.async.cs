/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace WellEngineered.Solder.Serialization.JsonNet
{
	public partial class NativeBsonSerializationStrategy
	{
		#region Methods/Operators

		public override async ValueTask<object> DeserializeObjectFromBinaryReaderAsync(BinaryReader binaryReader, Type targetType, CancellationToken cancellationToken = default)
		{
			object obj;

			if ((object)binaryReader == null)
				throw new ArgumentNullException(nameof(binaryReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (BsonReader bsonReader = new BsonReader(binaryReader))
				obj = await this.DeserializeObjectFromNativeAsync(bsonReader, targetType, cancellationToken);

			return obj;
		}

		public async ValueTask<object> DeserializeObjectFromNativeAsync(BsonReader bsonReader, Type targetType, CancellationToken cancellationToken = default)
		{
			// JSON serializer does not yet support async.
			JsonSerializer jsonSerializer;
			object obj;

			if ((object)bsonReader == null)
				throw new ArgumentNullException(nameof(bsonReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			jsonSerializer = GetJsonSerializer();
			obj = jsonSerializer.Deserialize(bsonReader, targetType);

			await Task.CompletedTask;
			return obj;
		}

		public async ValueTask<TObject> DeserializeObjectFromNativeAsync<TObject>(BsonReader bsonReader, CancellationToken cancellationToken = default)
		{
			TObject obj;
			Type targetType;

			if ((object)bsonReader == null)
				throw new ArgumentNullException(nameof(bsonReader));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromNativeAsync(bsonReader, targetType, cancellationToken);

			return obj;
		}

		public override async ValueTask<object> DeserializeObjectFromStreamAsync(Stream stream, Type targetType, CancellationToken cancellationToken = default)
		{
			object obj;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (BsonReader bsonReader = new BsonReader(stream))
				obj = await this.DeserializeObjectFromNativeAsync(bsonReader, targetType, cancellationToken);

			return obj;
		}

		public override ValueTask<object> DeserializeObjectFromTextReaderAsync(TextReader textReader, Type targetType, CancellationToken cancellationToken = default)
		{
			// JSON serializer does not yet support async.
			throw new NotSupportedException("BSON is binary, NOT textual.");
		}

		public override async ValueTask SerializeObjectToBinaryWriterAsync(BinaryWriter binaryWriter, Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			if ((object)binaryWriter == null)
				throw new ArgumentNullException(nameof(binaryWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (BsonWriter bsonWriter = new BsonWriter(binaryWriter))
				await this.SerializeObjectToNativeAsync(bsonWriter, targetType, obj, cancellationToken);
		}

		public async ValueTask SerializeObjectToNativeAsync(BsonWriter bsonWriter, Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			// JSON serializer does not yet support async.
			JsonSerializer jsonSerializer;

			if ((object)bsonWriter == null)
				throw new ArgumentNullException(nameof(bsonWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			jsonSerializer = GetJsonSerializer();
			jsonSerializer.Serialize(bsonWriter, obj, targetType);

			await Task.CompletedTask;
		}

		public async ValueTask SerializeObjectToNativeAsync<TObject>(BsonWriter bsonWriter, TObject obj, CancellationToken cancellationToken = default)
		{
			Type targetType;

			if ((object)bsonWriter == null)
				throw new ArgumentNullException(nameof(bsonWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			await this.SerializeObjectToNativeAsync(bsonWriter, targetType, (object)obj, cancellationToken);
		}

		public override async ValueTask SerializeObjectToStreamAsync(Stream stream, Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (BsonWriter bsonWriter = new BsonWriter(stream))
				await this.SerializeObjectToNativeAsync(bsonWriter, targetType, obj, cancellationToken);
		}

		public override ValueTask SerializeObjectToTextWriterAsync(TextWriter textWriter, Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			// JSON serializer does not yet support async.
			throw new NotSupportedException("BSON is binary, NOT textual.");
		}

		#endregion
	}
}
#endif