using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinphoneXamarin.components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactTabbedPage : TabbedPage
    {
        public ContactTabbedPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("omer40: " + "on ContactTabbedPage appearing");
            this.CurrentPageChanged += onCurrentPageChanged;
        }

        public void onCurrentPageChanged(object sender, System.EventArgs e)
        {
            //has the updated tab-page title
            if (((TabbedPage)sender).CurrentPage.Title.Equals("Recent"))
                ((Recent)((TabbedPage)sender).CurrentPage).resetCallLog();
        }
    }
}