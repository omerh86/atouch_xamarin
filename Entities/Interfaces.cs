using System;
using Linphone;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace LinphoneXamarin.Entities
{
    public interface LinphoneRegistrationListener
    {
        void onStatusChanged(RegistrationState state);
    }

    public interface LoginRegistrationListener
    {
        void onLoginFailed(LoginError loginError);
        void onLoginSuccsses();

    }

    public interface MyRegistrationListener
    {
        void onStatusChanged(MyRegistrationState state, string message);
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
