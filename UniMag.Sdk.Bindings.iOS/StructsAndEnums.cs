using System;
using System.Runtime.InteropServices;
using Foundation;

namespace UniMag.Sdk.Bindings.iOS
{
    public enum UmReader : uint
    {
        Unknown,
        UnimagOriginal,
        UnimagPro,
        UnimagIi,
        Shuttle
    }

    static class CFunctions
    {
        // NSString * UmReader_lookup (UmReader c);
//        [DllImport ("__Internal")]
//          [Verify (PlatformInvoke)]
        static extern NSString UmReader_lookup (UmReader c);

        // NSString * UmRet_lookup (UmRet c);
//        [DllImport ("__Internal")]
//          [Verify (PlatformInvoke)]
        static extern NSString UmRet_lookup (UmRet c);
    }

    public enum UmTask : uint
    {
        None,
        Connect,
        Swipe,
        Cmd,
        FwUpdate
    }

    public enum UmRet : uint
    {
        Success,
        NoReader,
        SdkBusy,
        MonoAudio,
        AlreadyConnected,
        LowVolume,
        NotConnected,
        NotApplicable,
        InvalidArg,
        UfInvalidStr,
        UfNoFile,
        UfInvalidFile
    }

    public enum UmUfCode : uint
    {
        SendingBlock = 21,
        VerifyingChecksum = 30,
        ResendingBlock = 40,
        FailedToEnterBootloaderMode = 303,
        FailedToSendBlock = 305,
        FailedToVerifyChecksum = 306,
        Canceled = 307
    }
}

