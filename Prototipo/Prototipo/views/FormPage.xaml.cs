using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Plugin.Toasts;
using Plugin.Media;
using System.Diagnostics;

namespace Prototipo
{
    public partial class FormPage : ContentPage
    {
        private User user;
        private IToastNotificator notificator;
        public FormPage(User user = null)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            notificator = DependencyService.Get<IToastNotificator>();
            if (user == null)
                user = new User();
            this.BindingContext = user;
        }

        private async void OnSave(object sender, EventArgs e)
        {
            user = (User)BindingContext;

            var response = await App.Manager.SignUp(user);

            if (response != null)
            {
                notificator.Notify(ToastNotificationType.Success, "Wiishper", "Bienvenido a wiishper", TimeSpan.FromSeconds(2));
                RestService.LoggedUser = user;
                Navigation.InsertPageBefore(new ProfilePage(RestService.LoggedUser), this);
                await Navigation.PopAsync();
            }
            else
            {
                await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ooops, ocurrió un error en el registro", TimeSpan.FromSeconds(2));
            }
        }

        private async void OnDismiss(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void OnGenderChanged(object sender, EventArgs e)
        {
            user = (User)BindingContext;
            user.gender = ((Switch)sender).IsToggled ? "F" : "M";
        }

        private async void OnPickPhoto(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                notificator.Notify(ToastNotificationType.Error, "Wiishper", "Tu teléfono no permite acceso a tu galería de fotos", TimeSpan.FromSeconds(2));
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;


            User user = this.BindingContext as User;


            using (var memoryStream = new MemoryStream())
            {
                file.GetStream().CopyTo(memoryStream);
                file.Dispose();
                bool state = await ImageUploader.UploadPic(memoryStream.ToArray(), user.idusers + ".png");
                if(state)
                {
                    user.profilepic = "http://wiishper.com/profilepics/" + user.idusers + ".png";
                    Debug.WriteLine(user.profilepic);
                }
            }
        }
    }
}
