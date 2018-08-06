using System;
using System.Collections.Generic;
using LinphoneXamarin.Services;
using Xamarin.Forms;

namespace LinphoneXamarin.components
{
    public partial class CallLog : ContentPage
    {
        string phoneNumber = "";
        public CallLog()
        {
            InitializeComponent();
            this.BindingContext = phoneNumber;
        }

      
        private void onDialPadClicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            phoneNumber += btn.Text;
            this.BindingContext = phoneNumber;
        }

        private void OnCallClicked(object sender, EventArgs e)
        {
            CallService.Instance.call(phoneNumber);

        }

        private void clear(object sender, EventArgs e)
        {
            if (phoneNumber.Length > 0)
            {
                phoneNumber = phoneNumber.Substring(0, phoneNumber.Length - 1);
                this.BindingContext = phoneNumber;
            }

        }

    }
}
