using KitaBantu.Models;
using KitaBantu.Services;
using KitaBantu.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KitaBantu
{
    public partial class MainPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        string userId;

        private ObservableCollection<UserDetailWrapper> wrappers = new ObservableCollection<UserDetailWrapper>();
        public ObservableCollection<UserDetailWrapper> Wrappers
        {
            get { return wrappers; }
            set { wrappers = value; }
        }
        public MainPage()
        {
            InitializeComponent();
        }


        protected override async void OnAppearing()
        {

            base.OnAppearing();

            this.userId = await firebaseHelper.GetCurrentUserRealtimeDbId();

            var allPosts = await firebaseHelper.GetActivePosts();

            if(Wrappers.Count != 0)
            {
                Wrappers.Clear();
            }

            foreach (Post post in allPosts)
            {
                Wrappers.Add(new UserDetailWrapper()
                {
                    AppUser = await firebaseHelper.GetUserFromId(post.userId),
                    Post = post
                });
            }

            loader.IsRunning = false;
            loader.IsVisible = false;
            IsRefreshing = false;
            // Wrappers = wrappers;
            BindingContext = this;
            // BindingContext = this;
            // Console.WriteLine(wrappers[0].Post.userId);
            // Binding postBinding = new Binding();
            // postBinding.Source = wrappers;
            // postBinding.Path = "ItemsSource";
            // allPostListView.SetBinding(ListView.ItemsSourceProperty, postBinding);
        }

        private async void allPostListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var content = (UserDetailWrapper)e.Item;

            if(content.Post.userId == userId)
            {
                await this.Navigation.PushAsync(new MyPostViewPage(content));
            }
            else
            {
                await this.Navigation.PushAsync(new OtherPostViewPage(content));
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            // var _container = BindingContext as List<UserDetailWrapper>;
            var _container = wrappers;
            //allPostListView.BeginRefresh();

            if(string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                allPostListView.ItemsSource = _container;
            }
            else
            {
                allPostListView.ItemsSource = _container.Where(item => item.Post.title.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase)>=0);
            }

            allPostListView.EndRefresh();
        }

        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }


        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    Wrappers.Clear();
                    Wrappers = new ObservableCollection<UserDetailWrapper>();
                    Console.WriteLine("*************************************");
                    Console.WriteLine(wrappers.Count);
                    allPostListView.ItemsSource = wrappers;
                    var allPosts = await firebaseHelper.GetActivePosts();

                    foreach (Post post in allPosts)
                    {
                        Wrappers.Add(new UserDetailWrapper()
                        {
                            AppUser = await firebaseHelper.GetUserFromId(post.userId),
                            Post = post
                        });
                    }

                    Console.WriteLine("*************************************");
                    Console.WriteLine(wrappers.Count);

                    IsRefreshing = false;

                    // Wrappers = wrappers;

                   // BindingContext = this;
                });
            }
        }
    }
}
