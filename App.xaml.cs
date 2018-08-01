using System;
using System.Threading;

using Xamarin.Forms;
using LinphoneXamarin.components;

using Linphone;
using System.Net;
using System.IO;
using System.Reflection;
using LinphoneXamarin.Util;
using LinphoneXamarin.Entities;
using LinphoneXamarin.Services;
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

        public Core LinphoneCore { get; set; }
        public CoreListener coreListener { get; set; }

        

        public App(string rc_path = null)
        {

            LinphoneWrapper.setNativeLogHandler();
            LinphoneCore = LinphoneBase.Instance.linphoneCore;
            coreListener = LinphoneBase.Instance.coreListener;
            MainPage = new NavigationPage(new components.Login());


        }


        public StackLayout getLayoutView()
        {

            return MainPage.FindByName<StackLayout>("stack_layout");
        }




        protected override void OnStart()
        {
            // Handle when your app starts
#if WINDOWS_UWP
            TimeSpan period = TimeSpan.FromSeconds(1);
            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer(LinphoneCoreIterate , period);
#else

            LinphoneBase.Instance.startForgroundItreation();
            Console.WriteLine("omer50: onstart");
#endif
        }

        protected override void OnSleep()
        {

            Console.WriteLine("omer50: onsleep");
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            Console.WriteLine("omer50: onresume");
            // Handle when your app resumes
        }
    }
}
