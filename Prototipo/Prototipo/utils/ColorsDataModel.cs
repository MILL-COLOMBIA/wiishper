using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Prototipo
{
    class ColorsDataModel
    {
        public string name { get; set; }
        public Color Color { get; set; }
        public string image { get; set; }

        public static IList<ColorsDataModel> All { get; set; }

        static ColorsDataModel()
        {
            All = new ObservableCollection<ColorsDataModel>
            {
                new ColorsDataModel
                {
                    name = "Red",
                    Color = Color.Red,
                    image = "main.png"
                },
                new ColorsDataModel
                {
                    name = "Blue",
                    Color = Color.Blue,
                    image = "shop.png"
                },
                new ColorsDataModel
                {
                    name = "Green",
                    Color = Color.Green,
                    image = "profile.png"
                }
            };
        }

    }
}
