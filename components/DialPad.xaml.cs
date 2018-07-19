using System;
using LinphoneXamarin.Services;
using Xamarin.Forms;


namespace LinphoneXamarin.components
{

    public partial class DialPad : ContentPage
    {
        string phoneNumber = "";
        public DialPad()
        {
            InitializeComponent();
            this.BindingContext = phoneNumber;
        }

        private void onDialPadClicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (phoneNumber.Length == 0)
                phoneNumber = btn.Text;
            else
            {
                phoneNumber=phoneNumber.Insert(phoneNumber.Length, btn.Text);
            }
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