using System;
using System.Collections.Generic;
using Linphone;
using Xamarin.Forms;
using LinphoneXamarin.Entities;
using LinphoneXamarin.Services;

using LinphoneXamarin.Util;

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

        public struct GetConnectionRequest
        {
            public GetConnectionProp GetConnection;

            public GetConnectionRequest(GetConnectionProp p)
            {
                GetConnection = p;
            }
        }

        public struct GetConnectionProp
        {
            public string userName, deviceId;

            public GetConnectionProp(string a, string b)
            {
                userName = a;
                deviceId = b;
            }
        }

        private void OnsendRequest(object sender, EventArgs e)
        {
            GetConnectionProp getConnectionProp = new GetConnectionProp("4050", "4050A0D3C10DE55B");
            GetConnectionRequest getConnection = new GetConnectionRequest(getConnectionProp);
            string s = MyFileSystem.Instance.objToJson<GetConnectionRequest>(getConnection);
            AeonixInfoService.Instance.sendToInfoAeonix(s);
        }


        public void onLoginFailed(LoginError loginError)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                registration_status.Text = "Failed";
                Send.IsVisible = false;
            });

        }

        public void onLoginSuccsses()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                registration_status.Text = "Succsses";
                // Send.IsVisible = true;
                // Navigation.PushAsync(new Tabs());
                // Navigation.RemovePage(this);
                ((App)App.Current).MainPage = new components.navBar();
            });
        }
    }
}
