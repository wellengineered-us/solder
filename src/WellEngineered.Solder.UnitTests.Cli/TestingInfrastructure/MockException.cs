/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace WellEngineered.Solder.UnitTests.Cli.TestingInfrastructure
{
	public class MockException : Exception
	{
		#region Constructors/Destructors

		public MockException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion

		#region Fields/Constants

		private readonly IList<Exception> collectedExceptions = new List<Exception>();

		#endregion

		#region Properties/Indexers/Events

		public IList<Exception> CollectedExceptions
		{
			get
			{
				return this.collectedExceptions;
			}
		}

		#endregion
	}
}