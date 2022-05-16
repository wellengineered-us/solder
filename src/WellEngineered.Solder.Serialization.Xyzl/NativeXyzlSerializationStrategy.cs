/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Xml;

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Serialization.Xyzl
{
	public partial class NativeXyzlSerializationStrategy : CommonSerializationStrategy, INativeXmlSerializationStrategy
	{
		#region Constructors/Destructors

		public NativeXyzlSerializationStrategy(IXyzlSerializer xyzlSerializer)
		{
			if ((object)xyzlSerializer == null)
				throw new ArgumentNullException(nameof(xyzlSerializer));

			this.xyzlSerializer = xyzlSerializer;
		}

		#endregion

		#region Fields/Constants

		private readonly IXyzlSerializer xyzlSerializer;

		#endregion

		#region Properties/Indexers/Events

		private IXyzlSerializer XyzlSerializer
		{
			get
			{
				return this.xyzlSerializer;
			}
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified text reader.
		/// </summary>
		/// <param name="binaryReader"> The text reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public override object DeserializeObjectFromBinaryReader(BinaryReader binaryReader, Type targetType)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Deserializes an object from the specified xml reader.
		/// </summary>
		/// <param name="xmlReader"> The xml reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object DeserializeObjectFromNative(XmlReader xmlReader, Type targetType)
		{
			return this.XyzlSerializer.Deserialize(xmlReader);
		}

		/// <summary>
		/// Deserializes an object from the specified xml reader. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="xmlReader"> The xml reader to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject DeserializeObjectFromNative<TObject>(XmlReader xmlReader)
		{
			TObject obj;
			Type targetType;

			if ((object)xmlReader == null)
				throw new ArgumentNullException(nameof(xmlReader));

			targetType = typeof(TObject);
			obj = (TObject)this.DeserializeObjectFromNative(xmlReader, targetType);

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
			return this.XyzlSerializer.Deserialize(stream);
		}

		/// <summary>
		/// Deserializes an object from the specified text reader.
		/// </summary>
		/// <param name="textReader"> The text reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public override object DeserializeObjectFromTextReader(TextReader textReader, Type targetType)
		{
			return this.XyzlSerializer.Deserialize(textReader);
		}

		/// <summary>
		/// Serializes an object to the specified binary writer.
		/// </summary>
		/// <param name="binaryWriter"> The binary writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public override void SerializeObjectToBinaryWriter(BinaryWriter binaryWriter, Type targetType, object obj)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Serializes an object to the specified xml writer.
		/// </summary>
		/// <param name="xmlWriter"> The xml writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SerializeObjectToNative(XmlWriter xmlWriter, Type targetType, object obj)
		{
			this.XyzlSerializer.Serialize((ISolderConfiguration)obj, xmlWriter);
		}

		/// <summary>
		/// Serializes an object to the specified xml writer. This is the generic overload.
		/// </summary>
		/// <param name="xmlWriter"> The xml writer to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SerializeObjectToNative<TObject>(XmlWriter xmlWriter, TObject obj)
		{
			Type targetType;

			if ((object)xmlWriter == null)
				throw new ArgumentNullException(nameof(xmlWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			this.SerializeObjectToNative(xmlWriter, targetType, (object)obj);
		}

		/// <summary>
		/// Serializes an object to the specified writable stream.
		/// </summary>
		/// <param name="stream"> The writable stream to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public override void SerializeObjectToStream(Stream stream, Type targetType, object obj)
		{
			this.XyzlSerializer.Serialize((ISolderConfiguration)obj, stream);
		}

		/// <summary>
		/// Serializes an object to the specified text writer.
		/// </summary>
		/// <param name="textWriter"> The text writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public override void SerializeObjectToTextWriter(TextWriter textWriter, Type targetType, object obj)
		{
			this.XyzlSerializer.Serialize((ISolderConfiguration)obj, textWriter);
		}

		#endregion
	}
}