using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Plugin.Toasts;

namespace Prototipo
{
    public partial class ActivityPage : ContentPage
    {
        IToastNotificator notificator;
        public ActivityPage()
        {
            InitializeComponent();
            notificator = DependencyService.Get<IToastNotificator>();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void OnMonth(object sender, EventArgs e)
        {
            switch (((Button)sender).Text)
            {
                case "Enero":
                    break;
                case "Febrero":
                    break;
                case "Marzo":
                    break;
                case "Abril":
                    break;
                case "Mayo":
                    break;
                case "Junio":
                    break;
                case "Julio":
                    break;
                case "Agosto":
                    break;
                case "Septiembre":
                    break;
                case "Octubre":
                    break;
                case "Noviembre":
                    break;
                case "Diciembre":
                    break;
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
