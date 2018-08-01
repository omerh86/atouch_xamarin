using LinphoneXamarin.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LinphoneXamarin.Util
{


    public sealed class MyFileSystem
    {

        private static MyFileSystem instance = null;
        private static readonly object padlock = new object();

        private readonly string LOGIN_CARDENTIAL_TR87 = "LoginCardentialTr87.txt";
        private readonly string LOGIN_CARDENTIAL_AEONIX = "LoginCardentialAeonix.txt";

        MyFileSystem()
        {


        }
        public static MyFileSystem Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MyFileSystem();
                    }
                    return instance;
                }
            }
        }

        public void saveLoginCardential(LoginInfo loginInfo, CardentialState cardentialState)
        {
            string s = "";
            if (loginInfo != null)
            {
                s = objToJson<LoginInfo>(loginInfo);
            }
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), cardentialState == CardentialState.TR87 ? LOGIN_CARDENTIAL_TR87 : LOGIN_CARDENTIAL_AEONIX);
            File.WriteAllText(fileName, s);

        }

        public LoginInfo loadLoginCardential(CardentialState cardentialState)
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), cardentialState == CardentialState.TR87 ? LOGIN_CARDENTIAL_TR87 : LOGIN_CARDENTIAL_AEONIX);
            if (!File.Exists(fileName))
            {
                return null;
            }
            string s = File.ReadAllText(fileName);
            if (s != null && s!= "")
            {
                LoginInfo loginInfo = jsonToObj<LoginInfo>(s);
                return loginInfo;
            }
            return null;

        }

        public T jsonToObj<T>(string val)
        {
            T loginInfo = JsonConvert.DeserializeObject<T>(val);
            return loginInfo;
        }

        public string objToJson<T>(T obj)
        {
            string res = JsonConvert.SerializeObject(obj);
            return res;
        }


    }
}
