﻿using System;
using System.Collections.Generic;
using Linphone;
using LinphoneXamarin.Entities;
using LinphoneXamarin.Util;
using System.Threading.Tasks;
using System.Threading;

namespace LinphoneXamarin.Services
{

    public sealed class LoginService : LinphoneRegistrationListener, MyRegistrationListener, Tr87stateListener
    {
        RegistrationService registrationService;
        LoginRegistrationListener loginRegistrationListener;

        private static LoginService instance = null;
        private static readonly object padlock = new object();
        RegistrationProcess registrationProcess;

        CallService callService;
        AeonixInfoService aeonixInfoService;

        LoginService()
        {
            registrationService = RegistrationService.Instance;
            registrationProcess = new RegistrationProcess();
            registrationProcess.MyRegistrationListener = this;
            callService = CallService.Instance;
            callService.setTr87Listener(this);
            aeonixInfoService = AeonixInfoService.Instance;

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

        public bool isRegistered()
        {
            RegistrationState r = registrationService.registrationState;
            if (r == RegistrationState.Ok)
                return true;
            else
                return false;

        }

        public void register(RegisterCommands command)
        {
            registrationService.setRegistrationListener(this);
            CancellationTokenSource ts = new CancellationTokenSource();
            try
            {

                new Task(() =>
                {

                    resetLoginProccesss(false);
                    registrationProcess.MoveNext(command);

                }).Start();

                //throw new TimeoutException();
            }
            catch (AggregateException aggregateException)
            {

                //  resetLoginProccesss(true, LoginError.TimeOut);
                throw aggregateException.InnerException;
            }



        }

        public void setLoginRegistrationListener(LoginRegistrationListener loginRegistrationListener)
        {
            this.loginRegistrationListener = loginRegistrationListener;
        }


        private void loginTR87()
        {
            Console.WriteLine("loginTR87");
            LoginInfo loginInfo = getTr87Cardential();
            if (loginInfo != null)
            {
                registrationService.register(loginInfo.name, loginInfo.password, loginInfo.ip);
            }
            else
            {

            }
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
            registrationProcess.MoveNext(RegisterCommands.Continue);
        }

        public void onLinphoneStatusChanged(RegistrationState state, string message)
        {
            switch (state)
            {
                case RegistrationState.Ok:
                    if (registrationProcess.CurrentState == MyRegistrationState.ConnectingAeonix || registrationProcess.CurrentState == MyRegistrationState.DisconnectingTR87 || registrationProcess.CurrentState == MyRegistrationState.ConnectingTR87)
                        registrationProcess.MoveNext(RegisterCommands.Continue);
                    break;
                case RegistrationState.Failed:
                case RegistrationState.Cleared:
                case RegistrationState.None:
                    switch (message)
                    {
                        case "Unauthorized":
                            resetLoginProccesss(true, LoginError.RegistrationFailed);
                            break;
                        case "Request Terminated":
                            // I believe it means that we are logged in allready
                           // registrationProcess.MoveNext(RegisterCommands.End);
                            break;
                        default:
                            resetLoginProccesss(true, LoginError.RegistrationFailed);
                            break;

                    }
                    break;
                case RegistrationState.Progress:
                    //do nothing
                    break;
            }

        }

        private void resetLoginProccesss(bool isFailed, LoginError loginError = LoginError.None)
        {
            registrationProcess.MoveNext(RegisterCommands.Clear);
            if (isFailed)
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
                    Console.WriteLine("omer40: " + "ConnectingTR87");
                    this.loginTR87();
                    break;
                case MyRegistrationState.GetingInfo:
                    Console.WriteLine("omer40: " + "GetingInfo");
                    registrationProcess.MoveNext(RegisterCommands.End);
                   // fireLoginSuccsess();
                    //  test("GetingInfo");
                    break;
                case MyRegistrationState.DisconnectingTR87:
                    Console.WriteLine("omer40: " + "DisconnectingTR87");
                    test("DisconnectingTR87");
                    break;
                case MyRegistrationState.ConnectingAeonix:
                    Console.WriteLine("omer40: " + "ConnectingAeonix");
                    this.loginAeonix();
                    break;
                case MyRegistrationState.ReConnectingAeonix:
                    Console.WriteLine("omer40: " + "ReConnectingAeonix");
                    this.loginAeonix();
                    break;
                case MyRegistrationState.InviteAeonix:
                    Console.WriteLine("omer40: " + "InviteAeonix");
                     CallService.Instance.makeTr87Call();
                    //onTr87Established();
                    break;
                case MyRegistrationState.AfterAeonix:
                    Console.WriteLine("omer40: " + "AfterAeonix");
                    fireLoginSuccsess();
                    //do nothing
                    break;
            }
        }

