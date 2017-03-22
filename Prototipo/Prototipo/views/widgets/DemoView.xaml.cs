using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Prototipo
{
    public partial class DemoView : ContentView
    {
        public ProductsPage page;

        public DemoView()
        {
            InitializeComponent();
        }

        private void OnUp(object sender, EventArgs e)
        {
            page.OnChange();
           
        }

    }
}
