using CardReader.Interfaces;
using Payment;
using IDTech.MSR.UniMag;
using Android.Util;
using IDTech.MSR.XMLManager;
using Java.Lang;
using Java.Util.Regex;
using Android.OS;


namespace SquareRoot.Droid
{
	class UnimagCardReaderHelper : ICardReaderHelper,IUniMagReaderMsg
    {
        public bool IsReaderPlugged { get; private set; }

		UniMagReader UniMagReader;
        
		public CardDetails CreditCardDetails { get; private set; }

		public void StartListening(Android.Content.Context Context)
        {
            IsReaderPlugged = false;
            CreditCardDetails = null;
			UniMagReader = new UniMagReader (this, Context);
			UniMagReader.SetSaveLogEnable(false);
			UniMagReader.SetXMLFileNameWithPath(null);
			UniMagReader.LoadingConfigurationXMLFile(true);
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

		public bool GetUserGrant (int arg0, string arg1)
		{
			Log.Debug("UniMag", "getUserGrant -- " + arg1);
			return true;
		}

		public void OnReceiveMsgAutoConfigProgress(int arg0) {
			// TODO Auto-generated method stub
			Log.Debug("UniMag", "OnReceiveMsgAutoConfigProgress");
		}

		public void OnReceiveMsgCardData(byte arg0, byte[] arg1) {
			Log.Debug("UniMag", "OnReceiveMsgCardData");
			Log.Debug("UniMag", "Successful swipe!");

			String strData = new String (arg1);
			Log.Debug("UniMag", "SWIPE - " + strData);
			if(UniMagReader.IsSwipeCardRunning) {
				UniMagReader.StopSwipeCard();
			}

			// Match the data we want.
			String pattern = "%B(\\d+)\\^([^\\^]+)\\^(\\d{4})";
			Log.Debug("UniMag", pattern);
			Java.Util.Regex.Pattern r = Java.Util.Regex.Pattern.Compile(pattern);
			Matcher m = r.Matcher(strData);
			string card = "";
			string name = "";
			string exp = "";
			string data = "";
			if(m.Find()) {
				for(int a = 0; a < m.GroupCount(); ++a) {
					Log.Debug("UniMag", a + " - "+m.Group(a));
				}
				card = m.Group(1);
				name = m.Group(2);
				exp = m.Group(3);

				data = "Data: " + name + " -- " + card + " -- " + exp;
				Log.Debug("UniMag", data);
			}

		}

		public void OnReceiveMsgCommandResult(int arg0, byte[] arg1) {
			Log.Debug("UniMag", "OnReceiveMsgCommandResult");
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
			
		public void OnReceiveMsgFailureInfo(int arg0, String arg1) {
			Log.Debug("UniMag","OnReceiveMsgFailureInfo -- " + arg1);
		}
		 
		public void OnReceiveMsgSDCardDFailed(String arg0) {
			Log.Debug("UniMag", "OnReceiveMsgSDCardDFailed -- " + arg0);
		}
		 
		public void OnReceiveMsgTimeout(String arg0) {
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
    }
}
