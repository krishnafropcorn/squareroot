using CardReader.Interfaces;
using Payment;
using IDTech.MSR.UniMag;
using Android.Util;
using IDTech.MSR.XMLManager;
using Java.Lang;
using Java.Util.Regex;
using Android.OS;
using System;
using Common;


namespace SquareRoot.Droid
{
	class UnimagCardReaderHelper : ICardReaderHelper,IUniMagReaderMsg
	{
		public bool IsReaderPlugged { get; private set; }

		public IntPtr Handle{ get; set;}

		UniMagReader UniMagReader;

		public CardDetails CreditCardDetails { get; private set; }

		Action OnCreditCardSwiped;

		public void StartListening(Action OnCreditCardSwiped)
		{
			IsReaderPlugged = false;
			CreditCardDetails = null;
			UniMagReader = new UniMagReader (this, MainActivity.GetApplicationContext(), UniMagReader.ReaderType.UmOrPro);
			UniMagReader.SetSaveLogEnable(false);
			UniMagReader.SetXMLFileNameWithPath(null);
			//UniMagReader.LoadingConfigurationXMLFile(true);
			this.OnCreditCardSwiped = OnCreditCardSwiped;
			//myUniMagReader.setVerboseLoggingEnable(true);
			UniMagReader.RegisterListen();
			// ToDo: Attach to listeners
		}

		public void StopListening()
		{
			IsReaderPlugged = false;
			CreditCardDetails = null;
			UniMagReader.StopSwipeCard();
			UniMagReader.UnregisterListen();
			UniMagReader.Release();
			// ToDo: Detach from listeners
		}

		public void OnReceiveMsgAutoConfigProgress (int p0, double p1, string p2)
		{
			Log.Debug("UniMag", "OnReceiveMsgAutoConfigProgress");
		}

		public bool GetUserGrant (int arg0, string arg1)
		{
			Log.Debug("UniMag", "getUserGrant -- " + arg1);
			return true;
		}

		public void OnReceiveMsgAutoConfigProgress(int arg0) {
			// TODO Auto-generated method stub
			Log.Debug("UniMag", "OnReceiveMsgAutoConfigProgress");
		}

		public void OnReceiveMsgCardData(sbyte arg0, byte[] rawData) {
			Log.Debug ("UniMag", "OnReceiveMsgCardData");
			Log.Debug ("UniMag", "Successful swipe!");

			var rawSwipeData = new Java.Lang.String (rawData);
			string swipeData = rawSwipeData.ToString ();

			CreditCardDetails = new CardDetails (swipeData);
			Log.Debug ("UniMag", "SWIPE - " + swipeData);
			if (UniMagReader.IsSwipeCardRunning) {
				UniMagReader.StopSwipeCard ();
			}

			OnCreditCardSwiped ();
		}

		public void OnReceiveMsgProcessingCardData (){
		}

		public void OnReceiveMsgCommandResult(int arg0, byte[] arg1) {
			Log.Debug("UniMag", "OnReceiveMsgCommandResult");
		}

		public void OnReceiveMsgToCalibrateReader ()
		{
		}

		public void OnReceiveMsgConnected() {
			Log.Debug("UniMag", "OnReceiveMsgConnected");
			Log.Debug("UniMag", "Card reader is connected.");
		}

		public void OnReceiveMsgDisconnected() {
			Log.Debug("UniMag", "OnReceiveMsgDisconnected");
			if(UniMagReader.IsSwipeCardRunning) {
				UniMagReader.StopSwipeCard();
			}

			UniMagReader.Release();
		}

		public void OnReceiveMsgFailureInfo(int arg0, string arg1) {
			Log.Debug("UniMag","OnReceiveMsgFailureInfo -- " + arg1);
		}

		public void OnReceiveMsgSDCardDFailed(string arg0) {
			Log.Debug("UniMag", "OnReceiveMsgSDCardDFailed -- " + arg0);
		}

		public void OnReceiveMsgTimeout(string arg0) {
			Log.Debug("UniMag", "OnReceiveMsgTimeout -- " + arg0);
			Log.Debug("UniMag","Timed out!");
		}

		public void OnReceiveMsgToConnect() {
			Log.Debug("UniMag","Swiper Powered Up");
		}

		public void OnReceiveMsgToSwipeCard() {
			Log.Debug("UniMag","OnReceiveMsgToSwipeCard");      
		}

		public void OnReceiveMsgAutoConfigCompleted(StructConfigParameters arg0) {
			Log.Debug("UniMag", "OnReceiveMsgAutoConfigCompleted");
		}

		public void Dispose()
		{}
	}
}