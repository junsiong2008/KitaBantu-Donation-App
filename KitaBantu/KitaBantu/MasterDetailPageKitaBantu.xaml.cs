using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace KitaBantu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPageKitaBantu : MasterDetailPage
    {
        public MasterDetailPageKitaBantu()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            // Detail = new MainTabbedPage();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            var item = e.SelectedItem as MasterDetailPageKitaBantuMasterMenuItem;
            if (item == null)
                return;

            // if the sign out menu item is tapped
            if(item.TargetType.Equals(typeof(SignInPage)))
            {
                // Sign out the user and removed saved preference
                Preferences.Remove("FirebaseAuthToken");
               
                // Bring the user to sign in page
                App.Current.MainPage = new SignInPage();
            }

            else
            {
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;

                Detail = new NavigationPage(page);
                IsPresented = false;

                MasterPage.ListView.SelectedItem = null;
            }
            
        }
    }
}