        public void onTr87Established()
        {
            registrationProcess.MoveNext(RegisterCommands.Continue);
        }


        public class RegistrationProcess
        {
            class StateTransition
            {
                readonly MyRegistrationState CurrentState;
                readonly RegisterCommands Command;


                public StateTransition(MyRegistrationState currentState, RegisterCommands command)
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
                { new StateTransition(MyRegistrationState.BeforeTR87, RegisterCommands.StartAll), MyRegistrationState.ConnectingTR87},
                { new StateTransition(MyRegistrationState.BeforeTR87, RegisterCommands.StartAeonix), MyRegistrationState.ConnectingAeonix},
                { new StateTransition(MyRegistrationState.BeforeTR87, RegisterCommands.ReRegister), MyRegistrationState.ReConnectingAeonix},
                { new StateTransition(MyRegistrationState.BeforeTR87, RegisterCommands.Continue), MyRegistrationState.ConnectingTR87},
                { new StateTransition( MyRegistrationState.ConnectingTR87, RegisterCommands.Continue), MyRegistrationState.GetingInfo},
                { new StateTransition(MyRegistrationState.GetingInfo, RegisterCommands.Continue), MyRegistrationState.DisconnectingTR87},
                { new StateTransition(MyRegistrationState.DisconnectingTR87, RegisterCommands.Continue), MyRegistrationState.ConnectingAeonix},
                { new StateTransition(MyRegistrationState.ConnectingAeonix, RegisterCommands.Continue), MyRegistrationState.InviteAeonix},
                { new StateTransition(MyRegistrationState.ReConnectingAeonix, RegisterCommands.Continue), MyRegistrationState.AfterAeonix},
                { new StateTransition(MyRegistrationState.InviteAeonix, RegisterCommands.Continue), MyRegistrationState.AfterAeonix},
                { new StateTransition(MyRegistrationState.AfterAeonix, RegisterCommands.Continue), MyRegistrationState.AfterAeonix},
                { new StateTransition(MyRegistrationState.BeforeTR87, RegisterCommands.Clear), MyRegistrationState.BeforeTR87},
                { new StateTransition(MyRegistrationState.ConnectingTR87, RegisterCommands.Clear), MyRegistrationState.BeforeTR87},
                { new StateTransition(MyRegistrationState.GetingInfo, RegisterCommands.Clear), MyRegistrationState.BeforeTR87},
                { new StateTransition(MyRegistrationState.DisconnectingTR87, RegisterCommands.Clear), MyRegistrationState.BeforeTR87},
                { new StateTransition(MyRegistrationState.ConnectingAeonix, RegisterCommands.Clear), MyRegistrationState.BeforeTR87},
                { new StateTransition(MyRegistrationState.ReConnectingAeonix, RegisterCommands.Clear), MyRegistrationState.BeforeTR87},
                { new StateTransition(MyRegistrationState.InviteAeonix, RegisterCommands.Clear), MyRegistrationState.BeforeTR87},
                { new StateTransition(MyRegistrationState.AfterAeonix, RegisterCommands.Clear), MyRegistrationState.BeforeTR87},
                { new StateTransition(MyRegistrationState.BeforeTR87, RegisterCommands.End), MyRegistrationState.AfterAeonix},
                { new StateTransition(MyRegistrationState.ConnectingTR87, RegisterCommands.End), MyRegistrationState.AfterAeonix},
                { new StateTransition(MyRegistrationState.GetingInfo, RegisterCommands.End), MyRegistrationState.AfterAeonix},
                { new StateTransition(MyRegistrationState.DisconnectingTR87, RegisterCommands.End), MyRegistrationState.AfterAeonix},
                { new StateTransition(MyRegistrationState.ConnectingAeonix, RegisterCommands.End), MyRegistrationState.AfterAeonix},
                { new StateTransition(MyRegistrationState.ReConnectingAeonix, RegisterCommands.End), MyRegistrationState.AfterAeonix},
                { new StateTransition(MyRegistrationState.InviteAeonix, RegisterCommands.End), MyRegistrationState.AfterAeonix},
                { new StateTransition(MyRegistrationState.AfterAeonix, RegisterCommands.End), MyRegistrationState.AfterAeonix}
            };
            }

            public MyRegistrationState GetNext(RegisterCommands command)
            {
                StateTransition transition = new StateTransition(CurrentState, command);
                MyRegistrationState nextState;
                if (!transitions.TryGetValue(transition, out nextState))
                    throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
                return nextState;
            }

            public void MoveNext(RegisterCommands command)
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
