/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Solder.Component
{
	public abstract partial class UnknownSolderConfiguration
		: SolderConfiguration,
			IUnknownSolderConfiguration
	{
		#region Constructors/Destructors

		protected UnknownSolderConfiguration()
			: this(new Dictionary<string, object>(), typeof(ISolderConfiguration))
		{
		}

		protected UnknownSolderConfiguration(IDictionary<string, object> specification, Type specificationType)
		{
			if ((object)specification == null)
				throw new ArgumentNullException(nameof(specification));

			if ((object)specificationType == null)
				throw new ArgumentNullException(nameof(specificationType));

			this.specification = specification;
			this.specificationType = specificationType;
		}

		#endregion

		#region Fields/Constants

		private readonly IDictionary<string, object> specification;
		private readonly Type specificationType;
		private string assemblyQualifiedTypeName;
		private ISolderSpecification untypedSolderSpecification;

		#endregion

		#region Properties/Indexers/Events

		public IDictionary<string, object> Specification
		{
			get
			{
				return this.specification;
			}
		}

		public Type SpecificationType
		{
			get
			{
				return this.specificationType;
			}
		}

		public string AssemblyQualifiedTypeName
		{
			get
			{
				return this.assemblyQualifiedTypeName;
			}
			set
			{
				this.assemblyQualifiedTypeName = value;
			}
		}

		public ISolderSpecification UntypedSolderComponentSpecification
		{
			get
			{
				this.CoreApplyUntypedSpecification(); // special case
				return this.untypedSolderSpecification;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.untypedSolderSpecification, value);
				this.untypedSolderSpecification = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected virtual void CoreApplyUntypedSpecification()
		{
			this.CoreApplyUntypedSpecification(this.SpecificationType);
		}

		protected virtual void CoreApplyUntypedSpecification(Type specificationType)
		{
			if ((object)specificationType == null)
				throw new ArgumentNullException(nameof(specificationType));

			if ((object)this.Specification != null)
			{
				this.UntypedSolderComponentSpecification = (ISolderSpecification)
					JObject.FromObject(this.Specification).ToObject(specificationType);
			}
		}

		protected override IEnumerable<IMessage> CoreValidate(object context)
		{
			IEnumerable<IMessage> messages = null;
			Type componentType, specificationType;
			ISolderComponent2 solderComponent;
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
				else if (!typeof(ISolderComponent2).IsAssignableFrom(componentType))
					yield return new Message(string.Empty, string.Format("'{0}' {1} is not an assignable type.", nameof(this.AssemblyQualifiedTypeName), componentContext), Severity.Error);
				else
				{
					// new-ing up via default public constructor should be low friction
					solderComponent = (ISolderComponent2)Activator.CreateInstance(componentType);

					if ((object)solderComponent == null)
						yield return new Message(string.Empty, string.Format("'{0}' {1} failed to instantiate type.", nameof(this.AssemblyQualifiedTypeName), componentContext), Severity.Error);
					else
					{
						try
						{
							using (solderComponent)
							{
								solderComponent.Configuration = this;
								solderComponent.Create();

								// "Hey Component, tell me what your Specification type is?"
								specificationType = solderComponent.SpecificationType;
								this.CoreApplyUntypedSpecification(specificationType);

								//messages = this.UntypedComponentSpecification.Validate(componentContext);
								messages = solderComponent.Specification.Validate(componentContext);
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
							foreach (IMessage message in messages)
							{
								yield return message;
							}
						}
					}
				}
			}
		}

		public Type GetComponentType()
		{
			return ReflectionExtensions.GetTypeFromAssemblyQualifiedTypeName(this.AssemblyQualifiedTypeName);
		}

		public void ResetSpecification()
		{
			this.Specification.Clear();
			this.UntypedSolderComponentSpecification = null;
		}

		#endregion
	}
}