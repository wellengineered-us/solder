/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Serialization
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects using textual semantics.
	/// </summary>
	public interface ITextualSerializationStrategy : ICharArraySerializationStrategy, IStringSerializationStrategy, ITextReaderWriterSerializationStrategy
	{
	}
}