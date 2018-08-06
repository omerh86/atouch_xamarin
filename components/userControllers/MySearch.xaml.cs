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

            searchInput.BindingContext = alias;
            inputComponent.IsVisible = isSearchInput;
        }

        public static readonly BindableProperty aliasProperty =
         BindableProperty.Create("alias", typeof(string), typeof(ContactCell), "");

        public string alias
        {
            get { return (string)GetValue(aliasProperty); }
            set
            {
                SetValue(aliasProperty, value);
                doSearch(value);
            }
        }

        public static readonly BindableProperty isSearchInputProperty =
        BindableProperty.Create("isSearchInput", typeof(bool), typeof(ContactCell), true);

        public bool isSearchInput
        {
            get { return (bool)GetValue(isSearchInputProperty); }
            set
            {
                SetValue(isSearchInputProperty, value);
                inputComponent.IsVisible = value;
            }
        }

        private void onSearchedClicked(object sender, TextChangedEventArgs e)
        {
            doSearch(searchInput.Text);
        }

        private void onClearSearch(object sender, EventArgs e)
        {
            searchInput.Text = "";
        }

        private void doSearch(string filter)
        {


            if (filter.Length < 3)
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


        void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            Console.WriteLine("omer40" + e);

        }

    }
}