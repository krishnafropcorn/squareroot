namespace Payment
{
    public class CardDetails
    {
//        public string ProviderToken
//        {
//            get;
//            set;
//        }
//
//        public string ProviderCardFingerprint
//        {
//            get;
//            set;
//        }

//        public string ProviderCardId
//        {
//            get;
//            set;
//        }
//
//        public string CardBrand
//        {
//            get;
//            set;
//        }

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

//        public string CardLast4
//        {
//            get;
//            set;
//        }

		public string CreditCardNumber {
			get;
			set;
		}

		public CreditCardProvider CreditCardProvider {
			get;
			set;
		}
    }
}