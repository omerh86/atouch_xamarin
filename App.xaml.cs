using System;
using System.Threading;

using Xamarin.Forms;
using LinphoneXamarin.components;

using Linphone;
using System.Net;
using System.IO;
using System.Reflection;
#if WINDOWS_UWP
using System.Diagnostics;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
#endif

namespace LinphoneXamarin
{
    public partial class App : Application
    {
        public static MainViewModel viewModel;
        public Core LinphoneCore { get; set; }
        public CoreListener coreListener { get; set; }


        public App(string rc_path = null)
        {

            LinphoneWrapper.setNativeLogHandler();
            viewModel = new MainViewModel();
            Core.SetLogLevelMask(0xFF);
            CoreListener listener = Factory.Instance.CreateCoreListener();
            listener.OnGlobalStateChanged = OnGlobal;
            LinphoneCore = Factory.Instance.CreateCore(listener, rc_path, null);
            coreListener= Factory.Instance.CreateCoreListener();
            setLinphoneSettings();

            MainPage = new NavigationPage(new components.Login());
            MainPage.Title = "Aeonix";
            var settings = new ToolbarItem
            {
                Icon = "icon.png",
                Command = new Command(this.ShowSettingsPage),
            };
            MainPage.ToolbarItems.Add(settings);
            
        }

        private void ShowSettingsPage()
        {

            //this.Navigation.PushAsync(new SettingsPage());
        }

        private void setLinphoneSettings()
        {

            LinphoneCore.VerifyServerCertificates(false);
            LinphoneCore.VerifyServerCn(false);
            LinphoneCore.NetworkReachable = true;
            LinphoneCore.Ring = "Resource/ring.wav";
            LinphoneCore.Ringback = "Resource/ring.wav";
            LinphoneCore.RingDuringIncomingEarlyMedia = true;
            LinphoneCore.RemoteRingbackTone = "LinphoneXamarin.share_res.ring.wav";

            //Log.Info("omer", "" + LinphoneCore.Ringback);
            //Log.Info("omer2", "" + LinphoneCore.Ring);

            var assembly = typeof(App).GetTypeInfo().Assembly;


        }



        public StackLayout getLayoutView()
        {

            return MainPage.FindByName<StackLayout>("stack_layout");
        }

#if WINDOWS_UWP
        private void LinphoneCoreIterate(ThreadPoolTimer timer) {
#else
        private void LinphoneCoreIterate()
        {
#endif
            while (true)
            {
#if WINDOWS_UWP
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High,
                () => {
                    LinphoneCore.Iterate();
                });
#else
                Device.BeginInvokeOnMainThread(() =>
                {
                    LinphoneCore.Iterate();
                });
                Thread.Sleep(50);
#endif
            }
        }


        private void OnGlobal(Core lc, GlobalState gstate, string message)
        {
#if WINDOWS_UWP
            Debug.WriteLine("Global state changed: " + gstate);
#else
            Console.WriteLine("Global state changed: " + gstate);
#endif
        }

        protected override void OnStart()
        {
            // Handle when your app starts
#if WINDOWS_UWP
            TimeSpan period = TimeSpan.FromSeconds(1);
            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer(LinphoneCoreIterate , period);
#else
            Thread iterate = new Thread(LinphoneCoreIterate);
            iterate.IsBackground = false;
            iterate.Start();
#endif
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
