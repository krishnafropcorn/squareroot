using System;
using System.Collections.Generic;
using Xamarin.Forms;
using CardReader.Interfaces;
using System.Threading.Tasks;
using CardReader;
using Microsoft.Practices.Unity;

namespace SquareRoot
{
    public partial class SwipePage : ContentPage
    {
        private readonly IHeadsetVolumeHelper _volumeHelper;
        private readonly IMagStripeReader _magStripeReader;
        private readonly ISecurityManager _securityManager;
        private readonly IPlatformService _platformService;

        private bool _isReaderPlugged;

        public SwipePage ()
        {
            InitializeComponent ();

            _securityManager = UnityProvider.Container.Resolve<ISecurityManager>();
            _platformService = UnityProvider.Container.Resolve<IPlatformService>();
            _volumeHelper = UnityProvider.Container.Resolve<IHeadsetVolumeHelper>();
            _magStripeReader = UnityProvider.Container.Resolve<IMagStripeReader>();
        }

        protected override async void OnAppearing ()
        {
            base.OnAppearing ();

            if (_volumeHelper.GetCurrentVolume() < 1d)
                _volumeHelper.SetVolume();

            _magStripeReader.ConnectionListener.ReaderConnected += _magStripeReader_PlugListener_ReaderPlugged;
            _magStripeReader.ConnectionListener.ReaderDisconnected += _magStripeReader_PlugListener_ReaderUnPlugged;

            if (_magStripeReader.IsReaderConnected) {
                _isReaderPlugged = true;
                _platformService.RunInThreadPool (() => _platformService.InvokeOnMainThread (async () => await InitiateMagStripeReading ()));
            } else {
                _isReaderPlugged = false;
                await DisplayAlert ("Reader Unavailable", "Try connecting the reader first and turn on the app", "OK");
            }
        }

        private void OnAppSleep(App app)
        {
            _magStripeReader.ConnectionListener.ReaderConnected -= _magStripeReader_PlugListener_ReaderPlugged;
            _magStripeReader.ConnectionListener.ReaderDisconnected -= _magStripeReader_PlugListener_ReaderUnPlugged;

            _magStripeReader.StopReading ();
        }

        private async Task InitiateMagStripeReading(bool showProcessing = true)
        {
            while (true)
            {
                try
                {
                    //                  byte[] key = new byte[] { 
                    //                      0x4E, 
                    //                      0x61, 
                    //                      0x74, 
                    //                      0x68, 
                    //                      0x61, 
                    //                      0x6E, 
                    //                      0x2E, 
                    //                      0x4C, 
                    //                      0x69, 
                    //                      0x20, 
                    //                      0x54, 
                    //                      0x65, 
                    //                      0x64, 
                    //                      0x64,
                    //                      0x79, 
                    //                      0x20 
                    //                  };

                    MagStripeResultBase result = await _magStripeReader.ReadMagStripeAsync(Convert.FromBase64String(_securityManager.Acr35EncryptionKey));

                    if (result.Succeeded)
                    {
                        await DisplayAlert("Swipe Succeeded", "Swiping the card worked.", "OK");
                    } else
                    {
                        await DisplayAlert("Swipe Failed", "Try swiping the card again.", "OK");
                    }
                } catch (Exception)
                {
                    break;
                }
            }
        }

        private void _magStripeReader_PlugListener_ReaderPlugged (IReaderConnectionListener obj)
        {
            _isReaderPlugged = true;

            _platformService.RunInThreadPool(() => this._platformService.InvokeOnMainThread (async () => await InitiateMagStripeReading (false)));
        }

        private void _magStripeReader_PlugListener_ReaderUnPlugged (IReaderConnectionListener obj)
        {
            _isReaderPlugged = false;
        }
    }
}

