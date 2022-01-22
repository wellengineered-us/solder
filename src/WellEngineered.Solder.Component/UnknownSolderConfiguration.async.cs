/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

using ISolderComponent = WellEngineered.Solder.Component.ISolderComponent2;

namespace WellEngineered.Solder.Component
{
	public abstract partial class UnknownSolderConfiguration
	{
		#region Methods/Operators

		protected override async IAsyncEnumerable<IMessage> CoreValidateAsync(object context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			IAsyncEnumerable<IMessage> messages = null;
			Type componentType, specificationType;
			ISolderComponent solderComponent;
			string componentContext;
			IMessage errorMessage = null;

			componentContext = context as string;

			if (string.IsNullOrWhiteSpace(this.AssemblyQualifiedTypeName))
				yield return new Message(string.Empty, string.Format("'{0}' {1} is required.", nameof(this.AssemblyQualifiedTypeName), componentContext), Severity.Error);
			else
			{
				componentType = this.GetComponentType();

				if ((object)componentType == null)
					yield return new Message(string.Empty, string.Format("'{0}' {1} failed to load type.", nameof(this.AssemblyQualifiedTypeName), componentContext), Severity.Error);
				else if (!typeof(ISolderComponent).IsAssignableFrom(componentType))
					yield return new Message(string.Empty, string.Format("'{0}' {1} is not an assignable type.", nameof(this.AssemblyQualifiedTypeName), componentContext), Severity.Error);
				else
				{
					// new-ing up via default public constructor should be low friction
					solderComponent = (ISolderComponent)Activator.CreateInstance(componentType);

					if ((object)solderComponent == null)
						yield return new Message(string.Empty, string.Format("'{0}' {1} failed to instantiate type.", nameof(this.AssemblyQualifiedTypeName), componentContext), Severity.Error);
					else
					{
						try
						{
							using (solderComponent)
							{
								solderComponent.Configuration = this;
								await solderComponent.CreateAsync();

								// "Hey Component, tell me what your Specification type is?"
								specificationType = solderComponent.SpecificationType;
								this.ApplyUntypedSpecification(specificationType);

								//messages = this.UntypedComponentSpecification.Validate(componentContext);
								messages = solderComponent.Specification.ValidateAsync(componentContext, cancellationToken);
							}
						}
						catch (Exception ex)
						{
							errorMessage = new Message(string.Empty, string.Format("'{0}' {1} failed with an exception: '{2}'.", nameof(this.AssemblyQualifiedTypeName), componentContext, ex), Severity.Error);
						}

						if ((object)errorMessage != null)
							yield return errorMessage;

						if ((object)messages != null)
						{
							await foreach (IMessage message in messages.WithCancellation(cancellationToken))
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
#endif