/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Tokenization
{
	public sealed partial class Tokenizer
		: ITokenizer
	{
		#region Methods/Operators

		public ValueTask<string> ExpandTokensAsync(string tokenizedValue, CancellationToken cancellationToken = default)
		{
			return default;
		}

		public ValueTask<string> ExpandTokensAsync(string tokenizedValue, ITokenReplacement optionalWildcardTokenReplacement, CancellationToken cancellationToken = default)
		{
			return default;
		}

		#endregion
	}
}
#endif