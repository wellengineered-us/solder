/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace WellEngineered.Solder.Serialization
{
	public partial class NativeXmlSerializationStrategy
	{
		#region Methods/Operators

		public override async ValueTask<object> DeserializeObjectFromBinaryReaderAsync(BinaryReader binaryReader, Type targetType)
		{
			await Task.FromException(new NotSupportedException("XML serializer does not yet support async."));
			return default;
		}

		public async ValueTask<object> DeserializeObjectFromNativeAsync(XmlReader xmlReader, Type targetType)
		{
			await Task.FromException(new NotSupportedException("XML serializer does not yet support async."));
			return default;
		}

		public async ValueTask<TObject> DeserializeObjectFromNativeAsync<TObject>(XmlReader xmlReader)
		{
			await Task.FromException(new NotSupportedException("XML serializer does not yet support async."));
			return default;
		}

		public override async ValueTask<object> DeserializeObjectFromStreamAsync(Stream stream, Type targetType)
		{
			await Task.FromException(new NotSupportedException("XML serializer does not yet support async."));
			return default;
		}

		public override async ValueTask<object> DeserializeObjectFromTextReaderAsync(TextReader textReader, Type targetType)
		{
			await Task.FromException(new NotSupportedException("XML serializer does not yet support async."));
			return default;
		}

		public override async ValueTask SerializeObjectToBinaryWriterAsync(BinaryWriter binaryWriter, Type targetType, object obj)
		{
			await Task.FromException(new NotSupportedException("XML serializer does not yet support async."));
		}

		public async ValueTask SerializeObjectToNativeAsync(XmlWriter xmlWriter, Type targetType, object obj)
		{
			await Task.FromException(new NotSupportedException("XML serializer does not yet support async."));
		}

		public async ValueTask SerializeObjectToNativeAsync<TObject>(XmlWriter xmlWriter, TObject obj)
		{
			await Task.FromException(new NotSupportedException("XML serializer does not yet support async."));
		}

		public override async ValueTask SerializeObjectToStreamAsync(Stream stream, Type targetType, object obj)
		{
			await Task.FromException(new NotSupportedException("XML serializer does not yet support async."));
		}

		public override async ValueTask SerializeObjectToTextWriterAsync(TextWriter textWriter, Type targetType, object obj)
		{
			await Task.FromException(new NotSupportedException("XML serializer does not yet support async."));
		}

		#endregion
	}
}