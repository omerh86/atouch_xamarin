using LinphoneXamarin.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinphoneXamarin.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;

namespace LinphoneXamarin.components.userControllers
{
    public partial class MySearch : Grid
    {
        ContactService contactService;
        List<Contact> contactList = new List<Contact>();

        public static readonly BindableProperty CommandProperty =
       BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(MySearch), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Helper method for invoking commands savely
        public static void Execute(ICommand command, bool val)
        {
            if (command == null) return;
            if (command.CanExecute(null))
            {
                command.Execute(val);
            }
        }

        public MySearch()
        {
            InitializeComponent();
            contactService = ContactService.Instance;
            this.BindingContext = contactList;
            searchList.IsVisible = false;
        }

     

        private void onSearchedClicked(object sender, TextChangedEventArgs e)
        {
            string filter = searchInput.Text;
            if (filter != e.OldTextValue)
            {
                if (filter.Length < 2)
                {
                    contactList = new List<Contact>();
                    this.BindingContext = contactList;
                    searchList.IsVisible = false;
                    Execute(Command, true);
                }
                else
                {
                    searchList.IsVisible = true;
                    contactList = contactService.getContactByPartialName(filter);
                    this.BindingContext = contactList;
                    Execute(Command, false);
                }
            }
        }

        private void onClearSearch(object sender, EventArgs e)
        {
            searchInput.Text = "";
        }


    }
}