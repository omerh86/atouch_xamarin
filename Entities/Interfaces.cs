using System;
using Linphone;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace LinphoneXamarin.Entities
{
    public interface RegistrationListener
    {
        void onStatusChanged(RegistrationState state);
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
