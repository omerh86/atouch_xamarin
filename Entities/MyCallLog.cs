using System;
using System.Collections.Generic;
using System.ComponentModel;
using Linphone;

namespace LinphoneXamarin.Entities
{
    public class MyCallLog : INotifyPropertyChanged
    {
        public string alias { set; get; }
        public string userName { set; get; }
        public string displayName { set; get; }
        public string img { set; get; }
        bool _IsFav;
        public bool isFav
        {
            get { return _IsFav; }
            set
            {
                _IsFav = value;
                NotifyPropertyChanged("isFav");
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
