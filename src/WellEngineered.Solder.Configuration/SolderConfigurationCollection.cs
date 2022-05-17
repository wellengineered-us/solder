/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Configuration
{
	public partial class SolderConfigurationCollection
		: Collection<ISolderConfiguration>,
			ISolderConfigurationCollection
	{
		#region Constructors/Destructors

		public SolderConfigurationCollection(ISolderConfiguration site)
		{
			if ((object)site == null)
				throw new ArgumentNullException(nameof(site));

			this.site = site;
		}

		#endregion

		#region Fields/Constants

		private readonly ISolderConfiguration site;

		#endregion

		#region Properties/Indexers/Events

		[SolderConfigurationIgnore]
		public ISolderConfiguration Site
		{
			get
			{
				return this.site;
			}
		}

		#endregion

		#region Methods/Operators

		public void AddRange(IEnumerable<ISolderConfigurationCollection> items)
		{
			if ((object)items == null)
				throw new ArgumentNullException(nameof(items));

			foreach (ISolderConfiguration item in items)
			{
				this.Add(item);
			}
		}

		/// <summary>
		/// Removes all elements from the collection.
		/// </summary>
		protected override void ClearItems()
		{
			foreach (ISolderConfiguration item in this.Items)
			{
				item.Surround = null;
				item.Parent = null;
			}

			base.ClearItems();
		}

		private IEnumerable<IMessage> CoreValidate(object context)
		{
			int index = 0;

			foreach (ISolderConfiguration item in this)
			{
				string value = context == null ? string.Format("{0}", index++) : string.Format("{0}[{1}]", context, index++);

				if (item == null)
					continue;

				var childMessages = item.Validate(value);

				foreach (var childMessage in childMessages)
				{
					yield return childMessage;
				}
			}
		}

		/// <summary>
		/// Inserts an element into the collection at the specified index.
		/// </summary>
		/// <param name="index"> The zero-based index at which item should be inserted. </param>
		/// <param name="item"> The object to insert. The value can be null for reference types. </param>
		protected override void InsertItem(int index, ISolderConfiguration item)
		{
			if ((object)item == null)
				throw new ArgumentNullException(nameof(item));

			item.Surround = this;
			item.Parent = this.Site;

			base.InsertItem(index, item);
		}

		/// <summary>
		/// Removes the element at the specified index of the collection.
		/// </summary>
		/// <param name="index"> The zero-based index of the element to remove. </param>
		protected override void RemoveItem(int index)
		{
			ISolderConfiguration item;

			item = base[index];

			if ((object)item != null)
			{
				item.Surround = null;
				item.Parent = null;
			}

			base.RemoveItem(index);
		}

		/// <summary>
		/// Replaces the element at the specified index.
		/// </summary>
		/// <param name="index"> The zero-based index of the element to replace. </param>
		/// <param name="item"> The new value for the element at the specified index. The value can be null for reference types. </param>
		protected override void SetItem(int index, ISolderConfiguration item)
		{
			if ((object)item == null)
				throw new ArgumentNullException(nameof(item));

			item.Surround = this;
			item.Parent = this.Site;

			base.SetItem(index, item);
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