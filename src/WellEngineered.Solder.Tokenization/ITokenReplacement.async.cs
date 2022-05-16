/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Tokenization
{
	/// <summary>
	/// Represents a token replacement strategy pattern.
	/// </summary>
	public partial interface ITokenReplacement
	{
		#region Methods/Operators

		ValueTask<object> EvaluateAsync(string[] parameters, CancellationToken cancellationToken = default);

		ValueTask<object> EvaluateAsync(string token, object[] parameters, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif