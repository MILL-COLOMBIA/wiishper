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
        public ProductsPage()
        {
            InitializeComponent();
            //CarouselProducts.ItemsSource = new List<Product>() { new Product { name = "Buscando productos", image = "profilepic.png" } };
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(ToastNotificationType.Info, "Wiishper", "Estamos cargando productos para tí", TimeSpan.FromSeconds(2));
            CarouselProducts.ItemsSource = await App.Manager.GetProducts();
            if(CarouselProducts.ItemsSource == null || ((IEnumerable<Product>)CarouselProducts.ItemsSource).Count() < 0)
            {
                tapped = await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ooops, no encontramos ningún producto", TimeSpan.FromSeconds(2));
            }
        }

        private async void OnReject(object sender, EventArgs e)
        {
            await DisplayAlert("Function", "Rejecting product", "OK");
        }

        private async void OnLike(object sender, EventArgs e)
        {
            await DisplayAlert("Function", "Adding product", "OK");
        }
    }
}
