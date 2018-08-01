using System;
using System.Collections.Generic;
using System.Text;

namespace LinphoneXamarin.Entities
{

    public enum CardentialState
    {
        TR87,
        Aeonix
    }

    public enum LoginError
    {
        TimeOut,
        RestartLogin,
        RegistrationFailed,
        RequestTerminated,
        InfoFailed,
        None

    }
    public enum MyRegistrationState
    {
        BeforeTR87,
        ConnectingTR87,
        GetingInfo,
        DisconnectingTR87,
        ConnectingAeonix,
        ReConnectingAeonix,
        InviteAeonix,
        AfterAeonix

    }

    public enum RegisterCommands
    {
        StartAll,
        StartAeonix,
        ReRegister,
        Continue,
        Clear,
        End
    }

    public enum MyInfoProcessState
    {
        Before,
        GetRsUser,
        GetPicture,
        GetConnection,
        GetCallTargetDevices,
        GetAITSettings,
        GetExplicitPresenceInfo,
        StartMonitorUser,
        GetFavorites,
        GetGroups,
        GetCallLogCountersRequest,
        GetDialPlanFeatures,
        StartMonitorInstantMessages,
        GetCallLog,
        After
    }

    public enum MyInfoProcessCommands
    {
        StartAll,
        Continue,
        Repeat,
        Clear

    }

}
