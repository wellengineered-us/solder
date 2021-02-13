/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Injection;

namespace WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure
{
	public class MockAmbiguousCtorMatchDependantObject
	{
		#region Constructors/Destructors

		public MockAmbiguousCtorMatchDependantObject()
		{
			this.text = null;
		}

		[DependencyInjection(SelectorKey = "i_haz_no_marked_params")]
		public MockAmbiguousCtorMatchDependantObject(object value, MockAmbiguousCtorMatchDependantObject text)
		{
			this.text = null;
		}

		[DependencyInjection(SelectorKey = "named_dep_obj")]
		public MockAmbiguousCtorMatchDependantObject([DependencyInjection(SelectorKey = "named_dep_obj")]
			MockAmbiguousCtorMatchDependantObject left, [DependencyInjection] MockAmbiguousCtorMatchDependantObject right)
		{
			this.text = string.Empty;
			this.left = left;
			this.right = right;
		}

		[DependencyInjection(SelectorKey = "named_dep_obj")]
		public MockAmbiguousCtorMatchDependantObject([DependencyInjection] MockAmbiguousCtorMatchDependantObject both)
		{
			this.text = string.Empty;
			this.left = both;
			this.right = both;
		}

		#endregion

		#region Fields/Constants

		private readonly MockAmbiguousCtorMatchDependantObject left;
		private readonly MockAmbiguousCtorMatchDependantObject right;
		private readonly string text;

		#endregion

		#region Properties/Indexers/Events

		public MockAmbiguousCtorMatchDependantObject Left
		{
			get
			{
				return this.left;
			}
		}

		public MockAmbiguousCtorMatchDependantObject Right
		{
			get
			{
				return this.right;
			}
		}

		public string Text
		{
			get
			{
				return this.text;
			}
		}

		#endregion
	}
}