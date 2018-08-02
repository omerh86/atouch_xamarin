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
        private LinphoneRegistrationListener registrationListener;
        private Core LinphoneCore;
        private CoreListener Listener;

        private void OnRegistration(Core lc, ProxyConfig config, RegistrationState state, string message)
        {
            if (this.registrationListener != null && this.registrationState != state)
            {
                registrationListener.onLinphoneStatusChanged(state, message);
            }

            this.registrationState = state;
        }

        RegistrationService()
        {
            LinphoneCore = LinphoneBase.Instance.linphoneCore;
            Listener = LinphoneBase.Instance.coreListener;
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

        public void setRegistrationListener(LinphoneRegistrationListener registrationListener)
        {
            this.registrationListener = registrationListener;
        }

        public void register(string name, string password, string ip)
        {
            var authInfo = Factory.Instance.CreateAuthInfo(name, null, password, null, null, ip);

            LinphoneCore.AddAuthInfo(authInfo);
            var proxyConfig = LinphoneCore.CreateProxyConfig();
            //  var identity = Factory.Instance.CreateAddress("sip:sample@domain.tld");
            var identity = Factory.Instance.CreateAddress("sip:" + name + "@" + ip);
            identity.Username = name;
            identity.Domain = ip;
            identity.Transport = TransportType.Tcp;
            identity.Port = 5060;
            proxyConfig.Edit();

            proxyConfig.SetCustomHeader("User-Agent", "Tadiran ATouch PC/1.0.201 (belle-sip/1.6.3)");
            // proxyConfig.SetCustomHeader("User-Agent", "Tadiran ATouch Android/1.0.201 (belle-sip/1.6.3)");
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
