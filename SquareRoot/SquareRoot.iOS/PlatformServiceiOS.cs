using System;
using System.Threading;

namespace SquareRoot.iOS
{
	public class PlatformServiceiOS : IPlatformService
	{
		public void InvokeOnMainThread(Action action)
		{
			AppDelegate.Self.InvokeOnMainThread (action);
		}

		public void RunInThreadPool(Action action)
		{
			ThreadPool.QueueUserWorkItem((data) => action());
		}
	}
}

