using System;
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
            ItemsSource = ColorsDataModel.All;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

        }

        async private void OnNext(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FormPage());
        }
    }
}
