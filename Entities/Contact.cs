using System;
using Linphone;

namespace LinphoneXamarin.Entities
{
    public class Contact
    {

        public string userName { set; get; }
        public string displayName { set; get; }
        public int alias { set; get; }
        public bool isFav{ set; get; }

        public Contact(string userName, string displayName, int alias)
        {
            this.userName = userName;
            this.displayName = displayName;
            this.alias = alias;
            this.isFav = false;
        }

    }
}
