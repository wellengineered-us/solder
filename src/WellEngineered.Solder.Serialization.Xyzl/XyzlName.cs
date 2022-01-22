/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Serialization.Xyzl
{
	/// <summary>
	/// Represents a local name and namespace URI of a configuration object.
	/// </summary>
	public sealed class XyzlName : IXyzlName
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the XyzlName class.
		/// </summary>
		public XyzlName()
		{
		}

		public XyzlName(string localName, string namespaceUri)
		{
			if ((object)localName == null)
				throw new ArgumentNullException(nameof(localName));

			if ((object)namespaceUri == null)
				throw new ArgumentNullException(nameof(namespaceUri));

			this.localName = localName;
			this.namespaceUri = namespaceUri;
		}

		#endregion

		#region Fields/Constants

		private string localName;
		private string namespaceUri;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets or sets the local name of the configuration object.
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
		/// Gets or sets the namespace URI of the configuration object.
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

		#region Methods/Operators

		/// <summary>
		/// Performs a custom equals test against two configured name objects using value semantics over the local name and namespace URI.
		/// </summary>
		/// <param name="a"> The first configured name to test. </param>
		/// <param name="b"> The second configured name object to test. </param>
		/// <returns> A value indicating whether the two configured name objects are equal using value semantics. </returns>
		private static bool TestEquals(XyzlName a, XyzlName b)
		{
			return (a.LocalName == b.LocalName) &&
					(a.NamespaceUri == b.NamespaceUri);
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" /> .
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" /> ; otherwise, false.
		/// </returns>
		/// <param name="obj">
		/// The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" /> .
		/// </param>
		/// <filterpriority> 2 </filterpriority>
		public override bool Equals(object obj)
		{
			XyzlName that;

			if ((object)obj == null)
				return false;

			that = obj as XyzlName;

			if ((object)that == null)
				return false;

			return TestEquals(this, that);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object" /> .
		/// </returns>
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" /> .
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" /> .
		/// </returns>
		public override string ToString()
		{
			return (this.NamespaceUri ?? string.Empty) + "#" + (this.LocalName ?? string.Empty);
		}

		/// <summary>
		/// Determines whether two specified configuration name objects are equal.
		/// </summary>
		/// <param name="a"> The first configured name to test. </param>
		/// <param name="b"> The second configured name object to test. </param>
		/// <returns> A value indicating whether the two configured name objects are equal using value semantics. </returns>
		public static bool operator ==(XyzlName a, XyzlName b)
		{
			return TestEquals(a, b);
		}

		/// <summary>
		/// Determines whether two specified configured name objects are not equal.
		/// </summary>
		/// <param name="a"> The first configured name to test. </param>
		/// <param name="b"> The second configured name object to test. </param>
		/// <returns> A value indicating whether the two configured name objects are not equal using value semantics. </returns>
		public static bool operator !=(XyzlName a, XyzlName b)
		{
			return !TestEquals(a, b);
		}

		#endregion
	}
}