/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Component;
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.IntegrationTests.Cli.Component._.Stubs.Configuration
{
	public class UnknownEndToEndConfiguration<TEndToEndSpecification>
		: UnknownSolderConfiguration<TEndToEndSpecification>,
			IUnknownEndToEndConfiguration<TEndToEndSpecification>
		where TEndToEndSpecification : class, IEndToEndSpecification, new()
	{
		#region Constructors/Destructors

		public UnknownEndToEndConfiguration(IUnknownSolderConfiguration that)
			: base(that)
		{
		}

		#endregion
	}
}