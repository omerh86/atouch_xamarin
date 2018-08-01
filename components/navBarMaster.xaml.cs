using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinphoneXamarin.components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class navBarMaster : ContentPage
    {
        public ListView ListView;

        public navBarMaster()
        {
            InitializeComponent();

            BindingContext = new navBarMasterViewModel();
            ListView = MenuItemsListView;
        }

        class navBarMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<navBarMenuItem> MenuItems { get; set; }

            public navBarMasterViewModel()
            {
                MenuItems = new ObservableCollection<navBarMenuItem>(new[]
                {
                    new navBarMenuItem { Id = 1, Title = "Settings",TargetType=typeof(Settings) },
                    new navBarMenuItem { Id = 2, Title = "About" ,TargetType=typeof(About)},
                    new navBarMenuItem { Id = 3, Title = "Log Out",TargetType=typeof(Login) },
                    new navBarMenuItem { Id = 4, Title = "Exit" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}