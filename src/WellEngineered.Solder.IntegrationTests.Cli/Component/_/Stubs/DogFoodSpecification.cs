/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;
using System.Threading;

using WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs
{
	public sealed partial class DogFoodSpecification : EndToEndSpecification
	{
		#region Constructors/Destructors

		public DogFoodSpecification()
		{
		}

		#endregion

		#region Properties/Indexers/Events

		public string PropA
		{
			get;
			set;
		}

		public int? PropB
		{
			get;
			set;
		}

		public bool? PropC
		{
			get;
			set;
		}

		public double? PropD
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