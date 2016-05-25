using CardReader.Interfaces;
using System;
using UniMag.Sdk.Bindings.iOS;
using Foundation;
using Common;

namespace SquareRoot.iOS.Reader
{
    public class UnimagCardReaderHelper : ICardReaderHelper
    {
        private uniMag _reader;
        private bool _areCallbacksRegistered;
        private Action _onCreditCardSwiped;

        public bool IsReaderPlugged { get; private set; }

        public CardDetails CreditCardDetails { get; private set; }

        public void StopListening()
        {
            IsReaderPlugged = false;
            CreditCardDetails = null;
        }

        public void StartListening(Action onCreditCardSwiped)
        {
            this._onCreditCardSwiped = onCreditCardSwiped;
            uniMag.EnableLogging(true);
            _reader = new uniMag();
            _reader.Init();
            _reader.SetAutoConnect(true);
            _reader.SetSwipeTimeoutDuration(0);
            _reader.SetAutoAdjustVolume(true);

            var center = NSNotificationCenter.DefaultCenter;
            center.AddObserver(new NSString("uniMagAttachmentNotification"), umDevice_attachment);
        }

        //called when uniMag is physically attached
        private void umDevice_attachment(NSNotification notification)
        {
            RegisterCallbacks();
            UmRet currentStatus = _reader.StartUniMag(true);
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
                NSObject data = notification.Object;
                CreditCardDetails = new CardDetails(data.ToString());
                _onCreditCardSwiped();
            }
            catch (Exception ex)
            {
                UniMagAlert.ShowAlert("Error while parsing data", ex.Message);
            }
        }

        //called when the SDK hasn't received a swipe from the device within a configured
        // "swipe timeout interval".
        private void umSwipe_timeout(NSNotification notification)
        {
            UniMagAlert.ShowAlert("Tired of waiting", "We did not receive a swipe in time. Ideally, we should receive this message since we have set timeout to infinite");
        }

        private void uniMag_deactivate()
        {
            UmRet currentStatus = UmRet.NotConnected;
            if (_reader != null && _reader.ConnectionStatus)
                currentStatus = _reader.StartUniMag(false);
            if (currentStatus == UmRet.NotConnected)
                StopListening();
        }

        //called when SDK failed to handshake with reader in time. ie, the connection task has timed out
        private void umConnection_timeout(NSNotification notification)
        {
            StopListening();
            UniMagAlert.ShowAlert("Error", "We could not connect to reader in time. Please reinsert");
        }

        ////called when the connection task is successful. SDK's connection state changes to true
        private void umConnection_connected(NSNotification notification)
        {
            IsReaderPlugged = true;
        }

        // wait for a swipe to be made
        private void umSwipe_starting(NSNotification notification)
        {
        }

        //called when umSwipe_invalid
        private void umSwipe_invalid(NSNotification notification)
        {
            UniMagAlert.ShowAlert("Error", "Invaild swipe, please try again.");
        }

        //called when attempting to start the connection task but iDevice's headphone playback volume is too low
        private void umConnection_lowVolume(NSNotification notification)
        {
            StopListening();
            UniMagAlert.ShowAlert("Volume too low", "Increase volume of your device and reinsert the reader.");
        }

        //called when umSystemMessage
        private void umSystemMessage(NSNotification notification)
        {
        }

        //called when umConnection_disconnected
        private void umConnection_disconnected(NSNotification notification)
        {
            StopListening();
        }

        //called whenumDevice_detachment
        private void umDevice_detachment(NSNotification notification)
        {
            StopListening();
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
            StopListening();
            UniMagAlert.ShowAlert("Error", "We could not connect to reader in time. Please reinsert");
        }

        //called when SDK successfully received a response to a command
        private void umCommand_receivedResponse(NSNotification notification)
        {
        }

        private void RegisterCallbacks()
        {
            if (_areCallbacksRegistered)
                return;
            _areCallbacksRegistered = true;

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

        private void UnregisterCallbacks()
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
}
