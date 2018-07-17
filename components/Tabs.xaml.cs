using System;
using System.Collections.Generic;
using Linphone;
using LinphoneXamarin.Entities;
using LinphoneXamarin.Services;

using Xamarin.Forms;

namespace LinphoneXamarin.components
{
    public partial class Tabs : TabbedPage, CallViewInitiater, LinphoneRegistrationListener
    {
        RegistrationService registrationService;
        public Tabs()
        {
            registrationService = RegistrationService.Instance;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CallService.Instance.setCallsViewInitiater(this);
            registrationService.setRegistrationListener(this);
        }

        public void onInitiateCallView()
        {
            Navigation.PushAsync(new MainPage());
        }

        public void onLinphoneStatusChanged(RegistrationState state,string message)
        {
            if (state == RegistrationState.Failed || state == RegistrationState.None || state == RegistrationState.Cleared)
            {
                //registrationService.unRegister();
                registrationService.setRegistrationListener(null);
                ((App)App.Current).MainPage = new NavigationPage(new components.Login());
            }
        }
    }
}
