using System;
using Linphone;

namespace LinphoneXamarin.Entities
{
    public class MyCall
    {

        public string alias { set; get; }
        public string remoteAddress { set; get; }
        public CallState state { set; get; }
        public bool isCurrentCall{ set; get; }

        public MyCall(string alias, string remoteAddress, CallState state)
        {
            this.alias = alias;
            this.remoteAddress = remoteAddress;
            this.state = state;
            this.isCurrentCall = false;
        }
        public override string ToString()
        {
            return this.alias + " , " + state.ToString();
        }

    }
}
