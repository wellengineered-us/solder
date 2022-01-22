/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Serialization.Xyzl
{
	public abstract class XyzlValue<TValue> : XyzlModel, IXyzlValue<TValue>
	{
		#region Constructors/Destructors

		protected XyzlValue()
		{
		}

		#endregion

		#region Fields/Constants

		private TValue value;

		#endregion

		#region Properties/Indexers/Events

		public TValue Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		object IXyzlValue.Value
		{
			get
			{
				return this.Value;
			}
			set
			{
				this.Value = (TValue)value;
			}
		}

		#endregion
	}
}