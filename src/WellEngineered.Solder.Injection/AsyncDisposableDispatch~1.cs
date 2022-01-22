/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading.Tasks;

namespace WellEngineered.Solder.Injection;

public sealed class AsyncDisposableDispatch<TAsyncDisposable>
	: IAsyncDisposableDispatch<TAsyncDisposable>
	where TAsyncDisposable : IAsyncDisposable
{
	#region Constructors/Destructors

	public AsyncDisposableDispatch(IAsyncDisposable asyncProxy, TAsyncDisposable asyncTarget)
	{
		this.asyncProxy = asyncProxy;
		this.asyncTarget = asyncTarget;
	}

	#endregion

	#region Fields/Constants

	private readonly IAsyncDisposable asyncProxy;
	private readonly TAsyncDisposable asyncTarget;

	#endregion

	#region Properties/Indexers/Events

	private IAsyncDisposable AsyncProxy
	{
		get
		{
			return this.asyncProxy;
		}
	}

	public TAsyncDisposable AsyncTarget
	{
		get
		{
			return this.asyncTarget;
		}
	}

	#endregion

	#region Methods/Operators

	private async ValueTask DisposeAsync(bool disposing)
	{
		if (disposing)
		{
			await this.AsyncTarget.DisposeAsync();
		}
	}

	public async ValueTask DisposeAsync()
	{
		try
		{
			await this.DisposeAsync(true);
		}
		finally
		{
			GC.SuppressFinalize(this);
		}
	}

	#endregion
}
#endif