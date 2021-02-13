/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Component
{
	public partial class UnknownComponentConfigurationObject : ComponentConfigurationObject, IUnknownComponentConfigurationObject
	{
		#region Constructors/Destructors

		public UnknownComponentConfigurationObject()
			: this(new Dictionary<string, object>(), typeof(IComponentConfigurationObject))
		{
		}

		public UnknownComponentConfigurationObject(IDictionary<string, object> componentSpecificConfiguration, Type componentSpecificConfigurationType)
		{
			if ((object)componentSpecificConfigurationType == null)
				throw new ArgumentNullException(nameof(componentSpecificConfigurationType));

			if ((object)componentSpecificConfiguration == null)
				throw new ArgumentNullException(nameof(componentSpecificConfiguration));

			this.componentSpecificConfigurationType = componentSpecificConfigurationType;
			this.componentSpecificConfiguration = componentSpecificConfiguration;
		}

		#endregion

		#region Fields/Constants

		private readonly IDictionary<string, object> componentSpecificConfiguration;
		private readonly Type componentSpecificConfigurationType;
		private string componentAssemblyQualifiedTypeName;
		private IComponentConfigurationObject untypedComponentSpecificConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public IDictionary<string, object> ComponentSpecificConfiguration
		{
			get
			{
				return this.componentSpecificConfiguration;
			}
		}

		public Type ComponentSpecificConfigurationType
		{
			get
			{
				return this.componentSpecificConfigurationType;
			}
		}

		public string ComponentAssemblyQualifiedTypeName
		{
			get
			{
				return this.componentAssemblyQualifiedTypeName;
			}
			set
			{
				this.componentAssemblyQualifiedTypeName = value;
			}
		}

		public IComponentConfigurationObject UntypedComponentSpecificConfiguration
		{
			get
			{
				this.ApplyUntypedSpecification(); // special case
				return this.untypedComponentSpecificConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.untypedComponentSpecificConfiguration, value);
				this.untypedComponentSpecificConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		private void ApplyUntypedSpecification()
		{
			this.ApplyUntypedSpecification(this.ComponentSpecificConfigurationType);
		}

		private void ApplyUntypedSpecification(Type specificationType)
		{
			if ((object)this.ComponentSpecificConfiguration != null)
			{
				this.UntypedComponentSpecificConfiguration = (IUnknownComponentConfigurationObject)
					JObject.FromObject(this.ComponentSpecificConfiguration).ToObject(specificationType);
			}
		}

		protected override IEnumerable<IMessage> CoreValidate(object context)
		{
			IEnumerable<IMessage> messages = null;
			Type componentType, specificationType;
			ISpecifiableConfigurableComponent component;
			string componentContext;
			IMessage errorMessage = null;

			componentContext = context as string;

			if (string.IsNullOrWhiteSpace(this.ComponentAssemblyQualifiedTypeName))
				yield return new Message(string.Empty, string.Format("'{0}' {1} is required.", nameof(this.ComponentAssemblyQualifiedTypeName), componentContext), Severity.Error);
			else
			{
				componentType = this.GetComponentType(null);

				if ((object)componentType == null)
					yield return new Message(string.Empty, string.Format("'{0}' {1} is failed to load type.", nameof(this.ComponentAssemblyQualifiedTypeName), componentContext), Severity.Error);
				else if (!typeof(ISpecifiableConfigurableComponent).IsAssignableFrom(componentType))
					yield return new Message(string.Empty, string.Format("'{0}' {1} is an unassignable type.", nameof(this.ComponentAssemblyQualifiedTypeName), componentContext), Severity.Error);
				else
				{
					// new-ing up via default public constructor should be low friction
					component = (ISpecifiableConfigurableComponent)Activator.CreateInstance(componentType);

					if ((object)component == null)
						yield return new Message(string.Empty, string.Format("'{0}' {1} failed to instantiate type.", nameof(this.ComponentAssemblyQualifiedTypeName), componentContext), Severity.Error);
					else
					{
						try
						{
							using (component)
							{
								component.Configuration = this;
								component.Create();

								// "Hey Component, tell me what your Specification type is?"
								specificationType = component.SpecificationType;
								this.ApplyUntypedSpecification(specificationType);

								//messages = this.UntypedComponentSpecificConfiguration.Validate(componentContext);
								messages = component.Specification.Validate(componentContext);
							}
						}
						catch (Exception ex)
						{
							errorMessage = new Message(string.Empty, string.Format("'{0}' {1} failed with an exception: '{2}'.", nameof(this.ComponentAssemblyQualifiedTypeName), componentContext, ex), Severity.Error);
						}

						if ((object)errorMessage != null)
							yield return errorMessage;

						if ((object)messages != null)
						{
							foreach (IMessage message in messages)
							{
								yield return message;
							}
						}
					}
				}
			}
		}

		public Type GetComponentType(IList<Message> messages = null)
		{
			return GetTypeFromString(this.ComponentAssemblyQualifiedTypeName, messages);
		}

		public void ResetSpecification()
		{
			this.ComponentSpecificConfiguration.Clear();
			this.UntypedComponentSpecificConfiguration = null;
		}

		#endregion
	}
}