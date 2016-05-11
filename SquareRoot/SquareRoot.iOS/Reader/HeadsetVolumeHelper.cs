using System;
using System.Collections.Generic;
using System.Text;
using AVFoundation;
using CardReader.Interfaces;
using MediaPlayer;
using UIKit;

namespace SquareRoot.iOS.Reader
{
    public class HeadsetVolumeHelper : IHeadsetVolumeHelper
    {
        private readonly Lazy<MPVolumeView> _lazyVolumeView;

        public HeadsetVolumeHelper()
        {
            _lazyVolumeView = new Lazy<MPVolumeView>(() => {

                MPVolumeView volumeView = new MPVolumeView();
                volumeView.ShowsRouteButton = true;
                volumeView.ShowsVolumeSlider = true;

                return volumeView;
            }); ;
        }

        public double GetCurrentVolume()
        {
            return AVAudioSession.SharedInstance().OutputVolume;
        }

        public void SetVolume(double volume = 1)
        {
            UISlider volumeSlider = null;
            ObjCRuntime.Class uiSliderClass = new ObjCRuntime.Class(typeof(UISlider));
            foreach (UIView eachView in _lazyVolumeView.Value.Subviews)
            {
                if (eachView.IsKindOfClass(uiSliderClass))
                {
                    volumeSlider = eachView as UISlider;
                    break;
                }
            }

            volumeSlider?.SetValue((float)volume, true);
        }
    }
}
