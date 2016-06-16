using System;
using System.Collections.Generic;
using System.Linq;

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

		public string CardFullName {
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
			string[] dataSubstrings = data.Split ('^');
			if (dataSubstrings.Length >= 3) {
                CreditCardNumber = dataSubstrings[0].Substring(2);

                if (!dataSubstrings[1].Contains("/"))
				{
					CardFullName = dataSubstrings[1].Substring(0, dataSubstrings[1].Length - 2);
				}
				else {
					string[] nameValues = dataSubstrings[1].Split('/');
					CardFullName = nameValues[1].Trim() + nameValues[0].Trim();
				}

                CardFullName = CardFullName.Trim ();

				CardExpiryYear = Convert.ToInt32(dataSubstrings[2].Substring(0, 2));
				CardExpiryMonth = Convert.ToInt32(dataSubstrings[2].Substring(2, 2));
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