using System;
using System.Runtime.InteropServices;
using System.Security;
using CardReader;

namespace SquareRoot.iOS.Reader
{
    public class MagStripeResultSecure : MagStripeResultBase
    {
        private SecureString _primaryAccountNumberSecure;

        public override string PrimaryAccountNumberSecure
        {
            get
            {
                return GetPrimaryAccountNumberUnsecure();
            }
            set
            {
                SetPrimaryAccountNumberSecure(value);
            }
        }

        public override void DestroyPrimaryAccountNumber()
        {
            if (null != _primaryAccountNumberSecure)
            {
                _primaryAccountNumberSecure.Dispose();
                _primaryAccountNumberSecure = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != _primaryAccountNumberSecure)
                {
                    _primaryAccountNumberSecure.Dispose();
                    _primaryAccountNumberSecure = null;
                }
            }

            base.Dispose(disposing);
        }

        private string GetPrimaryAccountNumberUnsecure()
        {
            if (null == _primaryAccountNumberSecure)
                return string.Empty;

            IntPtr unmanagedString = IntPtr.Zero;

            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(_primaryAccountNumberSecure);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        private void SetPrimaryAccountNumberSecure(string text)
        {
            if (null != _primaryAccountNumberSecure)
                throw new InvalidOperationException("Already set!");

            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text), "Argument is empty or null.");

            unsafe
            {
                fixed (char* textChars = text)
                {
                    _primaryAccountNumberSecure = new SecureString(textChars, text.Length);
                    _primaryAccountNumberSecure.MakeReadOnly();
                }
            }
        }
    }
}
