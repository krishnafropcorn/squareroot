using System.Threading.Tasks;
using Payment;
using System;

namespace CardReader.Interfaces
{
    public interface ICardReaderHelper
    {
        bool IsReaderPlugged { get; }

		CardDetails CreditCardDetails { get; }

		void StartListening(Action<string> OnCreditCardSwiped);

		void StopListening();
    }
}