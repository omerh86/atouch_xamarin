using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Linphone;
using Xamarin.Forms;

namespace LinphoneXamarin.Services
{
    class LinphoneBase
    {

        public Core linphoneCore { get; set; }
        public CoreListener coreListener { get; set; }
        public bool shoulIterate { get; set; }
        Thread iterateBackground;
        Thread iterateforground;

        private static LinphoneBase instance = null;
        private static readonly object padlock = new object();

        public static LinphoneBase Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new LinphoneBase();
                    }
                    return instance;
                }
            }
        }

        public LinphoneBase()
        {
            Console.WriteLine("omer50: initialize");
            Core.SetLogLevelMask(0xFF);
            CoreListener listener = Factory.Instance.CreateCoreListener();
            listener.OnGlobalStateChanged = OnGlobal;
            linphoneCore = Factory.Instance.CreateCore(listener, null, null);
            coreListener = Factory.Instance.CreateCoreListener();
            linphoneCore.VerifyServerCertificates(false);
            linphoneCore.VerifyServerCn(false);
            linphoneCore.NetworkReachable = true;

            //if (App.Current != null)
            //    iterate = new Thread(startForgroundLinphoneIteration);
            //else
            //    iterate = new Thread(startBackgroundLinphoneIteration);

            //iterate.IsBackground = false;
            //iterate.Start();
            //Thread iterate = new Thread(LinphoneCoreIterate);
        }

        public void startBackgroundItreation()
        {
            if (iterateBackground == null || !iterateBackground.IsAlive)
            {
                iterateBackground = new Thread(doBackgroundLinphoneIteration);
                iterateBackground.IsBackground = false;
                iterateBackground.Start();
            }

        }

        public void startForgroundItreation()
        {
            if (iterateforground == null || !iterateforground.IsAlive)
            {
                iterateforground = new Thread(doForgroundLinphoneIteration);
                iterateforground.IsBackground = false;
                iterateforground.Start();
            }
        }


        private void doForgroundLinphoneIteration()
        {
            while (true)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    linphoneCore.Iterate();
                });
            
                Thread.Sleep(500);
            }
        }

        private void doBackgroundLinphoneIteration()
        {
            int count = 0;
            while (count < 5)
            {
                linphoneCore.Iterate();
                count++;
               
                Thread.Sleep(10000);
            }
            instance = null;
        }


        private void OnGlobal(Core lc, GlobalState gstate, string message)
        {
#if WINDOWS_UWP
            Debug.WriteLine("Global state changed: " + gstate);
#else
            Console.WriteLine("Global state changed: " + gstate);
#endif
        }

        //private void LinphoneCoreIterate()
        //{

        //    while (true)
        //    {
        //        if (isBackgroundMode)
        //        {
        //            linphoneCore.Iterate();

        //        }
        //        else
        //        {
        //            Device.BeginInvokeOnMainThread(() =>
        //            {
        //                linphoneCore.Iterate();
        //            });
        //        }
        //        Thread.Sleep(50);
        //    }
        //}


    }





}
