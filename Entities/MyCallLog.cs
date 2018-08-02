using System;
using System.Collections.Generic;
using Linphone;

namespace LinphoneXamarin.Entities
{
    public class MyCallLog
    {
        public string alias { set; get; }
        public string userName { set; get; }
        public string displayName { set; get; }
        public string img { set; get; }
        public bool isFav { set; get; }
        public List<CallInfo> callsInfo { set; get; }


        public MyCallLog(string alias, string userName, string displayName, string img, bool isFav, List<CallInfo> callsInfo)
        {
            this.alias = alias;
            this.userName = userName;
            this.img = img;
            this.callsInfo = callsInfo;
            this.isFav = isFav;
            this.displayName = displayName;
        }

    }
}
