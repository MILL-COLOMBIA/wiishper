using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;

namespace Prototipo
{
    public partial class FriendsPage : ContentPage
    {
        public FriendsPage()
        {
            InitializeComponent();
            //friendsView.ItemsSource = App.Database.GetUsers();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            friendsView.ItemsSource = await App.Manager.GetFriends();
        }
    }
}
