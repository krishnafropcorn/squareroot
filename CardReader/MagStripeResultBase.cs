using System;
using CardReader.Interfaces;

namespace CardReader
{
	public abstract class MagStripeResultBase : IMagStripeResult, IDisposable
	{
		public bool Succeeded { get; set; }

        public Exception Error { get; set; }

		public string ExpirationDate { get; set; }

        public string DiscretionaryData { get; set; }

        public string Jis2Data { get; set; }

        public string Name { get; set; }

        public string ServiceCode { get; set; }

		public string CardProvider { get; set; }

		public string FirstName { get; set; }

        public string LastName { get; set; }

		public string CardLast4
		{
			get;
			set;
		}

		public DateTime? ExpirationDateObj
		{
			get
			{
				if (!string.IsNullOrEmpty(this.ExpirationDate) && this.ExpirationDate.Length == 4)
				{
					string year = this.ExpirationDate.Substring(0, 2);
					string month = this.ExpirationDate.Substring(2, 2);

					return new DateTime(Convert.ToInt32(year) + 2000, Convert.ToInt32(month), 1);
				} else
				{
					return null;
				}
			}
		}

		public abstract string PrimaryAccountNumberSecure { get; set; }

        public abstract void DestroyPrimaryAccountNumber();

		public void Dispose ()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
		}

		~MagStripeResultBase()
		{
			Dispose(false);
		}
	}
}

