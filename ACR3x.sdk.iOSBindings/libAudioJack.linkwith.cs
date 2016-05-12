using ObjCRuntime;

//[assembly: LinkWith ("libAudioJack.a",LinkTarget.Arm64|LinkTarget.ArmV7|LinkTarget.Simulator|LinkTarget.Simulator64, SmartLink = true, ForceLoad = false)]

[assembly: LinkWith ("libAudioJack.a", LinkTarget.ArmV7 | LinkTarget.Simulator | LinkTarget.Simulator64 | LinkTarget.Arm64, 
	//[assembly: LinkWith("libAudioJack.a", LinkTarget.ArmV7,
	IsCxx = true,
	SmartLink = true, 
	ForceLoad = true,
	Frameworks = "AudioToolbox",
	LinkerFlags = "-ObjC")]