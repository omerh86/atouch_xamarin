using LinphoneXamarin.Entities;
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CallLogCell : ViewCell
    {
        ContactService contactService;
        CallService callService;
        bool isActionsBar = false;

        public CallLogCell()
        {
            InitializeComponent();
            contactService = ContactService.Instance;
            callService = CallService.Instance;

            favBtn.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => onToggleContactFavStatus()),
            });

            callBtn.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => onCallClicked()),
            });

            infoBtn.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => onInfoClicked()),
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            actionBar.IsVisible = isActionsBar;
        }


        public static readonly BindableProperty callsInfoProperty =
BindableProperty.Create("callsInfo", typeof(List<CallInfo>), typeof(CallLogCell), new List<CallInfo>());

        public List<CallInfo> callsInfo
        {
            get
            {
                List<CallInfo> callsInfo = (List<CallInfo>)GetValue(callsInfoProperty);
                return callsInfo;
            }
            set
            {
                SetValue(callsInfoProperty, value);

            }
        }

        public static readonly BindableProperty isFavProperty =
BindableProperty.Create("isFav", typeof(bool), typeof(CallLogCell), false);

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
                favBtn.TextColor = Color.DimGray;
            }
        }


        public static readonly BindableProperty displayNameProperty =
       BindableProperty.Create("displayName", typeof(string), typeof(CallLogCell), "");

        public string displayName
        {
            get { return (string)GetValue(displayNameProperty); }
            set
            {
                SetValue(displayNameProperty, value);
            }
        }

        public static readonly BindableProperty userNameProperty =
BindableProperty.Create("userName", typeof(string), typeof(CallLogCell), "");

        public string userName
        {
            get { return (string)GetValue(userNameProperty); }
            set { SetValue(userNameProperty, value); }
        }


        public static readonly BindableProperty aliasProperty =
          BindableProperty.Create("alias", typeof(int), typeof(CallLogCell), 0);

        public int alias
        {
            get { return (int)GetValue(aliasProperty); }
            set { SetValue(aliasProperty, value); }
        }

        public void onToggleContactFavStatus()
        {
            this.isFav = contactService.toggleContactFavStatus(userName);
        }

        public void onCallClicked()
        {
            callService.call(alias.ToString());
        }

        public void onInfoClicked()
        {
            Console.WriteLine("On Info");
        }


        void onToggleActions(object sender, EventArgs args)
        {
            isActionsBar = !isActionsBar;
            actionBar.IsVisible = isActionsBar;
            Console.WriteLine("" + this.callsInfo);
            for (int i = 0; i < callsInfo.Count; i++)
            {
                var direction = new Label();
                direction.Text = callsInfo[i].callDirection.ToString();
                direction.FontAttributes = FontAttributes.Bold;
                direction.HorizontalOptions = LayoutOptions.Center;

                var duration = new Label();
                duration.Text = callsInfo[i].duration.ToString();
                duration.HorizontalOptions = LayoutOptions.Center;

                var time = new Label();
                time.Text = callsInfo[i].time.ToString();

                time.HorizontalOptions = LayoutOptions.Center;

                actionBar.Children.Add(direction, 0, i + 1);
                actionBar.Children.Add(duration, 1, i + 1);
                actionBar.Children.Add(time, 2, i + 1);
            }


        }


    }
}
