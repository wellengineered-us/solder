/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Component
{
	public abstract partial class UnknownSolderConfiguration<TComponentSpecification>
		: UnknownSolderConfiguration,
			IUnknownSolderConfiguration<TComponentSpecification>
		where TComponentSpecification : class, ISolderSpecification, new()
	{
		#region Constructors/Destructors

		protected UnknownSolderConfiguration(IUnknownSolderConfiguration that)
			: this(new Dictionary<string, object>(), that)
		{
		}

		protected UnknownSolderConfiguration(IDictionary<string, object> untypedSpecification, IUnknownSolderConfiguration that)
			: base(untypedSpecification, typeof(TComponentSpecification))
		{
			if ((object)untypedSpecification == null)
				throw new ArgumentNullException(nameof(untypedSpecification));

			if ((object)that == null)
				throw new ArgumentNullException(nameof(that));

			if ((object)base.Specification != null &&
				(object)that.Specification != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in that.Specification)
					base.Specification.Add(keyValuePair.Key, keyValuePair.Value);
			}

			this.AssemblyQualifiedTypeName = that.AssemblyQualifiedTypeName;
			this.Parent = that.Parent;
			this.Surround = that.Surround;
		}

		#endregion

		#region Fields/Constants

		private TComponentSpecification specification;

		#endregion

		#region Properties/Indexers/Events

		public new TComponentSpecification Specification
		{
			get
			{
				this.ApplyTypedSpecification(); // special case
				return this.specification;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.specification, value);
				this.specification = value;
			}
		}

		#endregion

		#region Methods/Operators

		private void ApplyTypedSpecification()
		{
			if ((object)base.Specification != null) // MUST BE base.Prop here or SOE throws
			{
				this.UntypedSolderComponentSpecification =
					this.Specification =
						JObject.FromObject(base.Specification).ToObject<TComponentSpecification>();
			}
		}

		#endregion
	}
}