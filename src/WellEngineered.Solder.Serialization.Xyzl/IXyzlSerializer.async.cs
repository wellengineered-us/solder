/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Threading.Tasks;
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

		ValueTask<ISolderConfiguration> DeserializeAsync(string fileName);

		ValueTask<ISolderConfiguration> DeserializeAsync(Stream stream);

		ValueTask<ISolderConfiguration> DeserializeAsync(XmlReader xmlReader);

		ValueTask<ISolderConfiguration> DeserializeAsync(TextReader textReader);

		ValueTask SerializeAsync(ISolderConfiguration document, string fileName);

		ValueTask SerializeAsync(ISolderConfiguration document, Stream stream);

		ValueTask SerializeAsync(ISolderConfiguration document, XmlWriter xmlWriter);

		ValueTask SerializeAsync(ISolderConfiguration document, TextWriter textWriter);

		#endregion
	}
}