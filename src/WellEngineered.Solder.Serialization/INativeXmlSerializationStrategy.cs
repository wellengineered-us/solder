/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Xml;

namespace WellEngineered.Solder.Serialization
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects using XML semantics.
	/// </summary>
	public interface INativeXmlSerializationStrategy : INativeSerializationStrategy<XmlReader, XmlWriter>
	{
	}
}