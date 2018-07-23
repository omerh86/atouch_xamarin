using LinphoneXamarin.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LinphoneXamarin.Util
{


    public sealed class MySendRequestHelper
    {

        private static MySendRequestHelper instance = null;
        private static readonly object padlock = new object();


        MySendRequestHelper()
        {


        }
        public static MySendRequestHelper Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MySendRequestHelper();
                    }
                    return instance;
                }
            }
        }

   
     public string getConnectionRequest(ConnectionProp connectionProp)
        {
            ConnectionRequest connectionRequest = new ConnectionRequest(connectionProp);
            return objToJson<ConnectionRequest>(connectionRequest);
        }

       public struct ConnectionProp
        {
            public string token;

            public ConnectionProp(string a)
            {
                token = a;
            }
        }

        private struct ConnectionRequest
        {
            public ConnectionProp fcmToken;

            public ConnectionRequest(ConnectionProp p)
            {
                fcmToken = p;
            }
        }

        private T jsonToObj<T>(string val)
        {
            T loginInfo = JsonConvert.DeserializeObject<T>(val);
            return loginInfo;
        }

        private string objToJson<T>(T obj)
        {
            string res = JsonConvert.SerializeObject(obj);
            return res;
        }

    }
}
