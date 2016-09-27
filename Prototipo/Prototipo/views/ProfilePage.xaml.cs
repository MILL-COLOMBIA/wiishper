using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Plugin.Toasts;

namespace Prototipo
{
    public partial class ProfilePage : ContentPage
    {
        private User user;
        private IToastNotificator notificator;
        public ProfilePage(User user)
        {
            InitializeComponent();
            notificator = DependencyService.Get<IToastNotificator>();
            //user = App.Database.GetUser(1) != null ? App.Database.GetUser(1) : new User();
            this.user = user;
            this.BindingContext = user;
        }

        async private void OnEdit(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FormPage((User)BindingContext));
        }

        private async void OnShowPeople(object sender, EventArgs e)
        {
            List<User> people = await App.Manager.ShowPeople();

            if(people != null)
            {
                await Navigation.PushAsync(new FriendsPage(people));
            }
            else
            {
                await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ocurrió un error al desplegar la información de personas", TimeSpan.FromSeconds(2));
            }
        }
    }
}
