using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Prototipo
{
    public partial class CarruselPage : CarouselPage
    {
        public CarruselPage()
        {
            InitializeComponent();
            ItemsSource = App.Manager.products();
            //ItemsSource = ColorsDataModel.All;
            //ItemsSource = new List<Products>();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            ItemsSource = await App.Manager.GetProducts();
        }

        async private void OnNext(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FormPage());
        }
    }
}
