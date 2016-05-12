using System.Diagnostics;
using System.Threading.Tasks;
using CardReader.Interfaces;
using CardReader.Utils;

namespace CardReader
{
	public class CardReaderMagStripeReader : IMagStripeReader
	{
		private readonly ICardReaderHelper _cardReaderHelper;

        public CardReaderMagStripeReader(ICardReaderHelper cardReaderHelper, IReaderConnectionListener plugListener)
        {
            _cardReaderHelper = cardReaderHelper;
			this.ConnectionListener = plugListener;
        }

		public IReaderConnectionListener ConnectionListener { get; private set; }

		public bool IsReaderConnected {
			get{ return _cardReaderHelper.IsReaderPlugged;}
		}

	    public async Task<MagStripeResultBase> ReadMagStripeAsync(byte[] key)
        {
            await _cardReaderHelper.PowerOn();

            MagStripeResultBase result = await _cardReaderHelper.ReadMagStripeAsync(key);

			string firstName = null;
			string lastName = null;

			StringUtils.ParseMagneticStripeName(result.Name, out firstName, out lastName);
			result.FirstName = firstName;
			result.LastName = lastName;

            _cardReaderHelper.PowerOff();

            return result;
        }

		public void StopReading ()
		{
			Debug.WriteLine("StopReading in MagStripe!");
			this._cardReaderHelper.PowerOff();
		}
    }
}