using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Plugin.Toasts;
using FFImageLoading.Forms;

namespace Prototipo
{
    public partial class FriendsPage : ContentPage
    {

        private List<User> friends;
        private List<User> people;
        IToastNotificator notificator;
        public FriendsPage()
        {
            InitializeComponent();
            notificator = DependencyService.Get<IToastNotificator>();
            NavigationPage.SetHasNavigationBar(this, false);
            //list = new List<User>() { new User() { name = "Andrés Felipe", followers = 10, following = 20, wishcount = 230, isfriend=true },
            //                          new User() { name = "Juan Manuel", followers=230, following=229, wishcount=10, isfriend=false } };
            //friendsView.ItemsSource = list;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            friendsStack.Children.Clear();
            friends = await App.Manager.GetFriends();
            if(friends != null && friends.Count > 0)
            {
                foreach(User u in friends)
                {
                    StackLayout layout = new StackLayout() { Orientation = StackOrientation.Vertical, BackgroundColor = Color.White };
                    CachedImage pic = new CachedImage() { BackgroundColor= Color.White, HeightRequest = 61, Aspect = Aspect.Fill, DownsampleHeight = 61, DownsampleUseDipUnits = false, LoadingPlaceholder = "image_loading.png", ErrorPlaceholder = "image_error.png", Source = u.profilepic };
                    Button name = new Button() { Text=u.name, HeightRequest=30, FontSize=10, TextColor=Color.FromHex("#A8A9AD"), BackgroundColor=Color.Transparent, HorizontalOptions=LayoutOptions.Center, BindingContext=u};
                    name.Clicked += async (sender, e) => { await Navigation.PushAsync(new ProfilePage(((Button)sender).BindingContext as User)); };
                    layout.Children.Add(pic);
                    layout.Children.Add(name);
                    friendsStack.Children.Add(layout);
                }
            }
            else
            {
                StackLayout layout = new StackLayout() { Orientation = StackOrientation.Vertical, BackgroundColor = Color.White };
                CachedImage pic = new CachedImage() { BackgroundColor = Color.FromHex("#A8A9AD"), HeightRequest = 61, Aspect = Aspect.Fill, DownsampleHeight = 61, DownsampleUseDipUnits = false, LoadingPlaceholder = "image_loading.png", ErrorPlaceholder = "image_error.png", Source = "icon.png" };
                Button name = new Button() { Text = "WIISHPER", HeightRequest = 30, FontSize = 10, TextColor = Color.FromHex("#A8A9AD"), BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.Center };
                layout.Children.Add(pic);
                layout.Children.Add(name);
                friendsStack.Children.Add(layout);
            }
            people = await App.Manager.ShowPeople();
            friendsView.ItemsSource = people;
        }

        private async void OnSelectUser(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedUser = e.SelectedItem as User;
            await Navigation.PushAsync(new ProfilePage(selectedUser));
        }

        private async void OnAddFriend(object sender, EventArgs e)
        {
            Button button = sender as Button;   
            if (button.Text.Equals("Seguir"))
            {
                string result = await App.Manager.AddFriend((int)button.CommandParameter);
                if (result.Equals("FAIL"))
                {
                    await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Error al agregar amigo", TimeSpan.FromSeconds(2));
                }
                else
                {
                    button.BackgroundColor = Color.FromHex("74C7CD");
                    button.Text = "Dejar de seguir";
                    await notificator.Notify(ToastNotificationType.Success, "Wiishper", "Amigo agregado", TimeSpan.FromSeconds(2));
                }
            }
            else
            {
                string result = await App.Manager.Unfriend((int)button.CommandParameter);
                if (result.Equals("FAIL"))
                {
                    await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Error al agregar amigo", TimeSpan.FromSeconds(2));
                }
                else
                {
                    button.BackgroundColor = Color.FromHex("BCBDC1");
                    button.Text = "Seguir";
                    await notificator.Notify(ToastNotificationType.Success, "Wiishper", "Amigo eliminado", TimeSpan.FromSeconds(2));
                }
            }
        }

        private async void OnNewsfeed(object sender, EventArgs e)
        {
            if (RestService.LoggedUser == null)
            {
                Navigation.InsertPageBefore(new SignUp(), this);
            }
            else
            {
                Navigation.InsertPageBefore(new NotificationsPage(), this);
            }
            await Navigation.PopAsync();
        }

        private async void OnFriends(object sender, EventArgs e)
        {
            
        }

        private async void OnProducts(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new ProductsPage(), this);
            await Navigation.PopAsync();
        }

        private async void OnActivity(object sender, EventArgs e)
        {
            if (RestService.LoggedUser == null)
            {
                Navigation.InsertPageBefore(new SignUp(), this);
            }
            else
            {
                Navigation.InsertPageBefore(new ActivityPage(), this);
            }
            await Navigation.PopAsync();
        }

        private async void OnProfile(object sender, EventArgs e)
        {
            if (RestService.LoggedUser == null)
            {
                Navigation.InsertPageBefore(new SignUp(), this);
            }
            else
            {
                Navigation.InsertPageBefore(new ProfilePage(RestService.LoggedUser), this);
            }
            await Navigation.PopAsync();
        }

    }
}
