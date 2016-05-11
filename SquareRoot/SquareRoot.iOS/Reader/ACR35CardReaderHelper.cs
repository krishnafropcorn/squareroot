using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using AudioToolbox;
using AVFoundation;
using CardReader;
using CardReader.Interfaces;
using Foundation;
using ACR35.sdk.iOSBindings;

namespace SquareRoot.iOS.Reader
{
    public class Acr35CardReaderHelper : NSObject, ICardReaderHelper, IACRAudioJackReaderDelegate
    {
        private ACRAudioJackReader jackReader;
        private TaskCompletionSource<string> _uuidTcs;
        private TaskCompletionSource<MagStripeResultBase> _magTcs;
        private volatile bool _isReadingWritingNfc;
        private volatile bool _isNfcMode;
        private volatile bool _isMagMode;
        private volatile bool _keepLoopActive;
        private volatile bool _powerOffRequested;
        private NSTimer _timer, _magTimer;
        private byte[] _magStripeEncryptionKey;

        public Acr35CardReaderHelper(IReaderConnectionListener connectionListener, IHeadsetVolumeHelper volumeHelper)
        {
            VolumeHelper = volumeHelper;
            ConnectionListener = connectionListener;

            jackReader = new ACRAudioJackReader(true);
            jackReader.WeakDelegate = this;

            NSError error = AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.PlayAndRecord);
            Debug.WriteLine($"Setting category error: {(error?.Description ?? "ERROR NULL")}");
            AudioSessionErrors err = AudioSession.AddListener(AudioSessionProperty.AudioRouteChange, PropertyListener);
            Debug.WriteLine("Add listener result: " + err);
        }

        public IHeadsetVolumeHelper VolumeHelper { get; }

        public IReaderConnectionListener ConnectionListener { get; }

        public bool IsReaderPlugged => AudioSession.AudioRoute == "HeadsetInOut";

        public async Task PowerOn()
        {
            _powerOffRequested = false;
            if (jackReader.Mute && IsReaderPlugged)
            {
                jackReader.Mute = false;
            }

            // call "reset" to power on the reader
            await jackReader.ResetWithCompletionAsync();
        }

        public void PowerOff()
        {
            // call "sleep" to power off the reader

            _powerOffRequested = true;

            if (_isNfcMode)
            {
                Debug.WriteLine("Is NFC mode in PowerOff: " + _isNfcMode);
                if (null != _timer)
                {
                    _timer.Invalidate();
                    _timer = null;
                }

                jackReader.PiccPowerOff();
                _uuidTcs.TrySetCanceled();
            }

            if (_isMagMode)
            {
                Debug.WriteLine("Is mag mode: " + _isMagMode);
                if (null != _magTimer)
                {
                    _magTimer.Invalidate();
                    _magTimer = null;
                }

                _magTcs.TrySetCanceled();
            }

            ThreadPool.QueueUserWorkItem(delegate
            {
                jackReader.Sleep();
                Debug.WriteLine("Powered off!");
            });
        }

        public async Task<MagStripeResultBase> ReadMagStripeAsync(byte[] key)
        {
            // power up the mag stripe reading
            // read something
            // power down mag stripe reading
            _isMagMode = true;
            _magStripeEncryptionKey = key;

            try
            {
                _magTcs = new TaskCompletionSource<MagStripeResultBase>();

                if (null != _magTimer)
                {
                    _magTimer.Invalidate();
                    _magTimer = null;
                }

                _magTimer = NSTimer.CreateRepeatingScheduledTimer(15,
                    (tmr) => ThreadPool.QueueUserWorkItem(delegate
                    {
                        jackReader.GetStatus();
                    }));

                return await _magTcs.Task;
            }
            finally
            {
                Debug.WriteLine("Will set isMagMode to false...");
                _isMagMode = false;
            }
        }

