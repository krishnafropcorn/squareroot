using IDTech.MSR.UniMag;
using Android.Util;
using IDTech.MSR.XMLManager;
using System;
using Common;
using System.IO;
using Android.App;
using CardReader.Interfaces;
using IDTech.MSR.UniMag.UniMagTools;

namespace SquareRoot.Droid
{
	public class UnimagCardReaderHelper : Java.Lang.Object, IUniMagReaderMsg, ICardReaderHelper, IUniMagReaderToolsMsg
	{
		public bool IsReaderPlugged { get; private set; }

		UniMagReader _uniMagReader;

		public CardDetails CreditCardDetails { get; private set; }

		private Action _onReaderStateChanged;

		public void StartListening(Action OnReaderStateChanged)
		{
            this._onReaderStateChanged = OnReaderStateChanged;

			_uniMagReader = new UniMagReader(this, Application.Context.ApplicationContext);

			_uniMagReader.SetVerboseLoggingEnable(true); 

			_uniMagReader.RegisterListen();

			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, "idt_unimagcfg_default.xml");

			_uniMagReader.SetXMLFileNameWithPath(filePath);
			_uniMagReader.LoadingConfigurationXMLFile(true);

			var firmwareUpdateTool = new UniMagSDKTools(this, Application.Context.ApplicationContext);
			firmwareUpdateTool.SetUniMagReader(_uniMagReader);
			_uniMagReader.SetSDKToolProxy(firmwareUpdateTool.SDKToolProxy);
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

			CreditCardDetails = new CardDetails (swipeData, true);
			Log.Debug ("UniMag", "SWIPE - " + swipeData);
			if (_uniMagReader.IsSwipeCardRunning) {
				_uniMagReader.StopSwipeCard ();
			}

			_onReaderStateChanged ();
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
			_uniMagReader.SetTimeoutOfSwipeCard(0);
			IsReaderPlugged = _uniMagReader.StartSwipeCard();
			_onReaderStateChanged();
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
			Log.Debug("UniMag","Reader powering up...");
		}

		public void OnReceiveMsgToSwipeCard() {
			Log.Debug("UniMag","OnReceiveMsgToSwipeCard");      
		}

		public void OnReceiveMsgAutoConfigCompleted(StructConfigParameters arg0) {
			Log.Debug("UniMag", "OnReceiveMsgAutoConfigCompleted");
		}

		public void OnReceiveMsgChallengeResult(int p0, byte[] p1)
		{
		}

		public void OnReceiveMsgUpdateFirmwareProgress(int p0)
		{
		}

		public void OnReceiveMsgUpdateFirmwareResult(int p0)
		{
		}
	}
}