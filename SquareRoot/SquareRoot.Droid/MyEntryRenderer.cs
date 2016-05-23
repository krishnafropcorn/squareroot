using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using SquareRoot;
using SquareRoot.Droid;
using Android.Support.V4.Content;
using Android.Graphics.Drawables;

[assembly: ExportRenderer (typeof(NumberedEntry), typeof(MyEntryRenderer))]
namespace SquareRoot.Droid
{
	class MyEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);

			if (Control != null) {
                Control.SetBackgroundColor (global::Android.Graphics.Color.LightGray);
				//Control.Background = Control.Context.GetDrawable (Resource.Drawable.entryBorder);

                var sdk = global::Android.OS.Build.VERSION.SdkInt;
                var lollipop = global::Android.OS.BuildVersionCodes.Lollipop;
//                var drawable = ContextCompat.GetDrawable(Control.Context, Resource.Drawable.entryBorder);
//                Control.Background = drawable;
				Control.SetPadding (10, 10, 10, 10);

			}
		}
	}
}

