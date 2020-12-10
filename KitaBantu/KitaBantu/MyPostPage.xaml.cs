using KitaBantu.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KitaBantu.Models;
using KitaBantu.Wrapper;

namespace KitaBantu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyPostPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public MyPostPage()
        {
            InitializeComponent();
        }

        

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            loader.IsVisible = true;
            loader.IsRunning = true;

            var myPosts = await firebaseHelper.GetCurrentUserPosts();

            List<UserDetailWrapper> wrappers = new List<UserDetailWrapper>();

            foreach(Post post in myPosts)
            {
                wrappers.Add(new UserDetailWrapper()
                {
                    AppUser = await firebaseHelper.GetUserFromId(post.userId),
                    Post = post
                }); 
            }

            loader.IsRunning = false;
            loader.IsVisible = false;

            // Console.WriteLine(wrappers[0].Post.userId);
            Binding postBinding = new Binding();
            postBinding.Source = wrappers;
            // postBinding.Path = "ItemsSource";
            myPostListView.SetBinding(ListView.ItemsSourceProperty, postBinding);


        }

        private async void myPostListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var content = (UserDetailWrapper)e.Item;
            await this.Navigation.PushAsync(new MyPostViewPage(content));
        }
    }
}