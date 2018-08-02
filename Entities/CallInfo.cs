using System;
using Linphone;

namespace LinphoneXamarin.Entities
{
    public class CallInfo
    {
        
        public CallDirection callDirection { set; get; }
        public long duration { set; get; }
        public long time { set; get; }


        public CallInfo(CallDirection callDirection, long duration, long time)
        {
            this.callDirection = callDirection;
            this.duration = duration;
            this.time = time;
        }

    }
}
