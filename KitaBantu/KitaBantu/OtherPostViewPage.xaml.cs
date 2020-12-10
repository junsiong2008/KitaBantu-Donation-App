using KitaBantu.Services;
using KitaBantu.Wrapper;
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
    public partial class OtherPostViewPage : ContentPage
    {
        private UserDetailWrapper userPost;

        public UserDetailWrapper UserPost
        {
            get { return userPost; }
            set { userPost = value; }
        }

        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public OtherPostViewPage(UserDetailWrapper content)
        {
            InitializeComponent();
            this.userPost = content;

            BindingContext = this;
        }

        private async void contactButton_Clicked(object sender, EventArgs e)
        {
            var whatsappUrl = "https://api.whatsapp.com/send?phone=" + userPost.AppUser.phoneNumber;

            try
            {
                await Browser.OpenAsync(whatsappUrl, BrowserLaunchMode.SystemPreferred);
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.Title = userPost.Post.title;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await this.Navigation.PushModalAsync(new FullImagePage(userPost.Post.imageUrl));
        }
    }
}