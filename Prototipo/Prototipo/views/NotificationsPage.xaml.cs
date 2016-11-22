using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Prototipo
{
    public partial class NotificationsPage : ContentPage
    {
        public NotificationsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            notifications.ItemsSource = new List<Notification>() {
                new Notification() { subject="Juan", action="ingresó a wiishper", timestamp= new DateTime(2016, 11, 15) },
                new Notification() {subject="Andrés Felipe", action="agregó 5 productos a su wiishlist", timestamp=new DateTime(2016, 11, 16) } };
        }
    }
}
