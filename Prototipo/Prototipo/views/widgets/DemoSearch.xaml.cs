using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Prototipo
{
    public partial class DemoSearch : ContentView
    {
        public ProductsPage page;

        public DemoSearch()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, EventArgs e)
        {
            page.OnChange();
        }
    }
}
