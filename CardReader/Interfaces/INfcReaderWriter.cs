using System;
using System.Threading.Tasks;

namespace CardReader.Interfaces
{
    public interface INfcReaderWriter
    {
		bool IsReaderConnected { get; }

        IReaderConnectionListener ConnectionListener { get; }

        Task<string> ReadUuidAsync();

        void StartReadingMultipleUuids(Func<string, Exception, Task> callback);

        void StopReadingMultipleUuids();
    }
}
