using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KitaBantu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPageKitaBantuMaster : ContentPage
    {
        public ListView ListView;

        public MasterDetailPageKitaBantuMaster()
        {
            InitializeComponent();

            BindingContext = new MasterDetailPageKitaBantuMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MasterDetailPageKitaBantuMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MasterDetailPageKitaBantuMasterMenuItem> MenuItems { get; set; }

            public MasterDetailPageKitaBantuMasterViewModel()
            {
                MenuItems = new ObservableCollection<MasterDetailPageKitaBantuMasterMenuItem>(new[]
                {
                    new MasterDetailPageKitaBantuMasterMenuItem { Id = 0, Title = "Home" , IconSource="home_2_line.png", TargetType=typeof(MainTabbedPage) },
                    new MasterDetailPageKitaBantuMasterMenuItem{Id = 1, Title = "Profile", IconSource="account_icon.png", TargetType=typeof(ProfilePage) },
                    new MasterDetailPageKitaBantuMasterMenuItem { Id = 2, Title = "About KitaBantu" , IconSource="information_line.png", TargetType=typeof(About)},
                    new MasterDetailPageKitaBantuMasterMenuItem {  Id = 3, Title = "Sign Out", IconSource="logout_box_line.png", TargetType=typeof(SignInPage)},

                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}