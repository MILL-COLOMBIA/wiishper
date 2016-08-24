using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Prototipo
{
    public partial class ProfilePage : ContentPage
    {
        private User user;
        public ProfilePage()
        {
            InitializeComponent();
            user = App.Database.GetUser(1) != null ? App.Database.GetUser(1) : new User();
            this.BindingContext = user;
        }

        async private void OnEdit(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FormPage());
        }
    }
}
