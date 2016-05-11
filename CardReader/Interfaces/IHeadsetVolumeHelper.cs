namespace CardReader.Interfaces
{
    public interface IHeadsetVolumeHelper
    {
        double GetCurrentVolume();

        void SetVolume(double volume = 1d);
    }
}
