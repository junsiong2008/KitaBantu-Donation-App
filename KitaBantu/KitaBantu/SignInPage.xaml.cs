using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Text.RegularExpressions;
using KitaBantu.Services;

namespace KitaBantu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        
        public SignInPage()
        {
            InitializeComponent();
        }

        async private void signInButton_Clicked(object sender, EventArgs e)
        {
            

            var userEmail = email.Text ?? "";
            var userPassword = password.Text ?? "";
            bool formValid = true;

            // Form validation

            // Validate email
            var emailPattern = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";

            if (!Regex.IsMatch(userEmail, emailPattern))
            {
                emailErrorLabel.Text = "Please enter a valid email address";
                emailErrorLabel.IsVisible = true;
                formValid = false;
            }
            else
            {
                emailErrorLabel.IsVisible = false;
            }

            // validate password
            var passwordPattern = @"^.{6,}$";

            if (!Regex.IsMatch(userPassword, passwordPattern))
            {
                passwordErrorLabel.Text = "Password must be of minimum 6 characters";
                passwordErrorLabel.IsVisible = true;
                formValid = false;
            }
            else
            {
                passwordErrorLabel.IsVisible = false;
            }

          

            if (formValid)
            {
                loader.IsRunning = true;
                loader.IsVisible = true;

                try
                {
                    await firebaseHelper.SignInWithEmailAndPasswordAsync(userEmail, userPassword);
                    loader.IsVisible = false;
                    loader.IsRunning = false;
                    App.Current.MainPage = new MasterDetailPageKitaBantu();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    loader.IsVisible = false;
                    loader.IsRunning = false;
                    await App.Current.MainPage.DisplayAlert("Alert", "Invalid username or password", "OK");
                }
            }

            
        }

        private void registerButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new RegisterPage();
        }
    }
}