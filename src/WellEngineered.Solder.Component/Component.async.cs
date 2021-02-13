/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Component
{
	public abstract partial class Component
	{
		#region Methods/Operators

		protected override ValueTask CoreCreateAsync(bool creating)
		{
			if (creating)
			{
				// do nothing
			}

			return default;
		}

		protected override ValueTask CoreDisposeAsync(bool disposing)
		{
			if (disposing)
			{
				// do nothing
			}

			return default;
		}

		#endregion
	}
}