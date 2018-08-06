using LinphoneXamarin.Entities;
using LinphoneXamarin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinphoneXamarin.components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Recent : ContentPage
    {
        CallLogService callLogService;
        private ObservableCollection<MyCallLog> allCallsLog;

        public Recent()
        {
            InitializeComponent();
            callLogService = CallLogService.Instance;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.Write("omer40: recent on appearing");
            resetCallLog();
        }

        public void resetCallLog()
        {
            allCallsLog = new ObservableCollection<MyCallLog>();
            allCallsLog = callLogService.getCallsLog();
            favorites.ItemsSource = allCallsLog;
        }
    }
}