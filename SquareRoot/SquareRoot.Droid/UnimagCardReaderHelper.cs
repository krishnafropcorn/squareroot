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
using System.IO;


namespace SquareRoot.Droid
{
	class UnimagCardReaderHelper : ICardReaderHelper,IUniMagReaderMsg
	{
		public bool IsReaderPlugged { get; private set; }

		public IntPtr Handle{ get; set;}

		UniMagReader _uniMagReader;

		public CardDetails CreditCardDetails { get; private set; }

		Action OnCreditCardSwiped;

		public void StartListening(Action OnCreditCardSwiped)
		{
            this.OnCreditCardSwiped = OnCreditCardSwiped;

            _uniMagReader = new UniMagReader(this, MainActivity.GetApplicationContext(), UniMagReader.ReaderType.UmOrPro);

            _uniMagReader.RegisterListen();

            _uniMagReader.SetTimeoutOfSwipeCard(0);

			_uniMagReader.SetSaveLogEnable(false);

			var documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, "idt_unimagcfg_default.xml");

			_uniMagReader.SetXMLFileNameWithPath(filePath);

			_uniMagReader.SetVerboseLoggingEnable(true);

            _uniMagReader.LoadingConfigurationXMLFile(true);

		}

		public void StopListening()
		{
			IsReaderPlugged = false;
			CreditCardDetails = null;
			_uniMagReader.StopSwipeCard();
			_uniMagReader.UnregisterListen();
			_uniMagReader.Release();
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
			if (_uniMagReader.IsSwipeCardRunning) {
				_uniMagReader.StopSwipeCard ();
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
			if(_uniMagReader.IsSwipeCardRunning) {
				_uniMagReader.StopSwipeCard();
			}

			_uniMagReader.Release();
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