        public async Task<string> ReadUuidAsync(bool keepLoopActive = false)
        {
            Debug.WriteLine("ReadUuidASync!");
            try
            {
                _uuidTcs = new TaskCompletionSource<string>();
                _isNfcMode = true;

                // power up the nfc reader
                // read a single uuid
                // power down the nfc reader

                if (null == _timer)
                {
                    jackReader.PiccPowerOnWithTimeout(1u, (uint)ACRPiccCardType.ACRPiccCardTypeIso14443TypeA);
                    _timer = NSTimer.CreateRepeatingScheduledTimer(1.1, (tmr) =>
                    {

                        if (_powerOffRequested)
                        {
                            if (null != _timer)
                            {
                                _timer.Invalidate();
                                _timer = null;
                            }

                            							ThreadPool.QueueUserWorkItem(delegate {
                            							
                            								jackReader.PiccPowerOff();
                            								jackReader.Sleep();
                            
                            							});
						}
                        else
                        {
                            ThreadPool.QueueUserWorkItem(delegate
                            {

                                if (!_isReadingWritingNfc && !_powerOffRequested)
                                {
                                    Debug.WriteLine("Calling picc power on...");
                                                 jackReader.PiccPowerOnWithTimeout(1u, (uint)ACRPiccCardType.ACRPiccCardTypeIso14443TypeA);
                                } //end if
                            });
                        }
                    });
                }

                return await _uuidTcs.Task;
            }
            finally
            {
                Debug.WriteLine("Finally!");
                _isNfcMode = false;
            }
        }

        private void PropertyListener(AudioSessionProperty prop, int size, IntPtr data)
        {
            Debug.WriteLine("Audio session property: " + prop);
            Debug.WriteLine("Audio route: " + AudioSession.AudioRoute);

            bool plugged = IsReaderPlugged;
            jackReader.Mute = !plugged;

            if (plugged)
                ConnectionListener.OnReaderConnected();
            else
                ConnectionListener.OnReaderDisconnected();

            if (!plugged)
            {
                if (_isNfcMode)
                {
                    if (null != _timer)
                    {
                        _timer.Invalidate();
                        _timer = null;
                    }

                    _uuidTcs.TrySetCanceled();
                }

                if (_isMagMode)
                {
                    if (null != _magTimer)
                    {
                        _magTimer.Invalidate();
                        _magTimer = null;
                    }

                    _magTcs.TrySetCanceled();
                }
            }
            else
            {
                //				if (null == volumeView)
                //				{
                //					volumeView = new MPVolumeView();
                //					volumeView.ShowsRouteButton = true;
                //					volumeView.ShowsVolumeSlider = true;
                //				}
                //
                //
                //				UISlider volumeSlider = null;
                //				ObjCRuntime.Class uiSliderClass = new ObjCRuntime.Class(typeof(UISlider));
                //				foreach (UIView eachView in volumeView.Subviews)
                //				{
                //					if (eachView.IsKindOfClass(uiSliderClass))
                //					{
                //						volumeSlider = eachView as UISlider;
                //						break;
                //					}
                //				}
                //
                //				if (null != volumeSlider)
                //				{
                //					volumeSlider.SetValue(1, true);
                //				}//end if
                VolumeHelper.SetVolume();
            }
        }

        #region IACRAudioJackReaderDelegate implementation

        [Foundation.Export("reader:didAuthenticate:")]
		public void ReaderDidAuthenticate(ACR35.sdk.iOSBindings.ACRAudioJackReader reader,
            System.nint errorCode)
        {
            Debug.WriteLine("ReaderDidAuthenticate");
        }

        [Foundation.Export("reader:didNotifyResult:")]
		public void ReaderDidNotifyResult(ACR35.sdk.iOSBindings.ACRAudioJackReader reader,
			ACR35.sdk.iOSBindings.ACRResult result)
        {
            Debug.WriteLine("ReaderDidNotifyResult: " + (ACRError) (uint) result.ErroCode);
            if (_powerOffRequested)
            {
                reader.Sleep();
            }
        }

        [Foundation.Export("readerDidNotifyTrackData:")]
		public void ReaderDidNotifyTrackData(ACR35.sdk.iOSBindings.ACRAudioJackReader reader)
        {
            Debug.WriteLine("ReaderDidNotifyTrackData");
        }

