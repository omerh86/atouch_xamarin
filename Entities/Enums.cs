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

    public enum MyRegistrationState
    {
        BeforeTR87,
        ConnectingTR87,
        TR87Succsess,
        TR87Fail,
        ConnectingAeonix,
        AeonixSucssess,
        AeonixFail
    }

    public enum LoginError
    {
        TimeOut,
        RestartLogin,
        RegistrationFailed,
        InfoFailed,
        None
    }


}
