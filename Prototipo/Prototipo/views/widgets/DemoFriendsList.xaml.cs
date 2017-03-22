using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Prototipo
{
    public partial class DemoFriendsList : ContentView
    {
        public DemoFriendsList()
        {
            InitializeComponent();
            
        }
        private void OnClick(object sender, EventArgs e)
        {
            this.IsVisible = false;
          //  Helpers.Settings.TutorialFriends = "show";
        }
    }
}