        [Foundation.Export("readerDidReset:")]
		public void ReaderDidReset(ACR35.sdk.iOSBindings.ACRAudioJackReader reader)
        {
            Debug.WriteLine("ReaderDidReset");
        }

        [Foundation.Export("reader:didSendCustomId:length:")]
        public void ReaderDidSendCustomId(ACR35.sdk.iOSBindings.ACRAudioJackReader reader,
            System.IntPtr customId, System.nuint length)
        {
            Debug.WriteLine("ReaderDidSendCustomId");
        }

        [Foundation.Export("reader:didSendDeviceId:length:")]
        public void ReaderDidSendDeviceId(ACR35.sdk.iOSBindings.ACRAudioJackReader reader,
            System.IntPtr deviceId, System.nuint length)
        {
            Debug.WriteLine("ReaderDidSendDeviceId");
        }

        public void ReaderDidSendDukptOption(ACR35.sdk.iOSBindings.ACRAudioJackReader reader, bool enabled)
        {
            Debug.WriteLine("ReaderDidSendDupktOption");
        }

        [Foundation.Export("reader:didSendFirmwareVersion:")]
        public void ReaderDidSendFirmwareVersion(ACR35.sdk.iOSBindings.ACRAudioJackReader reader,
            string firmwareVersion)
        {
            Debug.WriteLine("ReaderDidSendFirmwareVersion");
        }

        [Foundation.Export("reader:didSendPiccAtr:length:")]
        public void ReaderDidSendPiccAttr(ACR35.sdk.iOSBindings.ACRAudioJackReader reader, System.IntPtr attr,
            System.nuint length)
        {
            Debug.WriteLine("ReaderDidSendPiccAttr");

            if (!_isReadingWritingNfc)
            {
                _isReadingWritingNfc = true;

                byte[] commandApdu = new byte[] {0xFF, 0xCA, 0x00, 0x00, 0x00};
                GCHandle apduHandle = GCHandle.Alloc(commandApdu, GCHandleType.Pinned);
                jackReader.PiccTransmitWithTimeout(1u, apduHandle.AddrOfPinnedObject(), 5);
                apduHandle.Free();
            }
        }

        [Foundation.Export("reader:didSendPiccResponseApdu:length:")]
        public void ReaderDidSendPiccResponseApdu(ACR35.sdk.iOSBindings.ACRAudioJackReader reader,
            System.IntPtr responseApdu, System.nuint length)
        {
            Debug.WriteLine("ReaderDidSendPiccResponseApdu");
            if (_isReadingWritingNfc)
            {
                byte[] responseBuffer = new byte[(int) length];
                Marshal.Copy(responseApdu, responseBuffer, 0, responseBuffer.Length);

                if (!_keepLoopActive)
                {
                    if (null != _timer)
                    {
                        _timer.Invalidate();
                        _timer = null;
                    }
                }

                if (responseBuffer.SequenceEqual(ApduErrorBuffer))
                    _uuidTcs.TrySetException(new Exception("Error reading NFC ID."));
                else
                    _uuidTcs.TrySetResult(BitConverter.ToString(responseBuffer.Take(7).ToArray()).Replace("-", ":"));

                _isReadingWritingNfc = false;
            }
        }

        [Foundation.Export("reader:didSendRawData:length:")]
        public void ReaderDidSendRawData(ACR35.sdk.iOSBindings.ACRAudioJackReader reader,
            System.IntPtr rawData, System.nuint length)
        {
            Debug.WriteLine("ReaderDidSendRawData");
        }

        [Foundation.Export("reader:didSendStatus:")]
        public void ReaderDidSendStatus(ACR35.sdk.iOSBindings.ACRAudioJackReader reader,
            ACR35.sdk.iOSBindings.ACRStatus status)
        {
            Debug.WriteLine("ReaderDidSendStatus - BatteryLevel: " + status.BatteryLevel + ", SleepTimeout: " +
                            status.SleepTimeout);
        }

