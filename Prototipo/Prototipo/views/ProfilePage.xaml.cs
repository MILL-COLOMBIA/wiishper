using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Plugin.Toasts;
using DLToolkit.Forms;

namespace Prototipo
{
    public partial class ProfilePage : ContentPage
    {
        private IToastNotificator notificator;
        private List<Product> products;
        public ProfilePage(User user)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            notificator = DependencyService.Get<IToastNotificator>();
            if (user.email != RestService.LoggedUser.email)
            {
                Button back = new Button() { BackgroundColor=Color.Transparent, Image="back.png", HorizontalOptions=LayoutOptions.Start, HeightRequest=50, WidthRequest=50 };
                back.Clicked += async (sender, e) => { await Navigation.PopAsync(); };
                mainbar.Children.Insert(0, back);
                mainbar.Children.Add(new Button() { BackgroundColor = Color.Transparent, HeightRequest = 50, WidthRequest = 50 });
            }
            else
            {
                Button settings = new Button() { BackgroundColor = Color.Transparent, Image = "settings.png", HorizontalOptions = LayoutOptions.Start, HeightRequest = 50, WidthRequest = 50 };
                settings.Clicked += OnSettings;
                mainbar.Children.Insert(0, new Button() { BackgroundColor = Color.Transparent, HeightRequest = 50, WidthRequest = 50 });
                mainbar.Children.Add(settings);
                btnFollow.IsEnabled = false;
                btnFollow.IsVisible = false;
            }
            this.BindingContext = user; ;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            products = await App.Manager.ShowLikedProducts(((User)BindingContext).idusers);
            ProductsView.FlowItemsSource = products;            
        }


        private async void OnFollow(object sender, EventArgs e)
        {
            User user = BindingContext as User;
            Button button = sender as Button;
            if (button.Text.Equals("Seguir"))
            {
                string result = await App.Manager.AddFriend(user.idusers);
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
                string result = await App.Manager.Unfriend(user.idusers);
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
            if (RestService.LoggedUser == null)
            {
                Navigation.InsertPageBefore(new SignUp(), this);
            }
            else
            {
                Navigation.InsertPageBefore(new FriendsPage(), this);
            }
            await Navigation.PopAsync();
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
            
        }

        private async void OnSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FormPage(BindingContext as User));
        }
    }
}
