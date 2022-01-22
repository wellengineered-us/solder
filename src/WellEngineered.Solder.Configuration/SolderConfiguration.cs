/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Configuration
{
	/// <summary>
	/// Provides a base for all configuration objects.
	/// </summary>
	public abstract partial class SolderConfiguration
		: ISolderConfiguration
	{
		#region Constructors/Destructors

		protected SolderConfiguration()
		{
			this.items = new SolderConfigurationCollection(this);
		}

		protected SolderConfiguration(ISolderConfigurationCollection items)
		{
			if ((object)items == null)
				throw new ArgumentNullException(nameof(items));

			this.items = items;
		}

		#endregion

		#region Fields/Constants

		private readonly ISolderConfigurationCollection items;
		private ISolderConfiguration content;
		private ISolderConfiguration parent;
		private ISolderConfigurationCollection surround;

		#endregion

		#region Properties/Indexers/Events

		[SolderConfigurationIgnore]
		public virtual IEnumerable<Type> AllowedChildTypes
		{
			get
			{
				return new Type[] { typeof(ISolderConfiguration) };
			}
		}

		[SolderConfigurationIgnore]
		public ISolderConfigurationCollection Items
		{
			get
			{
				return this.items;
			}
		}

		[SolderConfigurationIgnore]
		public ISolderConfiguration Content
		{
			get
			{
				return this.content;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.content, value);
				this.content = value;
			}
		}

		[SolderConfigurationIgnore]
		public ISolderConfiguration Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		[SolderConfigurationIgnore]
		public ISolderConfigurationCollection Surround
		{
			get
			{
				return this.surround;
			}
			set
			{
				this.surround = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected abstract IEnumerable<IMessage> CoreValidate(object context);

		/// <summary>
		/// Ensures that for any configuration object property, the correct parent instance is set/unset.
		/// Should be called in the setter for all configuration object properties, before assigning the value.
		/// Example:
		/// set { this.EnsureParentOnPropertySet(this.content, value); this.content = value; }
		/// </summary>
		/// <param name="oldValueObj"> The old configuration object value (the backing field). </param>
		/// <param name="newValueObj"> The new configuration object value (value). </param>
		protected void EnsureParentOnPropertySet(ISolderConfiguration oldValueObj, ISolderConfiguration newValueObj)
		{
			if ((object)oldValueObj != null)
			{
				oldValueObj.Surround = null;
				oldValueObj.Parent = null;
			}

			if ((object)newValueObj != null)
			{
				newValueObj.Surround = null;
				newValueObj.Parent = this;
			}
		}

		public IEnumerable<IMessage> Validate()
		{
			return this.Validate(null);
		}

		public virtual IEnumerable<IMessage> Validate(object context)
		{
			return this.CoreValidate(context);
		}

		#endregion
	}
}