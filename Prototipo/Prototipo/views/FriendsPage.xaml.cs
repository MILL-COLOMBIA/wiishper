using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;

namespace Prototipo
{
    public partial class FriendsPage : ContentPage
    {

        private List<User> list;
        public FriendsPage(List<User> list)
        {
            InitializeComponent();
            this.list = list;
            friendsView.ItemsSource = list;
        }

    }
}
