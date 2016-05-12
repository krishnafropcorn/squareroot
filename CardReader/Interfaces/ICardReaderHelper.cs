using System.Threading.Tasks;
using Payment;

namespace CardReader.Interfaces
{
    public interface ICardReaderHelper
    {
        bool IsReaderPlugged { get; }

		CardDetails CreditCardDetails { get; }

		void StartListening();

		void StopListening();
    }
}