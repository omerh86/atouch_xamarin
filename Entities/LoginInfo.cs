using System;
using Linphone;

namespace LinphoneXamarin.Entities
{
    public class LoginInfo
    {
        public string name { set; get; }
        public string password { set; get; }
        public string ip { set; get; }

        public LoginInfo(string name, string password, string ip)
        {
            this.name = name;
            this.password = password;
            this.ip = ip;

        }

    }
}
