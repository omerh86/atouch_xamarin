using System;
using Linphone;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace LinphoneXamarin.Entities
{
    public interface LinphoneRegistrationListener
    {
        void onLinphoneStatusChanged(RegistrationState state, string message);
    }

    public interface MyRegistrationListener
    {
        void onMyRegistrationStateChanged(MyRegistrationState state);
    }

    public interface Tr87stateListener
    {
        void onTr87Established();
    }

    public interface LoginRegistrationListener
    {
        void onLoginFailed(LoginError loginError);
        void onLoginSuccsses();

    }

    public interface CallsListener
    {
        void onListUpdated(List<MyCall> list);
    }

    public interface FavListener
    {
        void onFavListChanged();
    }

    public interface CallViewInitiater
    {
        void onInitiateCallView();
    }

}
