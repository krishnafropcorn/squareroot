using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using SquareRoot;
using SquareRoot.iOS;
using System.Diagnostics;
using UIKit;
using CoreGraphics;

[assembly: ExportRendererAttribute(typeof(NumberedEntry), typeof(NumberedEntryRender))]
namespace SquareRoot.iOS
{
    public class NumberedEntryRender : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, Control.Frame.Size.Width, 44.0f));

            toolbar.Items = new[]
                {
                    new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                    new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { Control.ResignFirstResponder(); })
                };

            this.Control.InputAccessoryView = toolbar;
        }
    }
}

