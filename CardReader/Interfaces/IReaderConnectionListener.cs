using System;

namespace CardReader.Interfaces
{
	public interface IReaderConnectionListener
	{
		event Action<IReaderConnectionListener> ReaderConnected;

        event Action<IReaderConnectionListener> ReaderDisconnected;

        void OnReaderConnected();

        void OnReaderDisconnected();
	}
}

