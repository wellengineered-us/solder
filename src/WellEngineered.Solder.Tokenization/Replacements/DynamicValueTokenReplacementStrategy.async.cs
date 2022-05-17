/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Tokenization.Replacements
{
	/// <summary>
	/// Provides a dynamic token replacement strategy which executes an on-demand callback method to obtain a replacement value.
	/// </summary>
	public partial class DynamicValueTokenReplacement
		: TokenReplacement
	{
		#region Methods/Operators

		protected override ValueTask<object> CoreEvaluateAsync(string[] parameters, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override ValueTask<object> CoreEvaluateAsync(string token, object[] parameters, CancellationToken cancellationToken = default)
		{
			return default;
		}

		#endregion
	}
}
#endif