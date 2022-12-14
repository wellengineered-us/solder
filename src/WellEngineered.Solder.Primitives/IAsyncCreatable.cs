/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Threading.Tasks;

namespace WellEngineered.Solder.Primitives
{
	public interface IAsyncCreatable
	{
		#region Methods/Operators

		ValueTask CreateAsync();

		#endregion
	}
}