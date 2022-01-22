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
	/// This is a custom XML configuration object serializer/deserializer that does not suffer from the rigidities of the .NET Framework supplied ones. This implementation was designed to be fast and flexible for XML driven tools.
	/// </summary>
	public sealed partial class XyzlSerializer : IXyzlSerializer
	{
		#region Methods/Operators

		public ValueTask<ISolderConfiguration> DeserializeAsync(string fileName)
		{
			return default;
		}

		public ValueTask<ISolderConfiguration> DeserializeAsync(Stream stream)
		{
			return default;
		}

		public ValueTask<ISolderConfiguration> DeserializeAsync(XmlReader xmlReader)
		{
			return default;
		}

		public ValueTask<ISolderConfiguration> DeserializeAsync(TextReader textReader)
		{
			return default;
		}

		public ValueTask SerializeAsync(ISolderConfiguration document, string fileName)
		{
			return default;
		}

		public ValueTask SerializeAsync(ISolderConfiguration document, Stream stream)
		{
			return default;
		}

		public ValueTask SerializeAsync(ISolderConfiguration document, XmlWriter xmlWriter)
		{
			return default;
		}

		public ValueTask SerializeAsync(ISolderConfiguration document, TextWriter textWriter)
		{
			return default;
		}

		#endregion
	}
}