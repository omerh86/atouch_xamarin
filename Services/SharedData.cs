using System;
using System.Collections.Generic;
using System.Text;

namespace LinphoneXamarin.Services
{
   
    class SharedData
    {
       
        public string FCMToken { set; get; }

        private static SharedData instance = null;
        private static readonly object padlock = new object();


        public static SharedData Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SharedData();
                    }
                    return instance;
                }
            }
        }

    }
}
