using System;
using CardReader.Interfaces;

namespace CardReader
{
	public class CardReaderConnectionListener : IReaderConnectionListener
	{
		public event Action<IReaderConnectionListener> ReaderConnected = (s) => {};
		public event Action<IReaderConnectionListener> ReaderDisconnected = (s) => {};

		public void OnReaderConnected ()
		{
			this.ReaderConnected(this);
		}

        public void OnReaderDisconnected ()
		{
			this.ReaderDisconnected(this);
		}
	}
}

