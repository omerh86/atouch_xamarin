using System;
using Linphone;
using LinphoneXamarin.Entities;

namespace LinphoneXamarin.Services
{



    public sealed class RegistrationService
    {

        private static RegistrationService instance = null;
        private static readonly object padlock = new object();

        public RegistrationState registrationState = RegistrationState.None;
        private RegistrationListener registrationListener;
        private Core LinphoneCore
        {
            get
            {
                return ((App)App.Current).LinphoneCore;
            }
        }
        private CoreListener Listener;
        private void OnRegistration(Core lc, ProxyConfig config, RegistrationState state, string message)
        {

            if (this.registrationListener != null && this.registrationState != state)
            {
                registrationListener.onStatusChanged(state);
            }
            this.registrationState = state;
        }

        RegistrationService()
        {
            Listener = Factory.Instance.CreateCoreListener();
            Listener.OnRegistrationStateChanged = OnRegistration;
            LinphoneCore.AddListener(Listener);
            logger();

        }

        public static RegistrationService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new RegistrationService();
                    }
                    return instance;
                }
            }
        }

        public void logger()
        {
            Core.ResetLogCollection();
            Core.EnableLogCollection(LogCollectionState.Enabled);
            LinphoneWrapper.setNativeLogHandler();
            LoggingService.Instance.SetLogFile("Aeonix", "linphone_Log", 100000);
            LoggingService.Instance.LogLevel = LogLevel.Debug;

        }

        public void setRegistrationListener(RegistrationListener registrationListener)
        {
            this.registrationListener = registrationListener;
        }

        public void register(RegistrationListener registrationListener, string name, string password, string ip)
        {

            setRegistrationListener(registrationListener);

            var authInfo = Factory.Instance.CreateAuthInfo(name, null, password, null, null, ip);
            
            LinphoneCore.AddAuthInfo(authInfo);
            var proxyConfig = LinphoneCore.CreateProxyConfig();
            //  var identity = Factory.Instance.CreateAddress("sip:sample@domain.tld");
            var identity = Factory.Instance.CreateAddress("sip:" + name + "@" + ip);
            identity.Username = name;
            identity.Domain = ip;
            identity.Transport = TransportType.Tcp;
            //identity.Port = 5083;
            proxyConfig.Edit();
            proxyConfig.IdentityAddress = identity;
            proxyConfig.ServerAddr = identity.AsString();
            proxyConfig.Route = identity.AsString();
            proxyConfig.RegisterEnabled = true;
            proxyConfig.Done();
            LinphoneCore.AddProxyConfig(proxyConfig);
            LinphoneCore.DefaultProxyConfig = proxyConfig;

            LinphoneCore.RefreshRegisters();
        }


        public void unRegister()
        {
            if (LinphoneCore.DefaultProxyConfig != null)
            {
                LinphoneCore.DefaultProxyConfig.Edit();
                LinphoneCore.DefaultProxyConfig.RegisterEnabled = false;
                LinphoneCore.DefaultProxyConfig.Done();
                LinphoneCore.RefreshRegisters();
            }



        }
    }
}
