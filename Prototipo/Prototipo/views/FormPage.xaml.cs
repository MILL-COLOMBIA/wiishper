using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Prototipo
{
    public partial class FormPage : ContentPage
    {
        private User user;
        public FormPage()
        {
            InitializeComponent();
            //user = App.Database.GetUser(1) != null ? App.Database.GetUser(1) : new User();
            user = new User();
            this.BindingContext = user;
        }

        async void OnSave(object sender, EventArgs e)
        {
            user = (User)BindingContext;
         
            var response = await App.Manager.SignUp(user);
            if(response != null)
            {
                Result.Text = "User Saved " + response;
                Result.BackgroundColor = Color.Green;
            }
            else
            {
                Result.Text = "ERROR";
                Result.BackgroundColor = Color.Red;
            }
        }
    }
}
