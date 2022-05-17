/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Serialization.Xyzl
{
	/// <summary>
	/// Marks an interface as an XML object which is mapped to/from an XML element.
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
	public sealed class XyzlKnownElementMappingAttribute : XyzlElementMappingAttribute
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the XmlKnownElementMappingAttribute class.
		/// </summary>
		public XyzlKnownElementMappingAttribute()
		{
		}

		#endregion
	}
}