using System;
using Xamarin.Forms;
using LinphoneXamarin.Services;
using LinphoneXamarin.Entities;

using Linphone;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

namespace LinphoneXamarin.components
{

    public partial class MainPage : ContentPage, CallsListener
    {


        CallService callService;

        List<MyCall> callsList;


        public MainPage()
        {
            InitializeComponent();
            callService = CallService.Instance;
            callsList = callService.myCalls;
            this.BindingContext = callsList;
            this.setActionsStatus();
            callService.setCallsListener(this);

        }

        public void onListUpdated(List<MyCall> list)
        {
            this.callsList = list;
            //this.BindingContext = callsList;
            this.setActionsStatus();

        }

        private void setActionsStatus()
        {
            if (callsList.Count > 0 && callsList[0] != null)
            {
                this.BindingContext = callsList[0];
                switch (callsList[0].state)
                {
                    case CallState.IncomingReceived:
                        setAnswerMode();
                        break;
                    case CallState.OutgoingRinging:
                        setOutgoingMode();
                        break;
                    case CallState.StreamsRunning:
                        setStreamMode();
                        break;
                }

            }
            if (callService.isPaused)
                hold.TextColor = Color.AliceBlue;
            else
                hold.TextColor = Color.DimGray;

        }


        private void OnAnswerClicked(object sender, EventArgs e)
        {
            callService.answerCall();

        }

        private void OnTerminateClicked(object sender, EventArgs e)
        {
            callService.terminateCall();
        }

        private void OnHoldClicked(object sender, EventArgs e)
        {
            callService.toggleHold();
        }

        private void setCurrentCall(object sender, ItemTappedEventArgs e)
        {
            MyCall selected = e.Item as MyCall;
            if (!selected.isCurrentCall)
                callService.switchCurrentCall(selected);
        }

        private void onConferenceCalls(object sender, EventArgs e)
        {
            callService.conferenceCalls();
        }

        private void onToggleSpeakerAsync(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            callService.toggleSpeakerAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void setAnswerMode()
        {
            controlGrid.Children.Clear();

            controlGrid.Children.Add(Answer, 0, 2, 0, 2);
            controlGrid.Children.Add(Terminate, 2, 4, 0, 2);
        }

        private void setStreamMode()
        {
            controlGrid.Children.Clear();
            controlGrid.Children.Add(hold, 0, 0);
            controlGrid.Children.Add(conference, 1, 0);
            controlGrid.Children.Add(speaker, 2, 0);
            controlGrid.Children.Add(settings, 3, 0);
            controlGrid.Children.Add(Terminate, 1, 3, 1, 2);
            controlGrid.RowSpacing = 20;
        }

        private void setOutgoingMode()
        {
            controlGrid.Children.Clear();
            controlGrid.Children.Add(Terminate, 1, 3, 1, 2);
            controlGrid.RowSpacing = 20;
        }
    }
}