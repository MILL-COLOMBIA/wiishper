using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Prototipo
{
    public partial class SignUp : ContentPage
    {
        public SignUp()
        {
            InitializeComponent();
        }

        async private void OnSignUp(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new CarruselPage());
            await Navigation.PushAsync(new FormPage());
        }

        private void OnFbSignUp(object sender, EventArgs e)
        {

        }

        private void OnTwitterSignUp(object sender, EventArgs e)
        {

        }

        private void OnGoogleSignUp(object sender, EventArgs e)
        {

        }
    }
}
