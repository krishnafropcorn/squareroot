using System.Threading.Tasks;

namespace CardReader.Interfaces
{
    public interface IMagStripeReader
    {
		IReaderConnectionListener ConnectionListener { get; }

        bool IsReaderConnected { get; }

        Task<MagStripeResultBase> ReadMagStripeAsync(byte[] key);

        void StopReading();
    }
}
