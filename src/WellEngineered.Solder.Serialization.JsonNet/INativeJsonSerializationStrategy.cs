/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using Newtonsoft.Json;

namespace WellEngineered.Solder.Serialization.JsonNet
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects using JSON semantics.
	/// </summary>
	public interface INativeJsonSerializationStrategy : INativeSerializationStrategy<JsonReader, JsonWriter>
	{
	}
}