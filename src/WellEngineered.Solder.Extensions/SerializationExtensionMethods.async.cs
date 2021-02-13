﻿/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using WellEngineered.Solder.Serialization;

namespace WellEngineered.Solder.Extensions
{
	public static partial class SerializationExtensionMethods
	{
		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from an assembly manifest resource.
		/// </summary>
		/// <typeparam name="TObject"> The run-time type of the object root to deserialize. </typeparam>
		/// <param name="serializationStrategy"> The serialization strategy. </param>
		/// <param name="resourceType"> A type within the source assembly where the manifest resource lives. </param>
		/// <param name="resourceName"> The fully qualified manifest resource name to load. </param>
		/// <returns> A valid object of the specified type or null if the manifest resource name was not found in the assembly of the resource type. </returns>
		public static async ValueTask<TObject> GetFromAssemblyResourceAsync<TObject>(this ICommonSerializationStrategy serializationStrategy, Type resourceType, string resourceName)
		{
			TObject result;

			if ((object)serializationStrategy == null)
				throw new ArgumentNullException(nameof(serializationStrategy));

			if ((object)resourceType == null)
				throw new ArgumentNullException(nameof(resourceType));

			if ((object)resourceName == null)
				throw new ArgumentNullException(nameof(resourceName));

			result = default(TObject);

			using (Stream stream = resourceType.GetTypeInfo().Assembly.GetManifestResourceStream(resourceName))
			{
				if ((object)stream != null)
					result = await serializationStrategy.DeserializeObjectFromStreamAsync<TObject>(stream);
			}

			return result;
		}

		/// <summary>
		/// Deserializes a string from an assembly manifest resource.
		/// </summary>
		/// <param name="resourceType"> A type within the source assembly where the manifest resource lives. </param>
		/// <param name="resourceName"> The fully qualified manifest resource name to load. </param>
		/// <returns> A valid string or null if the manifest resource name was not found in the assembly of the resource type. </returns>
		public static async ValueTask<string> GetStringFromAssemblyResourceAsync(this Type resourceType, string resourceName)
		{
			string result;

			if ((object)resourceType == null)
				throw new ArgumentNullException(nameof(resourceType));

			if ((object)resourceName == null)
				throw new ArgumentNullException(nameof(resourceName));

			result = null;

			using (Stream stream = resourceType.GetTypeInfo().Assembly.GetManifestResourceStream(resourceName))
			{
				if ((object)stream != null)
				{
					using (StreamReader streamReader = new StreamReader(stream))
						result = await streamReader.ReadToEndAsync();
				}
			}

			return result;
		}

		#endregion
	}
}