using System;
using System.Collections.Generic;
using Linphone;
using LinphoneXamarin.Entities;
using LinphoneXamarin.Util;



namespace LinphoneXamarin.Services
{

    public sealed class LoginService : MyRegistrationListener, LinphoneRegistrationListener
    {
        RegistrationService registrationService;
        LoginRegistrationListener loginRegistrationListener;

        private static LoginService instance = null;
        private static readonly object padlock = new object();
        List<Action> registerProcess = new List<Action>();
        int processIndex = -1;

        LoginService()
        {
            registrationService = RegistrationService.Instance;


        }
        public static LoginService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new LoginService();
                    }
                    return instance;
                }
            }
        }

        public LoginInfo getTr87Cardential()
        {
            return MyFileSystem.Instance.loadLoginCardential(CardentialState.TR87);
        }

        public void saveTr87Cardential(LoginInfo loginInfo)
        {
            MyFileSystem.Instance.saveLoginCardential(loginInfo, CardentialState.TR87);
        }

        public void login(bool isIncludeTr87, LoginInfo loginInfo)
        {
            registrationService.setRegistrationListener(this);
            clearLoginProcess(false);
            if (isIncludeTr87)
            {
                registerProcess.Add(() => loginTR87(loginInfo));
                registerProcess.Add(() => getInfo());
            }
           // registerProcess.Add(() => loginAeonix());
            continueLoginProcess();
        }

        public void onStatusChanged(MyRegistrationState state, string message)
        {

        }

        public void onStatusChanged(RegistrationState state)
        {
            switch (state)
            {
                case RegistrationState.Ok:
                    continueLoginProcess();
                    break;
                case RegistrationState.Failed:
                case RegistrationState.Cleared:
                case RegistrationState.None:
                    clearLoginProcess(true,LoginError.RegistrationFailed);
                    break;
                case RegistrationState.Progress:
                    //do nothing
                    break;
            }

        }

        public void setLoginRegistrationListener(LoginRegistrationListener loginRegistrationListener)
        {
            this.loginRegistrationListener = loginRegistrationListener;
        }


        private void loginTR87(LoginInfo loginInfo)
        {
            Console.WriteLine("loginTR87");
            registrationService.register(loginInfo.name, loginInfo.password, loginInfo.ip);
        }

        private void getInfo()
        {
            Console.WriteLine("getInfo");
            MyFileSystem.Instance.saveLoginCardential(new LoginInfo("name", "pass", "ip"), CardentialState.Aeonix);
            continueLoginProcess();
        }

        private void loginAeonix()
        {
            LoginInfo loginInfo = MyFileSystem.Instance.loadLoginCardential(CardentialState.Aeonix);
            if (loginInfo != null)
            {
                Console.WriteLine("loginAeonix");
                registrationService.register(loginInfo.name, loginInfo.password, loginInfo.ip);
            }
            else
            {
                clearLoginProcess(true,LoginError.InfoFailed);
            }
        }

        private void fireLoginFailed(LoginError error)
        {
            if (loginRegistrationListener != null)
            {
                loginRegistrationListener.onLoginFailed(error);
            }
        }

        private void fireLoginSuccsess()
        {
            if (loginRegistrationListener != null)
            {
                loginRegistrationListener.onLoginSuccsses();
            }
        }

        private void continueLoginProcess()
        {
            processIndex++;
            if (registerProcess.Count == processIndex)
            {

                fireLoginSuccsess();
                clearLoginProcess(false);
                return;
            }

            registerProcess[processIndex]();
        }

        private void clearLoginProcess(Boolean isFireFailed, LoginError error=LoginError.None)
        {
            registerProcess.Clear();
            if (processIndex > -1 && isFireFailed)
                fireLoginFailed(error);

            processIndex = -1;
        }

    }
}
