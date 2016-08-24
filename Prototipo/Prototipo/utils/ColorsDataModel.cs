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
        public string Name { get; set; }
        public Color Color { get; set; }

        public static IList<ColorsDataModel> All { get; set; }

        static ColorsDataModel()
        {
            All = new ObservableCollection<ColorsDataModel>
            {
                new ColorsDataModel
                {
                    Name = "Red",
                    Color = Color.Red
                },
                new ColorsDataModel
                {
                    Name = "Blue",
                    Color = Color.Blue
                },
                new ColorsDataModel
                {
                    Name = "Green",
                    Color = Color.Green
                }
            };
        }

    }
}
