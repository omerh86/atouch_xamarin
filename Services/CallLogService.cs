using System;
using LinphoneXamarin.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Linphone;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace LinphoneXamarin.Services
{
    public sealed class CallLogService
    {

        private List<MyCallLog> allCallsLog = new List<MyCallLog>();
        private static CallLogService instance = null;
        private static readonly object padlock = new object();

        CallLogService()
        {
            this.addCallsLog();
        }

        public static CallLogService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CallLogService();
                    }
                    return instance;
                }
            }
        }

        public List<MyCallLog> getCallsLog()
        {
            return allCallsLog;
        }

        public void addCallsLog()
        {
            var x = new List<CallInfo>();
            x.Add(new CallInfo(CallDirection.Missed, 2121, 4345));
            x.Add(new CallInfo(CallDirection.OutGoing, 1212, 333));
            allCallsLog.Add(new MyCallLog("2007", "1", "yuval", null, true, x));
            allCallsLog.Add(new MyCallLog("2008", "2", "roi", null, false, x));
            allCallsLog.Add(new MyCallLog("2008", "2", "roi", null, false, x));
            allCallsLog.Add(new MyCallLog("2008", "2", "roi", null, false, x));
            allCallsLog.Add(new MyCallLog("2008", "2", "roi", null, false, x));
            allCallsLog.Add(new MyCallLog("2008", "2", "roi", null, false, x));
            allCallsLog.Add(new MyCallLog("2008", "2", "roi", null, false, x));
            allCallsLog.Add(new MyCallLog("2008", "2", "roi", null, false, x));
            allCallsLog.Add(new MyCallLog("2008", "2", "roi", null, false, x));

        }


    }
}