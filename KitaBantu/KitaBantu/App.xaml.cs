using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KitaBantu
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // MainPage = new MainPage();
            // MainPage = new MasterDetailPageKitaBantu();


            // if user is previously signed in, direct them to home page

            // MainPage = new MainTabbedPage();

           
            if (!string.IsNullOrEmpty(Preferences.Get("FirebaseAuthToken", ""))) 
            {
                // App.Current.MainPage = new MainTabbedPage();
                App.Current.MainPage = new MasterDetailPageKitaBantu();
            }
            else
            {
                MainPage = new NavigationPage(new SignInPage());
            }
            

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
