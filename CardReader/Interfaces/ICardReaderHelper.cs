using System.Threading.Tasks;
using System;
using Common;

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