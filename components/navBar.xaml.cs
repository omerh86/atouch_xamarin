using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linphone;
using LinphoneXamarin.Entities;
using LinphoneXamarin.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinphoneXamarin.components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class navBar : MasterDetailPage, CallViewInitiater, LinphoneRegistrationListener
    {
        RegistrationService registrationService;
        private bool isCallView = false;
        private bool isCallActive = false;
        ToolbarItem returnToCallBtn;


        public navBar()
        {
            registrationService = RegistrationService.Instance;
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CallService.Instance.setCallsViewInitiater(this);
            registrationService.setRegistrationListener(this);
            returnToCallBtn = new ToolbarItem
            {
                Icon = "call.png",
                Command = new Command(this.returnToCallAction),
            };
        }

        private void goToCallView()
        {
            var page = (Page)new MainPage();
            page.Disappearing += (sender, e) =>
            {
                if (isCallActive)
                {
                    this.ToolbarItems.Add(returnToCallBtn);
                }
                else
                {
                    this.ToolbarItems.Remove(returnToCallBtn);
                }
            };

            page.Appearing += (sender, e) =>
            {
                this.ToolbarItems.Remove(returnToCallBtn);
            };

            Detail.Navigation.PushAsync(page);
        }

        private void returnToCallAction()
        {
            goToCallView();
        }

        public void onInitiateCallView()
        {
            isCallActive = true;

            if (!this.isCallView)
            {
                goToCallView();
                isCallView = true;

            }
        }


        public void onDestroyCallView()
        {
            isCallActive = false;
            this.ToolbarItems.Remove(returnToCallBtn);
            if (this.isCallView)
            {
                Detail.Navigation.PopAsync();

                this.isCallView = false;
            }
        }

        public void onLinphoneStatusChanged(RegistrationState state, string message)
        {
            if (state == RegistrationState.Failed || state == RegistrationState.None || state == RegistrationState.Cleared)
            {
                //registrationService.unRegister();
                registrationService.setRegistrationListener(null);
                ((App)App.Current).MainPage = new NavigationPage(new components.Login());
            }
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