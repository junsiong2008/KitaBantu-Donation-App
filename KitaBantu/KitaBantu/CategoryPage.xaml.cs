using KitaBantu.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KitaBantu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : ContentPage
    {
        Category category = new Category();
        
        public CategoryPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            CategoryData category = new CategoryData();
            Binding categoryBinding = new Binding();
            categoryBinding.Source = category.categories;
            categoryListView.SetBinding(ListView.ItemsSourceProperty, categoryBinding);

            
        }

        private async void categoryListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var content = (Category)e.Item;
            await this.Navigation.PushAsync(new PostByCategory(content.categoryName));
        }
    }
}