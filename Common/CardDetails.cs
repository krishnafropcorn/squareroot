using System;

namespace Common
{
	public class CardDetails
	{
		public int CardExpiryMonth {
			get;
			set;
		}

		public int CardExpiryYear {
			get;
			set;
		}

		public string CardFirstName {
			get;
			set;
		}

		public string CardLastName {
			get;
			set;
		}

		public string CardRawNameCaptured {
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

		public CardDetails (string data)
		{
			string tempData = data.ToString ();
			string[] dataSubstrings = tempData.Split ('^');
			if (dataSubstrings.Length > 2) {
				string[] nameValues = dataSubstrings [1].Split ('/');
				if (nameValues.Length >= 2) {
					CardLastName = nameValues [0].Trim ();
					CardFirstName = nameValues [1].Trim ();
				}

				string[] cardDetails = dataSubstrings [2].Split (';');
				if (cardDetails.Length >= 2) {
					string[] cardNumbers = cardDetails [1].Split ('=');
					if (cardNumbers.Length >= 1)
						CreditCardNumber = cardNumbers [0].Trim ();

					CardExpiryYear = Convert.ToInt32 (cardDetails [0].Substring (0, 2));
					CardExpiryMonth = Convert.ToInt32 (cardDetails [0].Substring (2, 2));
				}
			}
		}
	}

	public class CardToken
	{

		public string ProviderTokenId {
			get;
			set;
		}

		public string ProviderCardFingerprint {
			get;
			set;
		}

		public string ProviderCardId {
			get;
			set;
		}

		public string CardBrand {
			get;
			set;
		}

		public string CardLast4 {
			get;
			set;
		}
	}

	public class ChargeCardResponse
	{
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