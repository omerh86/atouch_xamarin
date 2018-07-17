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
            this.callsList.ForEach(c =>
            {
                Console.WriteLine("omeriko", "" + c.alias);
            });
            this.BindingContext = callsList;
            if (this.callsList.Count == 0)
            {
               // Navigation.RemovePage(this);
            }
            this.setActionsStatus();

        }

        private void setActionsStatus()
        {
            if (callService.isPaused)
                hold.BackgroundColor = Color.Aqua;
            else
                hold.BackgroundColor = Color.Transparent;

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

    }
}
