namespace Common
{
    public class CardDetails
    {
        public int CardExpiryMonth
        {
            get;
            set;
        }

        public int CardExpiryYear
        {
            get;
            set;
        }

        public string CardFirstName
        {
            get;
            set;
        }

        public string CardLastName
        {
            get;
            set;
        }

        public string CardRawNameCaptured
        {
            get;
            set;
        }

		public string CreditCardNumber {
			get;
			set;
		}

		public string CVV {
			get;
			set;
		}

		public CreditCardProvider CreditCardProvider {
			get;
			set;
		}
    }

	public class CardToken {

		public string ProviderTokenId
        {
            get;
            set;
        }

        public string ProviderCardFingerprint
        {
            get;
            set;
        }

        public string ProviderCardId
        {
            get;
            set;
        }

        public string CardBrand
        {
            get;
            set;
        }

        public string CardLast4
        {
            get;
            set;
        }
	}

	public class ChargeCardResponse {
		public bool IsSuccessFull {
			get;
			set;
		}

		public string FailureMessage {
			get;
			set;
		}

		public string FailureCode {
			get;
			set;
		}
	}
}