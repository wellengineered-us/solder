/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Primitives
{
	public interface IMessage
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the message category.
		/// </summary>
		string Category
		{
			get;
		}

		/// <summary>
		/// Gets the message description.
		/// </summary>
		string Description
		{
			get;
		}

		/// <summary>
		/// Gets the message severity.
		/// </summary>
		Severity Severity
		{
			get;
		}

		#endregion
	}
}