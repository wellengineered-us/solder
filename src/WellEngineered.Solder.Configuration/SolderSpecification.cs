/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Configuration
{
	/// <summary>
	/// Provides a base for all configuration objects.
	/// </summary>
	public abstract partial class SolderSpecification
		: SolderConfiguration,
			ISolderSpecification
	{
		#region Constructors/Destructors

		protected SolderSpecification()
		{
		}

		#endregion
	}
}