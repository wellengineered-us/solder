/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure
{
	public interface IMockCloneable
	{
		#region Methods/Operators

		object Clone();

		#endregion
	}
}