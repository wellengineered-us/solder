/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json.Bson;

namespace WellEngineered.Solder.Serialization.JsonNet
{
	public partial class NativeBsonSerializationStrategy
	{
		#region Methods/Operators

		public override async ValueTask<object> DeserializeObjectFromBinaryReaderAsync(BinaryReader binaryReader, Type targetType)
		{
			object obj;

			if ((object)binaryReader == null)
				throw new ArgumentNullException(nameof(binaryReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (BsonReader bsonReader = new BsonReader(binaryReader))
				obj = await this.DeserializeObjectFromNativeAsync(bsonReader, targetType);

			return obj;
		}

		public async ValueTask<object> DeserializeObjectFromNativeAsync(BsonReader bsonReader, Type targetType)
		{
			await Task.FromException(new NotSupportedException("JSON serializer does not yet support async."));
			return default;
		}

		public async ValueTask<TObject> DeserializeObjectFromNativeAsync<TObject>(BsonReader bsonReader)
		{
			TObject obj;
			Type targetType;

			if ((object)bsonReader == null)
				throw new ArgumentNullException(nameof(bsonReader));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromNativeAsync(bsonReader, targetType);

			return obj;
		}

		public override async ValueTask<object> DeserializeObjectFromStreamAsync(Stream stream, Type targetType)
		{
			object obj;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (BsonReader bsonReader = new BsonReader(stream))
				obj = await this.DeserializeObjectFromNativeAsync(bsonReader, targetType);

			return obj;
		}

		public override async ValueTask<object> DeserializeObjectFromTextReaderAsync(TextReader textReader, Type targetType)
		{
			await Task.FromException(new NotSupportedException());
			return default;
		}

		public override async ValueTask SerializeObjectToBinaryWriterAsync(BinaryWriter binaryWriter, Type targetType, object obj)
		{
			if ((object)binaryWriter == null)
				throw new ArgumentNullException(nameof(binaryWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (BsonWriter bsonWriter = new BsonWriter(binaryWriter))
				await this.SerializeObjectToNativeAsync(bsonWriter, targetType, obj);
		}

		public async ValueTask SerializeObjectToNativeAsync(BsonWriter bsonWriter, Type targetType, object obj)
		{
			await Task.FromException(new NotSupportedException("JSON serializer does not yet support async."));
		}

		public async ValueTask SerializeObjectToNativeAsync<TObject>(BsonWriter bsonWriter, TObject obj)
		{
			Type targetType;

			if ((object)bsonWriter == null)
				throw new ArgumentNullException(nameof(bsonWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			await this.SerializeObjectToNativeAsync(bsonWriter, targetType, (object)obj);
		}

		public override async ValueTask SerializeObjectToStreamAsync(Stream stream, Type targetType, object obj)
		{
			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (BsonWriter bsonWriter = new BsonWriter(stream))
				await this.SerializeObjectToNativeAsync(bsonWriter, targetType, obj);
		}

		public override async ValueTask SerializeObjectToTextWriterAsync(TextWriter textWriter, Type targetType, object obj)
		{
			await Task.FromException(new NotSupportedException());
		}

		#endregion
	}
}