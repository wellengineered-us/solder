/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Solder.Injection;

public sealed class DisposableDispatch<TDisposable>
	: IDisposableDispatch<TDisposable>
	where TDisposable : IDisposable
{
	#region Constructors/Destructors

	public DisposableDispatch(IDisposable proxy, TDisposable target)
	{
		this.proxy = proxy;
		this.target = target;
	}

	#endregion

	#region Fields/Constants

	private readonly IDisposable proxy;
	private readonly TDisposable target;

	#endregion

	#region Properties/Indexers/Events

	private IDisposable Proxy
	{
		get
		{
			return this.proxy;
		}
	}

	public TDisposable Target
	{
		get
		{
			return this.target;
		}
	}

	#endregion

	#region Methods/Operators

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			this.Target.Dispose();
		}
	}

	public void Dispose()
	{
		try
		{
			this.Dispose(true);
		}
		finally
		{
			GC.SuppressFinalize(this);
		}
	}

	#endregion
}