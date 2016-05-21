using CardReader.Interfaces;
using Payment;
using Xamarin.Forms;
using SquareRoot.iOS.Reader;
using System;
using UniMag.Sdk.Bindings.iOS;
using Foundation;
using UIKit;
using CoreGraphics;
using CardReader;
using Common;

namespace SquareRoot.iOS.Reader
{
    public class UnimagCardReaderHelper : ICardReaderHelper
    {
        uniMag reader;

        public bool IsReaderPlugged { get; private set; }

        private Action<string> onCreditCardSwiped;

        public CardDetails CreditCardDetails { get; private set; }

        public void StopListening()
        {
            IsReaderPlugged = false;
            CreditCardDetails = null;

            // ToDo: Detach from listeners
        }

        public void StartListening(Action<string> onCreditCardSwiped)
        {

            this.onCreditCardSwiped = onCreditCardSwiped;
            uniMag.EnableLogging(true);
            reader = new uniMag();
            reader.Init();
            reader.SetAutoConnect(true);
            reader.SetSwipeTimeoutDuration(0);
            reader.SetAutoAdjustVolume(true);

            var center = NSNotificationCenter.DefaultCenter;
            center.AddObserver(new NSString("uniMagAttachmentNotification"), umDevice_attachment);
            // uniMag_activate();
        }

        //called when uniMag is physically attached
        private void umDevice_attachment(NSNotification notification)
        {
            uniMag_activate();
        }

        void uniMag_activate()
        {
            UmRet currentStatus = reader.StartUniMag(true);
            if (currentStatus == UmRet.Success)
            {
                uniMag_registerObservers(true);
                displayDeviceStatus(currentStatus);
            }
        }

        //called when the SDK has read something from the uniMag device
        // (eg a swipe, a response to a command) and is in the process of decoding it
        // Use this to provide an early feedback on the UI
        private void umDataProcessing(NSNotification notification)
        {

        }

