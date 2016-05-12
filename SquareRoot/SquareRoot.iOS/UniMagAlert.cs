using Foundation;
using UIKit;

namespace SquareRoot.iOS
{
    public class UniMagAlert : NSObject
    {
        public static void ShowAlert (string title, string message)
        {
            var alert = new UIAlertView (title, message, null, "OK");
            alert.Show ();
        }
    }
}
