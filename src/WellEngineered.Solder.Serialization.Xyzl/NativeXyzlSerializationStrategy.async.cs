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

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Serialization.Xyzl
{
	public partial class NativeXyzlSerializationStrategy
	{
		#region Methods/Operators

		public override ValueTask<object> DeserializeObjectFromBinaryReaderAsync(BinaryReader binaryReader, Type targetType, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public async ValueTask<object> DeserializeObjectFromNativeAsync(XmlReader xmlReader, Type targetType, CancellationToken cancellationToken)
		{
			return await this.XyzlSerializer.DeserializeAsync(xmlReader, cancellationToken);
		}

		public async ValueTask<TObject> DeserializeObjectFromNativeAsync<TObject>(XmlReader xmlReader, CancellationToken cancellationToken)
		{
			TObject obj;
			Type targetType;

			if ((object)xmlReader == null)
				throw new ArgumentNullException(nameof(xmlReader));

			targetType = typeof(TObject);
			obj = (TObject)await this.DeserializeObjectFromNativeAsync(xmlReader, targetType, cancellationToken);

			return obj;
		}

		public override async ValueTask<object> DeserializeObjectFromStreamAsync(Stream stream, Type targetType, CancellationToken cancellationToken)
		{
			return await this.XyzlSerializer.DeserializeAsync(stream, cancellationToken);
		}

		public override async ValueTask<object> DeserializeObjectFromTextReaderAsync(TextReader textReader, Type targetType, CancellationToken cancellationToken)
		{
			return await this.XyzlSerializer.DeserializeAsync(textReader, cancellationToken);
		}

		public override ValueTask SerializeObjectToBinaryWriterAsync(BinaryWriter binaryWriter, Type targetType, object obj, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public async ValueTask SerializeObjectToNativeAsync(XmlWriter xmlWriter, Type targetType, object obj, CancellationToken cancellationToken)
		{
			await this.XyzlSerializer.SerializeAsync((ISolderConfiguration)obj, xmlWriter, cancellationToken);
		}

		public async ValueTask SerializeObjectToNativeAsync<TObject>(XmlWriter xmlWriter, TObject obj, CancellationToken cancellationToken)
		{
			Type targetType;

			if ((object)xmlWriter == null)
				throw new ArgumentNullException(nameof(xmlWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			await this.SerializeObjectToNativeAsync(xmlWriter, targetType, (object)obj, cancellationToken);
		}

		public override async ValueTask SerializeObjectToStreamAsync(Stream stream, Type targetType, object obj, CancellationToken cancellationToken)
		{
			await this.XyzlSerializer.SerializeAsync((ISolderConfiguration)obj, stream, cancellationToken);
		}

		public override async ValueTask SerializeObjectToTextWriterAsync(TextWriter textWriter, Type targetType, object obj, CancellationToken cancellationToken)
		{
			await this.XyzlSerializer.SerializeAsync((ISolderConfiguration)obj, textWriter, cancellationToken);
		}

		#endregion
	}
}
#endif