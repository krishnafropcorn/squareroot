using System;
using Microsoft.Practices.Unity;
using SquareRoot.iOS.Reader;
using CardReader.Interfaces;
using CardReader;

namespace SquareRoot.iOS
{
	[assembly: Xamarin.Forms.Dependency(typeof(UnityConfig))]
	public static class UnityConfig
	{
        public static void Initialize() 
        {
            UnityProvider.Container = new UnityContainer();
            RegisterTypes(UnityProvider.Container);
        }

		private static void RegisterTypes(IUnityContainer container)
		{
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

