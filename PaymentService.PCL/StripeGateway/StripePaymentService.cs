using System;
using System.Threading.Tasks;
using Stripe;

namespace Payment.StripeGateway
{
    public class StripePaymentService: IPaymentService
    {
        public Task<CardDetails> TokenizeCard(string cardNumber, int expiryMonth, int expiryYear, string cvv, string firstName, string lastName,
            string cardRawNameCaptured)
        {
            StripeCreditCardOptions option = new StripeCreditCardOptions
            {
                Number = cardNumber,
                ExpirationMonth = expiryMonth.ToString(),
                ExpirationYear = expiryYear.ToString(),
                Cvc = cvv
            };

            TaskCompletionSource<CardDetails> taskCompletionSource = new TaskCompletionSource<CardDetails>();

            Task.Run(() =>
            {
                StripeToken cardToken =
                    (new StripeTokenService()).Create(new StripeTokenCreateOptions {Card = option});

                CardDetails toReturn = new CardDetails()
                {
                    CardBrand = cardToken.StripeCard.Brand,
					CardExpiryMonth = System.Int32.Parse(cardToken.StripeCard.ExpirationMonth),
					CardExpiryYear = System.Int32.Parse(cardToken.StripeCard.ExpirationYear),
                    CardFirstName = firstName,
                    CardLastName = lastName,
                    CardRawNameCaptured = cardRawNameCaptured,
                    ProviderCardFingerprint = "TEST_FINGERPRINT",
                    ProviderCardId = cardToken.StripeCard.AccountId,
                    ProviderToken = cardToken.Id,
                    CardLast4 = cardToken.StripeCard.Last4
                };

                taskCompletionSource.SetResult(toReturn);
            });

            return taskCompletionSource.Task;
        }

        public CreditCardProvider GetCreditCardProvider(string cardNumber)
        {
            throw new NotImplementedException();
        }

        public PaymentExceptionInfo GetPaymentExceptionInfo(Exception ex)
        {
            var stripeException = ex as StripeException;
            if (null != stripeException)
            {
                return new PaymentExceptionInfo()
                {
                    ErrorCode = stripeException.StripeError.DeclineCode,
                    ErrorParam = stripeException.StripeError.Parameter,
                    ErrorType = stripeException.StripeError.ErrorType
                };
            }

            return null;
        }
    }
}
