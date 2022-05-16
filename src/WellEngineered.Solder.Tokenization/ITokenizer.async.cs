/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Tokenization
{
	public partial interface ITokenizer
	{
		#region Methods/Operators

		ValueTask<string> ExpandTokensAsync(string tokenizedValue, CancellationToken cancellationToken = default);

		ValueTask<string> ExpandTokensAsync(string tokenizedValue, ITokenReplacement optionalWildcardTokenReplacement, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif