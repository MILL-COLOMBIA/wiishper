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
            this.user = user;
            this.BindingContext = user;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if(this.user.email == RestService.LoggedUser.email)
            {
                Button btnEdit = new Button() { Text = "Editar Perfil"};
                btnEdit.Clicked += OnEdit;
                MainGrid.Children.Add(btnEdit, 0, 1);
            }
            else
            {
                if(await App.Manager.IsFriend(this.user.idusers))
                {
                    Button btnUnfriend = new Button() { Text = "Eliminar" };
                    btnUnfriend.Clicked += OnUnfriend;
                    MainGrid.Children.Add(btnUnfriend, 0, 1);
                }
                else
                {
                    Button btnAddFriend = new Button() { Text = "Agregar" };
                    btnAddFriend.Clicked += OnAddFriend;
                    MainGrid.Children.Add(btnAddFriend, 0, 1);
                }
            }
        }

        async private void OnEdit(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FormPage((User)BindingContext));
        }

        private async void OnUnfriend(object sender, EventArgs e)
        {
            string result = await App.Manager.Unfriend(this.user.idusers);
            if(result.Equals("FAIL"))
            {
                await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Error al eliminar amigo", TimeSpan.FromSeconds(2));
            }
            else
            {
                await notificator.Notify(ToastNotificationType.Success, "Wiishper", "Amigo eliminado", TimeSpan.FromSeconds(2));
                await Navigation.PopAsync();
            }
        }

        private async void OnAddFriend(object sender, EventArgs e)
        {
            string result = await App.Manager.AddFriend(this.user.idusers);
            if (result.Equals("FAIL"))
            {
                await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Error al agregar amigo", TimeSpan.FromSeconds(2));
            }
            else
            {
                await notificator.Notify(ToastNotificationType.Success, "Wiishper", "Amigo agregado", TimeSpan.FromSeconds(2));
                await Navigation.PopAsync();
            }
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

        private async void OnShowFriends(object sender, EventArgs e)
        {
            List<User> friends = await App.Manager.GetFriends();

            if (friends != null)
            {
                await Navigation.PushAsync(new FriendsPage(friends));
            }
            else
            {
                await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ocurrió un error al desplegar la información de amigos", TimeSpan.FromSeconds(2));
            }
        }

        private async void OnShowProducts(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductsPage());
        }
    }
}
