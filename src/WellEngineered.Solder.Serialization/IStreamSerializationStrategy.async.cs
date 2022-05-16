/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Serialization
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects to/from streams.
	/// </summary>
	public partial interface IStreamSerializationStrategy
	{
		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified readable stream.
		/// </summary>
		/// <param name="stream"> The readable stream to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		ValueTask<object> DeserializeObjectFromStreamAsync(Stream stream, Type targetType, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deserializes an object from the specified readable stream. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="stream"> The readable stream to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		ValueTask<TObject> DeserializeObjectFromStreamAsync<TObject>(Stream stream, CancellationToken cancellationToken = default);

		/// <summary>
		/// Serializes an object to the specified writable stream.
		/// </summary>
		/// <param name="stream"> The writable stream to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		ValueTask SerializeObjectToStreamAsync(Stream stream, Type targetType, object obj, CancellationToken cancellationToken = default);

		/// <summary>
		/// Serializes an object to the specified writable stream.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="stream"> The writable stream to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		ValueTask SerializeObjectToStreamAsync<TObject>(Stream stream, TObject obj, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif