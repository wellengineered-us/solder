/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Serialization
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects using char array semantics.
	/// </summary>
	public partial interface ICharArraySerializationStrategy : ISerializationStrategy
	{
		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified char array value.
		/// </summary>
		/// <param name="value"> The char array value to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		object DeserializeObjectFromCharArray(char[] value, Type targetType);

		/// <summary>
		/// Deserializes an object from the specified char array value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="value"> The char array value to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		TObject DeserializeObjectFromCharArray<TObject>(char[] value);

		/// <summary>
		/// Serializes an object to a char array value.
		/// </summary>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A char array representation of the object graph. </returns>
		char[] SerializeObjectToCharArray(Type targetType, object obj);

		/// <summary>
		/// Serializes an object to a char array value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A char array representation of the object graph. </returns>
		char[] SerializeObjectToCharArray<TObject>(TObject obj);

		#endregion
	}
}