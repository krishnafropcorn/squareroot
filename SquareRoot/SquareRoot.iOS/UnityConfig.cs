using Microsoft.Practices.Unity;
using SquareRoot.iOS.Reader;
using CardReader.Interfaces;
using Payment;

namespace SquareRoot.iOS
{
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
			container.RegisterType<IPaymentService, StripePaymentService>(new ContainerControlledLifetimeManager());

			container.RegisterType<ICardReaderHelper, UnimagCardReaderHelper>(new ContainerControlledLifetimeManager());
		}
	}
}

