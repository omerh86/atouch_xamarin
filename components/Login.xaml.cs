using System;
using System.Collections.Generic;
using Linphone;
using Xamarin.Forms;
using LinphoneXamarin.Entities;
using LinphoneXamarin.Services;


namespace LinphoneXamarin.components
{
    public partial class Login : ContentPage, LoginRegistrationListener
    {
        // RegistrationService registrationService;
        LoginService loginService;

        public Login()
        {
            loginService = LoginService.Instance;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoginInfo loginInfo = loginService.getTr87Cardential();

            if (loginInfo != null)
            {
                username.Text = loginInfo.name;
                password.Text = loginInfo.password;
                domain.Text = loginInfo.ip;
            }
        }

        private void OnRegisterClicked(object sender, EventArgs e)
        {
            loginService.setLoginRegistrationListener(this);
            LoginInfo loginInfo = new LoginInfo(username.Text, password.Text, domain.Text);
            loginService.saveTr87Cardential(loginInfo);
            registration_status.Text = "Progress";
            loginService.login(true);
        }

        private void OnRegisterClicked2(object sender, EventArgs e)
        {
            loginService.setLoginRegistrationListener(this);
            LoginInfo loginInfo = new LoginInfo(username.Text, password.Text, domain.Text);
            loginService.saveTr87Cardential(loginInfo);
            registration_status.Text = "Progress";
            loginService.login(false);
        }


        public void onLoginFailed(LoginError loginError)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                registration_status.Text = "Failed";
            });
          
        }

        public void onLoginSuccsses()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                registration_status.Text = "Succsses";
            });
        }
    }
}
