using KitaBantu.Models;
using KitaBantu.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KitaBantu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPostPage : ContentPage
    {

        MediaFile file;
        FirebaseHelper firebaseHelper = new FirebaseHelper();

        private string selectedCategory;
        private string imageUrl;
        public ObservableCollection<Category> categories;
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

        public NewPostPage()
        {
            InitializeComponent();

            CategoryData categoryData = new CategoryData();
            categories = categoryData.categories;

            /* CategorySelectedItem = Categories
                .Where(item => item.categoryName == "Animals")
                .FirstOrDefault();
            */

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // CategoryData categoryData = new CategoryData();
            // Binding categoryBinding = new Binding();
            // categoryBinding.Source = categoryData.categories;
            // categoryPicker.SetBinding(Picker.ItemsSourceProperty, categoryBinding);
            // categoryPicker.SelectedIndex = 0;
        }

        private async void postButton_Clicked(object sender, EventArgs e)
        { 
            bool formValid = true;
            var postTitle = title.Text;
            var postDescription = description.Text;
            imageUrl = "";
            var postId = $@"{Guid.NewGuid()}";
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
                    var uniqueFileName =  postId;

                    try
                    {
                        imageUrl = await firebaseHelper.UploadImage(file.GetStream(), uniqueFileName);
                    }
                    catch (Exception ex)
                    {
                        postButton.IsEnabled = true;
                        await DisplayAlert("Alert", "Fail to Upload Image: " + ex.Message, "OK");
                    }
                }

                // Add post to Realtime Database
                try
                {
                    await firebaseHelper.AddPost(postId, postTitle, postDescription, CategorySelectedItem.categoryName, imageUrl);
                    loader.IsVisible = false;
                    loader.IsRunning = false;
                    postButton.IsEnabled = true;
                    await DisplayAlert("Alert", "Post Added", "OK");
                    Categories.Clear();
                    BindingContext = this;
                    App.Current.MainPage = new MasterDetailPageKitaBantu();
                }
                catch (Exception ex)
                {
                    loader.IsVisible = false;
                    loader.IsRunning = false;
                    postButton.IsEnabled = true;
                    await DisplayAlert("Alert", "Fail to Add Post: " + ex.Message, "OK");
                }

                
            }
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void categoryPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoryPicker.SelectedIndex != -1)
            {
                selectedCategory = categoryPicker.Items[categoryPicker.SelectedIndex];
            }
        }
    }
}