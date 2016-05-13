﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace SquareRoot.Droid
{
    [Activity(Label = "SquareRoot", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
		private static Context ApplicationContext;

		protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            UnityConfig.Initialize();
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

		public static Context GetApplicationContext()
		{
			return ApplicationContext;
		}
    }
}

