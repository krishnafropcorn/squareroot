// ApiDefinition.cs

using System;

using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using Accounts;

namespace ACR35.sdk.iOSBindings
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

	[BaseType(typeof(NSObject))]
	interface ACRTrackData
	{
		[ExportAttribute("batteryStatus")]
		nuint BatteryStatus { get; }

		[ExportAttribute("track1Length")]
		nuint Track1Length { get; }

		[ExportAttribute("track1ErrorCode")]
		nuint Track1ErrorCode { get; }

		[ExportAttribute("track2Length")]
		nuint Track2Length { get; }

		[ExportAttribute("track2ErrorCode")]
		nuint Track2ErrorCode { get; }
	}




	[BaseType(typeof(ACRTrackData))]
	interface ACRAesTrackData
	{

		[ExportAttribute("initWithBytes:length:")]
		IntPtr Constructor(IntPtr buffer, nuint length);

		[ExportAttribute("trackData")]
		NSData TrackData { get; }
	}




	[BaseType(typeof(NSObject))]
	interface ACRAudioJackReader
	{

		[ExportAttribute("initWithMute:")]
		IntPtr Constructor(bool mute);

		[ExportAttribute("mute")]
		bool Mute { get; set; }

		[Wrap("WeakDelegate")]
		ACRAudioJackReaderDelegate Delegate { get; set; }

		[NullAllowed, ExportAttribute("delegate", ArgumentSemantic.UnsafeUnretained)]
		NSObject WeakDelegate { get; set; }


		[ExportAttribute("reset")]
		void Reset();


		[ExportAttribute("resetWithCompletion:")]
		[Async]
		void ResetWithCompletion(Action completion);


		[ExportAttribute("sleep")]
		bool Sleep();


		[ExportAttribute("getFirmwareVersion")]
		bool GetFirmwareVersion();


		[ExportAttribute("getStatus")]
		bool GetStatus();


		[ExportAttribute("setSleepTimeout:")]
		bool SetSleepTimeout(nuint timeout);


		[ExportAttribute("authenticateWithMasterKey:length:")]
		void AuthenticateWithMasterKey(IntPtr masterKey, nuint length);


		[ExportAttribute("authenticateWithMasterKey:length:completion:")]
		[Async]
		void AuthenticateWithMasterKey(IntPtr masterKey, nuint length, Action<nuint> completion);


		[ExportAttribute("getCustomId")]
		bool GetCustomId();


		[ExportAttribute("setCustomId:length:")]
		bool SetCustomId(IntPtr customId, nuint length);


		[ExportAttribute("getDeviceId")]
		bool GetDeviceId();



		[ExportAttribute("setMasterKey:length:")]
		bool SetMasterKey(IntPtr masterKey, nuint length);



		[ExportAttribute("setAesKey:length:")]
		bool SetAesKey(IntPtr aesKey, nuint length);



		[ExportAttribute("getDukptOption")]
		bool GetDukptOption();



		[ExportAttribute("setDukptOption:")]
		bool SetDukptOptin(bool enabled);


		[ExportAttribute("initializeDukptWithIksn:iksnLength:ipek:ipekLength:")]
		bool InitializeDukptWithIksn(IntPtr iksn, nuint iksnLength, IntPtr ipek, nuint ipekLength);



		[ExportAttribute("getTrackDataOption")]
		bool GetTrackDataOption();



		[ExportAttribute("setTrackDataOption:")]
		bool SetTrackDataOption(ACRTrackDataOption option);



		[ExportAttribute("piccPowerOnWithTimeout:cardType:")]
		bool PiccPowerOnWithTimeout(nuint timeout, nuint cardType);



		[ExportAttribute("piccTransmitWithTimeout:commandApdu:length:")]
		bool PiccTransmitWithTimeout(nuint timeout, IntPtr commandApdu, nuint length);


		[ExportAttribute("piccPowerOff")]
		bool PiccPowerOff();



		[ExportAttribute("setPiccRfConfig:length:")]
		bool SetPiccRfConfig(IntPtr rfConfig, nuint length);


		[ExportAttribute("powerCardWithAction:slotNum:timeout:error:")]
		NSData PowerCardWithAction(ACRCardPowerAction action, nuint slotNum, TimeSpan timeout, out NSError error);


		[ExportAttribute("setProtocol:slotNum:timeout:error:")]
		ACRCardProtocol SetProtocol(ACRCardProtocol preferredProtocols, nuint slotNum, TimeSpan timeout, out NSError error);


		[ExportAttribute("transmitApdu:slotNum:timeout:error:")]
		NSData TransmitApdu(NSData command, nuint slotNum, TimeSpan timeout, out NSError error);


		[ExportAttribute("transmitApdu:length:slotNum:timeout:error:")]
		NSData TransmitApdu(IntPtr command, nuint length, nuint slotNum, nuint timeout, out NSError error);


		[ExportAttribute("transmitControlCommand:controlCode:slotNum:timeout:error:")]
		NSData TransmitControlCommand(NSData command, nuint controlCode, nuint slotNum, TimeSpan timeout, out NSError error);


		[ExportAttribute("transmitControlCommand:length:controlCode:slotNum:timeout:error:")]
		NSData TransmitControlCommand(IntPtr command, nuint length, nuint controlCode, nuint slotNum, TimeSpan timeout, out NSError error);


		[ExportAttribute("updateCardStateWithSlotNumber:timeout:error:")]
		void UpdateCardStateWithSlotNumber(nuint slotNum, TimeSpan timeout, out NSError error);


		[ExportAttribute("getAtrWithSlotNumber:")]
		NSData GetAtrWithSlotNumber(nuint slotNum);



		[ExportAttribute("getCardStateWithSlotNumber:")]
		ACRCardState GetCardStateWithSlotNumber(nuint slotNum);


		[ExportAttribute("getProtocolWithSlotNumber:")]
		ACRCardProtocol GetProtocolWithSlotNumber(nuint slotNum);


		[ExportAttribute("sendCommand:length:")]
		bool SendCommand(IntPtr buffer, nuint length);


		[ExportAttribute("createFrame:length:")]
		NSData CreateFrame(IntPtr buffer, nuint length);


		[ExportAttribute("sendFrame:")]
		bool SendFrame(NSData frameData);


		[ExportAttribute("verifyData:length:")]
		bool VerifyData(IntPtr buffer, nuint length);

	}



	[BaseType(typeof(NSObject))]
	[Model, Protocol]
	interface ACRAudioJackReaderDelegate
	{
		[ExportAttribute("readerDidReset:")]
		void ReaderDidReset(ACRAudioJackReader reader);


		[ExportAttribute("reader:didNotifyResult:")]
		void ReaderDidNotifyResult(ACRAudioJackReader reader, ACRResult result);


		[ExportAttribute("reader:didSendFirmwareVersion:")]
		void ReaderDidSendFirmwareVersion(ACRAudioJackReader reader, string firmwareVersion);


		[ExportAttribute("reader:didSendStatus:")]
		void ReaderDidSendStatus(ACRAudioJackReader reader, ACRStatus status);


		[ExportAttribute("readerDidNotifyTrackData:")]
		void ReaderDidNotifyTrackData(ACRAudioJackReader reader);


		[ExportAttribute("reader:didSendTrackData:")]
		void ReaderDidSendTrackData(ACRAudioJackReader reader, ACRTrackData trackData);



		[ExportAttribute("reader:didSendRawData:length:")]
		void ReaderDidSendRawData(ACRAudioJackReader reader, IntPtr rawData, nuint length);



		[ExportAttribute("reader:didAuthenticate:")]
		void ReaderDidAuthenticate(ACRAudioJackReader reader, nint errorCode);


		[ExportAttribute("reader:didSendCustomId:length:")]
		void ReaderDidSendCustomId(ACRAudioJackReader reader, IntPtr customId, nuint length);



		[ExportAttribute("reader:didSendDeviceId:length:")]
		void ReaderDidSendDeviceId(ACRAudioJackReader reader, IntPtr deviceId, nuint length);


		[ExportAttribute("reader:didSendDukptOption:")]
		void ReaderDidSendDukptOption(ACRAudioJackReader reader, bool enabled);


		//		(void)reader:(ACRAudioJackReader *)reader didSendTrackDataOption:(ACRTrackDataOption)option;
		[ExportAttribute("reader:didSendTrackDataOption:")]
		void ReaderDidSendTrackDataOption(ACRAudioJackReader reader, ACRTrackDataOption option);



		[ExportAttribute("reader:didSendPiccAtr:length:")]
		void ReaderDidSendPiccAttr(ACRAudioJackReader reader, IntPtr attr, nuint length);


		[ExportAttribute("reader:didSendPiccResponseApdu:length:")]
		void ReaderDidSendPiccResponseApdu(ACRAudioJackReader reader, IntPtr responseApdu, nuint length);
	}



	[BaseType(typeof(NSObject))]
	interface ACRCRC16
	{

		[ExportAttribute("value")]
		IntPtr GetValue();


		[ExportAttribute("reset")]
		void Reset();


		[ExportAttribute("updateWithBytes:length:")]
		void UpdateWithBytes(IntPtr bytes, nuint length);

	}




	[BaseType(typeof(NSObject))]
	interface ACRDukptReceiver
	{

		[ExportAttribute("keySerialNumber")]
		NSData GetKeySerialNumber();


		[ExportAttribute("setKeySerialNumber:")]
		void SetKeySerialNumber(NSData keySerialNumber);


		[ExportAttribute("setKeySerialNumber:length:")]
		void SetKeySerialNumber(IntPtr keySerialNumber, nuint length);


		[ExportAttribute("encryptionCounter")]
		nuint GetEncryptionCounter();


		[ExportAttribute("loadInitialKey:")]
		void LoadInitialKey(NSData initialKey);



		[ExportAttribute("loadInitialKey:length:")]
		void LoadInitialKey(IntPtr buffer, nuint length);



		[ExportAttribute("key")]
		NSData GetKey();



		[ExportAttribute("pinEncryptionKeyFromKey:")]
		NSData GeneratePinEncryptionKeyFromKey(NSData key);



		[ExportAttribute("pinEncryptionKeyFromKey:length:")]
		NSData GeneratePinEncryptionKeyFromKey(IntPtr key, nuint length);



		[ExportAttribute("macRequestKeyFromKey:")]
		NSData GenerateMacRequestKeyFromKey(NSData key);



		[ExportAttribute("macRequestKeyFromKey:length:")]
		NSData GenerateMacRequestKeyFromKey(IntPtr key, nuint length);



		[ExportAttribute("macResponseKeyFromKey:")]
		NSData GenerateMacResponseKeyFromKey(NSData key);


		[ExportAttribute("macResponseKeyFromKey:length:")]
		NSData GenerateMacResponseKeyFromKey(IntPtr key, nuint length);


		[ExportAttribute("dataEncryptionRequestKeyFromKey:")]
		NSData GenerateDataEncryptionRequestKeyFromKey(NSData key);


		[ExportAttribute("dataEncryptionRequestKeyFromKey:length:")]
		NSData GenerateDataEncryptionRequestKeyFromKey(IntPtr key, nuint length);


		[ExportAttribute("dataEncryptionResponseKeyFromKey:")]
		NSData GenerateDataEncryptionResponseKeyFromKey(NSData key);


		[ExportAttribute("dataEncryptionResponseKeyFromKey:length:")]
		NSData GenerateDataEncryptionResponseKeyFromKey(IntPtr key, nuint length);


		[Static, ExportAttribute("macFromData:key:")]
		NSData GenerateMacFromData(NSData data, NSData key);



		[Static, ExportAttribute("macFromData:dataLength:key:keyLength:")]
		NSData GenerateMacFromData(IntPtr data, nuint dataLength, IntPtr key, nuint keyLength);



		[Static, ExportAttribute("compareKeySerialNumber:ksn2:")]
		bool CompareKeySerialNumber(NSData ksn1, NSData ksn2);


		[Static, ExportAttribute("compareKeySerialNumber:ksn1Length:ksn2:ksn2Length:")]
		bool CompareKeySerialNumber(IntPtr ksn1, nuint ksn1Length, IntPtr ksn2, nuint ksn2Length);


		[Static, ExportAttribute("encryptionCounterFromKeySerialNumber:")]
		nuint GetEncryptionCounterFromKeySerialNumber(NSData ksn);


		[Static, ExportAttribute("encryptionVounterFromKeySerialNumber:length:")]
		nuint GetEncryptionCounterFromKeySerialNumber(IntPtr ksn, nuint length);

	}




	[BaseType(typeof(ACRTrackData))]
	interface ACRDukptTrackData
	{

		[ExportAttribute("track1Data")]
		NSData Track1Data { get; }


		[ExportAttribute("track1Mac")]
		NSData Track1Mac { get; }



		[ExportAttribute("track2Data")]
		NSData Track2Data { get; }


		[ExportAttribute("track2Mac")]
		NSData Track2Mac { get; }


		[ExportAttribute("track1MaskedData")]
		string Track1MaskedData { get; }


		[ExportAttribute("track2MaskedData")]
		string Track2MaskedData { get; }


		[ExportAttribute("keySerialNumber")]
		NSData KeySerialNumber { get; }


		[ExportAttribute("initWithBytes:length:")]
		IntPtr Constructor(IntPtr bytes, nuint length);

	}



	[BaseType(typeof(NSObject))]
	interface ACRResult
	{
		[ExportAttribute("errorCode")]
		nuint ErroCode { get; }


		[ExportAttribute("initWithBytes:length:")]
		IntPtr Constructor(IntPtr bytes, nuint length);
	}




	[BaseType(typeof(NSObject))]
	interface ACRStatus
	{

		/*
		 * Returns the battery level.
		 * <table>
		 * <tr><th>Value</th><th>Meaning</th></tr>
		 * <tr><td>00h</td><td>Battery Level &gt;= 3.0V</td></tr>
		 * <tr><td>01h</td><td>2.9 &lt;= Battery Level &lt; 3.0V</td></tr>
		 * <tr><td>02h</td><td>2.8 &lt;= Battery Level &lt; 2.9V</td></tr>
		 * <tr><td>03h</td><td>2.7 &lt;= Battery Level &lt; 2.8V</td></tr>
		 * <tr><td>04h</td><td>2.6 &lt;= Battery Level &lt; 2.7V</td></tr>
		 * <tr><td>05h</td><td>2.5 &lt;= Battery Level &lt; 2.6V</td></tr>
		 * <tr><td>06h</td><td>2.4 &lt;= Battery Level &lt; 2.5V</td></tr>
		 * <tr><td>07h</td><td>2.3 &lt;= Battery Level &lt; 2.4V</td></tr>
		 * <tr><td>08h</td><td>Battery Level &lt; 2.3V</td></tr>
		 * </table>
		 */
		[ExportAttribute("batteryLevel")]
		nuint BatteryLevel { get; }


		[ExportAttribute("sleepTimeout")]
		nuint SleepTimeout { get; }



		[ExportAttribute("initWithBytes:length:")]
		IntPtr Constructor(IntPtr bytes, nuint length);

	}




	[BaseType(typeof(NSObject))]
	interface ACRTrack1Data
	{

		[ExportAttribute("jis2Data")]
		string Jis2Data { get; }



		[ExportAttribute("track1String")]
		string Track1String { get; }



		[ExportAttribute("primaryAccountNumber")]
		string PrimaryAccountNumber { get; }


		[ExportAttribute("name")]
		string Name { get; }


		[ExportAttribute("expirationDate")]
		string ExpirationDate { get; }


		[ExportAttribute("serviceCode")]
		string ServiceCode { get; }



		[ExportAttribute("discretionaryData")]
		string DiscretionaryData { get; }


		[ExportAttribute("initWithBytes:length:")]
		IntPtr Constructor(IntPtr bytes, nuint length);


		[ExportAttribute("initWithString:")]
		IntPtr Constructor(string track1String);

	}

	[BaseType(typeof(NSObject))]
	interface ACRTrack2Data
	{


		[ExportAttribute("track1String")]
		string Track2String { get; }



		[ExportAttribute("primaryAccountNumber")]
		string PrimaryAccountNumber { get; }



		[ExportAttribute("expirationDate")]
		string ExpirationDate { get; }


		[ExportAttribute("serviceCode")]
		string ServiceCode { get; }



		[ExportAttribute("discretionaryData")]
		string DiscretionaryData { get; }


		[ExportAttribute("initWithBytes:length:")]
		IntPtr Constructor(IntPtr bytes, nuint length);


		[ExportAttribute("initiWithString:")]
		IntPtr Constructor(string track2String);

	}

}

