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
            loginService.login(true, loginInfo);
        }

      
        public void onLoginFailed(LoginError loginError)
        {
            Console.WriteLine("failed:" + loginError);
        }

        public void onLoginSuccsses()
        {
            Console.WriteLine("Succsses:");
        }
    }
}
