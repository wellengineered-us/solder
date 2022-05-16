/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using WellEngineered.Solder.Configuration;

namespace WellEngineered.Solder.Component
{
	public abstract partial class UnknownSolderConfiguration<TSolderConfiguration>
		: UnknownSolderConfiguration,
			IUnknownSolderConfiguration<TSolderConfiguration>
		where TSolderConfiguration : class, ISolderSpecification, new()
	{
		#region Properties/Indexers/Events

		protected new TSolderConfiguration AsyncSpecification
		{
			get
			{
				this.CoreApplyTypedSpecificationAsync().GetAwaiter().GetResult(); // special case
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

		protected virtual async ValueTask CoreApplyTypedSpecificationAsync()
		{
			if ((object)base.Specification != null) // MUST BE base.Prop here or SOE throws
			{
				this.UntypedSolderComponentSpecification =
					this.Specification =
						JObject.FromObject(base.Specification).ToObject<TSolderConfiguration>();

				await Task.CompletedTask;
			}
		}

		#endregion
	}
}
#endif