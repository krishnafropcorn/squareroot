using System;
using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace UniMag.Sdk.Bindings.iOS
{
    // The first step to creating a binding is to add your native library ("libNativeLibrary.a")
    // to the project by right-clicking (or Control-clicking) the folder containing this source
    // file and clicking "Add files..." and then simply select the native library (or libraries)
    // that you want to bind.
    //
    // When you do that, you'll notice that MonoDevelop generates a code-behind file for each
    // native library which will contain a [LinkWith] attribute. MonoDevelop auto-detects the
    // architectures that the native library supports and fills in that information for you,
    // however, it cannot auto-detect any Frameworks or other system libraries that the
    // native library may depend on, so you'll need to fill in that information yourself.
    //
    // Once you've done that, you're ready to move on to binding the API...
    //
    //
    // Here is where you'd define your API definition for the native Objective-C library.
    //
    // For example, to bind the following Objective-C class:
    //
    //     @interface Widget : NSObject {
    //     }
    //
    // The C# binding would look like this:
    //
    //     [BaseType (typeof (NSObject))]
    //     interface Widget {
    //     }
    //
    // To bind Objective-C properties, such as:
    //
    //     @property (nonatomic, readwrite, assign) CGPoint center;
    //
    // You would add a property definition in the C# interface like so:
    //
    //     [Export ("center")]
    //     CGPoint Center { get; set; }
    //
    // To bind an Objective-C method, such as:
    //
    //     -(void) doSomething:(NSObject *)object atIndex:(NSInteger)index;
    //
    // You would add a method definition to the C# interface like so:
    //
    //     [Export ("doSomething:atIndex:")]
    //     void DoSomething (NSObject object, int index);
    //
    // Objective-C "constructors" such as:
    //
    //     -(id)initWithElmo:(ElmoMuppet *)elmo;
    //
    // Can be bound as:
    //
    //     [Export ("initWithElmo:")]
    //     IntPtr Constructor (ElmoMuppet elmo);
    //
    // For more information, see http://developer.xamarin.com/guides/ios/advanced_topics/binding_objective-c/
    //
    // @interface uniMag : NSObject
    [BaseType (typeof(NSObject))]
    interface uniMag
    {
        // +(NSString *)SDK_version;
        [Static]
        [Export ("SDK_version")]
        //  // [Verify (MethodToProperty)]
        string SDK_version { get; }

        // -(BOOL)isReaderAttached;
        [Export ("isReaderAttached")]
        //  // [Verify (MethodToProperty)]
        bool IsReaderAttached { get; }

        // -(BOOL)getConnectionStatus;
        [Export ("getConnectionStatus")]
        //  // [Verify (MethodToProperty)]
        bool ConnectionStatus { get; }

        // -(UmTask)getRunningTask;
        [Export ("getRunningTask")]
        //  // [Verify (MethodToProperty)]
        UmTask RunningTask { get; }

        // -(float)getVolumeLevel;
        [Export ("getVolumeLevel")]
        //  // [Verify (MethodToProperty)]
        float VolumeLevel { get; }

        // @property (nonatomic) UmReader readerType;
        [Export ("readerType", ArgumentSemantic.Assign)]
        UmReader ReaderType { get; set; }

        // -(void)setAutoConnect:(BOOL)autoConnect;
        [Export ("setAutoConnect:")]
        void SetAutoConnect (bool autoConnect);

        // -(BOOL)setSwipeTimeoutDuration:(NSInteger)seconds;
        [Export ("setSwipeTimeoutDuration:")]
        bool SetSwipeTimeoutDuration (nint seconds);

        // -(void)setAutoAdjustVolume:(BOOL)b;
        [Export ("setAutoAdjustVolume:")]
        void SetAutoAdjustVolume (bool b);

        // -(void)setDeferredActivateAudioSession:(BOOL)b;
        [Export ("setDeferredActivateAudioSession:")]
        void SetDeferredActivateAudioSession (bool b);

        // -(void)cancelTask;
        [Export ("cancelTask")]
        void CancelTask ();

        // -(UmRet)startUniMag:(BOOL)start;
        [Export ("startUniMag:")]
        UmRet StartUniMag (bool start);

        // -(UmRet)requestSwipe;
        [Export ("requestSwipe")]
        //  // [Verify (MethodToProperty)]
        UmRet RequestSwipe { get; }

        // -(NSData *)getFlagByte;
        [Export ("getFlagByte")]
        //  // [Verify (MethodToProperty)]
        NSData FlagByte { get; }

        // -(UmRet)sendCommandGetVersion;
        [Export ("sendCommandGetVersion")]
        //  // [Verify (MethodToProperty)]
        UmRet SendCommandGetVersion { get; }

        // -(UmRet)sendCommandGetSettings;
        [Export ("sendCommandGetSettings")]
        //  // [Verify (MethodToProperty)]
        UmRet SendCommandGetSettings { get; }

        // -(UmRet)sendCommandEnableTDES;
        [Export ("sendCommandEnableTDES")]
        // [Verify (MethodToProperty)]
        UmRet SendCommandEnableTDES { get; }

        // -(UmRet)sendCommandEnableAES;
        [Export ("sendCommandEnableAES")]
        // [Verify (MethodToProperty)]
        UmRet SendCommandEnableAES { get; }

        // -(UmRet)sendCommandDefaultGeneralSettings;
        [Export ("sendCommandDefaultGeneralSettings")]
        // [Verify (MethodToProperty)]
        UmRet SendCommandDefaultGeneralSettings { get; }

        // -(UmRet)sendCommandGetSerialNumber;
        [Export ("sendCommandGetSerialNumber")]
        // [Verify (MethodToProperty)]
        UmRet SendCommandGetSerialNumber { get; }

        // -(UmRet)sendCommandGetNextKSN;
        [Export ("sendCommandGetNextKSN")]
        // [Verify (MethodToProperty)]
        UmRet SendCommandGetNextKSN { get; }

        // -(UmRet)sendCommandEnableErrNotification;
        [Export ("sendCommandEnableErrNotification")]
        // [Verify (MethodToProperty)]
        UmRet SendCommandEnableErrNotification { get; }

        // -(UmRet)sendCommandDisableErrNotification;
        [Export ("sendCommandDisableErrNotification")]
        // [Verify (MethodToProperty)]
        UmRet SendCommandDisableErrNotification { get; }

        // -(UmRet)sendCommandEnableExpDate;
        [Export ("sendCommandEnableExpDate")]
        // [Verify (MethodToProperty)]
        UmRet SendCommandEnableExpDate { get; }

        // -(UmRet)sendCommandDisableExpDate;
        [Export ("sendCommandDisableExpDate")]
        // [Verify (MethodToProperty)]
        UmRet SendCommandDisableExpDate { get; }

        // -(UmRet)sendCommandEnableForceEncryption;
        [Export ("sendCommandEnableForceEncryption")]
        // [Verify (MethodToProperty)]
        UmRet SendCommandEnableForceEncryption { get; }

        // -(UmRet)sendCommandDisableForceEncryption;
        [Export ("sendCommandDisableForceEncryption")]
        // [Verify (MethodToProperty)]
        UmRet SendCommandDisableForceEncryption { get; }

        // -(UmRet)sendCommandSetPrePAN:(NSInteger)prePAN;
        [Export ("sendCommandSetPrePAN:")]
        UmRet SendCommandSetPrePAN (nint prePAN);

        // -(UmRet)sendCommandClearBuffer;
        [Export ("sendCommandClearBuffer")]
        // [Verify (MethodToProperty)]
        UmRet SendCommandClearBuffer { get; }

        // -(UmRet)sendCommandResetBaudRate;
        [Export ("sendCommandResetBaudRate")]
        // [Verify (MethodToProperty)]
        UmRet SendCommandResetBaudRate { get; }

        // -(UmRet)sendCommandCustom:(NSData *)cmd;
        [Export ("sendCommandCustom:")]
        UmRet SendCommandCustom (NSData cmd);

        // -(UmRet)getAuthentication;
        [Export ("getAuthentication")]
        // [Verify (MethodToProperty)]
        UmRet Authentication { get; }

        // -(BOOL)setFirmwareFile:(NSString *)location;
        [Export ("setFirmwareFile:")]
        bool SetFirmwareFile (string location);

        // -(UmRet)updateFirmware:(NSString *)encrytedBytes;
        [Export ("updateFirmware:")]
        UmRet UpdateFirmware (string encrytedBytes);

        // -(UmRet)updateFirmware2:(NSString *)string withFile:(NSString *)path;
        [Export ("updateFirmware2:withFile:")]
        UmRet UpdateFirmware2 (string @string, string path);

        // +(void)enableLogging:(BOOL)enable;
        [Static]
        [Export ("enableLogging:")]
        void EnableLogging (bool enable);

        // -(NSData *)getWave;
        [Export ("getWave")]
        // [Verify (MethodToProperty)]
        NSData Wave { get; }

        // -(BOOL)setWavePath:(NSString *)path;
        [Export ("setWavePath:")]
        bool SetWavePath (string path);

        // -(void)autoDetect:(BOOL)autoDetect;
        [Export ("autoDetect:")]
        void AutoDetect (bool autoDetect);

        // -(void)promptForConnection:(BOOL)prompt;
        [Export ("promptForConnection:")]
        void PromptForConnection (bool prompt);

        // -(UmRet)proceedPoweringUp:(BOOL)proceedPowerUp;
        [Export ("proceedPoweringUp:")]
        UmRet ProceedPoweringUp (bool proceedPowerUp);

        // -(void)closeConnection;
        [Export ("closeConnection")]
        void CloseConnection ();

        // -(void)cancelSwipe;
        [Export ("cancelSwipe")]
        void CancelSwipe ();

        // -(BOOL)setCmdTimeoutDuration:(NSInteger)seconds;
        [Export ("setCmdTimeoutDuration:")]
        bool SetCmdTimeoutDuration (nint seconds);
    }
}

