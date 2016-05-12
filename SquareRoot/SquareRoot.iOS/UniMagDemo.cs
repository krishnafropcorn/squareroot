using System;

using UIKit;
using UniMag.Sdk.Bindings.iOS;
using Foundation;
using CoreGraphics;

namespace SquareRoot.iOS
{
    public partial class UniMagDemo : UIViewController
    {
        public UniMagDemo()
            : base("UniMagDemo", null)
        {
        }
        uniMag reader;
        UILabel _lblText,_lblText1,_lblText2;

        //CALLED WHEN DETECTED
        private void Attached(NSNotification notification)
        {
            try
            {
                UniMagAlert.ShowAlert("Info", "Reader DETECTED");
                if (reader.ConnectionStatus == false)
                {
                    reader.StartUniMag(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //CALLED WHEN SWIPE READY
        private void SwipeReady(NSNotification notification)
        {
            UniMagAlert.ShowAlert("Info", "Ready For Swipe");
        }

        //CALLED WHEN READY READER
        private void Connected(NSNotification notification)
        {
            UniMagAlert.ShowAlert("Info", "Reader Connected");
        }

        // CALLED ON SWIPE
        private void DataProcess(NSNotification notification)
        {
            // USERINFO IS NULL
            UniMagAlert.ShowAlert("Info", "DataProcess");
            try
            {
                var data = notification.UserInfo;
                var data1 = notification.Object;
                var data2 = notification.Description;

                if(data!= null){
                    _lblText.Text = data.ToString();
                }else if(data1!= null){
                    _lblText1.Text = data1.ToString();
                }else if(data2 != null){
                    _lblText2.Text = data2.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //CALLED WHEN UnimagSystemMessage
        private void UnimagSystemMessage(NSNotification notification)
        {
            UniMagAlert.ShowAlert("Info", "UnimagSystemMessage");
        }

        //CALLED WHEN DataProcessNot
        private void DataProcessNot(NSNotification notification)
        {
            UniMagAlert.ShowAlert("Info", "DataProcessNot");
        }

        //CALLED WHEN InvalidSwipe
        private void InvalidSwipe(NSNotification notification)
        {
            UniMagAlert.ShowAlert("Info", "InvalidSwipe");
        }

        //CALLED WHEN SwipeNotification
        private void SwipeNotification(NSNotification notification)
        {
            UniMagAlert.ShowAlert("Info", "SwipeNotification");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _lblText = new UILabel(new CGRect(10,80,300,150)){ Font = UIFont.FromName("HelveticaNeue-Bold", 11f), TextColor = UIColor.Black, TextAlignment = UITextAlignment.Left, Lines = 2,LineBreakMode = UILineBreakMode.TailTruncation ,AdjustsFontSizeToFitWidth = true};
            _lblText1 = new UILabel(new CGRect(10,160,300,150)){ Font = UIFont.FromName("HelveticaNeue-Bold", 11f), TextColor = UIColor.Black, TextAlignment = UITextAlignment.Left, Lines = 2,LineBreakMode = UILineBreakMode.TailTruncation ,AdjustsFontSizeToFitWidth = true};
            _lblText2 = new UILabel(new CGRect(10,200,300,150)){ Font = UIFont.FromName("HelveticaNeue-Bold", 11f), TextColor = UIColor.Black, TextAlignment = UITextAlignment.Left, Lines = 2,LineBreakMode = UILineBreakMode.TailTruncation ,AdjustsFontSizeToFitWidth = true};

            var center = NSNotificationCenter.DefaultCenter;
            center.AddObserver(new NSString("uniMagAttachmentNotification"), Attached);
            center.AddObserver(new NSString("uniMagDidConnectNotification"), Connected);
            center.AddObserver(new NSString("uniMagSwipeNotification"), SwipeReady);
            center.AddObserver(new NSString("uniMagDidReceiveDataNotification"), DataProcess);
            center.AddObserver(new NSString("uniMagSystemMessageNotification"), UnimagSystemMessage);
            center.AddObserver(new NSString("uniMagDataProcessingNotification"), DataProcessNot);
            center.AddObserver(new NSString("uniMagInvalidSwipeNotification"), InvalidSwipe);
            center.AddObserver(new NSString("uniMagSwipeNotification"), SwipeNotification);

            var val = reader.RequestSwipe;
            UniMagAlert.ShowAlert("Info", val.ToString());

            View.AddSubview(_lblText);
            View.AddSubview(_lblText1);
            View.AddSubview(_lblText2);
            View.BackgroundColor = UIColor.Red;

            base.ViewDidLoad();

            reader = new uniMag();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}


