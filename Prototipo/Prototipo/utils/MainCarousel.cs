using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Prototipo
{
    class MainCarousel : Xamarin.Forms.CarouselPage
    {
        public MainCarousel()
        {
            ItemTemplate = new DataTemplate(() => {
                var nameLabel = new Label
                {
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    HorizontalOptions = LayoutOptions.Center
                };
                nameLabel.SetBinding(Label.TextProperty, "Name");

                var colorBoxView = new BoxView
                {
                    WidthRequest = 200,
                    HeightRequest = 200,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
                colorBoxView.SetBinding(BoxView.ColorProperty, "Color");

                return new ContentPage
                {
                    Padding = new Thickness(0, Device.OnPlatform(40, 40, 0), 0, 0),
                    Content = new StackLayout
                    {
                        Children = {
                            nameLabel,
                            colorBoxView
                        }
                    }
                };
            });

            ItemsSource = ColorsDataModel.All;
        }

    }
}
