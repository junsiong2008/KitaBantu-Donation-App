using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KitaBantu.Services;
using System.Collections.ObjectModel;
using KitaBantu.Wrapper;
using KitaBantu.Models;
using System.Windows.Input;

namespace KitaBantu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        string userId;
        private ObservableCollection<UserDetailWrapper> wrappers;
        public ObservableCollection<UserDetailWrapper> Wrappers
        {
            get { return wrappers; }
            set { wrappers = value; OnPropertyChanged(); }
        }

        public SearchPage()
        {
            InitializeComponent();
        }

        private async void searchBar_SearchButtonPressed(object sender, EventArgs e)
        {

            loader.IsRunning = true;
            loader.IsVisible = true;

            var searchResults = (await firebaseHelper.GetActivePosts())
                .Where(item => item.title.IndexOf(searchBar.Text, StringComparison.OrdinalIgnoreCase) >= 0);

            this.userId = await firebaseHelper.GetCurrentUserRealtimeDbId();

            wrappers = new ObservableCollection<UserDetailWrapper>();

            foreach (Post post in searchResults)
            {
                wrappers.Add(new UserDetailWrapper()
                {
                    AppUser = await firebaseHelper.GetUserFromId(post.userId),
                    Post = post
                });
            }

            searchResultsListView.ItemsSource = wrappers;

            loader.IsRunning = false;
            loader.IsVisible = false;

            if (wrappers.Count == 0)
                imageStackLayout.IsVisible = true;
            else
                imageStackLayout.IsVisible = false;

           
        }

        
        private async void searchResults_ItemTapped(object sender, ItemTappedEventArgs e)
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

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(searchBar.Text))
            {
                if(wrappers.Count != 0)
                {
                    wrappers.Clear();
                }
                
            }
            
        }
    }
}