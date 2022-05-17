/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure
{
	public interface IMockObject
	{
		#region Properties/Indexers/Events

		object SomeProp
		{
			get;
			set;
		}

		#endregion

		#region Methods/Operators

		byte SomeMethodWithVarietyOfParameters(int inparam, out string outparam, ref object refparam);

		#endregion
	}
}