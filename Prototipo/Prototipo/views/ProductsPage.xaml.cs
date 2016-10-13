using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Plugin.Toasts;

namespace Prototipo
{
    public partial class ProductsPage : ContentPage
    {

        IToastNotificator notificator;
        Product currentProd;
        public ProductsPage()
        {
            InitializeComponent();
            notificator = DependencyService.Get<IToastNotificator>();            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            bool tapped = await notificator.Notify(ToastNotificationType.Info, "Wiishper", "Estamos cargando productos para tí", TimeSpan.FromSeconds(2));
            CarouselProducts.ItemsSource = await App.Manager.GetProducts();
            if(CarouselProducts.ItemsSource == null || ((IEnumerable<Product>)CarouselProducts.ItemsSource).Count() < 0)
            {
                tapped = await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ooops, no encontramos ningún producto", TimeSpan.FromSeconds(2));
            }
        }

        private async void OnReject(object sender, EventArgs e)
        {
            string result = await App.Manager.RejectProduct(((Product)CarouselProducts.Item).idproducts);
            if(result.Equals("FAIL"))
            {
                await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Error al rechazar el producto", TimeSpan.FromSeconds(2));
            }
            else
            {
                await notificator.Notify(ToastNotificationType.Success, "Wiishper", "Producto rechazado", TimeSpan.FromSeconds(2));
            }
        }

        private async void OnLike(object sender, EventArgs e)
        {
            string result = await App.Manager.LikeProduct(((Product)CarouselProducts.Item).idproducts);
            if (result.Equals("FAIL"))
            {
                await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Error al agregar el producto", TimeSpan.FromSeconds(2));
            }
            else
            {
                await notificator.Notify(ToastNotificationType.Success, "Wiishper", "Producto agregado", TimeSpan.FromSeconds(2));
            }
        }

        private async void OnShowPeople(object sender, EventArgs e)
        {
            List<User> people = await App.Manager.ShowPeople();

            if (people != null)
            {
                await Navigation.PushAsync(new FriendsPage(people, false));
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
                await Navigation.PushAsync(new FriendsPage(friends, true));
            }
            else
            {
                await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ocurrió un error al desplegar la información de amigos", TimeSpan.FromSeconds(2));
            }
        }
    }
}
