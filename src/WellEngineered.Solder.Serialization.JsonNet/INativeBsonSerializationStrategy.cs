/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using Newtonsoft.Json.Bson;

namespace WellEngineered.Solder.Serialization.JsonNet
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects using BSON semantics.
	/// </summary>
	public interface INativeBsonSerializationStrategy : INativeSerializationStrategy<BsonReader, BsonWriter>
	{
	}
}