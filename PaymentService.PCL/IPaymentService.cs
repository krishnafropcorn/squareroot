using System;
using System.Threading.Tasks;

namespace Payment
{
    public interface IPaymentService
    {
        Task<CardDetails> TokenizeCard(string cardNumber, int expiryMonth, int expiryYear, string cvv, string firstName, string lastName, string cardRawNameCaptured);

        CreditCardProvider GetCreditCardProvider(string cardNumber);

        PaymentExceptionInfo GetPaymentExceptionInfo(Exception ex);
    }
}
