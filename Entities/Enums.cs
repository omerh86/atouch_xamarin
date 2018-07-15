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
        InviteAeonix,
        AfterAeonix

    }



}
