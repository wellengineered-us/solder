/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Serialization.Xyzl
{
	/// <summary>
	/// Marks a class as an XML configuration object which is mapped to/from an XML configuration element.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class XyzlElementMappingAttribute : Attribute
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the XmlElementMappingAttribute class.
		/// </summary>
		public XyzlElementMappingAttribute()
		{
		}

		#endregion

		#region Fields/Constants

		private XyzlChildMode childElementMode;
		private string localName;
		private string namespaceUri;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets or sets the child element model (applicable only to those child elements which are not well-known via properties with mapping attributes).
		/// </summary>
		public XyzlChildMode ChildElementMode
		{
			get
			{
				return this.childElementMode;
			}
			set
			{
				this.childElementMode = value;
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

		#endregion
	}
}