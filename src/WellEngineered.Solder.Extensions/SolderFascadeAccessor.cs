/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Utilities;

namespace WellEngineered.Solder.Extensions
{
	public static class SolderFascadeAccessor
	{
		#region Fields/Constants

		private static readonly Lazy<IAdoNetBufferingFascade> adoNetStreamingFascadeFactory = new Lazy<IAdoNetBufferingFascade>(() => new AdoNetBufferingFascade(DataTypeFascade));
		private static readonly Lazy<IDataTypeFascade> dataTypeFascadeFactory = new Lazy<IDataTypeFascade>(() => new DataTypeFascade());
		private static readonly Lazy<IReflectionFascade> reflectionFascadeFactory = new Lazy<IReflectionFascade>(() => new ReflectionFascade(DataTypeFascade));

		#endregion

		#region Properties/Indexers/Events

		[Obsolete("Proper dependency injection should be used instead.")]
		public static IAdoNetBufferingFascade AdoNetBufferingFascade
		{
			get
			{
				return AdoNetBufferingFascadeFactory.Value;
			}
		}

		private static Lazy<IAdoNetBufferingFascade> AdoNetBufferingFascadeFactory
		{
			get
			{
				return adoNetStreamingFascadeFactory;
			}
		}

		[Obsolete("Proper dependency injection should be used instead.")]
		public static IDataTypeFascade DataTypeFascade
		{
			get
			{
				return DataTypeFascadeFactory.Value;
			}
		}

		private static Lazy<IDataTypeFascade> DataTypeFascadeFactory
		{
			get
			{
				return dataTypeFascadeFactory;
			}
		}

		[Obsolete("Proper dependency injection should be used instead.")]
		public static IReflectionFascade ReflectionFascade
		{
			get
			{
				return ReflectionFascadeFactory.Value;
			}
		}

		private static Lazy<IReflectionFascade> ReflectionFascadeFactory
		{
			get
			{
				return reflectionFascadeFactory;
			}
		}

		#endregion
	}
}