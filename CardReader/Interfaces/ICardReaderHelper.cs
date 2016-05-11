using System.Threading.Tasks;

namespace CardReader.Interfaces
{
    public interface ICardReaderHelper
    {
        IHeadsetVolumeHelper VolumeHelper { get; }

        IReaderConnectionListener ConnectionListener { get; }

        bool IsReaderPlugged { get; }

        Task PowerOn();

        void PowerOff();

        Task<MagStripeResultBase> ReadMagStripeAsync(byte[] key);

        Task<string> ReadUuidAsync(bool keepLoopActive = false);
    }
}