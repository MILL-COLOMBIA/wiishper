using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Prototipo
{
    public partial class ProductsPage : ContentPage
    {
        public ProductsPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await DisplayAlert("WIISHPER", "Estamos buscando productos que te pueden gustar", "Aceptar");
            CarouselProducts.ItemsSource = await App.Manager.GetProducts();
            if(CarouselProducts.ItemsSource == null || ((IEnumerable<Product>)CarouselProducts.ItemsSource).Count() < 0)
            {
                await DisplayAlert("Error", "Ocurrió un error buscando productos", "Cancelar");
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
