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

namespace WellEngineered.Solder.Serialization.JsonNet
{
	public partial class NativeJsonSerializationStrategy
	{
		#region Methods/Operators

		public override async ValueTask<object> DeserializeObjectFromBinaryReaderAsync(BinaryReader binaryReader, Type targetType, CancellationToken cancellationToken = default)
		{
			object obj;

			if ((object)binaryReader == null)
				throw new ArgumentNullException(nameof(binaryReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (StreamReader streamReader = new StreamReader(binaryReader.BaseStream))
			{
				using (JsonReader jsonReader = new JsonTextReader(streamReader))
					obj = await this.DeserializeObjectFromNativeAsync(jsonReader, targetType, cancellationToken);
			}

			return obj;
		}

		public async ValueTask<object> DeserializeObjectFromNativeAsync(JsonReader jsonReader, Type targetType, CancellationToken cancellationToken = default)
		{
			// JSON serializer does not yet support async.
			JsonSerializer jsonSerializer;
			object obj;

			if ((object)jsonReader == null)
				throw new ArgumentNullException(nameof(jsonReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			jsonSerializer = GetJsonSerializer();
			obj = jsonSerializer.Deserialize(jsonReader, targetType);

			await Task.CompletedTask;
			return obj;
		}

		public async ValueTask<TObject> DeserializeObjectFromNativeAsync<TObject>(JsonReader jsonReader, CancellationToken cancellationToken = default)
		{
			TObject obj;
			Type targetType;

			if ((object)jsonReader == null)
				throw new ArgumentNullException(nameof(jsonReader));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromNativeAsync(jsonReader, targetType, cancellationToken);

			return obj;
		}

		public override async ValueTask<object> DeserializeObjectFromStreamAsync(Stream stream, Type targetType, CancellationToken cancellationToken = default)
		{
			object obj;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (StreamReader streamReader = new StreamReader(stream))
			{
				using (JsonReader jsonReader = new JsonTextReader(streamReader))
					obj = await this.DeserializeObjectFromNativeAsync(jsonReader, targetType, cancellationToken);
			}

			return obj;
		}

		public override async ValueTask<object> DeserializeObjectFromTextReaderAsync(TextReader textReader, Type targetType, CancellationToken cancellationToken = default)
		{
			object obj;

			if ((object)textReader == null)
				throw new ArgumentNullException(nameof(textReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (JsonReader jsonReader = new JsonTextReader(textReader))
				obj = await this.DeserializeObjectFromNativeAsync(jsonReader, targetType, cancellationToken);

			return obj;
		}

		public override async ValueTask SerializeObjectToBinaryWriterAsync(BinaryWriter binaryWriter, Type targetType, object obj, CancellationToken cancellationToken = default)
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
					await this.SerializeObjectToNativeAsync(jsonWriter, targetType, obj, cancellationToken);
			}
		}

		public async ValueTask SerializeObjectToNativeAsync(JsonWriter jsonWriter, Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			// JSON serializer does not yet support async.
			JsonSerializer jsonSerializer;

			if ((object)jsonWriter == null)
				throw new ArgumentNullException(nameof(jsonWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			jsonSerializer = GetJsonSerializer();
			jsonSerializer.Serialize(jsonWriter, obj, targetType);

			await Task.CompletedTask;
		}

		public async ValueTask SerializeObjectToNativeAsync<TObject>(JsonWriter jsonWriter, TObject obj, CancellationToken cancellationToken = default)
		{
			Type targetType;

			if ((object)jsonWriter == null)
				throw new ArgumentNullException(nameof(jsonWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			await this.SerializeObjectToNativeAsync(jsonWriter, targetType, (object)obj, cancellationToken);
		}

		public override async ValueTask SerializeObjectToStreamAsync(Stream stream, Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			await using (StreamWriter streamWriter = new StreamWriter(stream))
			{
				using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
					await this.SerializeObjectToNativeAsync(jsonWriter, targetType, obj, cancellationToken);
			}
		}

		public override async ValueTask SerializeObjectToTextWriterAsync(TextWriter textWriter, Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			if ((object)textWriter == null)
				throw new ArgumentNullException(nameof(textWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (JsonWriter jsonWriter = new JsonTextWriter(textWriter))
				await this.SerializeObjectToNativeAsync(jsonWriter, targetType, obj, cancellationToken);
		}

		#endregion
	}
}
#endif