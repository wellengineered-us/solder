/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
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

		public async ValueTask<ISolderConfiguration> DeserializeAsync(string fileName, CancellationToken cancellationToken = default)
		{
			ISolderConfiguration document;

			if ((object)fileName == null)
				throw new ArgumentNullException(nameof(fileName));

			if (string.IsNullOrWhiteSpace(fileName))
				throw new ArgumentOutOfRangeException(nameof(fileName));

			await using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				document = await this.DeserializeAsync(stream, cancellationToken);
				return document;
			}
		}

		public async ValueTask<ISolderConfiguration> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			XmlTextReader xmlTextReader;
			ISolderConfiguration document;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			// DO NOT USE A USING BLOCK HERE (CALLER OWNS STREAM) !!!
			xmlTextReader = new XmlTextReader(stream);
			document = await this.DeserializeAsync(xmlTextReader, cancellationToken);
			return document;
		}

		public async ValueTask<ISolderConfiguration> DeserializeAsync(XmlReader xmlReader, CancellationToken cancellationToken = default)
		{
			IXyzlName elementXyzlName = null, attributeXyzlName, previousElementXyzlName;

			ISolderConfiguration documentSolderConfiguration = null;
			ISolderConfiguration currentSolderConfiguration;

			Stack<ISolderConfiguration> contextStack;
			bool isEmptyElement, isTextElement;
			Dictionary<IXyzlName, string> attributes;

			IXmlLineInfo xmlLineInfo;

			if ((object)xmlReader == null)
				throw new ArgumentNullException(nameof(xmlReader));

			// setup contextual data
			attributes = new Dictionary<IXyzlName, string>();
			contextStack = new Stack<ISolderConfiguration>();
			xmlLineInfo = xmlReader as IXmlLineInfo;

			// walk the XML document
			while (await xmlReader.ReadAsync())
			{
				// determine node type
				if (xmlReader.NodeType == XmlNodeType.CDATA ||
					xmlReader.NodeType == XmlNodeType.Text) // textual node
				{
					// clear previous attributes
					attributes.Clear();

					// is this a text element?
					isTextElement = this.IsTextElement(contextStack, elementXyzlName ?? new XyzlName());

					// get the current XML configuration object as XML text object
					currentSolderConfiguration = this.DeserializeFromXmlText(xmlLineInfo, contextStack, xmlReader.Value, isTextElement ? elementXyzlName : null);

					// is this a text element? if so, deserialize into it in a special maner
					if (isTextElement)
						this.DeserializeFromXml(xmlLineInfo, contextStack, null, elementXyzlName, attributes, (IXyzlValue<string>)currentSolderConfiguration);
				}
				else if (xmlReader.NodeType == XmlNodeType.Element) // actual elements
				{
					// DebugWrite(string.Format("{2} <{0}{1}>", xmlReader.LocalName, xmlReader.IsEmptyElement ? " /" : string.Empty, xmlReader.IsEmptyElement ? "empty" : "begin"));

					// stash away previous element
					//previousElementXyzlName = elementXyzlName;
					// fixes a bug here
					if (contextStack.Count > 0)
						previousElementXyzlName = this.GetConfiguredName(contextStack.Peek());
					else
						previousElementXyzlName = null;

					// create the element XML name
					elementXyzlName = new XyzlName()
									{
										LocalName = xmlReader.LocalName,
										NamespaceUri = xmlReader.NamespaceURI
									};

					// is this an empty element?
					isEmptyElement = xmlReader.IsEmptyElement;

					// is this a text element?
					isTextElement = this.IsTextElement(contextStack, elementXyzlName);

					// clear previous attributes
					attributes.Clear();

					// iterate over attributes of current element
					for (int ai = 0; ai < xmlReader.AttributeCount; ai++)
					{
						// traverse to next attribute
						xmlReader.MoveToAttribute(ai);

						// create the attribute XML name
						attributeXyzlName = new XyzlName()
											{
												LocalName = xmlReader.LocalName,
												NamespaceUri = xmlReader.NamespaceURI
											};

						// append to attribute collection
						attributes.Add(attributeXyzlName, xmlReader.Value);
					}

					// clear attribute name
					attributeXyzlName = null;

					// is this not a text element?
					if (!isTextElement)
					{
						// deserialize current XML configuration object
						currentSolderConfiguration = this.DeserializeFromXml(xmlLineInfo, contextStack, previousElementXyzlName, elementXyzlName, attributes, null);
					}
					else
					{
						// use 'dummy' current XML configuration object (the parent so depth counts are correct and IsTextElement() works)
						currentSolderConfiguration = contextStack.Peek();
					}

					// check context stack depth for emptiness
					if (contextStack.Count <= 0)
					{
						// document element is current element when no context present
						documentSolderConfiguration = currentSolderConfiguration;
					}

					// push current XML configuration object as parent XML configuration object if there are children possible (no empty element)
					if (!isEmptyElement)
						contextStack.Push(currentSolderConfiguration);
				}
				else if (xmlReader.NodeType == XmlNodeType.EndElement) // closing element
				{
					// DebugWrite(string.Format("end <{0}>", xmlReader.LocalName));

					// create the element XML name
					elementXyzlName = new XyzlName()
									{
										LocalName = xmlReader.LocalName,
										NamespaceUri = xmlReader.NamespaceURI
									};

					// is this a text element?
					isTextElement = this.IsTextElement(contextStack, elementXyzlName);

					// sanity check
					if (contextStack.Count < 1)
						throw new XyzlException(string.Format("TODO: error message"));

					// pop element off stack (unwind)
					contextStack.Pop();

					// clear attribute name
					elementXyzlName = null;
				}
			}

			// sanity check
			if (contextStack.Count != 0)
				throw new XyzlException(string.Format("TODO: error message"));

			// ...and I'm spent!
			return documentSolderConfiguration;
		}

		public async ValueTask<ISolderConfiguration> DeserializeAsync(TextReader textReader, CancellationToken cancellationToken = default)
		{
			XmlTextReader xmlTextReader;
			ISolderConfiguration document;

			if ((object)textReader == null)
				throw new ArgumentNullException(nameof(textReader));

			// DO NOT USE A USING BLOCK HERE (CALLER OWNS TEXTWRITER) !!!
			xmlTextReader = new XmlTextReader(textReader);
			document = await this.DeserializeAsync(xmlTextReader, cancellationToken);
			return document;
		}

		public async ValueTask SerializeAsync(ISolderConfiguration document, string fileName, CancellationToken cancellationToken = default)
		{
			if ((object)document == null)
				throw new ArgumentNullException(nameof(document));

			if ((object)fileName == null)
				throw new ArgumentNullException(nameof(fileName));

			if (string.IsNullOrWhiteSpace(fileName))
				throw new ArgumentOutOfRangeException(nameof(fileName));

			await using (Stream stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
				await this.SerializeAsync(document, stream, cancellationToken);
		}

		public async ValueTask SerializeAsync(ISolderConfiguration document, Stream stream, CancellationToken cancellationToken = default)
		{
			XmlTextWriter xmlTextWriter;

			if ((object)document == null)
				throw new ArgumentNullException(nameof(document));

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			// DO NOT USE A USING BLOCK HERE (CALLER OWNS STREAM) !!!
			xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.Indentation = 1;
			xmlTextWriter.IndentChar = '\t';

			await this.SerializeAsync(document, xmlTextWriter, cancellationToken);
			await xmlTextWriter.FlushAsync();
		}

		public async ValueTask SerializeAsync(ISolderConfiguration document, XmlWriter xmlWriter, CancellationToken cancellationToken = default)
		{
			XmlTextWriter xmlTextWriter;

			if ((object)document == null)
				throw new ArgumentNullException(nameof(document));

			if ((object)xmlWriter == null)
				throw new ArgumentNullException(nameof(xmlWriter));

			xmlTextWriter = xmlWriter as XmlTextWriter;

			if ((object)xmlTextWriter != null &&
				xmlTextWriter.Formatting == Formatting.None)
			{
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.Indentation = 1;
				xmlTextWriter.IndentChar = '\t';
			}

			// no async available by intention...
			this.SerializeToXml(xmlWriter, document, null);
			await Task.CompletedTask;
		}

		public async ValueTask SerializeAsync(ISolderConfiguration document, TextWriter textWriter, CancellationToken cancellationToken = default)
		{
			XmlTextWriter xmlTextWriter;

			if ((object)document == null)
				throw new ArgumentNullException(nameof(document));

			if ((object)textWriter == null)
				throw new ArgumentNullException(nameof(textWriter));

			// DO NOT USE A USING BLOCK HERE (CALLER OWNS TEXTWRITER) !!!
			xmlTextWriter = new XmlTextWriter(textWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.Indentation = 1;
			xmlTextWriter.IndentChar = '\t';

			await this.SerializeAsync(document, xmlTextWriter, cancellationToken);
			await xmlTextWriter.FlushAsync();
		}

		#endregion
	}
}