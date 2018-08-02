using System;
using System.Collections.Generic;
using Linphone;
using LinphoneXamarin.Entities;
using LinphoneXamarin.Services;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

using Xamarin.Forms;

namespace LinphoneXamarin.components
{
    public partial class Tabs : TabbedPage
    {
      
        public Tabs()
        {
           InitializeComponent();
            On<iOS>().SetUseSafeArea(false);
        }

       

     
    }
}
