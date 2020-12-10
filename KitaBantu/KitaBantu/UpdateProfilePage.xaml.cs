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
    public partial class UpdateProfilePage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
    
        public UpdateProfilePage(string uName, string uPhoneNumber)
        {
            InitializeComponent();
            username.Text = uName;
            phoneNumber.Text = uPhoneNumber;
        }

        async private void updateProfileButton_Clicked(object sender, EventArgs e)
        {
            var newUsername = username.Text;
            var newPhoneNumber = phoneNumber.Text;
            bool formValid = true;

            // Form validation
            // validate phone number
            var phoneNumberPattern = @"[0][1][0-9]{8,9}$";

            if (!Regex.IsMatch(newPhoneNumber, phoneNumberPattern))
            {
                phoneNumberErrorLabel.Text = "Please enter a valid phone number eg: 0123456789";
                phoneNumberErrorLabel.IsVisible = true;
                formValid = false;
            }
            else
            {
                phoneNumberErrorLabel.IsVisible = false;
            }

            // validate username
            if(newUsername == string.Empty)
            {
                usernameErrorLabel.Text = "Username cannot be empty";
                usernameErrorLabel.IsVisible = true;
                formValid = false;
            }
            else
            {
                usernameErrorLabel.IsVisible = false;
            }

            if(formValid)
            {
                try
                {
                    await firebaseHelper.UpdateUser(newUsername, newPhoneNumber);
                    await ShowMessage("Alert", "Profile updated successfully", "OK", async () =>
                    {
                        await Navigation.PopAsync();
                    });

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Alert", "Fail to Update Profile: " + ex.Message, "OK");
                }
            }
           
            
            
        }

        // Function to Display Pop-Up and invoke callback function
        public async Task ShowMessage(string title, string message, string buttonTitle, Action afterHideCallback)
        {
            await DisplayAlert(title, message, buttonTitle);
            afterHideCallback?.Invoke();
        }
    }
}