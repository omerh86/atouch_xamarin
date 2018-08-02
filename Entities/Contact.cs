using System;
using Linphone;

namespace LinphoneXamarin.Entities
{
    public class Contact
    {
        public string userName { set; get; }
        public string displayName { set; get; }
        public string primaryAlias { set; get; }
        public bool isFav { set; get; }
        public ContactType type { set; get; }
        public string image { set; get; }
        public string allAliases { set; get; }


        public Contact(string userName, string displayName, string primaryAlias, ContactType type)
        {
            this.userName = userName;
            this.displayName = displayName;
            this.primaryAlias = primaryAlias;
            this.isFav = false;
            this.type = type;
        }

    }
}
