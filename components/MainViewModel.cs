using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LinphoneXamarin.Entities;
namespace LinphoneXamarin.components
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<MyCall> callsList;

        public MainViewModel()
        {
            callsList = new ObservableCollection<MyCall>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void refreshCalls(ObservableCollection<MyCall> list)
        {
            callsList = list;
        }
    }
}
