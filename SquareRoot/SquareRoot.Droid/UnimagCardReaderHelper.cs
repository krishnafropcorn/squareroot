using CardReader.Interfaces;
using Payment;

namespace SquareRoot.Droid
{
    class UnimagCardReaderHelper : ICardReaderHelper
    {
        public bool IsReaderPlugged { get; private set; }

        public CardDetails CreditCardDetails { get; private set; }

        public void StartListening()
        {
            IsReaderPlugged = false;
            CreditCardDetails = null;

            // ToDo: Attach to listeners
        }

        public void StopListening()
        {
            IsReaderPlugged = false;
            CreditCardDetails = null;

            // ToDo: Detach from listeners
        }
    }
}
