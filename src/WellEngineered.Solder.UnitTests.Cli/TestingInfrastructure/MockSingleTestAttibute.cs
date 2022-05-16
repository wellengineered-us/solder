/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
	public class MockSingleTestAttibute : Attribute
	{
		#region Constructors/Destructors

		public MockSingleTestAttibute(int value)
		{
			this.value = value;
		}

		#endregion

		#region Fields/Constants

		private int value;

		#endregion

		#region Properties/Indexers/Events

		public int Value
		{
			get
			{
				return this.value;
			}
		}

		#endregion
	}
}