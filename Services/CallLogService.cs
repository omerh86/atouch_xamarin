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

        private ObservableCollection<MyCallLog> allCallsLog = new ObservableCollection<MyCallLog>();
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

        public ObservableCollection<MyCallLog> getCallsLog()
        {
            this.addCallsLog();
            return allCallsLog;
        }

        public void addCallsLog()
        {
            allCallsLog = new ObservableCollection<MyCallLog>();
            var x = new List<CallInfo>();
            x.Add(new CallInfo(CallDirection.Missed, 2121, 4345));
            x.Add(new CallInfo(CallDirection.OutGoing, 1212, 333));
            doAddCallLogs("2007", x);
            doAddCallLogs("2008", x);
            doAddCallLogs("2009", x);
            doAddCallLogs("2007", x);
            doAddCallLogs("2008", x);
            doAddCallLogs("2009", x);
            doAddCallLogs("2007", x);
            doAddCallLogs("2008", x);
            doAddCallLogs("2009", x);
        }

        private void doAddCallLogs(string alias, List<CallInfo> callsInfo)
        {
            Contact contact = ContactService.Instance.getContactByAlias(alias);
            if (contact != null)
            {
                allCallsLog.Add(new MyCallLog(contact.primaryAlias, contact.userName, contact.displayName, null, contact.isFav, callsInfo));
            }
            else
            {
                allCallsLog.Add(new MyCallLog(alias, "", "Unknown", null, false, callsInfo));
            }
        }


    }
}