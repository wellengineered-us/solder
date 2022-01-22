/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Injection.Resolutions
{
	/// <summary>
	/// A dependency resolution implementation that executes a
	/// factory method callback each time a dependency resolution occurs.
	/// </summary>
	public sealed partial class TransientFactoryMethodDependencyResolution : DependencyResolution
	{
		#region Methods/Operators

		protected override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		protected override ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		protected override async ValueTask<object> CoreResolveAsync(IDependencyManager dependencyManager, Type resolutionType, string selectorKey, CancellationToken cancellationToken = default)
		{
			Type activatorType;
			const string PROP_NAME_RESULT = "Result";
			object resolvedValue;
			Task task;

			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			resolvedValue = this.FactoryMethod.DynamicInvoke(null);
			task = resolvedValue as Task;

			if (task != null)
			{
				PropertyInfo resultProperty;

				await task;

				resultProperty = task.GetType().GetProperty(PROP_NAME_RESULT);

				if ((object)resultProperty == null)
					throw new DependencyException(string.Format("Property '{1}' on type '{0}' was not found.", task.GetType().FullName, PROP_NAME_RESULT));

				activatorType = resultProperty.PropertyType;
				if (!resolutionType.IsAssignableFrom(activatorType))
					throw new DependencyException(string.Format("Resolution type '{1}' is not assignable from activator type '{0}'; selector key '{2}'.", activatorType.FullName, resolutionType.FullName, selectorKey));

				resolvedValue = resultProperty.GetValue(task);
			}
			else
			{
				activatorType = this.FactoryMethod.Method.ReturnType;
				if (!resolutionType.IsAssignableFrom(activatorType))
					throw new DependencyException(string.Format("Resolution type '{1}' is not assignable from activator type '{0}'; selector key '{2}'.", activatorType.FullName, resolutionType.FullName, selectorKey));

				await Task.CompletedTask;
				resolvedValue = this.FactoryMethod.DynamicInvoke(null);
			}

			return resolvedValue;
		}

		#endregion
	}
}
#endif