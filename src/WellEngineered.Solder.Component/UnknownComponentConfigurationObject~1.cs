/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace WellEngineered.Solder.Component
{
	public class UnknownComponentConfigurationObject<TSpecification> : UnknownComponentConfigurationObject, IUnknownComponentConfigurationObject<TSpecification>
		where TSpecification : IComponentConfigurationObject
	{
		#region Constructors/Destructors

		public UnknownComponentConfigurationObject(IUnknownComponentConfigurationObject that)
			: this(new Dictionary<string, object>(), that)
		{
		}

		public UnknownComponentConfigurationObject(IDictionary<string, object> componentSpecificConfiguration, IUnknownComponentConfigurationObject that)
			: base(componentSpecificConfiguration, typeof(TSpecification))
		{
			if ((object)componentSpecificConfiguration == null)
				throw new ArgumentNullException(nameof(componentSpecificConfiguration));

			if ((object)that == null)
				throw new ArgumentNullException(nameof(that));

			if ((object)base.ComponentSpecificConfiguration != null &&
				(object)that.ComponentSpecificConfiguration != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in that.ComponentSpecificConfiguration)
					base.ComponentSpecificConfiguration.Add(keyValuePair.Key, keyValuePair.Value);
			}

			this.ComponentAssemblyQualifiedTypeName = that.ComponentAssemblyQualifiedTypeName;
			this.Parent = that.Parent;
			this.Surround = that.Surround;
		}

		#endregion

		#region Fields/Constants

		private TSpecification componentSpecificConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public new TSpecification ComponentSpecificConfiguration
		{
			get
			{
				this.ApplyTypedSpecification(); // special case
				return this.componentSpecificConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.componentSpecificConfiguration, value);
				this.componentSpecificConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		private void ApplyTypedSpecification()
		{
			if ((object)base.ComponentSpecificConfiguration != null) // MUST BE base.Prop here or SOE throws
			{
				this.UntypedComponentSpecificConfiguration =
					this.ComponentSpecificConfiguration =
						JObject.FromObject(base.ComponentSpecificConfiguration).ToObject<TSpecification>();
			}
		}

		#endregion
	}
}