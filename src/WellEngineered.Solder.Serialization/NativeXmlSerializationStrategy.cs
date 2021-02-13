/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace WellEngineered.Solder.Serialization
{
	public partial class NativeXmlSerializationStrategy : CommonSerializationStrategy, INativeXmlSerializationStrategy
	{
		#region Constructors/Destructors

		public NativeXmlSerializationStrategy()
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
		public override object DeserializeObjectFromBinaryReader(BinaryReader binaryReader, Type targetType)
		{
			XmlSerializer xmlSerializer;
			object obj;

			if ((object)binaryReader == null)
				throw new ArgumentNullException(nameof(binaryReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			xmlSerializer = new XmlSerializer(targetType);
			obj = xmlSerializer.Deserialize(binaryReader.BaseStream);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified xml reader.
		/// </summary>
		/// <param name="xmlReader"> The xml reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object DeserializeObjectFromNative(XmlReader xmlReader, Type targetType)
		{
			XmlSerializer xmlSerializer;
			object obj;

			if ((object)xmlReader == null)
				throw new ArgumentNullException(nameof(xmlReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			xmlSerializer = new XmlSerializer(targetType);
			obj = xmlSerializer.Deserialize(xmlReader);

			return obj;
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
			XmlSerializer xmlSerializer;
			object obj;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			xmlSerializer = new XmlSerializer(targetType);
			obj = xmlSerializer.Deserialize(stream);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified text reader.
		/// </summary>
		/// <param name="textReader"> The text reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public override object DeserializeObjectFromTextReader(TextReader textReader, Type targetType)
		{
			XmlSerializer xmlSerializer;
			object obj;

			if ((object)textReader == null)
				throw new ArgumentNullException(nameof(textReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			xmlSerializer = new XmlSerializer(targetType);
			obj = xmlSerializer.Deserialize(textReader);

			return obj;
		}

		/// <summary>
		/// Serializes an object to the specified binary writer.
		/// </summary>
		/// <param name="binaryWriter"> The binary writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public override void SerializeObjectToBinaryWriter(BinaryWriter binaryWriter, Type targetType, object obj)
		{
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
		}

		/// <summary>
		/// Serializes an object to the specified xml writer.
		/// </summary>
		/// <param name="xmlWriter"> The xml writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SerializeObjectToNative(XmlWriter xmlWriter, Type targetType, object obj)
		{
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
			XmlSerializer xmlSerializer;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			xmlSerializer = new XmlSerializer(targetType);
			xmlSerializer.Serialize(stream, obj);
		}

		/// <summary>
		/// Serializes an object to the specified text writer.
		/// </summary>
		/// <param name="textWriter"> The text writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public override void SerializeObjectToTextWriter(TextWriter textWriter, Type targetType, object obj)
		{
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
		}

		#endregion
	}
}