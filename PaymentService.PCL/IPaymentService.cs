using System;
using System.Threading.Tasks;
using Common;

namespace Payment
{
    public interface IPaymentService
    {
		Task<ChargeCardResponse> ChargeCard (CardDetails cardDeatils, int charge);

		Task<CardToken> TokenizeCard (CardDetails cardDeatils);

        CreditCardProvider GetCreditCardProvider(string cardNumber);

        PaymentExceptionInfo GetPaymentExceptionInfo(Exception ex);
    }
}
