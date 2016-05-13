using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using SquareRoot;
using SquareRoot.Droid;

[assembly: ExportRenderer (typeof(MyEntry), typeof(MyEntryRenderer))]
namespace SquareRoot.Droid
{
	class MyEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);

			if (Control != null) {
				//Control.SetBackgroundColor (global::Android.Graphics.Color.LightGreen);

				Control.Background = Control.Context.GetDrawable (Resource.Drawable.entryBorder);
				Control.SetPadding (10, 10, 10, 10);

			}
		}
	}
}

