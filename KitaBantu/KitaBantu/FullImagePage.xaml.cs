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
    public partial class FullImagePage : ContentPage
    {
        string imageUrl;
        public FullImagePage(string imageUrl)
        {
            InitializeComponent();
            this.imageUrl = imageUrl;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            postImage.Source = imageUrl;
        }

        private async void OnDismiss(object sender, EventArgs e)
        {
            await this.Navigation.PopModalAsync();
        }

        private async void OnSwiped(object sender, EventArgs e)
        {
            await this.Navigation.PopModalAsync();
        }

       
        
    }
}