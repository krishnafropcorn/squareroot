using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using HockeyApp;
using Android.Content.Res;
using System.IO;

namespace SquareRoot.Droid
{
    [Activity(Label = "SquareRoot", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
		private static Context _applicationContent;

		protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			CrashManager.Register(this, "46cd80cc0e5d4a3fb523fd0fa5d64bd6");

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

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

		public static Context GetApplicationContext()
		{
			return _applicationContent;
		}
    }
}

