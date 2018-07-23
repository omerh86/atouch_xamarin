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

            LoginInfo aeonixLoginInfo = MyFileSystem.Instance.loadLoginCardential(CardentialState.Aeonix);
            if (aeonixLoginInfo != null)
            {
                doLogin(false);
                stack_registrar.IsVisible = false;
            }
            else
            {
                this.populateLoginFields();
            }

            loginService.setLoginRegistrationListener(this);
        }



        private void populateLoginFields()
        {
            LoginInfo loginInfo = loginService.getTr87Cardential();

            if (loginInfo != null)
            {
                username.Text = loginInfo.name;
                password.Text = loginInfo.password;
                domain.Text = loginInfo.ip;
            }
        }

        private void onAutoLogin(object sender, EventArgs e)
        {

              MyFileSystem.Instance.saveLoginCardential(new LoginInfo("U4002ecc359351475676a", "Po-570w", "172.28.10.127"), CardentialState.Aeonix);
            //MyFileSystem.Instance.saveLoginCardential(new LoginInfo("2006A0D3C10DE55B", "A7nhe~6", "172.28.11.141"), CardentialState.Aeonix);
            doLogin(false);
        }

        private void OnRegisterClicked(object sender, EventArgs e)
        {

            LoginInfo loginInfo = new LoginInfo(username.Text, password.Text, domain.Text);
            loginService.saveTr87Cardential(loginInfo);
            registration_status.Text = "Progress";
            doLogin(false);
        }

        private void doLogin(bool isIncludeTr87)
        {
            loginService.login(isIncludeTr87);
        }


        private void OnSendToken(object sender, EventArgs e)
        {
            string token = "fqyJf8zKpNo:APA91bFklY9PmYzmX1_rwT5Cg-6tpztSI4vRhWcZnIpB4n-npTLRN43PsE-q6b2IBqGuaD8Qixc7KhX6CiVwuaC4z2m8iqFcL_YfWFtz4F5KlfwMaR3ZzU2mH2eqWlYBbhZYI4sMzHCSqW8Ng1lbAJ-Zo_6CiMFOKA";
            string strToSend = MySendRequestHelper.Instance.getConnectionRequest(new MySendRequestHelper.ConnectionProp(token));
            AeonixInfoService.Instance.sendToInfoAeonix(strToSend);

        }


        public void onLoginFailed(LoginError loginError)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                stack_registrar.IsVisible = true;
                registration_status.Text = "Failed";
                this.populateLoginFields();
               
            });

        }

        public void onLoginSuccsses()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                registration_status.Text = "Succsses";
                stack_registrar.IsVisible = true;
                //((App)App.Current).MainPage = new components.navBar();
            });
        }
    }
}
