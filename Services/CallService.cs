﻿using System;
using LinphoneXamarin.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Linphone;
using Plugin.MediaManager;
using System.Runtime.CompilerServices;

namespace LinphoneXamarin.Services
{
    public sealed class CallService
    {

        public List<MyCall> myCalls = new List<MyCall>();

        public bool isSpeaker = false;
        public bool isMute = false;
        public bool isPaused = false;
        public bool isConferenced = false;
        public bool pendingcallView = false;

        public Call tr87Call;

        private static CallService instance = null;
        private static readonly object padlock = new object();
        private CallsListener callsListener;
        private Tr87stateListener tr87StateListener;
        private CallViewInitiater callViewInitiater;


        private bool isCallStateReallyChanged = false;


        private Core LinphoneCore;
        private CoreListener Listener;



        private void OnCall(Core lc, Call lcall, CallState state, string message)
        {

            if (state == CallState.StreamsRunning)
            {
                bool b = lcall.RemoteAddressAsString.Contains("sip:1234");
                if (b)
                {
                    tr87Call = lcall;
                    try
                    {
                        lcall.Pause();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("");
                    };
                    // AeonixInfoService.Instance.sendToInfoAeonix("Hi Avi(:");
                    this.firetr87Established();
                }
            }
            this.updateMycalls(lcall, state);
            if (isCallStateReallyChanged)
            {
                this.callViewInitiaterHandler(state);
                this.fireOnMycallsUpdated();
            }

        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        private void fireOnMycallsUpdated()
        {
            if (this.callsListener != null)
            {
                this.callsListener.onListUpdated(myCalls);
            }
        }

        private void callViewInitiaterHandler(CallState state)
        {
            pendingcallView = false;
            if (callViewInitiater != null)
            {

                if (LinphoneCore.CallsNb > 0)
                {
                    callViewInitiater.onInitiateCallView();
                }
                else
                {
                    callViewInitiater.onDestroyCallView();

                }

            }
            else
            {
                if (LinphoneCore.CallsNb > 0)
                    pendingcallView = true;
            }
        }

        private void setCurrentCall()
        {
            Call call = LinphoneCore.CurrentCall;
            if (call != null)
            {
                for (int i = 0; i < myCalls.Count; i++)
                {
                    if (myCalls[i].remoteAddress.Equals(call.RemoteAddressAsString))
                    {

                        if (!myCalls[i].isCurrentCall)
                        {
                            myCalls[i].isCurrentCall = true;
                            onCurrentCallChanged(call);

                        }
                    }
                    else
                    {
                        myCalls[i].isCurrentCall = false;
                    }
                }
            }
            else if (!isPaused)
            {
                if (myCalls.Count > 0 && LinphoneCore.CallsNb > 0)
                {
                    Call c = LinphoneCore.GetCallByRemoteAddress(myCalls[0].remoteAddress);
                    if (c != null && c.State == CallState.Paused)
                        c.Resume();

                }
            }

        }

        private void setCurrentCallOnTop()
        {
            myCalls.Sort(delegate (MyCall x, MyCall y)
           {
               int a = x.isCurrentCall ? 0 : 1;
               return a;
           });
        }


        private void onCurrentCallChanged(Call call)
        {
            this.isMute = false;
            this.isPaused = false;
            this.isSpeaker = false;
        }


        private void updateMycalls(Call call, CallState state)
        {
            int relevantIndex = -1;
            for (int i = 0; i < myCalls.Count; i++)
            {
                if (myCalls[i].remoteAddress.Equals(call.RemoteAddressAsString))
                {
                    relevantIndex = i;
                    break;
                }
            }

            if (relevantIndex > -1)
            {
                if (state == CallState.Released)
                {
                    myCalls.RemoveAt(relevantIndex);
                    this.isCallStateReallyChanged = true;
                }
                else
                {
                    if (myCalls[relevantIndex].state == state)
                    {
                        isCallStateReallyChanged = true;
                    }
                    else
                    {
                        myCalls[relevantIndex].state = state;
                        isCallStateReallyChanged = true;
                    }
                }
            }
            else
            {
                MyCall newCall = new MyCall(call.ToAddress.Username, call.RemoteAddressAsString, state);
                myCalls.Add(newCall);
                this.isCallStateReallyChanged = true;
            }
            setCurrentCall();
            setCurrentCallOnTop();
        }

        CallService()
        {
            LinphoneCore = LinphoneBase.Instance.linphoneCore;
            Listener = LinphoneBase.Instance.coreListener;
            Listener.OnCallStateChanged = OnCall;
            LinphoneCore.AddListener(Listener);
        }

        public static CallService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CallService();
                    }
                    return instance;
                }
            }
        }

        public void makeTr87Call()
        {
            var addr = LinphoneCore.InterpretUrl("1234");

            CallParams cp = LinphoneCore.CreateCallParams(null);
            cp.AddCustomHeader("Content-Disposition", "signal;handling=required");
            cp.AddCustomHeader("TR87-Mode", "true");
            cp.AddCustomHeader("User-Agent", "Tadiran ATouch PC/1.0.201 (belle-sip/1.6.3)");
            // cp.AddCustomHeader("User-Agent", "Tadiran ATouch Android/1.0.201 (belle-sip/1.6.3)");
            //proxyConfig.SetCustomHeader("User-Agent", "Tadiran ATouch Android/1.0.201 (belle-sip/1.6.3)");
            LinphoneCore.InviteAddressWithParams(addr, cp);
            //LinphoneCore.Subscribe(addr, "Tr87Subscription", 3600, null);
        }


        public void call(string address)
        {
            var addr = LinphoneCore.InterpretUrl(address);
            if (LinphoneCore.CallsNb == 0)
            {
                LinphoneCore.InviteAddress(addr);
            }
            else
            {
                string remoteAddress = "";
                for (int i = 0; i < myCalls.Count; i++)
                {
                    if (myCalls[i].alias.Equals(address))
                    {
                        remoteAddress = myCalls[i].remoteAddress;
                    }
                }
                if (remoteAddress.Length == 0)
                {
                    LinphoneCore.InviteAddress(addr);
                }
            }
        }

        public void answerCall()
        {
            for (int i = 0; i < myCalls.Count; i++)
            {
                if (myCalls[i].state == CallState.IncomingReceived)
                {
                    Call call = LinphoneCore.GetCallByRemoteAddress(myCalls[i].remoteAddress);
                    if (call != null)
                    {
                        LinphoneCore.AcceptCall(call);
                    }
                    break;
                }
            }
            //Call call = LinphoneCore.CurrentCall;
            //if (call != null && call.State == CallState.IncomingReceived)
            //    LinphoneCore.AcceptCall(call);
        }

        public void terminateCall()
        {
            Call call = LinphoneCore.CurrentCall;
            if (call != null)
                LinphoneCore.TerminateCall(call);

        }

        public void toggleHold()
        {
            Call call = LinphoneCore.GetCallByRemoteAddress(myCalls[0].remoteAddress);

            if (call.State == CallState.Paused)
            {
                call.Resume();
                isPaused = false;
            }
            else if (call.State == CallState.StreamsRunning)
            {
                call.Pause();
                isPaused = true;
            }
        }

        public async System.Threading.Tasks.Task toggleSpeakerAsync()
        {
            await CrossMediaManager.Current.Play("");
        }

        public void conferenceCalls()
        {
            ConferenceParams c = LinphoneCore.CreateConferenceParams();
            Conference conference = LinphoneCore.CreateConferenceWithParams(c);
            foreach (MyCall mycall in myCalls)
            {
                LinphoneCore.AddToConference(LinphoneCore.GetCallByRemoteAddress(mycall.remoteAddress));
            }
        }

        public void switchCurrentCall(MyCall desiredCall)
        {
            Call call = LinphoneCore.GetCallByRemoteAddress(desiredCall.remoteAddress);
            LinphoneCore.ResumeCall(call);
        }


        public void setCallsListener(CallsListener callsListener)
        {
            this.callsListener = callsListener;
        }

        public void setTr87Listener(Tr87stateListener tr87StateListener)
        {
            this.tr87StateListener = tr87StateListener;
        }

        private void firetr87Established()
        {
            if (this.tr87StateListener != null)
            {
                this.tr87StateListener.onTr87Established();
            }
        }

        public void setCallsViewInitiater(CallViewInitiater callViewInitiater)
        {
            this.callViewInitiater = callViewInitiater;
        }


    }
}