        [Foundation.Export("reader:didSendTrackData:")]
        public void ReaderDidSendTrackData(ACR35.sdk.iOSBindings.ACRAudioJackReader reader,
            ACR35.sdk.iOSBindings.ACRTrackData trackData)
        {
            Debug.WriteLine("ReaderDidSendTrackData");

            if (trackData.Track1ErrorCode != (uint) ACRTrackError.ACRTrackErrorSuccess ||
                trackData.Track2ErrorCode != (uint) ACRTrackError.ACRTrackErrorSuccess)
            {
                _magTcs.TrySetResult(new MagStripeResultSecure()
                {
                    Succeeded = false,
                    Error = new Exception("Error reading mag stripe - Track 1: " +
                                          (ACRTrackError) (uint) trackData.Track1ErrorCode + ". Track 2: " +
                                          (ACRTrackError) (uint) trackData.Track2ErrorCode)
                });
                return;
            }

            ACRAesTrackData aesTrackData = trackData as ACRAesTrackData;

            GCHandle arrayHandle = default(GCHandle);
            try
            {
                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    aes.BlockSize = 128;
                    aes.KeySize = 128;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.None;
                    aes.Key = _magStripeEncryptionKey;
                    aes.IV = new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

                    using (System.Security.Cryptography.ICryptoTransform cryptoTransform = aes.CreateDecryptor())
                    using (MemoryStream ms = new MemoryStream(aesTrackData.TrackData.ToArray()))
                    using (
                        System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(
                            ms, cryptoTransform, System.Security.Cryptography.CryptoStreamMode.Read))
                    {
                        byte[] decryptedData = new byte[trackData.Track1Length];
                        cs.Read(decryptedData, 0, decryptedData.Length);

                        arrayHandle = GCHandle.Alloc(decryptedData, GCHandleType.Pinned);

                        using (
                            ACRTrack1Data track1Data = new ACRTrack1Data(arrayHandle.AddrOfPinnedObject(),
                                (nuint) decryptedData.Length))
                        {
                            MagStripeResultSecure secureMagStripeResult = new MagStripeResultSecure()
                            {
                                Succeeded = true,
                                ExpirationDate = track1Data.ExpirationDate,
                                DiscretionaryData = track1Data.DiscretionaryData,
                                Jis2Data = track1Data.Jis2Data,
                                Name = track1Data.Name,
                                ServiceCode = track1Data.ServiceCode,
                            };
                       
//							secureMagStripeResult.CardProvider =
//                                StripeService.GetCreditCardProvider(track1Data.PrimaryAccountNumber);
                            
							secureMagStripeResult.CardLast4 =
                                track1Data.PrimaryAccountNumber.Substring(track1Data.PrimaryAccountNumber.Length - 4, 4);
                            secureMagStripeResult.PrimaryAccountNumberSecure = track1Data.PrimaryAccountNumber;

                            _magTcs.TrySetResult(secureMagStripeResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                if (null != _magTimer)
                {
                    _magTimer.Invalidate();
                    _magTimer = null;
                }

                arrayHandle.Free();
                trackData.Dispose();
            }
        }

        [Foundation.Export("reader:didSendTrackDataOption:")]
        public void ReaderDidSendTrackDataOption(ACR35.sdk.iOSBindings.ACRAudioJackReader reader,
            ACR35.sdk.iOSBindings.ACRTrackDataOption option)
        {
            Debug.WriteLine("ReaderDidSendTrackDataOption");
        }

        #endregion IACRAudioJackReaderDelegate implementation

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            AudioSession.RemoveListener(AudioSessionProperty.AudioRouteChange, PropertyListener);

            if (!disposing)
            {
                if (null != jackReader)
                {
                    jackReader.Mute = true;
                    jackReader.WeakDelegate = null;
                    jackReader.Dispose();
                    jackReader = null;
                }
            }

            base.Dispose(disposing);
        }

        ~Acr35CardReaderHelper()
        {
            Dispose(false);
        }

        private static readonly byte[] ApduErrorBuffer = new byte[] {0x63, 0x00};
    }
}