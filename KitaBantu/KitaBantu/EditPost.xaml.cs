using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KitaBantu.Wrapper;
using KitaBantu.Models;
using System.Collections.ObjectModel;
using Plugin.Media.Abstractions;
using KitaBantu.Services;
using System.IO;
using Plugin.Media;

namespace KitaBantu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPost : ContentPage
    {
        public ObservableCollection<Category> categories;
        UserDetailWrapper userPost = new UserDetailWrapper();
        MediaFile file;
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        private string imageUrl;
        private string postId;
        private string userId;
        private bool isCompleted;

        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set { categories = value; }
        }

        private Category categorySelectedItem;
        public Category CategorySelectedItem
        {
            get { return categorySelectedItem; }
            set { categorySelectedItem = value; OnPropertyChanged(); }
        }

        public EditPost(UserDetailWrapper content)
        {
            
            InitializeComponent();
            this.userPost = content;
           

            
            CategoryData categoryData = new CategoryData();
            categories = categoryData.categories;

            CategorySelectedItem = Categories
                .Where(item => item.categoryName == userPost.Post.category)
                .FirstOrDefault();

            postId = userPost.Post.postId;
            userId = userPost.Post.userId;
            isCompleted = userPost.Post.isCompleted;
            imageUrl = userPost.Post.imageUrl;

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            title.Text = userPost.Post.title;
            description.Text = userPost.Post.description;
            pickedImage.Source = userPost.Post.imageUrl;
        }

        private async void pickImageButton_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            try
            {
                file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });

                if (file == null)
                    return;

                pickedImage.Source = ImageSource.FromStream(() =>
                {
                    var imageStream = file.GetStream();
                    return imageStream;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void postButton_Clicked(object sender, EventArgs e)
        {
            bool formValid = true;
            var postTitle = title.Text ?? "";
            var postDescription = description.Text ?? "";
            var selectedCategory = CategorySelectedItem.categoryName;

            string postId = userPost.Post.postId;
            

            // Form validation

            // validate post title
            if (string.IsNullOrEmpty(postTitle))
            {
                titleErrorLabel.Text = "Title cannot be empty";
                titleErrorLabel.IsVisible = true;
                formValid = false;
            }
            else
            {
                titleErrorLabel.IsVisible = false;
            }

            // validate post description
            if (string.IsNullOrEmpty(postDescription))
            {
                descriptionErrorLabel.Text = "Description cannot be empty";
                descriptionErrorLabel.IsVisible = true;
                formValid = false;
            }
            else
            {
                descriptionErrorLabel.IsVisible = false;
            }

            if (categoryPicker.SelectedItem == null)
            {
                categoryErrorLabel.Text = "Please select a category";
                categoryErrorLabel.IsVisible = true;
                formValid = false;
            }
            else
            {
                categoryErrorLabel.IsVisible = false;
            }

            if (formValid)
            {
                loader.IsVisible = true;
                loader.IsRunning = true;
                postButton.IsEnabled = false;
                // only upload file to Firebase Storage if the form is valid
                if (file != null)
                {
                    // upload image to Firebase Storage
                    // use postId for image filename for easy query
                    // one post has only one image so the unique postId is safe to be used as a key for image 
                    var uniqueFileName = postId;

                    try
                    {
                        imageUrl = await firebaseHelper.UploadImage(file.GetStream(), uniqueFileName);
                    }
                    catch (Exception ex)
                    {
                        loader.IsVisible = false;
                        loader.IsRunning = false;
                        await DisplayAlert("Alert", "Fail to Upload Image: " + ex.Message, "OK");
                    }
                }

                // Add post to Realtime Database
                try
                {
                    await firebaseHelper.UpdatePost(postId, postTitle, postDescription, selectedCategory, userId, imageUrl, isCompleted);
                    var postObject = await firebaseHelper.GetUserDetailWrapperFromPostId(postId);
                    loader.IsVisible = false;
                    loader.IsRunning = false;
                    postButton.IsEnabled = true;
                    MessagingCenter.Send(postObject, "Update");
                    await DisplayAlert("Alert", "Post Updated", "OK");
                    await this.Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    loader.IsVisible = false;
                    loader.IsRunning = false;
                    postButton.IsEnabled = true;
                    await DisplayAlert("Alert", "Fail to Update Post: " + ex.Message, "OK");
                }

               
            }
        }
    }
}