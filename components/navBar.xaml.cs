﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinphoneXamarin.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinphoneXamarin.components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class navBar : MasterDetailPage
    {
        public navBar()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as navBarMenuItem;
            if (item == null)
                return;
            if (item.Id == 3)
            {
                RegistrationService.Instance.unRegister();
                return;
            }
            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail.Navigation.PushAsync(page);

            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}