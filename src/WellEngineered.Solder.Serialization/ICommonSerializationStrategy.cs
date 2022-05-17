/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Serialization
{
	/// <summary>
	/// Provides a common strategy pattern around serializing and deserializing objects.
	/// </summary>
	public interface ICommonSerializationStrategy : IStreamSerializationStrategy, IFileSerializationStrategy,
		IBinarySerializationStrategy, ITextualSerializationStrategy
	{
	}
}