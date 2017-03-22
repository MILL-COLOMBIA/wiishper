using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Prototipo
{
    public partial class DemoProfile : ContentView
    {
        public ProductsPage page;

        public DemoProfile()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, EventArgs e)
        {
            // page.OnChange();
            this.IsVisible = false;
            
        }
    }
}
