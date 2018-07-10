using System;
using System.Collections.Generic;
using Linphone;
using Xamarin.Forms;
using LinphoneXamarin.Entities;
using LinphoneXamarin.Services;

namespace LinphoneXamarin.components
{
    public partial class Login : ContentPage, RegistrationListener
    {
        RegistrationService registrationService;


        public Login()
        {
            registrationService = RegistrationService.Instance;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (registrationService.registrationState == RegistrationState.Ok)
            {
                Navigation.PushAsync(new Tabs());
                // registrationService.unRegister();
            }

        }

        private void OnRegisterClicked(object sender, EventArgs e)
        {
            if (registrationService.registrationState == RegistrationState.Ok)
            {
                Navigation.PushAsync(new Tabs());
                return;
            }
            registrationService.register(this, username.Text, password.Text, domain.Text);
        }

        public void onStatusChanged(RegistrationState state)
        {
            registration_status.Text = "Registration " + state;
            if (state == RegistrationState.Ok)
            {
                Navigation.PushAsync(new Tabs());
            }
        }
    }
}
