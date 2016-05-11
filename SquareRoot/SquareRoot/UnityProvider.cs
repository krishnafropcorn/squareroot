using System;
using Microsoft.Practices.Unity;
using Xamarin.Forms;

namespace SquareRoot
{
	public static class UnityProvider
	{
		static UnityProvider()
		{
			provider = DependencyServiceManager.Get<IUnityProvider>();
		}

		static readonly IUnityProvider provider;

		public static IUnityContainer GetContainer()
		{
			return provider.GetContainer();
		}
	}

	public class DependencyServiceManager
	{
		static IDependencyProvider _dependencyProvider;

		public static void Initialize(IDependencyProvider dependencyProvider)
		{
			_dependencyProvider = dependencyProvider;
		}

		public static T Get<T>() where T:class
		{
			return _dependencyProvider.Get<T>();
		}
	}

	public interface IDependencyProvider
	{
		T Get<T>() where T:class;
	}

	public class DependencyProvider:IDependencyProvider
	{
		public T Get<T>() where T:class
		{
			return DependencyService.Get<T>();
		}
	}
}

