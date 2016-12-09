using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Prototipo
{
    public partial class StartPage : ContentPage
    {
        private bool next = true;
        public StartPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            var tapper = new TapGestureRecognizer();
            tapper.NumberOfTapsRequired = 1;
            tapper.Tapped += Tapper_Tapped;
            image.GestureRecognizers.Add(tapper);
        }

        private async void Tapper_Tapped(object sender, EventArgs e)
        {
            if (next)
            {
                image.Source = "tutorial_2.png";
                next = false;
            }
            else
            {
                Navigation.InsertPageBefore(new ProductsPage(), this);
                await Navigation.PopAsync();
            }
        }
    }
}
