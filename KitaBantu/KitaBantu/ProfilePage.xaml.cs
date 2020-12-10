using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KitaBantu.Services;
using KitaBantu.Models;
using Xamarin.Essentials;
using Newtonsoft.Json.Linq;

namespace KitaBantu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

          
            try
            {
                FirebaseHelper firebaseHelper = new FirebaseHelper();
                var currentUser = await firebaseHelper.GetUserAsync();
                username.Text = currentUser.username;
                phoneNumber.Text = currentUser.phoneNumber;
                email.Text = currentUser.emailAddress;
                editProfileButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Fail to receive user info: " + ex.Message , "OK");
            }

            


        }

        async private void editProfileButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UpdateProfilePage(username.Text, phoneNumber.Text)); 
        }
    }
}