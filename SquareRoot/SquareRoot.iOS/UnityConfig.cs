using System;
using Microsoft.Practices.Unity;
using SquareRoot.iOS.Reader;
using CardReader.Interfaces;
using CardReader;

namespace SquareRoot.iOS
{
	[assembly: Xamarin.Forms.Dependency(typeof(UnityConfig))]
	public class UnityConfig : IUnityProvider
	{
		private static readonly Lazy<IUnityContainer> LazyContainer = new Lazy<IUnityContainer>(() =>
			{
				var container = new UnityContainer();
				RegisterTypes(container);
				return container;
			});

		public IUnityContainer GetContainer ()
		{
			return Container;
		}

		public static IUnityContainer Container => LazyContainer.Value;

		private static void RegisterTypes(IUnityContainer container)
		{
			container.RegisterType<IDependencyProvider, DependencyProvider>();
			DependencyServiceManager.Initialize(container.Resolve<IDependencyProvider>());

			container.RegisterType<IPlatformService, PlatformServiceiOS>(new ContainerControlledLifetimeManager());
			container.RegisterType<ISecurityManager, SecurityManager>(new ContainerControlledLifetimeManager());

			container.RegisterType<IHeadsetVolumeHelper, HeadsetVolumeHelper>(new ContainerControlledLifetimeManager());
			container.RegisterType<IReaderConnectionListener, CardReaderConnectionListener>(
				new ContainerControlledLifetimeManager());
			container.RegisterType<ICardReaderHelper, Acr35CardReaderHelper>(new ContainerControlledLifetimeManager());
			container.RegisterType<INfcReaderWriter, CardReaderNfcReaderWriter>(new ContainerControlledLifetimeManager());
			container.RegisterType<IMagStripeReader, CardReaderMagStripeReader>(new ContainerControlledLifetimeManager());
		}
	}
}

