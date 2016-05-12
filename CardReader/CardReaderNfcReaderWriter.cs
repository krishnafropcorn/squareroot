using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CardReader.Interfaces;

namespace CardReader
{
	public class CardReaderNfcReaderWriter : INfcReaderWriter
    {
        private ICardReaderHelper _cardReaderHelper;

        // There should probably only ever be one of these, so access to this variable probably doesn't need to be threadsafe
        private volatile bool _readingUuids = false;

        public CardReaderNfcReaderWriter(ICardReaderHelper cardReaderHelper, IReaderConnectionListener plugListener)
        {
            _cardReaderHelper = cardReaderHelper;
			this.ConnectionListener = plugListener;
        }

		public IReaderConnectionListener ConnectionListener { get; private set;}

		public bool IsReaderConnected {
			get {
				return _cardReaderHelper.IsReaderPlugged; 
			}
		}

		public bool IsReadingUuiDs {
			get { 
				return _readingUuids;
			}
		}

	    public async Task<string> ReadUuidAsync()
        {
            await _cardReaderHelper.PowerOn();

            string result = await _cardReaderHelper.ReadUuidAsync();

			_cardReaderHelper.PowerOff();
			            
            return result;
        }

	    public async void StartReadingMultipleUuids(Func<string, Exception, Task> callback)
	    {
	        await _cardReaderHelper.PowerOn();
	        _readingUuids = true;

	        while (_readingUuids)
	        {
	            string result = string.Empty;
	            try
	            {
	                result = await _cardReaderHelper.ReadUuidAsync(_readingUuids);
	                Debug.WriteLine("Keep reading ids: " + _readingUuids);
	                if (_readingUuids)
	                {
	                    await callback(result, null);
	                }
	            }
	            catch (Exception ex)
	            {
	                callback(result, ex);
	            }
	        }
	    }

	    public void StopReadingMultipleUuids()
        {
			Debug.WriteLine("StopReadingMultipleUuids!");
            _readingUuids = false;
            _cardReaderHelper.PowerOff();
        }

		public void Clear()
		{
			_cardReaderHelper = null;
		}
    }
}
