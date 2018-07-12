using System;
using System.Collections.Generic;
using Linphone;
using LinphoneXamarin.Entities;
using LinphoneXamarin.Util;
using System.Threading.Tasks;



namespace LinphoneXamarin.Services
{

    public sealed class LoginService : LinphoneRegistrationListener, MyRegistrationListener
    {
        RegistrationService registrationService;
        LoginRegistrationListener loginRegistrationListener;

        private static LoginService instance = null;
        private static readonly object padlock = new object();
        RegistrationProcess registrationProcess;

        LoginService()
        {
            registrationService = RegistrationService.Instance;
            registrationProcess = new RegistrationProcess();
            registrationProcess.MyRegistrationListener = this;

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

        public void login(bool isIncludeTr87)
        {
            registrationService.setRegistrationListener(this);

            new Task(() => {
                registrationProcess.MoveNext(Command.Clear);
                if (isIncludeTr87)
                {
                    registrationProcess.MoveNext(Command.StartAll);
                }
                else
                {
                    registrationProcess.MoveNext(Command.StartAeonix);
                }
            }).Start();
          

        }

        public void onStatusChanged(MyRegistrationState state, string message)
        {

        }

        public void onStatusChanged(RegistrationState state)
        {
            switch (state)
            {
                case RegistrationState.Ok:
                    registrationProcess.MoveNext(Command.Continue);
                    break;
                case RegistrationState.Failed:
                case RegistrationState.Cleared:
                case RegistrationState.None:
                    registrationProcess.MoveNext(Command.Clear);
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




        private void test(string a)
        {
            Console.WriteLine("omer40: " + a);
            registrationProcess.MoveNext(Command.Continue);
        }

        private void test2(string a)
        {
            Console.WriteLine("omer41: " + a);
            registrationProcess.MoveNext(Command.Clear);
            fireLoginFailed(LoginError.RegistrationFailed);
        }



        public void onMyRegistrationStateChanged(MyRegistrationState state)
        {
            switch (state)
            {
                case MyRegistrationState.BeforeTR87:
                    //make sure disconnection
                    Console.WriteLine("omer40: " + "BeforeTR87");
                    break;
                case MyRegistrationState.ConnectingTR87:
                    //this.loginTR87(getTr87Cardential());
                    test("ConnectingTR87");
                    break;
                case MyRegistrationState.GetingInfo:
                    test2("GetingInfo");
                    break;
                case MyRegistrationState.DisconnectingTR87:
                    test("DisconnectingTR87");
                    break;
                case MyRegistrationState.ConnectingAeonix:
                    test("ConnectingAeonix");
                    break;
                case MyRegistrationState.AfterAeonix:
                    fireLoginSuccsess();
                    //do nothing
                    break;
            }
        }

        public enum Command
        {
            StartAll,
            StartAeonix,
            Continue,
            Clear

        }


        public class RegistrationProcess
        {
            class StateTransition
            {
                readonly MyRegistrationState CurrentState;
                readonly Command Command;


                public StateTransition(MyRegistrationState currentState, Command command)
                {
                    CurrentState = currentState;
                    Command = command;
                }

                public override int GetHashCode()
                {
                    return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
                }

                public override bool Equals(object obj)
                {
                    StateTransition other = obj as StateTransition;
                    return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
                }
            }

            Dictionary<StateTransition, MyRegistrationState> transitions;
            public MyRegistrationListener MyRegistrationListener { get; set; }
            public MyRegistrationState CurrentState { get; private set; }
            public RegistrationProcess()
            {
                CurrentState = MyRegistrationState.BeforeTR87;
                transitions = new Dictionary<StateTransition, MyRegistrationState>
            {
                { new StateTransition(MyRegistrationState.BeforeTR87, Command.StartAeonix), MyRegistrationState.ConnectingAeonix},
                { new StateTransition(MyRegistrationState.BeforeTR87, Command.StartAll), MyRegistrationState.ConnectingTR87},
                { new StateTransition(MyRegistrationState.BeforeTR87, Command.Continue), MyRegistrationState.ConnectingTR87},
                 { new StateTransition( MyRegistrationState.ConnectingTR87, Command.Continue), MyRegistrationState.GetingInfo},
                { new StateTransition(MyRegistrationState.GetingInfo, Command.Continue), MyRegistrationState.DisconnectingTR87},
                { new StateTransition(MyRegistrationState.DisconnectingTR87, Command.Continue), MyRegistrationState.ConnectingAeonix},
                { new StateTransition(MyRegistrationState.ConnectingAeonix, Command.Continue), MyRegistrationState.AfterAeonix},
                { new StateTransition(MyRegistrationState.AfterAeonix, Command.Continue), MyRegistrationState.AfterAeonix},
                { new StateTransition(MyRegistrationState.BeforeTR87, Command.Clear), MyRegistrationState.BeforeTR87},
                { new StateTransition(MyRegistrationState.ConnectingTR87, Command.Clear), MyRegistrationState.BeforeTR87},
                { new StateTransition(MyRegistrationState.GetingInfo, Command.Clear), MyRegistrationState.BeforeTR87},
                { new StateTransition(MyRegistrationState.DisconnectingTR87, Command.Clear), MyRegistrationState.BeforeTR87},
                { new StateTransition(MyRegistrationState.ConnectingAeonix, Command.Clear), MyRegistrationState.BeforeTR87},
                { new StateTransition(MyRegistrationState.AfterAeonix, Command.Clear), MyRegistrationState.BeforeTR87}
            };
            }

            public MyRegistrationState GetNext(Command command)
            {
                StateTransition transition = new StateTransition(CurrentState, command);
                MyRegistrationState nextState;
                if (!transitions.TryGetValue(transition, out nextState))
                    throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
                return nextState;
            }

            public void MoveNext(Command command)
            {
                MyRegistrationState previewState = CurrentState;
                CurrentState = GetNext(command);
                if (this.MyRegistrationListener != null && previewState != CurrentState)
                {
                    MyRegistrationListener.onMyRegistrationStateChanged(CurrentState);
                }
            }
        }


    }
}
