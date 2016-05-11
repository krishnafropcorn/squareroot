using System;
using Microsoft.Practices.Unity;

namespace SquareRoot
{
	public interface IUnityProvider
	{
		IUnityContainer GetContainer();
	}
}

