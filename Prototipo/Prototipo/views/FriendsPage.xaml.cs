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

        private List<User> list;
        private bool visited = false;
        private bool friends;
        public FriendsPage(List<User> list, bool friends)
        {
            InitializeComponent();
            this.list = list;
            friendsView.ItemsSource = list;
            this.friends = friends;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (!visited)
                visited = true;
            else
            {
                list = friends ? await App.Manager.GetFriends() : await App.Manager.ShowPeople();
                friendsView.ItemsSource = list;
            }
        }

        private async void OnProfile(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage(RestService.LoggedUser));
        }

        private async void OnSelectUser(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedUser = e.SelectedItem as User;
            await Navigation.PushAsync(new ProfilePage(selectedUser));
        }

    }
}