        //called when SDK received a swipe successfully
        private void umSwipe_receivedSwipe(NSNotification notification)
        {
            try
            {
                var data = notification.Object;

//                B4342570993000729^JURANEK/ THOMAS           ^19052010000000      00207000000?;4342570993000729=19052010000000207?
//               %B4485240000223063^JURANEK/SCOTT             ^2005201000002200000000546000000?;4485240000223063=20052010000054622000?\r";
                CardDetails objCardDetails = new CardDetails();
                string tempData = data.ToString();
                string[] dataSubstrings = tempData.Split('^');
                if(dataSubstrings.Length>2)
                {
                    string[] nameValues = dataSubstrings[1].Split('/');
                    if(nameValues.Length>=2)
                    {
                        objCardDetails.CardLastName=nameValues[0].Trim();
                        objCardDetails.CardFirstName=nameValues[1].Trim();
                    }

                    string[] cardDetails = dataSubstrings[2].Split(';');
                    if (cardDetails.Length >= 2)
                    {
                        string[] cardNumbers = cardDetails[1].Split('=');
                        if (cardNumbers.Length >= 1)
                        {
                            objCardDetails.CreditCardNumber = cardNumbers[0].Trim();
                        }
                        objCardDetails.CardExpiryYear = Convert.ToInt32(cardDetails[0].Substring(0,2));
                        objCardDetails.CardExpiryMonth = Convert.ToInt32(cardDetails[0].Substring(2,2));
                    }
                }

                CreditCardDetails = objCardDetails;
                UniMagAlert.ShowAlert("Credit card details", tempData);
                this.onCreditCardSwiped(tempData);//This will rise the  StartListening event in the ContentPage
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //called when the SDK hasn't received a swipe from the device within a configured
        // "swipe timeout interval".
        private void umSwipe_timeout(NSNotification notification)
        {
        }

        void uniMag_deactivate()
        {
            UmRet currentStatus = UmRet.NotConnected;
            if (reader != null && reader.ConnectionStatus)
            {
                currentStatus = reader.StartUniMag(false);
            }
            uniMag_registerObservers(false);
            displayDeviceStatus(currentStatus);
        }

        //called when SDK failed to handshake with reader in time. ie, the connection task has timed out
        private void umConnection_timeout(NSNotification notification)
        {
            //connect again
            UmRet swipeStatus = this.reader.RequestSwipe;
            displayDeviceStatus(swipeStatus);
        }

        ////called when the connection task is successful. SDK's connection state changes to true
        private void umConnection_connected(NSNotification notification)
        {
            UmRet swipeStatus = this.reader.RequestSwipe;
            displayDeviceStatus(swipeStatus);
        }

        // wait for a swipe to be made
        private void umSwipe_starting(NSNotification notification)
        {
            UniMagAlert.ShowAlert("Info", "Waiting for card swipe...");
        }

        //called when umSwipe_invalid
        private void umSwipe_invalid(NSNotification notification)
        {
            //swipe again alert
            displaySwipeStatus(this.reader.RequestSwipe);
        }

        //called when attempting to start the connection task but iDevice's headphone playback volume is too low
        private void umConnection_lowVolume(NSNotification notification)
        {
        }

        //called when umSystemMessage
        private void umSystemMessage(NSNotification notification)
        {
        }

        //called when umConnection_disconnected
        private void umConnection_disconnected(NSNotification notification)
        {
            //update status
            UmRet swipeStatus = this.reader.RequestSwipe;
            displayDeviceStatus(swipeStatus);
        }

        //called whenumDevice_detachment
        private void umDevice_detachment(NSNotification notification)
        {
//            UmRet swipeStatus = this.reader.RequestSwipe;
//            displayDeviceStatus(swipeStatus);
        }

        //called when successfully starting the connection task
        private void umConnection_starting(NSNotification notification)
        {
        }

        //called when SDK successfully starts to send a command. SDK starts the command
        // task
        private void umCommand_starting(NSNotification notification)
        {
        }

        //called when SDK failed to receive a command response within a configured
        // "command timeout interval"
        private void umCommand_timeout(NSNotification notification)
        {
        }

        //called when SDK successfully received a response to a command
        private void umCommand_receivedResponse(NSNotification notification)
        {
        }

        private void uniMag_registerObservers(bool registerObservers)
        {
            if (registerObservers)
            {
                var center = NSNotificationCenter.DefaultCenter;
                //center.AddObserver(new NSString("uniMagAttachmentNotification"), umDevice_attachment);
                center.AddObserver(new NSString("uniMagDetachmentNotification"), umDevice_detachment);
                center.AddObserver(new NSString("uniMagInsufficientPowerNotification"), umConnection_lowVolume);
                center.AddObserver(new NSString("uniMagTimeoutNotification"), umConnection_timeout);
                center.AddObserver(new NSString("uniMagDidConnectNotification"), umConnection_connected);
                center.AddObserver(new NSString("uniMagDidDisconnectNotification"), umConnection_disconnected);
                center.AddObserver(new NSString("uniMagSwipeNotification"), umSwipe_starting);
                center.AddObserver(new NSString("uniMagTimeoutSwipeNotification"), umSwipe_timeout);
                center.AddObserver(new NSString("uniMagDataProcessingNotification"), umDataProcessing);
                center.AddObserver(new NSString("uniMagInvalidSwipeNotification"), umSwipe_invalid);
                center.AddObserver(new NSString("uniMagDidReceiveDataNotification"), umSwipe_receivedSwipe);
                center.AddObserver(new NSString("uniMagCmdSendingNotification"), umCommand_starting);
                center.AddObserver(new NSString("uniMagCommandTimeoutNotification"), umCommand_timeout);
                center.AddObserver(new NSString("uniMagDidReceiveCmdNotification"), umCommand_receivedResponse);
                center.AddObserver(new NSString("uniMagSystemMessageNotification"), umSystemMessage);
            }
            else
            {
                var center = NSNotificationCenter.DefaultCenter;
                center.RemoveObserver(new NSString("uniMagAttachmentNotification"));
                center.RemoveObserver(new NSString("uniMagDetachmentNotification"));
                center.RemoveObserver(new NSString("uniMagInsufficientPowerNotification"));
                center.RemoveObserver(new NSString("uniMagTimeoutNotification"));
                center.RemoveObserver(new NSString("uniMagDidConnectNotification"));
                center.RemoveObserver(new NSString("uniMagDidDisconnectNotification"));
                center.RemoveObserver(new NSString("uniMagSwipeNotification"));
                center.RemoveObserver(new NSString("uniMagTimeoutSwipeNotification"));
                center.RemoveObserver(new NSString("uniMagDataProcessingNotification"));
                center.RemoveObserver(new NSString("uniMagInvalidSwipeNotification"));
                center.RemoveObserver(new NSString("uniMagDidReceiveDataNotification"));
                center.RemoveObserver(new NSString("uniMagCmdSendingNotification"));
                center.RemoveObserver(new NSString("uniMagCommandTimeoutNotification"));
                center.RemoveObserver(new NSString("uniMagDidReceiveCmdNotification"));
                center.RemoveObserver(new NSString("uniMagSystemMessageNotification"));
            }
        }

        void displayDeviceStatus(UmRet swipeStatus)
        {
            switch (swipeStatus)
            {
                case UmRet.Success: 
                case UmRet.AlreadyConnected:
                    IsReaderPlugged = true;
                    UniMagAlert.ShowAlert("Info", "Reader is connected.");
                    break;
                    //handle other cases
                default : 
                    StopListening();
                    UniMagAlert.ShowAlert("Info", "Reader not connected, please try reinserting the reader firmly.");
                    break;
            }
        }

        void displaySwipeStatus(UmRet swipeStatus)
        {
            switch (swipeStatus)
            {
                case UmRet.Success: 
                    UniMagAlert.ShowAlert("Info", "Swipe Success.");
                    break;
                    //handle other cases
                default : 
                    CreditCardDetails = null;
                    UniMagAlert.ShowAlert("Info", "Invaild swipe, please try again.");
                    break;
            }
        }

        void determineNextStep()
        {
            // this is where we shuttle the data
        }
        //Our methods
        //        private void onCardDetailsReceived(string cardDetails)
        //        {
        //        }
    }
}
