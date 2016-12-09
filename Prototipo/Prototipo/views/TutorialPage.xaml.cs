using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Prototipo
{
    public partial class TutorialPage : ContentPage
    {
        public TutorialPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarouselSlides.ItemsSource = new List<Slide>
            {
                new Slide
                {
                    image="slide1.jpg",
                    visible=false
                },
                new Slide
                {
                    image="slide2.jpg",
                    visible=false
                },
                new Slide
                {
                    image="slide3.jpg",
                    visible=false
                },
                new Slide
                {
                    image="slide4.jpg",
                    visible=true
                }
            };
        }

        private async void OnBegin(object sender, EventArgs e)
        {
            Helpers.Settings.GeneralSettings = "used";
            Navigation.InsertPageBefore(new StartPage(), this);
            await Navigation.PopAsync();
        }
    }

    public class Slide
    {
        public string image { get; set; }
        public bool visible { get; set; }
    }
}
