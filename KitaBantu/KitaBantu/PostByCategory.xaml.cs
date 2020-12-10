using KitaBantu.Models;
using KitaBantu.Services;
using KitaBantu.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KitaBantu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostByCategory : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        string categoryName;
        string userId;
        public PostByCategory(string category)
        {
            this.categoryName = category;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            loader.IsRunning = true;
            loader.IsVisible = true;
            userId = await firebaseHelper.GetCurrentUserRealtimeDbId();
            var postByCategory = await firebaseHelper.GetPostsByCategory(categoryName);
            List<UserDetailWrapper> wrappers = new List<UserDetailWrapper>();

            foreach (Post post in postByCategory)
            {
                wrappers.Add(new UserDetailWrapper()
                {
                    AppUser = await firebaseHelper.GetUserFromId(post.userId),
                    Post = post
                });
            }

            loader.IsRunning = false;
            loader.IsVisible = false;

            Binding postBinding = new Binding();
            postBinding.Source = wrappers;
            postByCategoryListView.SetBinding(ListView.ItemsSourceProperty, postBinding);
            this.Title = "Posts In " + categoryName;
        }

        private async void postByCategoryListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var content = (UserDetailWrapper)e.Item;

            if (content.Post.userId == userId)
            {
                await this.Navigation.PushAsync(new MyPostViewPage(content));
            }
            else
            {
                await this.Navigation.PushAsync(new OtherPostViewPage(content));
            }
        }
    }
}