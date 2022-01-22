/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Serialization.Xyzl
{
	/// <summary>
	/// This is a custom XML configuration object serializer/deserializer that does not suffer from the rigidities of the .NET Framework supplied ones. This implementation was designed to be fast and flexible for XML driven tools.
	/// </summary>
	public sealed partial class XyzlSerializer : IXyzlSerializer
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the XyzlSerializer class.
		/// </summary>
		public XyzlSerializer()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly Dictionary<IXyzlName, Type> knownConfigurationObjectTypeRegistrations = new Dictionary<IXyzlName, Type>();
		private Type knownXmlTextObjectTypeRegistration;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the known XML configuration object type registrations.
		/// </summary>
		private Dictionary<IXyzlName, Type> KnownConfigurationObjectTypeRegistrations
		{
			get
			{
				return this.knownConfigurationObjectTypeRegistrations;
			}
		}

		/// <summary>
		/// Gets or sets the known XML text object type registration.
		/// </summary>
		private Type KnownXmlTextObjectTypeRegistration
		{
			get
			{
				return this.knownXmlTextObjectTypeRegistration;
			}
			set
			{
				this.knownXmlTextObjectTypeRegistration = value;
			}
		}

		#endregion

		#region Methods/Operators

		[Conditional("DEBUG")]
		private static void DebugWrite(string message)
		{
			Console.WriteLine(message);
		}

		/// <summary>
		/// Clears all known XML configuration object registrations.
		/// </summary>
		public void ClearAllKnownObjects()
		{
			this.KnownConfigurationObjectTypeRegistrations.Clear();
			this.KnownXmlTextObjectTypeRegistration = null;
		}

		/// <summary>
		/// Deserialize an XML configuration object graph from the specified XML file.
		/// </summary>
		/// <param name="fileName"> The XML file to load. </param>
		/// <returns> An XML configuration object graph. </returns>
		public ISolderConfiguration Deserialize(string fileName)
		{
			ISolderConfiguration document;

			if ((object)fileName == null)
				throw new ArgumentNullException(nameof(fileName));

			if (string.IsNullOrWhiteSpace(fileName))
				throw new ArgumentOutOfRangeException(nameof(fileName));

			using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				document = this.Deserialize(stream);
				return document;
			}
		}

		/// <summary>
		/// Deserialize an XML configuration object graph from the specified stream.
		/// </summary>
		/// <param name="stream"> The stream to load. </param>
		/// <returns> An XML configuration object graph. </returns>
		public ISolderConfiguration Deserialize(Stream stream)
		{
			XmlTextReader xmlTextReader;
			ISolderConfiguration document;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			// DO NOT USE A USING BLOCK HERE (CALLER OWNS STREAM) !!!
			xmlTextReader = new XmlTextReader(stream);
			document = this.Deserialize(xmlTextReader);
			return document;
		}

		/// <summary>
		/// Deserialize an XML configuration object graph from the specified XML reader.
		/// </summary>
		/// <param name="xmlReader"> The XML reader to load. </param>
		/// <returns> An XML configuration object graph. </returns>
		public ISolderConfiguration Deserialize(XmlReader xmlReader)
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
			while (xmlReader.Read())
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
					DebugWrite(string.Format("{2} <{0}{1}>", xmlReader.LocalName, xmlReader.IsEmptyElement ? " /" : string.Empty, xmlReader.IsEmptyElement ? "empty" : "begin"));

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
					DebugWrite(string.Format("end <{0}>", xmlReader.LocalName));

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

		/// <summary>
		/// Deserialize an XML configuration object graph from the specified text reader.
		/// </summary>
		/// <param name="textReader"> The text reader to load. </param>
		/// <returns> An XML configuration object graph. </returns>
		public ISolderConfiguration Deserialize(TextReader textReader)
		{
			XmlTextReader xmlTextReader;
			ISolderConfiguration document;

			if ((object)textReader == null)
				throw new ArgumentNullException(nameof(textReader));

			// DO NOT USE A USING BLOCK HERE (CALLER OWNS TEXTWRITER) !!!
			xmlTextReader = new XmlTextReader(textReader);
			document = this.Deserialize(xmlTextReader);
			return document;
		}

		/// <summary>
		/// Private method that processes XML configuration object deserialization.
		/// </summary>
		/// <param name="contextStack"> The context stack used to manage deserialization. </param>
		/// <param name="previousElementXyzlName"> The previously encountered XML name (parent). </param>
		/// <param name="currentElementXyzlName"> The current XML name (current). </param>
		/// <param name="attributes"> The current attributes for the current XML configuration object. </param>
		/// <param name="overrideCurrentXyzlText"> A special overriding XML text object. </param>
		/// <returns> The created current XML configuration object. </returns>
		private ISolderConfiguration DeserializeFromXml(IXmlLineInfo xmlLineInfo, Stack<ISolderConfiguration> contextStack, IXyzlName previousElementXyzlName, IXyzlName currentElementXyzlName, IDictionary<IXyzlName, string> attributes, IXyzlValue<string> overrideCurrentXyzlText)
		{
			ISolderConfiguration currentSolderConfiguration;
			Type currentType;
			PropertyInfo[] currentPropertyInfos;
			XyzlElementMappingAttribute currentXyzlElementMappingAttribute;

			ISolderConfiguration parentSolderConfiguration = null;
			Type parentType = null;
			PropertyInfo[] parentPropertyInfos;
			XyzlElementMappingAttribute parentXyzlElementMappingAttribute = null;

			Dictionary<PropertyInfo, XyzlAttributeMappingAttribute> parentPropertyToAttributeMappings;
			Dictionary<PropertyInfo, XyzlChildElementMappingAttribute> parentPropertyToChildElementMappings;

			Dictionary<PropertyInfo, XyzlAttributeMappingAttribute> currentPropertyToAttributeMappings;
			Dictionary<PropertyInfo, XyzlChildElementMappingAttribute> currentPropertyToChildElementMappings;

			KeyValuePair<PropertyInfo, XyzlChildElementMappingAttribute>? parentPropertyToChildElementMapping = null;

			int attributeCount;

			XyzlAttributeMappingAttribute xyzlAttributeMappingAttribute;
			XyzlChildElementMappingAttribute xyzlChildElementMappingAttribute;
			XyzlElementMappingAttribute parentOfChildXyzlElementMappingAttribute;

			Match match;
			string parentName;
			string propertyName;
			const string PROP_REGEX =
				@"( [a-zA-Z_][a-zA-Z_0-9\-]+ ) \. ( [a-zA-Z_][a-zA-Z_0-9\-]+ )";

			if ((object)contextStack == null)
				throw new ArgumentNullException(nameof(contextStack));

			if ((object)currentElementXyzlName == null)
				throw new ArgumentNullException(nameof(currentElementXyzlName));

			if ((object)attributes == null)
				throw new ArgumentNullException(nameof(attributes));

			if (string.IsNullOrWhiteSpace(currentElementXyzlName.LocalName))
				throw new ArgumentOutOfRangeException(nameof(currentElementXyzlName));

			if (contextStack.Count > 0) // is this NOT the root node?
			{
				// element on stack is parent
				parentSolderConfiguration = contextStack.Peek();

				// sanity check
				if ((object)parentSolderConfiguration == null)
					throw new XyzlException(string.Format("TODO: error message"));

				// interogate parent XML configuration object
				parentType = parentSolderConfiguration.GetType();
				parentPropertyInfos = parentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

				// sanity check
				if ((object)parentPropertyInfos == null)
					throw new XyzlException(string.Format("TODO: error message"));

				// get parent mapping metadata
				parentXyzlElementMappingAttribute = parentType.GetOneAttribute<XyzlElementMappingAttribute>();

				// sanity check
				if ((object)parentXyzlElementMappingAttribute == null)
					throw new XyzlException(string.Format("TODO: error message"));

				// create parent mapping tables for attributes and child elements
				parentPropertyToAttributeMappings = new Dictionary<PropertyInfo, XyzlAttributeMappingAttribute>();
				parentPropertyToChildElementMappings = new Dictionary<PropertyInfo, XyzlChildElementMappingAttribute>();

				foreach (PropertyInfo parentPropertyInfo in parentPropertyInfos)
				{
					// get potential attribute mapping metadata
					xyzlAttributeMappingAttribute = parentPropertyInfo.GetOneAttribute<XyzlAttributeMappingAttribute>();

					// get the potential child mapping metadata
					xyzlChildElementMappingAttribute = parentPropertyInfo.GetOneAttribute<XyzlChildElementMappingAttribute>();

					// count what we found; there can only be one
					attributeCount = 0;
					attributeCount += (object)xyzlAttributeMappingAttribute == null ? 0 : 1;
					attributeCount += (object)xyzlChildElementMappingAttribute == null ? 0 : 1;

					// sanity check
					if (attributeCount > 1)
						throw new XyzlException(string.Format("TODO: error message"));

					// append to the correct mapping table
					if ((object)xyzlAttributeMappingAttribute != null) // is this an attribute mapping?
						parentPropertyToAttributeMappings.Add(parentPropertyInfo, xyzlAttributeMappingAttribute);
					else if ((object)xyzlChildElementMappingAttribute != null) // or is this a child element mapping?
						parentPropertyToChildElementMappings.Add(parentPropertyInfo, xyzlChildElementMappingAttribute);
				}

				// is this a text element node override?
				if ((object)overrideCurrentXyzlText != null)
				{
					string svalue;
					object ovalue;

					// resolve the mapping to get text element property
					parentPropertyToChildElementMapping = parentPropertyToChildElementMappings
						.Where(x => x.Value.ChildElementType == XyzlChildType.Value)
						.Select(x => (KeyValuePair<PropertyInfo, XyzlChildElementMappingAttribute>?)x).Where(x => x.Value.Value.LocalName == overrideCurrentXyzlText.Name.LocalName &&
																												x.Value.Value.NamespaceUri == overrideCurrentXyzlText.Name.NamespaceUri).SingleOrDefault();

					// sanity check
					if ((object)parentPropertyToChildElementMapping == null)
						throw new XyzlException(string.Format("TODO: error message"));

					// sanity check
					if (!parentPropertyToChildElementMapping.Value.Key.CanWrite)
						throw new XyzlException(string.Format("TODO: error message"));

					// get the raw string value
					svalue = overrideCurrentXyzlText.Value;

					// convert to strongly-typed value
					if (!svalue.TryParse(parentPropertyToChildElementMapping.Value.Key.PropertyType, out ovalue))
						ovalue = parentPropertyToChildElementMapping.Value.Key.PropertyType.DefaultValue();

					// attempt to set the value
					if (!parentSolderConfiguration.SetLogicalPropertyValue(parentPropertyToChildElementMapping.Value.Key.Name, ovalue))
						throw new XyzlException(string.Format("TODO: error message"));

					// return null to prevent recursion
					return null;
				}

				// check if this element is a parent DOT property (<Parent.Property />) convention (similar to XAML)
				match = Regex.Match(currentElementXyzlName.LocalName, PROP_REGEX, RegexOptions.IgnorePatternWhitespace);

				if ((object)match != null && match.Success)
				{
					// yes, this is a parent DOT property naming
					parentName = match.Groups[1].Value;
					propertyName = match.Groups[2].Value;
				}
				else
				{
					// no, this is normal naming
					parentName = null;
					propertyName = currentElementXyzlName.LocalName;
				}

				// sanity check to ensure local names match if parent DOT property convention is used
				if ((object)previousElementXyzlName != null &&
					(object)parentName != null &&
					parentName != previousElementXyzlName.LocalName)
					throw new XyzlException(string.Format("TODO: error message"));

				// resolve the mapping to get child element property
				parentPropertyToChildElementMapping = parentPropertyToChildElementMappings
					.Where(x => x.Value.ChildElementType != XyzlChildType.ParentQualified)
					.Select(x => (KeyValuePair<PropertyInfo, XyzlChildElementMappingAttribute>?)x)
					.Where(x => (x.Value.Value.NamespaceUri == currentElementXyzlName.NamespaceUri &&
								x.Value.Value.LocalName == propertyName)).SingleOrDefault();

				if ((object)parentPropertyToChildElementMapping != null)
				{
					// sanity check
					if (!parentPropertyToChildElementMapping.Value.Key.CanWrite)
						throw new XyzlException(string.Format("TODO: error message"));

					// get parent-of-child mapping metadata
					parentOfChildXyzlElementMappingAttribute = parentPropertyToChildElementMapping.Value.Key.PropertyType.GetOneAttribute<XyzlElementMappingAttribute>();

					// sanity check
					if ((object)parentOfChildXyzlElementMappingAttribute == null)
						throw new XyzlException(string.Format("TODO: error message"));

					// override based on parent-of-child mapping metadata
					currentElementXyzlName.LocalName = parentOfChildXyzlElementMappingAttribute.LocalName;
					currentElementXyzlName.NamespaceUri = parentOfChildXyzlElementMappingAttribute.NamespaceUri;
				}
			}

			// factory-up the new XML configuration object based on registered types
			currentSolderConfiguration = this.ResolveConfigurationObject(currentElementXyzlName);

			// sanity check
			if ((object)currentSolderConfiguration == null)
				throw new XyzlException(string.Format("currentElementXyzlName: '{0}'", currentElementXyzlName));
			;

			// interogate the current type
			currentType = currentSolderConfiguration.GetType();
			currentPropertyInfos = currentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			// sanity check
			if ((object)currentPropertyInfos == null)
				throw new XyzlException(string.Format("TODO: error message"));

			// get current mapping metadata
			currentXyzlElementMappingAttribute = currentType.GetOneAttribute<XyzlElementMappingAttribute>();

			// sanity check
			if ((object)currentXyzlElementMappingAttribute == null)
				throw new XyzlException(string.Format("TODO: error message"));

			// create current mapping tables for attributes and child elements
			currentPropertyToAttributeMappings = new Dictionary<PropertyInfo, XyzlAttributeMappingAttribute>();
			currentPropertyToChildElementMappings = new Dictionary<PropertyInfo, XyzlChildElementMappingAttribute>();

			foreach (PropertyInfo currentPropertyInfo in currentPropertyInfos)
			{
				// get potential attribute mapping metadata
				xyzlAttributeMappingAttribute = currentPropertyInfo.GetOneAttribute<XyzlAttributeMappingAttribute>();

				// get the potential child mapping metadata
				xyzlChildElementMappingAttribute = currentPropertyInfo.GetOneAttribute<XyzlChildElementMappingAttribute>();

				// count what we found; there can only be one
				attributeCount = 0;
				attributeCount += (object)xyzlAttributeMappingAttribute == null ? 0 : 1;
				attributeCount += (object)xyzlChildElementMappingAttribute == null ? 0 : 1;

				// sanity check
				if (attributeCount > 1)
					throw new XyzlException(string.Format("TODO: error message"));

				// append to the correct mapping table
				if ((object)xyzlAttributeMappingAttribute != null) // is this an attribute mapping?
					currentPropertyToAttributeMappings.Add(currentPropertyInfo, xyzlAttributeMappingAttribute);
				else if ((object)xyzlChildElementMappingAttribute != null) // or is this a child element mapping?
					currentPropertyToChildElementMappings.Add(currentPropertyInfo, xyzlChildElementMappingAttribute);
			}

			// iterate over attributes of current element
			if ((object)currentPropertyToAttributeMappings != null)
			{
				foreach (KeyValuePair<PropertyInfo, XyzlAttributeMappingAttribute> currentPropertyToAttributeMapping in currentPropertyToAttributeMappings.OrderBy(m => m.Value.Order).ThenBy(m => m.Value.LocalName))
				{
					string svalue;
					object ovalue;
					var _currentPropertyToAttributeMapping = currentPropertyToAttributeMapping; // prevent closure bug

					// sanity check
					if (!currentPropertyToAttributeMapping.Key.CanWrite)
						throw new XyzlException(string.Format("TODO: error message"));

					// get the raw string value
					svalue = attributes.Where(a => a.Key.LocalName == _currentPropertyToAttributeMapping.Value.LocalName &&
													a.Key.NamespaceUri == _currentPropertyToAttributeMapping.Value.NamespaceUri)
						.Select(a => a.Value).SingleOrDefault();

					// convert to strongly-typed value
					if (!svalue.TryParse(currentPropertyToAttributeMapping.Key.PropertyType, out ovalue))
						ovalue = currentPropertyToAttributeMapping.Key.PropertyType.DefaultValue();

					// attempt to set the values
					if (!currentSolderConfiguration.SetLogicalPropertyValue(currentPropertyToAttributeMapping.Key.Name, ovalue))
						throw new XyzlException(string.Format("TODO: error message"));
				}
			}

			// determine where to store current XML configuration object
			if ((object)parentPropertyToChildElementMapping != null)
			{
				// store this as a child element of parent XML configuration object
				if (!parentSolderConfiguration.SetLogicalPropertyValue(parentPropertyToChildElementMapping.Value.Key.Name, currentSolderConfiguration))
					throw new XyzlException(string.Format("TODO: error message"));
			}
			else if ((object)parentXyzlElementMappingAttribute != null)
			{
				// store this as an element of parent XML configuration object

				// sanity check
				if ((object)parentSolderConfiguration == null)
					throw new XyzlException(string.Format("TODO: error message"));

				if (parentXyzlElementMappingAttribute.ChildElementMode == XyzlChildMode.Content)
				{
					// only one content element is allowed, check to see if it is non-null instance
					if ((object)parentSolderConfiguration.Content != null)
						throw new XyzlException(string.Format("TODO: error message"));

					// assign to anonymous content property
					parentSolderConfiguration.Content = currentSolderConfiguration;
				}
				else if (parentXyzlElementMappingAttribute.ChildElementMode == XyzlChildMode.Items)
				{
					// any number of elements are allowed

					// sanity check
					if ((object)parentSolderConfiguration.AllowedChildTypes == null)
						throw new XyzlException(string.Format("TODO: error message"));

					// sanity check
					if ((object)parentSolderConfiguration.Items == null)
						throw new XyzlException(string.Format("TODO: error message"));

					// new collection type check
					if (parentSolderConfiguration.AllowedChildTypes.Count(t => t.IsAssignableFrom(currentType)) <= 0)
						throw new XyzlException(string.Format("TODO: error message"));

					// add to anonymous collection
					parentSolderConfiguration.Items.Add(currentSolderConfiguration);
				}
			}

			return currentSolderConfiguration; // return current XML configuration object
		}

		/// <summary>
		/// Private method that processes XML text object deserialization.
		/// </summary>
		/// <param name="contextStack"> The context stack used to manage deserialization. </param>
		/// <param name="textValue"> The string value of the text element. </param>
		/// <param name="xyzlName"> The in-effect XML name. </param>
		/// <returns> The created current XML text object. </returns>
		private IXyzlValue<string> DeserializeFromXmlText(IXmlLineInfo xmlLineInfo, Stack<ISolderConfiguration> contextStack, string textValue, IXyzlName xyzlName)
		{
			IXyzlValue<string> currentXyzlText;
			ISolderConfiguration parentSolderConfiguration;
			Type parentType;
			XyzlElementMappingAttribute parentXyzlElementMappingAttribute;

			if ((object)contextStack == null)
				throw new ArgumentNullException(nameof(contextStack));

			if ((object)textValue == null)
				throw new ArgumentNullException(nameof(textValue));

			// sanity check
			if (contextStack.Count <= 0)
				throw new XyzlException(string.Format("TODO: error message"));

			// resolve the XML text object
			currentXyzlText = this.ResolveXmlTextObject(textValue);

			// sanity check
			if ((object)currentXyzlText == null)
				throw new XyzlException(string.Format("TODO: error message"));

			// grab the parent XML configuration object
			parentSolderConfiguration = contextStack.Peek();

			// sanity check
			if ((object)parentSolderConfiguration == null)
				throw new XyzlException(string.Format("TODO: error message"));

			// interogate the parent
			parentType = parentSolderConfiguration.GetType();

			// get the parent emlement mapping metadata
			parentXyzlElementMappingAttribute = parentType.GetOneAttribute<XyzlElementMappingAttribute>();

			// sanity check
			if ((object)parentXyzlElementMappingAttribute == null)
				throw new XyzlException(string.Format("TODO: error message"));

			// is this a named text element?
			if ((object)xyzlName != null)
			{
				// named, thus assign name and do not add to anonymous child element collection
				currentXyzlText.Name = xyzlName;
			}
			else
			{
				// it is OK to have text element and not allow anonymous children
				if (parentXyzlElementMappingAttribute.ChildElementMode != XyzlChildMode.Items)
					throw new XyzlException(string.Format("TODO: error message"));

				// sanity check
				if ((object)parentSolderConfiguration.Items == null)
					throw new XyzlException(string.Format("TODO: error message"));

				// anonymous, thus add to anonymous child element collection
				// daniel.bullington@wellengineered.us / 2012-10-29 (Issue #32): no longer need to explicitly assign parent
				// currentXyzlText.Parent = parentConfigurationObject;
				parentSolderConfiguration.Items.Add(currentXyzlText);
			}

			return currentXyzlText;
		}

		private IXyzlName GetConfiguredName(ISolderConfiguration solderConfiguration)
		{
			Type xmlObjectType;
			IXyzlName xyzlName;
			XyzlElementMappingAttribute xyzlElementMappingAttribute;

			if ((object)solderConfiguration == null)
				throw new ArgumentNullException(nameof(solderConfiguration));

			xmlObjectType = solderConfiguration.GetType();
			xyzlElementMappingAttribute = xmlObjectType.GetOneAttribute<XyzlElementMappingAttribute>();

			if ((object)xyzlElementMappingAttribute == null)
				xyzlName = null;
			else
			{
				xyzlName = new XyzlName()
							{
								LocalName = xyzlElementMappingAttribute.LocalName,
								NamespaceUri = xyzlElementMappingAttribute.NamespaceUri
							};
			}

			return xyzlName;
		}

		/// <summary>
		/// Determines whether an element is a text element.
		/// </summary>
		/// configuration object
		/// <param name="contextStack"> The effective context stack. </param>
		/// <param name="xyzlName"> The XML name to use. </param>
		/// <returns> A value indicating whether the element is a text element. </returns>
		private bool IsTextElement(Stack<ISolderConfiguration> contextStack, IXyzlName xyzlName)
		{
			ISolderConfiguration parentSolderConfiguration;
			Type parentType;
			PropertyInfo[] parentPropertyInfos;

			XyzlAttributeMappingAttribute xyzlAttributeMappingAttribute;
			XyzlChildElementMappingAttribute xyzlChildElementMappingAttribute;

			int attributeCount;

			if ((object)contextStack == null)
				throw new ArgumentNullException(nameof(contextStack));

			if ((object)xyzlName == null)
				throw new ArgumentNullException(nameof(xyzlName));

			// sanity check
			if (contextStack.Count < 1)
				return false;

			// interogate the parent (last pushed value)
			parentSolderConfiguration = contextStack.Peek();
			parentType = parentSolderConfiguration.GetType();
			parentPropertyInfos = parentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			// examine parent mapping tables for attributes and child elements
			if ((object)parentPropertyInfos != null)
			{
				foreach (PropertyInfo parentPropertyInfo in parentPropertyInfos)
				{
					// get potential attribute mapping metadata
					xyzlAttributeMappingAttribute = parentPropertyInfo.GetOneAttribute<XyzlAttributeMappingAttribute>();

					// get the potential child mapping metadata
					xyzlChildElementMappingAttribute = parentPropertyInfo.GetOneAttribute<XyzlChildElementMappingAttribute>();

					// count what we found; there can only be one
					attributeCount = 0;
					attributeCount += (object)xyzlAttributeMappingAttribute == null ? 0 : 1;
					attributeCount += (object)xyzlChildElementMappingAttribute == null ? 0 : 1;

					// sanity check
					if (attributeCount > 1)
						throw new XyzlException(string.Format("TODO: error message"));

					// we only care about child elements
					if ((object)xyzlChildElementMappingAttribute == null)
						continue;

					// is this mapped as a text element?
					if (xyzlChildElementMappingAttribute.ChildElementType == XyzlChildType.Value &&
						xyzlChildElementMappingAttribute.LocalName == xyzlName.LocalName &&
						xyzlChildElementMappingAttribute.NamespaceUri == xyzlName.NamespaceUri)
						return true;
				}
			}

			// nope, we exhausted our search
			return false;
		}

		/// <summary>
		/// Registers a known XML configuration object by target type and explicit XML name (local name and namespace URI). This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target type to register. </typeparam>
		/// <param name="xyzlName"> The XML name (local name and namespace URI). </param>
		public void RegisterKnownObject<TObject>(IXyzlName xyzlName)
			where TObject : ISolderConfiguration
		{
			Type targetType;

			if ((object)xyzlName == null)
				throw new ArgumentNullException(nameof(xyzlName));

			targetType = typeof(TObject);

			this.RegisterKnownObject(xyzlName, targetType);
		}

		/// <summary>
		/// Registers a known XML configuration object by target type and explicit XML name (local name and namespace URI). This is the non-generic overload.
		/// </summary>
		/// <param name="xyzlName"> The XML name (local name and namespace URI). </param>
		/// <param name="targetType"> The target type to register. </param>
		public void RegisterKnownObject(IXyzlName xyzlName, Type targetType)
		{
			if ((object)xyzlName == null)
				throw new ArgumentNullException(nameof(xyzlName));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if (this.KnownConfigurationObjectTypeRegistrations.ContainsKey(xyzlName))
				throw new XyzlException(string.Format("typeFullName: '{0}'", targetType.FullName));

			if (!typeof(ISolderConfiguration).IsAssignableFrom(targetType))
				throw new XyzlException(string.Format("typeFullName: '{0}'", targetType.FullName));

			this.KnownConfigurationObjectTypeRegistrations.Add(xyzlName, targetType);
		}

		/// <summary>
		/// Registers a known XML configuration object by target type and implicit attribute-based XML name (local name and namespace URI). This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target type to register. </typeparam>
		public void RegisterKnownObject<TObject>()
			where TObject : ISolderConfiguration
		{
			Type targetType;

			targetType = typeof(TObject);

			this.RegisterKnownObject(targetType);
		}

		/// <summary>
		/// Registers a known XML configuration object by target type. This is the non-generic overload.
		/// </summary>
		/// <param name="targetType"> The target type to register. </param>
		public void RegisterKnownObject(Type targetType)
		{
			IXyzlName xyzlName;
			XyzlElementMappingAttribute xyzlElementMappingAttribute;

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			xyzlElementMappingAttribute = targetType.GetOneAttribute<XyzlElementMappingAttribute>();

			if ((object)xyzlElementMappingAttribute == null)
				throw new XyzlException(string.Format("TODO: error message"));

			xyzlName = new XyzlName()
						{
							LocalName = xyzlElementMappingAttribute.LocalName,
							NamespaceUri = xyzlElementMappingAttribute.NamespaceUri
						};

			this.RegisterKnownObject(xyzlName, targetType);
		}

		/// <summary>
		/// Registers a known XML text object by target type. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target type to register. </typeparam>
		public void RegisterKnownValueObject<TObject>()
			where TObject : IXyzlValue<string>
		{
			Type targetType;

			targetType = typeof(TObject);

			this.RegisterKnownValueObject(targetType);
		}

		/// <summary>
		/// Registers a known XML text object by target type and implicit attribute-based XML name (local name and namespace URI). This is the non-generic overload.
		/// </summary>
		/// <param name="targetType"> The target type to register. </param>
		public void RegisterKnownValueObject(Type targetType)
		{
			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)this.KnownXmlTextObjectTypeRegistration != null)
				throw new XyzlException(string.Format("typeFullName: '{0}'", targetType.FullName));
			;

			if (!typeof(IXyzlValue<string>).IsAssignableFrom(targetType))
				throw new XyzlException(string.Format("typeFullName: '{0}'", targetType.FullName));
			;

			this.KnownXmlTextObjectTypeRegistration = targetType;
		}

		/// <summary>
		/// Private method to resolve an XML configuration object by XML name.
		/// </summary>
		/// <param name="xyzlName"> The XML name to lookup in the known registrations. </param>
		/// <returns> An IConfigurationObject instance or null if the XML name is not known. </returns>
		private ISolderConfiguration ResolveConfigurationObject(IXyzlName xyzlName)
		{
			object value;
			ISolderConfiguration solderConfiguration;
			Type targetType;

			if ((object)xyzlName == null)
				throw new ArgumentNullException(nameof(xyzlName));

			if (!this.KnownConfigurationObjectTypeRegistrations.TryGetValue(xyzlName, out targetType))
				return null;

			if ((object)targetType == null)
				throw new XyzlException(string.Format("TODO: error message"));

			value = Activator.CreateInstance(targetType);

			solderConfiguration = value as ISolderConfiguration;

			if ((object)solderConfiguration == null)
				throw new XyzlException(string.Format("TODO: error message"));

			return solderConfiguration;
		}

		/// <summary>
		/// Private method to resolve an XML text object.
		/// </summary>
		/// <param name="text"> The string value of the XML text object. </param>
		/// <returns> An IXmlTextObject instance or null if it is not known. </returns>
		private IXyzlValue<string> ResolveXmlTextObject(string text)
		{
			object value;
			IXyzlValue<string> xyzlText;
			Type targetType;

			if ((object)this.KnownXmlTextObjectTypeRegistration == null)
				return null;

			targetType = this.KnownXmlTextObjectTypeRegistration;

			if ((object)targetType == null)
				throw new XyzlException(string.Format("TODO: error message"));

			value = Activator.CreateInstance(targetType);

			xyzlText = value as IXyzlValue<string>;

			if ((object)xyzlText == null)
				throw new XyzlException(string.Format("TODO: error message"));

			xyzlText.Value = text;

			return xyzlText;
		}

		/// <summary>
		/// Serializes an XML configuration object graph to the specified XML file.
		/// </summary>
		/// <param name="document"> The document root XML configuration object. </param>
		/// <param name="fileName"> The XML file to save. </param>
		public void Serialize(ISolderConfiguration document, string fileName)
		{
			if ((object)document == null)
				throw new ArgumentNullException(nameof(document));

			if ((object)fileName == null)
				throw new ArgumentNullException(nameof(fileName));

			if (string.IsNullOrWhiteSpace(fileName))
				throw new ArgumentOutOfRangeException(nameof(fileName));

			using (Stream stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
				this.Serialize(document, stream);
		}

		/// <summary>
		/// Serializes an XML configuration object graph to the specified stream.
		/// </summary>
		/// <param name="document"> The document root XML configuration object. </param>
		/// <param name="stream"> The stream to save. </param>
		public void Serialize(ISolderConfiguration document, Stream stream)
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

			this.Serialize(document, xmlTextWriter);
			xmlTextWriter.Flush();
		}

		/// <summary>
		/// Serializes an XML configuration object graph to the specified XML writer.
		/// </summary>
		/// <param name="document"> The document root XML configuration object. </param>
		/// <param name="xmlWriter"> The XML writer to save. </param>
		public void Serialize(ISolderConfiguration document, XmlWriter xmlWriter)
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

			this.SerializeToXml(xmlWriter, document, null);
		}

		public void Serialize(ISolderConfiguration document, TextWriter textWriter)
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

			this.Serialize(document, xmlTextWriter);
			xmlTextWriter.Flush();
		}

		private void SerializeToXml(XmlWriter xmlWriter, ISolderConfiguration currentSolderConfiguration, IXyzlName overrideXyzlName)
		{
			IXyzlValue<string> currentXyzlText;
			Type currentType;
			PropertyInfo[] currentPropertyInfos;
			XyzlElementMappingAttribute currentXyzlElementMappingAttribute;

			Dictionary<PropertyInfo, XyzlAttributeMappingAttribute> currentPropertyToAttributeMappings;
			Dictionary<PropertyInfo, XyzlChildElementMappingAttribute> currentPropertyToChildElementMappings;

			XyzlAttributeMappingAttribute xyzlAttributeMappingAttribute;
			XyzlChildElementMappingAttribute xyzlChildElementMappingAttribute;

			int attributeCount;
			object ovalue;
			string svalue;

			if ((object)xmlWriter == null)
				throw new ArgumentNullException(nameof(xmlWriter));

			if ((object)currentSolderConfiguration == null)
				throw new ArgumentNullException(nameof(currentSolderConfiguration));

			// is this a text element?
			currentXyzlText = currentSolderConfiguration as IXyzlValue<string>;

			if ((object)currentXyzlText != null)
			{
				// write as CDATA if name is invalid (expected)
				if ((object)currentXyzlText.Name == null ||
					string.IsNullOrWhiteSpace(currentXyzlText.Name.LocalName))
				{
					xmlWriter.WriteCData(currentXyzlText.Value);
					return;
				}
			}

			// interogate the current XML configuration object
			currentType = currentSolderConfiguration.GetType();
			currentPropertyInfos = currentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			// sanity check
			if ((object)currentPropertyInfos == null)
				throw new XyzlException(string.Format("TODO: error message"));

			// get the current mapping metadata
			currentXyzlElementMappingAttribute = currentType.GetOneAttribute<XyzlElementMappingAttribute>();

			// sanity check
			if ((object)currentXyzlElementMappingAttribute == null)
				throw new XyzlException(string.Format("TODO: error message"));

			// create current mapping tables for attributes and child elements
			currentPropertyToAttributeMappings = new Dictionary<PropertyInfo, XyzlAttributeMappingAttribute>();
			currentPropertyToChildElementMappings = new Dictionary<PropertyInfo, XyzlChildElementMappingAttribute>();

			foreach (PropertyInfo currentPropertyInfo in currentPropertyInfos)
			{
				// get potential attribute mapping metadata
				xyzlAttributeMappingAttribute = currentPropertyInfo.GetOneAttribute<XyzlAttributeMappingAttribute>();

				// get potential child element mapping metadata
				xyzlChildElementMappingAttribute = currentPropertyInfo.GetOneAttribute<XyzlChildElementMappingAttribute>();

				// count what we found; there can only be one
				attributeCount = 0;
				attributeCount += (object)xyzlAttributeMappingAttribute == null ? 0 : 1;
				attributeCount += (object)xyzlChildElementMappingAttribute == null ? 0 : 1;

				// sanity check
				if (attributeCount > 1)
					throw new XyzlException(string.Format("TODO: error message"));

				// append to the correct mapping table
				if ((object)xyzlAttributeMappingAttribute != null) // is this an attribute mapping?
					currentPropertyToAttributeMappings.Add(currentPropertyInfo, xyzlAttributeMappingAttribute);
				else if ((object)xyzlChildElementMappingAttribute != null) // or is this a child element mapping?
					currentPropertyToChildElementMappings.Add(currentPropertyInfo, xyzlChildElementMappingAttribute);
			}

			// begin current element
			if ((object)overrideXyzlName != null &&
				!string.IsNullOrWhiteSpace(overrideXyzlName.LocalName)) // overriden element is special case for parent DOT property convention
			{
				// write the start of the element
				xmlWriter.WriteStartElement(overrideXyzlName.LocalName, overrideXyzlName.NamespaceUri);
			}
			else
			{
				// sanity check
				if (string.IsNullOrWhiteSpace(currentXyzlElementMappingAttribute.LocalName))
					throw new XyzlException(string.Format("TODO: error message"));

				// write the start of the element
				xmlWriter.WriteStartElement(currentXyzlElementMappingAttribute.LocalName, currentXyzlElementMappingAttribute.NamespaceUri);
			}

			// iterate over attributes of current element
			if ((object)currentPropertyToAttributeMappings != null)
			{
				foreach (KeyValuePair<PropertyInfo, XyzlAttributeMappingAttribute> currentPropertyToAttributeMapping in currentPropertyToAttributeMappings.OrderBy(m => m.Value.Order).ThenBy(m => m.Value.LocalName))
				{
					// sanity check
					if (!currentPropertyToAttributeMapping.Key.CanRead)
						throw new XyzlException(string.Format("TODO: error message"));

					// get the strongly-typed value
					if (!currentSolderConfiguration.GetLogicalPropertyValue(currentPropertyToAttributeMapping.Key.Name, out ovalue))
						throw new XyzlException(string.Format("TODO: error message"));

					// convert to loosely-typed formatted string
					svalue = ovalue?.ToString() ?? string.Empty;

					// write the attribute and value
					xmlWriter.WriteStartAttribute(currentPropertyToAttributeMapping.Value.LocalName, currentPropertyToAttributeMapping.Value.NamespaceUri);
					xmlWriter.WriteString(svalue);
					xmlWriter.WriteEndAttribute();
				}
			}

			if ((object)currentPropertyToChildElementMappings != null)
			{
				// write text elements
				foreach (KeyValuePair<PropertyInfo, XyzlChildElementMappingAttribute> currentPropertyToChildElementMapping in currentPropertyToChildElementMappings.Where(m => m.Value.ChildElementType == XyzlChildType.Value).OrderBy(m => m.Value.Order).ThenBy(m => m.Value.LocalName))
				{
					if (!currentPropertyToChildElementMapping.Key.CanRead)
						throw new XyzlException(string.Format("TODO: error message"));

					if (string.IsNullOrWhiteSpace(currentPropertyToChildElementMapping.Value.LocalName))
						throw new XyzlException(string.Format("TODO: error message"));

					if (!currentSolderConfiguration.GetLogicalPropertyValue(currentPropertyToChildElementMapping.Key.Name, out ovalue))
						throw new XyzlException(string.Format("TODO: error message"));

					svalue = ovalue?.ToString() ?? string.Empty;

					xmlWriter.WriteElementString(string.Empty, currentPropertyToChildElementMapping.Value.LocalName, currentPropertyToChildElementMapping.Value.NamespaceUri, svalue);
				}

				// write child elements
				foreach (KeyValuePair<PropertyInfo, XyzlChildElementMappingAttribute> currentPropertyToChildElementMapping in currentPropertyToChildElementMappings.Where(m => m.Value.ChildElementType != XyzlChildType.Value).OrderBy(m => m.Value.Order).ThenBy(m => m.Value.LocalName))
				{
					ISolderConfiguration childElement;
					IXyzlName xyzlName;

					// sanity check
					if (!currentPropertyToChildElementMapping.Key.CanRead)
						throw new XyzlException(string.Format("TODO: error message"));

					// sanity check
					if (!typeof(ISolderConfiguration).IsAssignableFrom(currentPropertyToChildElementMapping.Key.PropertyType))
						throw new XyzlException(string.Format("typeFullName: '{0}'", currentPropertyToChildElementMapping.Key.PropertyType.FullName));

					// get the XML configuration object property value
					object _out;
					if (!currentSolderConfiguration.GetLogicalPropertyValue(currentPropertyToChildElementMapping.Key.Name, out _out))
						throw new XyzlException(string.Format("TODO: error message"));
					childElement = (ISolderConfiguration)_out;

					// write the child element if not null
					if ((object)childElement != null)
					{
						// determine the name convention
						if (currentPropertyToChildElementMapping.Value.ChildElementType == XyzlChildType.ParentQualified)
						{
							// parent DOT property convention
							xyzlName = new XyzlName()
										{
											LocalName = string.Format("{0}.{1}",
												currentXyzlElementMappingAttribute.LocalName,
												currentPropertyToChildElementMapping.Value.LocalName),
											NamespaceUri = currentXyzlElementMappingAttribute.NamespaceUri
										};
						}
						else if (currentPropertyToChildElementMapping.Value.ChildElementType == XyzlChildType.Unqualified)
						{
							// normal convention
							xyzlName = new XyzlName()
										{
											LocalName = string.Format("{0}",
												currentPropertyToChildElementMapping.Value.LocalName),
											NamespaceUri = currentXyzlElementMappingAttribute.NamespaceUri
										};
						}
						else
							throw new XyzlException(string.Format("TODO: error message"));

						// recursively write the child element and so forth
						this.SerializeToXml(xmlWriter, childElement, xyzlName);
					}
				}
			}

			// write anonymous child elements (depending on element model)
			if (currentXyzlElementMappingAttribute.ChildElementMode == XyzlChildMode.Items &&
				(object)currentSolderConfiguration.Items != null)
			{
				// anonymous 0..n child elements
				foreach (ISolderConfiguration childElement in currentSolderConfiguration.Items)
					this.SerializeToXml(xmlWriter, childElement, null);
			}
			else if (currentXyzlElementMappingAttribute.ChildElementMode == XyzlChildMode.Content &&
					(object)currentSolderConfiguration.Content != null)
			{
				// anonymous 0..1 child element
				this.SerializeToXml(xmlWriter, currentSolderConfiguration.Content, null);
			}

			// write the end of the (current) element
			xmlWriter.WriteEndElement();
		}

		/// <summary>
		/// Unregisters a known XML configuration object by target type. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target type to unregister. </typeparam>
		/// <returns> A value indicating if the registration was present. </returns>
		public bool UnregisterKnownObject<TObject>()
			where TObject : ISolderConfiguration
		{
			Type targetType;

			targetType = typeof(TObject);

			return this.UnregisterKnownObject(targetType);
		}

		/// <summary>
		/// Unregisters a known XML configuration object by target type. This is the generic overload.
		/// </summary>
		/// <param name="targetType"> The target type to unregister. </param>
		/// <returns> A value indicating if the registration was present. </returns>
		public bool UnregisterKnownObject(Type targetType)
		{
			bool retval;
			IXyzlName xyzlName;
			XyzlElementMappingAttribute xyzlElementMappingAttribute;

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			xyzlElementMappingAttribute = targetType.GetOneAttribute<XyzlElementMappingAttribute>();

			if ((object)xyzlElementMappingAttribute == null)
				throw new XyzlException(string.Format("TODO: error message"));

			xyzlName = new XyzlName()
						{
							LocalName = xyzlElementMappingAttribute.LocalName,
							NamespaceUri = xyzlElementMappingAttribute.NamespaceUri
						};

			if (retval = this.KnownConfigurationObjectTypeRegistrations.ContainsKey(xyzlName))
				this.KnownConfigurationObjectTypeRegistrations.Remove(xyzlName);

			return retval;
		}

		/// <summary>
		/// Unregisters a known XML text object by target type. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target type to unregister. </typeparam>
		/// <returns> A value indicating if the registration was present. </returns>
		public bool UnregisterKnownValueObject<TObject>()
			where TObject : IXyzlValue<string>
		{
			Type targetType;

			targetType = typeof(TObject);

			return this.UnregisterKnownValueObject(targetType);
		}

		/// <summary>
		/// Unregisters a known XML text object by target type. This is the generic overload.
		/// </summary>
		/// <param name="targetType"> The target type to unregister. </param>
		/// <returns> A value indicating if the registration was present. </returns>
		public bool UnregisterKnownValueObject(Type targetType)
		{
			bool retval;

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if (retval = (object)this.KnownXmlTextObjectTypeRegistration != null)
				this.KnownXmlTextObjectTypeRegistration = null;

			return retval;
		}

		#endregion
	}
}