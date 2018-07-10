using LinphoneXamarin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinphoneXamarin.components.userControllers
{

    public partial class ContactCell : ViewCell
    {
        ContactService contactService;
        CallService callService;

        public ContactCell()
        {
            contactService = ContactService.Instance;
            callService = CallService.Instance;
            InitializeComponent();
        }

        public static readonly BindableProperty isFavProperty =
BindableProperty.Create("isFav", typeof(bool), typeof(ContactCell), false);

        public bool isFav
        {
            get
            {
                bool isFav = (bool)GetValue(isFavProperty);
                return isFav;
            }
            set
            {
                SetValue(isFavProperty, value);

            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(isFav):
                    UpdateParameterName();
                    break;
            }
        }

        private void UpdateParameterName()
        {
            if (isFav)
                favBtn.TextColor = Color.DarkGoldenrod;
            else
            {
                favBtn.BackgroundColor = Color.DimGray;
            }
        }


        public static readonly BindableProperty displayNameProperty =
       BindableProperty.Create("displayName", typeof(string), typeof(ContactCell), "");

        public string displayName
        {
            get { return (string)GetValue(displayNameProperty); }
            set
            {
                SetValue(displayNameProperty, value);
            }
        }

        public static readonly BindableProperty userNameProperty =
BindableProperty.Create("userName", typeof(string), typeof(ContactCell), "");

        public string userName
        {
            get { return (string)GetValue(userNameProperty); }
            set { SetValue(userNameProperty, value); }
        }


        public static readonly BindableProperty aliasProperty =
          BindableProperty.Create("alias", typeof(int), typeof(ContactCell), 0);

        public int alias
        {
            get { return (int)GetValue(aliasProperty); }
            set { SetValue(aliasProperty, value); }
        }

        public void onToggleContactFavStatus(object sender, EventArgs e)
        {
            this.isFav = contactService.toggleContactFavStatus(userName);
        }

        public void onCallClicked(object sender, EventArgs e)
        {
            callService.call(alias.ToString());
        }


    }

}