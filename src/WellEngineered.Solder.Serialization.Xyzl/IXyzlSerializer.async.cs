/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Threading;
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

		ValueTask<ISolderConfiguration> DeserializeAsync(string fileName, CancellationToken cancellationToken = default);

		ValueTask<ISolderConfiguration> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default);

		ValueTask<ISolderConfiguration> DeserializeAsync(XmlReader xmlReader, CancellationToken cancellationToken = default);

		ValueTask<ISolderConfiguration> DeserializeAsync(TextReader textReader, CancellationToken cancellationToken = default);

		ValueTask SerializeAsync(ISolderConfiguration document, string fileName, CancellationToken cancellationToken = default);

		ValueTask SerializeAsync(ISolderConfiguration document, Stream stream, CancellationToken cancellationToken = default);

		ValueTask SerializeAsync(ISolderConfiguration document, XmlWriter xmlWriter, CancellationToken cancellationToken = default);

		ValueTask SerializeAsync(ISolderConfiguration document, TextWriter textWriter, CancellationToken cancellationToken = default);

		#endregion
	}
}