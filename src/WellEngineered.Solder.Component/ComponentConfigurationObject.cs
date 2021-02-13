/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Component
{
	public abstract partial class ComponentConfigurationObject : ConfigurationObject, IComponentConfigurationObject
	{
		#region Constructors/Destructors

		protected ComponentConfigurationObject()
		{
		}

		#endregion

		#region Methods/Operators

		protected static Type GetTypeFromString(string aqtn, IList<Message> messages = null)
		{
			Type type = null;

			if (string.IsNullOrWhiteSpace(aqtn))
				return null;

			if ((object)messages == null)
				type = Type.GetType(aqtn, false);
			else
			{
				try
				{
					type = Type.GetType(aqtn, true);
				}
				catch (Exception ex)
				{
					messages.Add(new Message(string.Empty, string.Format("Error loading type '{0}' from string: {1}.", aqtn, ex.Message), Severity.Error));
				}
			}

			return type;
		}

		protected abstract IEnumerable<IMessage> CoreValidate(object context);

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