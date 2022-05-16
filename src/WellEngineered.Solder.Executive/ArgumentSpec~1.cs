/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Executive
{
	public class ArgumentSpec<T> : ArgumentSpec
	{
		#region Constructors/Destructors

		public ArgumentSpec(bool required, bool bounded)
			: base(typeof(T), required, bounded)
		{
		}

		#endregion
	}
}