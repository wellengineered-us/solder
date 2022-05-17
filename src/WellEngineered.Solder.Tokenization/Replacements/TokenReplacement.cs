/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Tokenization.Replacements
{
	public abstract partial class TokenReplacement
		: ITokenReplacement
	{
		#region Constructors/Destructors

		protected TokenReplacement()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract object CoreEvaluate(string[] parameters);

		protected abstract object CoreEvaluate(string token, object[] parameters);

		public object Evaluate(string[] parameters)
		{
			if ((object)parameters == null)
				throw new ArgumentNullException(nameof(parameters));

			try
			{
				return this.CoreEvaluate(parameters);
			}
			catch (Exception ex)
			{
				throw new TokenizationException(string.Format("The token replacement failed (see inner exception)."), ex);
			}
		}

		public object Evaluate(string token, object[] parameters)
		{
			if ((object)token == null)
				throw new ArgumentNullException(nameof(token));

			if ((object)parameters == null)
				throw new ArgumentNullException(nameof(parameters));

			try
			{
				return this.CoreEvaluate(token, parameters);
			}
			catch (Exception ex)
			{
				throw new TokenizationException(string.Format("The token replacement failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}