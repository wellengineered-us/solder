/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs
{
	public sealed partial class DogFoodConfiguration : EndToEndConfiguration
	{
		#region Constructors/Destructors

		public DogFoodConfiguration()
		{
		}

		#endregion

		#region Properties/Indexers/Events

		public string Prop1
		{
			get;
			set;
		}

		public int? Prop2
		{
			get;
			set;
		}

		public bool? Prop3
		{
			get;
			set;
		}

		public double? Prop4
		{
			get;
			set;
		}

		#endregion

		#region Methods/Operators

		protected override IEnumerable<IMessage> CoreValidate(object context)
		{
			yield break;
		}

		#endregion
	}
}