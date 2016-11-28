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
            this.BindingContext = user == null ? RestService.LoggedUser : user;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            products = await App.Manager.ShowLikedProducts(((User)this.BindingContext).idusers);
            //products = new List<Product>()
            //{
            //    new Product()
            //    {
            //        name = "LED 65\" 4K Ultra HD Smart Webos",
            //        price = "$6.299.900",
            //        image = "http://falabella.scene7.com/is/image/FalabellaCO/2696832?$lista160$"
            //    },
            //    new Product()
            //    {
            //        name = "Videojuego Super Mario Maker",
            //        price = "$239.900",
            //        image = "http://falabella.scene7.com/is/image/FalabellaCO/2522790?$lista160$"
            //    },
            //    new Product()
            //    {
            //        name = "Galaxy S6 Edge Plus Dorado Celular",
            //        price = "$2.879.900",
            //        image = "http://falabella.scene7.com/is/image/FalabellaCO/2652354?$lista160$"
            //    },
            //    new Product()
            //    {
            //        name = "Notebook 13,3\" 4GB 500GB Ci5 ",
            //        price = "$2.199.900",
            //        image = "http://falabella.scene7.com/is/image/FalabellaCO/2674749?$lista160$"
            //    }
            //};
            ProductsView.FlowItemsSource = products;
            //if (RestService.LoggedUser != null)
            //{
            //    if (this.user.email == RestService.LoggedUser.email)
            //    {
            //        Button btnEdit = new Button() { Text = "Editar Perfil" };
            //        btnEdit.Clicked += OnEdit;
            //        //MainGrid.Children.Add(btnEdit, 0, 2);
            //        List<Product> products = await App.Manager.ShowLikedProducts(-1);
            //        //ProductsView.ItemsSource = products;
            //    }
            //    else
            //    {
            //        if (await App.Manager.IsFriend(this.user.idusers))
            //        {
            //            Button btnUnfriend = new Button() { Text = "Eliminar amigo" };
            //            btnUnfriend.Clicked += OnUnfriend;
            //            //MainGrid.Children.Add(btnUnfriend, 0, 2);
            //            List<Product> products = await App.Manager.ShowLikedProducts(this.user.idusers);
            //            //ProductsView.ItemsSource = products;
            //        }
            //        else
            //        {
            //            Button btnAddFriend = new Button() { Text = "Agregar amigo" };
            //            btnAddFriend.Clicked += OnAddFriend;
            //            //MainGrid.Children.Add(btnAddFriend, 0, 2);
            //        }
            //    }
            //}
        }

        //async private void OnEdit(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new FormPage((User)BindingContext));
        //}

        //private async void OnUnfriend(object sender, EventArgs e)
        //{
        //    string result = await App.Manager.Unfriend(this.user.idusers);
        //    if(result.Equals("FAIL"))
        //    {
        //        await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Error al eliminar amigo", TimeSpan.FromSeconds(2));
        //    }
        //    else
        //    {
        //        notificator.Notify(ToastNotificationType.Success, "Wiishper", "Amigo eliminado", TimeSpan.FromSeconds(2));
        //        await Navigation.PopAsync();
        //    }
        //}

        //private async void OnAddFriend(object sender, EventArgs e)
        //{
        //    string result = await App.Manager.AddFriend(this.user.idusers);
        //    if (result.Equals("FAIL"))
        //    {
        //        await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Error al agregar amigo", TimeSpan.FromSeconds(2));
        //    }
        //    else
        //    {
        //        notificator.Notify(ToastNotificationType.Success, "Wiishper", "Amigo agregado", TimeSpan.FromSeconds(2));
        //        await Navigation.PopAsync();
        //    }
        //}

        //private async void OnShowPeople(object sender, EventArgs e)
        //{
        //    List<User> people = await App.Manager.ShowPeople();

        //    if(people != null)
        //    {
        //        await Navigation.PushAsync(new FriendsPage(people, false));
        //    }
        //    else
        //    {
        //        await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ocurrió un error al desplegar la información de personas", TimeSpan.FromSeconds(2));
        //    }
        //}

        //private async void OnShowFriends(object sender, EventArgs e)
        //{
        //    List<User> friends = await App.Manager.GetFriends();

        //    if (friends != null)
        //    {
        //        await Navigation.PushAsync(new FriendsPage(friends, true));
        //    }
        //    else
        //    {
        //        await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ocurrió un error al desplegar la información de amigos", TimeSpan.FromSeconds(2));
        //    }
        //}

        //private async void OnShowProducts(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new ProductsPage());
        //}

        //private async void OnLogout(object sender, EventArgs e)
        //{
        //    App.Database.DeleteUser(RestService.LoggedUser.idusers);
        //    RestService.LoggedUser = null;
        //    await Navigation.PopToRootAsync();
        //}

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
    }
}
