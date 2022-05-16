/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Tokenization.Replacements
{
	/// <summary>
	/// Provides a static token replacement strategy which returns the same static value each request.
	/// </summary>
	public partial class StaticValueTokenReplacement
		: TokenReplacement
	{
		#region Methods/Operators

		protected override async ValueTask<object> CoreEvaluateAsync(string[] parameters, CancellationToken cancellationToken = default)
		{
			return await Task.FromResult(this.Value);
		}

		protected override async ValueTask<object> CoreEvaluateAsync(string token, object[] parameters, CancellationToken cancellationToken = default)
		{
			return await Task.FromResult(this.Value);
		}

		#endregion
	}
}
#endif