/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Serialization.Xyzl
{
	/// <summary>
	/// Marks a property of an XML configuration object as being mapped to/from a well-known XML configuration element.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class XyzlChildElementMappingAttribute : Attribute
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the XmlChildElementMappingAttribute class.
		/// </summary>
		public XyzlChildElementMappingAttribute()
		{
		}

		#endregion

		#region Fields/Constants

		private XyzlChildType childElementType;
		private string localName;
		private string namespaceUri;
		private int order;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets or sets the child element type (applicable only to those child elements which are well-known via properties with mapping attributes).
		/// </summary>
		public XyzlChildType ChildElementType
		{
			get
			{
				return this.childElementType;
			}
			set
			{
				this.childElementType = value;
			}
		}

		/// <summary>
		/// Gets or sets the local name of the XML element.
		/// </summary>
		public string LocalName
		{
			get
			{
				return this.localName;
			}
			set
			{
				this.localName = value;
			}
		}

		/// <summary>
		/// Gets or sets the namespace URI of the XML element.
		/// </summary>
		public string NamespaceUri
		{
			get
			{
				return this.namespaceUri;
			}
			set
			{
				this.namespaceUri = value;
			}
		}

		/// <summary>
		/// Gets or sets the order of rendering of the element to the XML stream. Order is only applicable to XML output and is ignored during XML input.
		/// </summary>
		public int Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}

		#endregion
	}
}