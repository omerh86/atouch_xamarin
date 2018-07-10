using LinphoneXamarin.Entities;
using LinphoneXamarin.Services;
using System;
using System.Collections.Generic;
using System.Windows.Input;

using Xamarin.Forms;

namespace LinphoneXamarin.components
{
    public partial class Favorites : ContentPage, FavListener
    {
           
        List<Contact> favList = new List<Contact>();
        ContactService contactService;
        public ICommand searchFieldNotEmptyCommand { get; private set; }

        public Favorites()
        {
            InitializeComponent();
            contactService = ContactService.Instance;
            contactService.setFavListener(this);
            favList = contactService.getAllFav();
            this.BindingContext = favList;
            searchFieldNotEmptyCommand = new Command<bool>(onSearchFieldChanged);
            search.BindingContext = searchFieldNotEmptyCommand;

        }

        public void onFavListChanged()
        {
            favList = contactService.getAllFav();
            this.BindingContext = favList;
        }



        void onSearchFieldChanged(bool isEmpty)
        {
            favorites.IsVisible = isEmpty;
        }
    }
}
