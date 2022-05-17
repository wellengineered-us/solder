/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Serialization.Xyzl
{
	public abstract class XyzlModel : SolderConfiguration, IXyzlModel
	{
		#region Constructors/Destructors

		protected XyzlModel()
		{
		}

		#endregion

		#region Fields/Constants

		private IXyzlName name;

		#endregion

		#region Properties/Indexers/Events

		public IXyzlName Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		#endregion
	}
}