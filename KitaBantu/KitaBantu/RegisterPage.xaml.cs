using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using KitaBantu.Services;
using System.Text.RegularExpressions;

namespace KitaBantu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();

        
        public RegisterPage()
        {
            InitializeComponent();
        }

        async private void registerButton_Clicked(object sender, EventArgs e)
        {
            bool formValid = true;
            var userEmail = email.Text ?? "";
            var userPassword = password.Text ?? "";
            var userUsername = username.Text ?? "";
            var userPhoneNumber = phoneNumber.Text ?? "";
            var userReenterPassword = reenterPassword.Text ?? "";

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

            // validate phone number
            var phoneNumberPattern = @"[6][0][1][0-9]{8,9}$";

            if (!Regex.IsMatch(userPhoneNumber, phoneNumberPattern))
            {
                phoneNumberErrorLabel.Text = "Please enter a valid phone number eg: 60123456789";
                phoneNumberErrorLabel.IsVisible = true;
                formValid = false;
            }
            else
            {
                phoneNumberErrorLabel.IsVisible = false;
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

            // validate password matching


            if (userPassword != userReenterPassword)
            {
                repasswordErrorLabel.Text = "Passwords do not match";
                repasswordErrorLabel.IsVisible = true;
                formValid = false;
            }
            else
            {
                repasswordErrorLabel.IsVisible = false;
            }

            // validate username
            if (userUsername == string.Empty)
            {
                usernameErrorLabel.Text = "Username cannot be empty";
                usernameErrorLabel.IsVisible = true;
                formValid = false;
            }
            else
            {
                usernameErrorLabel.IsVisible = false;
            }

            if (formValid)
            {
                loader.IsRunning = true;
                loader.IsVisible = true;
                // Create User Account with Firebase Auth & Add a User record to Realtime DB
                try
                {
                    // string getToken = auth.FirebaseToken;
                    await firebaseHelper.CreateUserWithEmailAndPasswordAsync(userEmail, userPassword);
                    await firebaseHelper.AddUser(userEmail, userUsername, userPhoneNumber);
                    loader.IsRunning = false;
                    loader.IsVisible = false;
                    await DisplayAlert("Alert", "Registration Successful", "OK");
                    App.Current.MainPage = new MasterDetailPageKitaBantu();
                    // await App.Current.MainPage.DisplayAlert("Alert", getToken, "OK");
                }
                catch (Exception ex)
                {
                    loader.IsRunning = false;
                    loader.IsVisible = false;
                    await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                }
            }
          
        }

        private void signInLabel_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage = new SignInPage();
        }

        
    }
}