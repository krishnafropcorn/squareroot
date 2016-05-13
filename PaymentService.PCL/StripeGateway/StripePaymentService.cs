using System;
using System.Threading.Tasks;
using Stripe;
using Common;

namespace Payment
{
    public class StripePaymentService: IPaymentService
    {
		public Task<ChargeCardResponse> ChargeCard(CardDetails cardDeatils, int charge)
		{
			var myCharge = new StripeChargeCreateOptions();

			// always set these properties
			myCharge.Amount = charge;
			myCharge.Currency = "usd";

			// set this if you want to
			myCharge.Description = "Test charge";

			myCharge.SourceCard = new SourceCard
			{
				Number = cardDeatils.CreditCardNumber,
				ExpirationMonth = cardDeatils.CardExpiryMonth.ToString(),
				ExpirationYear = cardDeatils.CardExpiryYear.ToString(),
				Cvc = cardDeatils.CVV
			};

			TaskCompletionSource<ChargeCardResponse> taskCompletionSource = new TaskCompletionSource<ChargeCardResponse>();

			Task.Run(() =>
				{
					StripeCharge chargeResponse =
						(new StripeChargeService()).Create(myCharge);

					ChargeCardResponse toReturn = new ChargeCardResponse()
					{
						IsSuccessFull = string.IsNullOrEmpty(chargeResponse.FailureMessage),
						FailureMessage = chargeResponse.FailureMessage,
						FailureCode = chargeResponse.FailureCode
					};

					taskCompletionSource.SetResult(toReturn);
				});

			return taskCompletionSource.Task;
		}

		public Task<CardToken> TokenizeCard(CardDetails cardDeatils)
        {
            StripeCreditCardOptions option = new StripeCreditCardOptions
            {
				Number = cardDeatils.CreditCardNumber,
				ExpirationMonth = cardDeatils.CardExpiryMonth.ToString(),
				ExpirationYear = cardDeatils.CardExpiryYear.ToString(),
				Cvc = cardDeatils.CVV
            };

			TaskCompletionSource<CardToken> taskCompletionSource = new TaskCompletionSource<CardToken>();

            Task.Run(() =>
            {
                StripeToken cardToken =
                    (new StripeTokenService()).Create(new StripeTokenCreateOptions {Card = option});

					CardToken toReturn = new CardToken()
                {
                     CardBrand = cardToken.StripeCard.Brand,
                     ProviderCardFingerprint = "TEST_FINGERPRINT",
                     ProviderCardId = cardToken.StripeCard.AccountId,
                     ProviderTokenId = cardToken.Id,
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
