using System;
using Linphone;

namespace LinphoneXamarin.Entities
{
    public class Contact
    {

        public string userName { set; get; }
        public string displayName { set; get; }
        public string alias { set; get; }
        public bool isFav { set; get; }
        public bool isAeonix { set; get; }

        public Contact(string userName, string displayName, string alias, bool isAeonix)
        {
            this.userName = userName;
            this.displayName = displayName;
            this.alias = alias;
            this.isFav = false;
            this.isAeonix = isAeonix;
        }

    }
}
