/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Component
{
	public abstract partial class ComponentConfigurationObject
	{
		#region Methods/Operators

		protected abstract IAsyncEnumerable<IMessage> CoreValidateAsync(object context);

		public IAsyncEnumerable<IMessage> ValidateAsync()
		{
			return this.CoreValidateAsync(null);
		}

		public IAsyncEnumerable<IMessage> ValidateAsync(object context)
		{
			return this.CoreValidateAsync(context);
		}

		#endregion
	}
}