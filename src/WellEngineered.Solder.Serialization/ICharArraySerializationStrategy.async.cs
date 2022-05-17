/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Serialization
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects using char array semantics.
	/// </summary>
	public partial interface ICharArraySerializationStrategy
	{
		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified char array value.
		/// </summary>
		/// <param name="value"> The char array value to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		ValueTask<object> DeserializeObjectFromCharArrayAsync(char[] value, Type targetType, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deserializes an object from the specified char array value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="value"> The char array value to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		ValueTask<TObject> DeserializeObjectFromCharArrayAsync<TObject>(char[] value, CancellationToken cancellationToken = default);

		/// <summary>
		/// Serializes an object to a char array value.
		/// </summary>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A char array representation of the object graph. </returns>
		ValueTask<char[]> SerializeObjectToCharArrayAsync(Type targetType, object obj, CancellationToken cancellationToken = default);

		/// <summary>
		/// Serializes an object to a char array value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A char array representation of the object graph. </returns>
		ValueTask<char[]> SerializeObjectToCharArrayAsync<TObject>(TObject obj, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif