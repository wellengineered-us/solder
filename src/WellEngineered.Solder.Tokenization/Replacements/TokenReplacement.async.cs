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
	public abstract partial class TokenReplacement
		: ITokenReplacement
	{
		#region Methods/Operators

		protected abstract ValueTask<object> CoreEvaluateAsync(string[] parameters, CancellationToken cancellationToken = default);

		protected abstract ValueTask<object> CoreEvaluateAsync(string token, object[] parameters, CancellationToken cancellationToken = default);

		public ValueTask<object> EvaluateAsync(string[] parameters, CancellationToken cancellationToken = default)
		{
			if ((object)parameters == null)
				throw new ArgumentNullException(nameof(parameters));

			try
			{
				return this.CoreEvaluateAsync(parameters, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new TokenizationException(string.Format("The token replacement failed (see inner exception)."), ex);
			}
		}

		public ValueTask<object> EvaluateAsync(string token, object[] parameters, CancellationToken cancellationToken = default)
		{
			if ((object)token == null)
				throw new ArgumentNullException(nameof(token));

			if ((object)parameters == null)
				throw new ArgumentNullException(nameof(parameters));

			try
			{
				return this.CoreEvaluateAsync(token, parameters, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new TokenizationException(string.Format("The token replacement failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}
#endif