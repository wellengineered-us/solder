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
	/// <summary>
	/// Provides custom optimized XML configuration object serializer/deserializer behavior.
	/// </summary>
	public partial interface IXyzlSerializer
	{
		#region Methods/Operators

		/// <summary>
		/// Clears all known XML configuration object registrations.
		/// </summary>
		void ClearAllKnownObjects();

		/// <summary>
		/// Deserialize an XML configuration object graph from the specified XML configuration file.
		/// </summary>
		/// <param name="fileName"> The XML configuration file to load. </param>
		/// <returns> An XML configuration object graph. </returns>
		ISolderConfiguration Deserialize(string fileName);

		/// <summary>
		/// Deserialize an XML configuration object graph from the specified stream.
		/// </summary>
		/// <param name="stream"> The stream to load. </param>
		/// <returns> An XML configuration object graph. </returns>
		ISolderConfiguration Deserialize(Stream stream);

		/// <summary>
		/// Deserialize an XML configuration object graph from the specified XML configuration reader.
		/// </summary>
		/// <param name="xmlReader"> The XML configuration reader to load. </param>
		/// <returns> An XML configuration object graph. </returns>
		ISolderConfiguration Deserialize(XmlReader xmlReader);

		/// <summary>
		/// Deserialize an XML configuration object graph from the specified text reader.
		/// </summary>
		/// <param name="textReader"> The text reader to load. </param>
		/// <returns> An XML configuration object graph. </returns>
		ISolderConfiguration Deserialize(TextReader textReader);

		/// <summary>
		/// Registers a known XML configuration object by target type and explicit XML configuration name (local name and namespace URI). This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target type to register. </typeparam>
		/// <param name="xyzlName"> The XML configuration name (local name and namespace URI). </param>
		void RegisterKnownObject<TObject>(IXyzlName xyzlName)
			where TObject : ISolderConfiguration;

		/// <summary>
		/// Registers a known XML configuration object by target type and explicit XML configuration name (local name and namespace URI). This is the non-generic overload.
		/// </summary>
		/// <param name="xyzlName"> The XML configuration name (local name and namespace URI). </param>
		/// <param name="targetType"> The target type to register. </param>
		void RegisterKnownObject(IXyzlName xyzlName, Type targetType);

		/// <summary>
		/// Registers a known XML configuration object by target type and implicit attribute-based XML configuration name (local name and namespace URI). This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target type to register. </typeparam>
		void RegisterKnownObject<TObject>()
			where TObject : ISolderConfiguration;

		/// <summary>
		/// Registers a known XML configuration object by target type. This is the non-generic overload.
		/// </summary>
		/// <param name="targetType"> The target type to register. </param>
		void RegisterKnownObject(Type targetType);

		/// <summary>
		/// Registers a known XML configuration text object by target type. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target type to register. </typeparam>
		void RegisterKnownValueObject<TObject>()
			where TObject : IXyzlValue<string>;

		/// <summary>
		/// Registers a known XML configuration text object by target type and implicit attribute-based XML configuration name (local name and namespace URI). This is the non-generic overload.
		/// </summary>
		/// <param name="targetType"> The target type to register. </param>
		void RegisterKnownValueObject(Type targetType);

		/// <summary>
		/// Serializes an XML configuration object graph to the specified XML configuration file.
		/// </summary>
		/// <param name="document"> The document root XML configuration object. </param>
		/// <param name="fileName"> The XML configuration file to save. </param>
		void Serialize(ISolderConfiguration document, string fileName);

		/// <summary>
		/// Serializes an XML configuration object graph to the specified stream.
		/// </summary>
		/// <param name="document"> The document root XML configuration object. </param>
		/// <param name="stream"> The stream to save. </param>
		void Serialize(ISolderConfiguration document, Stream stream);

		/// <summary>
		/// Serializes an XML configuration object graph to the specified XML configuration writer.
		/// </summary>
		/// <param name="document"> The document root XML configuration object. </param>
		/// <param name="xmlWriter"> The XML configuration writer to save. </param>
		void Serialize(ISolderConfiguration document, XmlWriter xmlWriter);

		/// <summary>
		/// Serializes an XML configuration object graph to the specified text writer.
		/// </summary>
		/// <param name="document"> The document root XML configuration object. </param>
		/// <param name="textWriter"> The text writer to save. </param>
		void Serialize(ISolderConfiguration document, TextWriter textWriter);

		/// <summary>
		/// Unregisters a known XML configuration object by target type. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target type to unregister. </typeparam>
		/// <returns> A value indicating if the registration was present. </returns>
		bool UnregisterKnownObject<TObject>()
			where TObject : ISolderConfiguration;

		/// <summary>
		/// Unregisters a known XML configuration object by target type. This is the generic overload.
		/// </summary>
		/// <param name="targetType"> The target type to unregister. </param>
		/// <returns> A value indicating if the registration was present. </returns>
		bool UnregisterKnownObject(Type targetType);

		/// <summary>
		/// Unregisters a known XML configuration text object by target type. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target type to unregister. </typeparam>
		/// <returns> A value indicating if the registration was present. </returns>
		bool UnregisterKnownValueObject<TObject>()
			where TObject : IXyzlValue<string>;

		/// <summary>
		/// Unregisters a known XML configuration text object by target type. This is the generic overload.
		/// </summary>
		/// <param name="targetType"> The target type to unregister. </param>
		/// <returns> A value indicating if the registration was present. </returns>
		bool UnregisterKnownValueObject(Type targetType);

		#endregion
	}
}