/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Serialization.Xyzl
{
	/// <summary>
	/// Specifies the configuration child object type (applicable only to those which are well-known via properties with implicit or explicit mapping attributes).
	/// </summary>
	public enum XyzlChildType
	{
		/// <summary>
		/// The child configuration object is will be rendered as a value object using it's FQN. This is the default.
		/// Think: "text mode".
		/// </summary>
		Value = 0,

		/// <summary>
		/// The child configuration object will be rendered as a non-value object using it's FQN.
		/// Think: "property mode".
		/// </summary>
		Unqualified = 1,

		/// <summary>
		/// The child configuration object will be rendered as a non-value object using it's UPN dot prefixed with the UPN of it's parent configuration object.
		/// Think: "XAML mode".
		/// </summary>
		ParentQualified = 2
	}
}