/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WellEngineered.Solder.Serialization
{
	public partial class NativeXmlSerializationStrategy
	{
		#region Methods/Operators

		public override async ValueTask<object> DeserializeObjectFromBinaryReaderAsync(BinaryReader binaryReader, Type targetType, CancellationToken cancellationToken = default)
		{
			// XML serializer does not yet support async.
			XmlSerializer xmlSerializer;
			object obj;

			if ((object)binaryReader == null)
				throw new ArgumentNullException(nameof(binaryReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			xmlSerializer = new XmlSerializer(targetType);
			obj = xmlSerializer.Deserialize(binaryReader.BaseStream);

			await Task.CompletedTask;
			return obj;
		}

		public async ValueTask<object> DeserializeObjectFromNativeAsync(XmlReader xmlReader, Type targetType, CancellationToken cancellationToken = default)
		{
			// XML serializer does not yet support async.
			XmlSerializer xmlSerializer;
			object obj;

			if ((object)xmlReader == null)
				throw new ArgumentNullException(nameof(xmlReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			xmlSerializer = new XmlSerializer(targetType);
			obj = xmlSerializer.Deserialize(xmlReader);

			await Task.CompletedTask;
			return obj;
		}

		public async ValueTask<TObject> DeserializeObjectFromNativeAsync<TObject>(XmlReader xmlReader, CancellationToken cancellationToken = default)
		{
			// XML serializer does not yet support async.
			TObject obj;
			Type targetType;

			if ((object)xmlReader == null)
				throw new ArgumentNullException(nameof(xmlReader));

			targetType = typeof(TObject);

			obj = (TObject)(object)await this.DeserializeObjectFromNativeAsync(xmlReader, targetType, cancellationToken);

			await Task.CompletedTask;
			return obj;
		}

		public override async ValueTask<object> DeserializeObjectFromStreamAsync(Stream stream, Type targetType, CancellationToken cancellationToken = default)
		{
			// XML serializer does not yet support async.
			XmlSerializer xmlSerializer;
			object obj;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			xmlSerializer = new XmlSerializer(targetType);
			obj = xmlSerializer.Deserialize(stream);

			await Task.CompletedTask;
			return obj;
		}

		public override async ValueTask<object> DeserializeObjectFromTextReaderAsync(TextReader textReader, Type targetType, CancellationToken cancellationToken = default)
		{
			// XML serializer does not yet support async.
			XmlSerializer xmlSerializer;
			object obj;

			if ((object)textReader == null)
				throw new ArgumentNullException(nameof(textReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			xmlSerializer = new XmlSerializer(targetType);
			obj = xmlSerializer.Deserialize(textReader);

			await Task.CompletedTask;
			return obj;
		}

		public override async ValueTask SerializeObjectToBinaryWriterAsync(BinaryWriter binaryWriter, Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			// XML serializer does not yet support async.
			XmlSerializer xmlSerializer;

			if ((object)binaryWriter == null)
				throw new ArgumentNullException(nameof(binaryWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();
			xmlSerializer = new XmlSerializer(targetType);
			xmlSerializer.Serialize(binaryWriter.BaseStream, obj);

			await Task.CompletedTask;
		}

		public async ValueTask SerializeObjectToNativeAsync(XmlWriter xmlWriter, Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			// XML serializer does not yet support async.
			XmlSerializer xmlSerializer;

			if ((object)xmlWriter == null)
				throw new ArgumentNullException(nameof(xmlWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();
			xmlSerializer = new XmlSerializer(targetType);
			xmlSerializer.Serialize(xmlWriter, obj);

			await Task.CompletedTask;
		}

		public async ValueTask SerializeObjectToNativeAsync<TObject>(XmlWriter xmlWriter, TObject obj, CancellationToken cancellationToken = default)
		{
			// XML serializer does not yet support async.
			Type targetType;

			if ((object)xmlWriter == null)
				throw new ArgumentNullException(nameof(xmlWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			await this.SerializeObjectToNativeAsync(xmlWriter, targetType, (object)obj, cancellationToken);
		}

		public override async ValueTask SerializeObjectToStreamAsync(Stream stream, Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			// XML serializer does not yet support async.
			XmlSerializer xmlSerializer;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			xmlSerializer = new XmlSerializer(targetType);
			xmlSerializer.Serialize(stream, obj);

			await Task.CompletedTask;
		}

		public override async ValueTask SerializeObjectToTextWriterAsync(TextWriter textWriter, Type targetType, object obj, CancellationToken cancellationToken = default)
		{
			// XML serializer does not yet support async.
			XmlSerializer xmlSerializer;

			if ((object)textWriter == null)
				throw new ArgumentNullException(nameof(textWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();
			xmlSerializer = new XmlSerializer(targetType);
			xmlSerializer.Serialize(textWriter, obj);

			await Task.CompletedTask;
		}

		#endregion
	}
}
#endif