// StructsAndEnums.cs
//
// Author:
//       Dimitris Tavlikos <dimitris@tavlikos.com>
//
// Copyright (c) 2015 FastBar Technologies Inc.
//
using System;

namespace ACR3x.sdk.iOSBindings
{
	public enum ACRBatteryStatus
	{
		ACRBatteryStatusLow = 0,
		ACRBatteryStatusFull = 1
	}

	public enum ACRTrackError : uint
	{
		ACRTrackErrorSuccess = 0x00,    /**< Success. */
		ACRTrackErrorSS      = 0x01,    /**< Invalid start sentinel on the track. */
		ACRTrackErrorES      = 0x02,    /**< Invalid end sentinel on the track. */
		ACRTrackErrorLRC     = 0x04,    /**< Invalid checksum on the track. */
		ACRTrackErrorParity  = 0x08     /**< Invalid parity on the track. */
	}

	public enum ACRAuthError
	{
		ACRAuthErrorSuccess = 0,    /**< Success. */
		ACRAuthErrorFailure = 1,    /**< Failure. */
		ACRAuthErrorTimeout = 2     /**< Timeout. */
	}

	public enum ACRPiccCardType : uint
	{
		ACRPiccCardTypeIso14443TypeA = 0x01,    /**< ISO14443 Type A. */
		ACRPiccCardTypeIso14443TypeB = 0x02,    /**< ISO14443 Type B. */
		ACRPiccCardTypeFelica212kbps = 0x04,    /**< FeliCa 212kbps. */
		ACRPiccCardTypeFelica424kbps = 0x08,    /**< FeliCa 424kbps. */
		ACRPiccCardTypeAutoRats      = 0x80     /**< Auto RATS. */
	}

	public enum ACRCardPowerAction
	{
		ACRCardPowerDown = 0,   /**< Power down the card. */
		ACRCardColdReset = 1,   /**< Cycle power and reset the card. */
		ACRCardWarmReset = 2    /**< Force a reset on the card. */
	}


	[Flags]
	public enum ACRCardProtocol : uint
	{
		/**
     * There is no active protocol.
     */
		ACRProtocolUndefined = 0x00000000,

		/**
     * T=0 is the active protocol.
     */
		ACRProtocolT0 = 0x00000001,

		/**
     * T=1 is the active protocol.
     */
		ACRProtocolT1 = 0x00000002,

		/**
     * Raw is the active protocol.
     */
		ACRProtocolRaw = 0x00010000,

		/**
     * This is the mask of ISO defined transmission protocols.
     */
		ACRProtocolTx = 0x00000001 | 0x00000002,

		/**
     * Use the default transmission parameters or card clock frequency.
     */
		ACRProtocolDefault = 0x80000000,

		/**
     * Use optimal transmission parameters or card clock frequency. This is the
     * default.
     */
		ACRProtocolOptimal = 0x00000000
	}




	public enum ACRCardState
	{
		/**
     * The library is unaware of the current state of the reader.
     */
		ACRCardUnknown = 0,

		/**
     * There is no card in the reader.
     */
		ACRCardAbsent = 1,

		/**
     * There is a card in the reader, but it has not been moved into position
     * for use.
     */
		ACRCardPresent = 2,

		/**
     * There is a card in the reader in position for use. The card is not
     * powered.
     */
		ACRCardSwallowed = 3,

		/**
     * Power is being provided to the card, but the library is unaware of the
     * mode of the card.
     */
		ACRCardPowered = 4,

		/**
     * The card has been reset and is awaiting PTS negotiation.
     */
		ACRCardNegotiable = 5,

		/**
     * The card has been reset and specific communication protocols have been
     * established.
     */
		ACRCardSpecific = 6
	}




	public enum ACRIoctlCcid
	{
		Escape = 3500,
		XfrBlock = 3600
	}




	public enum ACRTrackDataOption
	{
		/** Enable the encrypted track 1 data. */
		ACRTrackDataOptionEncryptedTrack1 = 0x01,

		/** Enable the encrypted track 2 data. */
		ACRTrackDataOptionEncryptedTrack2 = 0x02,

		/** Enable the masked track 1 data. */
		ACRTrackDataOptionMaskedTrack1 = 0x04,

		/** Enable the masked track 2 data. */
		ACRTrackDataOptionMaskedTrack2 = 0x08
	}





	public enum ACRError : uint
	{
		ACRErrorSuccess              = 0x00,    /*< Success. */
		ACRErrorInvalidCommand       = 0xFF,    /*< Invalid command. */
		ACRErrorInvalidParameter     = 0xFE,    /*< Invalid parameters. */
		ACRErrorInvalidChecksum      = 0xFD,    /*< Invalid checksum. */
		ACRErrorInvalidStartByte     = 0xFC,    /*< Invalid start byte. */
		ACRErrorUnknown              = 0xFB,    /*< Unknown error. */
		ACRErrorDukptOperationCeased = 0xFA,    /*< DUKPT operation is ceased. */
		ACRErrorDukptDataCorrupted   = 0xF9,    /*< DUKPT data is corrupted. */
		ACRErrorFlashDataCorrupted   = 0xF8,    /*< Flash data is corrupted. */
		ACRErrorVerificationFailed   = 0xF7,    /*< Verification is failed. */
		ACRErrorPiccNoCard           = 0xF6     /*< No card in PICC slot. */
	}




	public enum ACRAudioJackError
	{
		/**
     * The card operation timed out.
     */
		ACRCardTimeoutError = 1,

		/**
     * There is an error occurred in the communication.
     */
		ACRCommunicationError = 2,

		/**
     * There is timeout in the communication.
     */
		ACRCommunicationTimeoutError = 3,

		/**
     * The state of reader is invalid.
     */
		ACRInvalidDeviceStateError = 4,

		/**
     * The requested protocols are incompatible with the protocol currently in
     * use with the card.
     */
		ACRProtocolMismatchError = 5,

		/**
     * The program attempts to access a card which is removed.
     */
		ACRRemovedCardError = 6,

		/**
     * The request queue is full.
     */
		ACRRequestQueueFullError = 7,

		/**
     * The program attempts to access a card which is not responding to a reset.
     */
		ACRUnresponsiveCardError = 8,

		/**
     * The program attempts to access a card which is not supported.
     */
		ACRUnsupportedCardError = 9
	}
}

