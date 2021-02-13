/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Serialization.Xyzl
{
	public abstract class XyzlValueObject<TValue> : XyzlObject, IXyzlValueObject<TValue>
	{
		#region Constructors/Destructors

		protected XyzlValueObject()
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

		object IXyzlValueObject.Value
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