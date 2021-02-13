/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Component
{
	public partial class UnknownComponentConfigurationObject
	{
		#region Methods/Operators

		protected async override IAsyncEnumerable<IMessage> CoreValidateAsync(object context)
		{
			IAsyncEnumerable<IMessage> messages = null;
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
								messages = component.Specification.ValidateAsync(componentContext);
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
							await foreach (IMessage message in messages)
							{
								yield return message;
							}
						}
					}
				}
			}
		}

		#endregion
	}
}