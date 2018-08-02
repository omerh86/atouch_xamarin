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

        LoginService loginService;

        public Login()
        {

            loginService = LoginService.Instance;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loginService.setLoginRegistrationListener(this);

            LoginInfo aeonixLoginInfo = MyFileSystem.Instance.loadLoginCardential(CardentialState.Aeonix);
            if (aeonixLoginInfo != null)
                doLogin(RegisterCommands.StartAeonix);
            else
                this.populateLoginFields();

        }

        protected override void OnDisappearing()
        {
            Console.WriteLine("omer40: " + "on logind Disappearing");
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
            //Amir
            //MyFileSystem.Instance.saveLoginCardential(new LoginInfo("U4002ecc359351475676a", "Po-570w", "172.28.10.127"), CardentialState.Aeonix);
            //Avi
            MyFileSystem.Instance.saveLoginCardential(new LoginInfo("2006A0D3C10DE55B", "A7nhe~6", "172.28.11.141"), CardentialState.Aeonix);
            doLogin(RegisterCommands.StartAeonix);
        }

        private void OnRegisterClicked(object sender, EventArgs e)
        {

            LoginInfo loginInfo = new LoginInfo(username.Text, password.Text, domain.Text);
            loginService.saveTr87Cardential(loginInfo);
            registration_status.Text = "Progress";
            doLogin(RegisterCommands.StartAll);
        }

        private void doLogin(RegisterCommands command)
        {
            stack_registrar.IsVisible = false;
            load.IsVisible = true;
            loginService.register(command);
        }


        public void onLoginFailed(LoginError loginError)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                load.IsVisible = false;
                stack_registrar.IsVisible = true;
                registration_status.Text = "Failed";
                this.populateLoginFields();

            });

        }

        public void onLoginSuccsses()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                load.IsVisible = false;
                registration_status.Text = "Succsses";
                stack_registrar.IsVisible = true;
                ((App)App.Current).MainPage = new components.navBar();
            });
        }
    }
}
