using System;
using LinphoneXamarin.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Linphone;
namespace LinphoneXamarin.Services
{
    public sealed class AeonixInfoService
    {

        private static AeonixInfoService instance = null;
        private static readonly object padlock = new object();

    
        AeonixInfoService()
        {
          
        }

        public static AeonixInfoService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new AeonixInfoService();
                    }
                    return instance;
                }
            }
        }


        public class InfoProcess

        {
            class StateTransition
            {
                readonly MyInfoProcessState CurrentState;
                readonly MyInfoProcessCommands Command;


                public StateTransition(MyInfoProcessState currentState, MyInfoProcessCommands command)
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

            Dictionary<StateTransition, MyInfoProcessState> transitions;
            public MyRegistrationListener MyRegistrationListener { get; set; }
            public MyInfoProcessState CurrentState { get; private set; }
            public InfoProcess()
            {
                CurrentState = MyInfoProcessState.Before;
                transitions = new Dictionary<StateTransition, MyInfoProcessState>
            {
                { new StateTransition(MyInfoProcessState.Before, MyInfoProcessCommands.StartAll), MyInfoProcessState.GetCallLog},
               
            };
            }

            public MyInfoProcessState GetNext(MyInfoProcessCommands command)
            {
                StateTransition transition = new StateTransition(CurrentState, command);
                MyInfoProcessState nextState;
                if (!transitions.TryGetValue(transition, out nextState))
                    throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
                return nextState;
            }

            public void MoveNext(MyInfoProcessCommands command)
            {
                MyInfoProcessState previewState = CurrentState;
                CurrentState = GetNext(command);
                if (this.MyRegistrationListener != null && previewState != CurrentState)
                {
                   // MyRegistrationListener.onMyRegistrationStateChanged(CurrentState);
                }
            }
        }

    }
}