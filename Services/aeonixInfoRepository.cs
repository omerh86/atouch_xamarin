using System;
using LinphoneXamarin.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Linphone;
using LinphoneXamarin.Util;
using static LinphoneXamarin.Util.MySendRequestHelper;

namespace LinphoneXamarin.Services
{
    public sealed class AeonixInfoRepository
    {

        private static AeonixInfoRepository instance = null;
        private static readonly object padlock = new object();

        private Core LinphoneCore;
        private CoreListener Listener;
        MyInfoListener myInfoListener;


        private void OnInfoRecived(Core lc, Call call, InfoMessage msg)
        {

            try
            {
                if (this.myInfoListener != null)
                    this.myInfoListener.onInfoResponse(msg);

            }
            catch (Exception e)
            {
                Console.WriteLine("omer40" + e);
            }

        }


        AeonixInfoRepository()
        {
            LinphoneCore = LinphoneBase.Instance.linphoneCore;
            Listener = LinphoneBase.Instance.coreListener;
            Listener.OnInfoReceived = OnInfoRecived;
            LinphoneCore.AddListener(Listener);
        }

        public static AeonixInfoRepository Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new AeonixInfoRepository();
                    }
                    return instance;
                }
            }
        }

        public void setInfoListener(MyInfoListener myInfoListener)
        {
            this.myInfoListener = myInfoListener;
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