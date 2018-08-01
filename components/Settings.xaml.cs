using LinphoneXamarin.Services;
using LinphoneXamarin.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static LinphoneXamarin.Util.MySendRequestHelper;

namespace LinphoneXamarin.components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        MySendRequestHelper mySendRequestHelper;

        public Settings()
        {
            InitializeComponent();
            mySendRequestHelper = MySendRequestHelper.Instance;

        }


        private void OnSendToken(object sender, EventArgs e)
        {
            string token = SharedData.Instance.FCMToken;
            string strToSend = MySendRequestHelper.Instance.getFcmRequest(new MySendRequestHelper.FcmProp("AMIR_4002", token));
            AeonixInfoRepository.Instance.sendToInfoAeonix(strToSend);

        }

        private void getPic(object sender, EventArgs e)
        {
            string strToSend = mySendRequestHelper.GetPictureRequest(new UserNameProp("AMIR_4002"));
            Console.WriteLine("omer40: " + strToSend);

        }

        private void NextResponseRequest(object sender, EventArgs e)
        {
            string strToSend = mySendRequestHelper.GetNextResponseRequest();
            Console.WriteLine("omer40: " + strToSend);

        }

        private void CreateAccountRequest(object sender, EventArgs e)
        {
            string strToSend = mySendRequestHelper.getCreateAccountRequest(new CreateAccountProp("ln", "pn", false, "111", true));
            Console.WriteLine("omer40: " + strToSend);

        }

        private void GetRsUserRequest(object sender, EventArgs e)
        {
            string strToSend = mySendRequestHelper.GetRsUserRequest(new UserNameProp("AMIR_4002"));
            Console.WriteLine("omer40: " + strToSend);

        }
    }
}