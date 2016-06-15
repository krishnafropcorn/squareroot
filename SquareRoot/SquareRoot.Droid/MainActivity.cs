using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Android.Content.Res;
using System.IO;
using IDTech.MSR.UniMag;

namespace SquareRoot.Droid
{
	[Activity(Label = "SquareRoot", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
		private static Context _applicationContent;

		protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			_applicationContent = Application.ApplicationContext;

            UnityConfig.Initialize();

			AssetManager assets = this.Assets;
			string content;
			using (StreamReader sr = new StreamReader (assets.Open ("idt_unimagcfg_default.xml")))
			{
				content = sr.ReadToEnd ();
			}

			var documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, "idt_unimagcfg_default.xml");
			System.IO.File.WriteAllText (filePath, content);

			//var _uniMagReader = new UniMagReader(new UnimagCardReaderHelper(), Application.ApplicationContext, UniMagReader.ReaderType.UmOrPro);

			//_uniMagReader.RegisterListen();

			//_uniMagReader.SetTimeoutOfSwipeCard(0);

			//_uniMagReader.SetSaveLogEnable(false);

			//_uniMagReader.SetXMLFileNameWithPath(filePath);

			//_uniMagReader.SetVerboseLoggingEnable(true);

			//_uniMagReader.LoadingConfigurationXMLFile(false);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

		public static Context GetApplicationContext()
		{
			return _applicationContent;
		}
    }
}

