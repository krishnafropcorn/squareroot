using System;

namespace SquareRoot
{
	public interface IPlatformService
	{
		void InvokeOnMainThread(Action action);

		void RunInThreadPool(Action action);
	}
}

