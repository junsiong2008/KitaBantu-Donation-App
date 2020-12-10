using KitaBantu.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KitaBantu.Services;

namespace KitaBantu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyPostViewPage : ContentPage
    {
        UserDetailWrapper userPost;
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public MyPostViewPage(UserDetailWrapper content)
        {
            this.userPost = content;
            InitializeComponent();
            MessagingCenter.Subscribe<UserDetailWrapper>(this, "Update", (wrapper) =>
            {
                userPost = wrapper;
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.Title = userPost.Post.title;
            postUsername.Text = userPost.AppUser.username;
            postTitle.Text = userPost.Post.title;
            postDescription.Text = userPost.Post.description;
            postCategory.Text = userPost.Post.category;
            postDatetimePosted.Text = userPost.Post.datetimePosted.ToString("dddd, dd MMMM yyyy h:mm tt");
            postIsCompletedIcon.IsVisible = userPost.Post.isCompleted;
            postIsCompletedLabel.IsVisible = userPost.Post.isCompleted;
            postImage.Source = userPost.Post.imageUrl;
            if (!string.IsNullOrEmpty(userPost.Post.imageUrl))
            {
                imageFrame.IsVisible = true;
            }
            markAsCompletedButton.IsVisible = !userPost.Post.isCompleted;
            editDeleteGrid.IsVisible = !userPost.Post.isCompleted;
            deleteButton.IsVisible = userPost.Post.isCompleted;
        }

        private async void editButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditPost(userPost));
        }

        private async void markAsCompletedButton_Clicked(object sender, EventArgs e)
        {
            await firebaseHelper.UpdatePost(userPost.Post.postId, userPost.Post.title, userPost.Post.description, userPost.Post.category, userPost.Post.userId, userPost.Post.imageUrl, true);
            await DisplayAlert("Alert", "Post Marked as Completed", "OK");
            await Navigation.PopAsync();
        }

        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            await firebaseHelper.DeletePost(userPost.Post.postId);
            await DisplayAlert("Alert", "Post Deleted", "OK");
            await Navigation.PopAsync();

        }

        private async void deleteButtonInGrid_Clicked(object sender, EventArgs e)
        {
            await firebaseHelper.DeletePost(userPost.Post.postId);
            await DisplayAlert("Alert", "Post Deleted", "OK");
            await Navigation.PopAsync();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await this.Navigation.PushModalAsync(new FullImagePage(userPost.Post.imageUrl));
        }
    }
}