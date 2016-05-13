using Microsoft.Practices.Unity;
using CardReader.Interfaces;
using Payment;

namespace SquareRoot.Droid
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
			container.RegisterType<ISecurityManager, SecurityManager>(new ContainerControlledLifetimeManager());
			container.RegisterType<ICardReaderHelper, UnimagCardReaderHelper>(new ContainerControlledLifetimeManager());
			container.RegisterType<IPaymentService, StripePaymentService>(new ContainerControlledLifetimeManager());
		}
	}
}

