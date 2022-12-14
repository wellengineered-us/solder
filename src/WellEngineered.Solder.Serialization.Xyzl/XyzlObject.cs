/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Serialization.Xyzl
{
	public abstract class XyzlObject : ConfigurationObject, IXyzlObject
	{
		#region Constructors/Destructors

		protected XyzlObject()
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