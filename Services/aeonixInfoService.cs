using System;
using LinphoneXamarin.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Linphone;
namespace LinphoneXamarin.Services
{
    public sealed class AeonixInfoService
    {

        private static AeonixInfoService instance = null;
        private static readonly object padlock = new object();

        private Core LinphoneCore
        {
            get
            {
                return ((App)App.Current).LinphoneCore;
            }
        }

        private void OnInfoRecived(Core lc, Call call, InfoMessage msg)
        {
            Console.WriteLine("omer40" + msg.ToString());
           
        }

        private void onNotifyReceived(Core lc, Object ev, string eventName, Object content)
        {
            Console.WriteLine("omer40" + eventName.ToString());
        }

        private void globalState(Core lc, GlobalState state, String message)
        {
            Console.WriteLine("omer40" + message.ToString());
        }

       


        private CoreListener Listener;

        AeonixInfoService()
        {
            Listener = ((App)App.Current).coreListener;
            Listener.OnInfoReceived = OnInfoRecived;
            Listener.OnNotifyReceived = onNotifyReceived;
            Listener.OnGlobalStateChanged = globalState;
            LinphoneCore.AddListener(Listener);
          }

        public static AeonixInfoService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new AeonixInfoService();
                    }
                    return instance;
                }
            }
        }

        public void sendToInfoAeonix(string info)
        {
            Call call = CallService.Instance.tr87Call;
            string sendInfo = info + "\r\n";

            InfoMessage infoMessage = LinphoneCore.CreateInfoMessage();
            infoMessage.AddHeader("Content-Disposition", "signal; handling=required");
            infoMessage.AddHeader("Content-Type", "application/csta+v6+json");

            Content content = LinphoneCore.CreateContent();
            content.Type = "application";
            content.Subtype = "csta+v6+json";
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] sendInfoBytes = encoding.GetBytes(sendInfo);
            IntPtr unmanagedPointer = System.Runtime.InteropServices.Marshal.AllocHGlobal(sendInfoBytes.Length);
            System.Runtime.InteropServices.Marshal.Copy(sendInfoBytes, 0, unmanagedPointer, sendInfoBytes.Length);
            content.SetBuffer(unmanagedPointer, sendInfoBytes.Length);

            // Call unmanaged code
            System.Runtime.InteropServices.Marshal.FreeHGlobal(unmanagedPointer);
            infoMessage.Content = content;

            call.SendInfoMessage(infoMessage);
          
        }


    }
}