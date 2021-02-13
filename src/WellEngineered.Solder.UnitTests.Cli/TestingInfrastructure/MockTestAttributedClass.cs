/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure;

[assembly: MockSingleTestAttibute(int.MaxValue)]
[module: MockSingleTestAttibute(int.MinValue)]

namespace WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure
{
	[MockSingleTestAttibute(1)]
	[MockMultipleTestAttibute(10)]
	[MockMultipleTestAttibute(20)]
	public class MockTestAttributedClass
	{
		#region Constructors/Destructors

		public MockTestAttributedClass()
		{
		}

		#endregion

		#region Methods/Operators

		[MockSingleTestAttibute(2)]
		[return: MockSingleTestAttibute(8)]
		public object MyMethod([MockSingleTestAttibute(4)] object obj)
		{
			return obj;
		}

		#endregion
	}
}