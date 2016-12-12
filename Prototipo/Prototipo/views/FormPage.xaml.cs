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
        private bool picChanged = false;
        private bool passChanged = false;
        public FormPage(User user = null)
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
				Button logout = new Button
				{
					Text = "Salir",
					TextColor = Color.FromHex("4E4E4E"),
                    BackgroundColor = Color.Transparent,
					HorizontalOptions=LayoutOptions.Center,
					VerticalOptions=LayoutOptions.End
				};
				logout.Clicked += OnLogOut;
				MainLayout.Children.Add(logout);
                notificator = DependencyService.Get<IToastNotificator>();
                if (user == null)
                    user = new User();
                this.BindingContext = user;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void OnPasswordChanged(object sender, EventArgs e)
        {
            passChanged = true;
        }

        private async void OnSave(object sender, EventArgs e)
        {
            user = BindingContext as User;
            if (RestService.IsUserLogged())
            {
                SaveExistingUser(user);
            }
            else
            {
                SaveNewUser(user);
            }
        }

        private async void SaveExistingUser(User user)
        {
            user.apikey = RestService.LoggedUser.apikey;
            user.password = passChanged ? user.password : RestService.userpass;
            if (picChanged)
            {
                var imageStream = new MemoryStream();
                StreamImageSource streamImageSource = (StreamImageSource)picture.Source;
                System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
                Task<Stream> task = streamImageSource.Stream(cancellationToken);
                Stream stream = task.Result;
                stream.CopyTo(imageStream);
                bool uploaded = await ImageUploader.UploadPic(imageStream.ToArray(), user.idusers + ".png");
                user.profilepic = uploaded ? "http://wiishper.com/profilepics/" + RestService.LoggedUser.idusers + ".png" : "http://wiishper.com/profilepics/main.png";
            }
            else
            {
                user.profilepic = user.profilepic == null ? "http://wiishper.com/profilepics/main.png" : user.profilepic;
            }
            var response = await App.Manager.UpdateUser(user);
            if (response != null)
            {
                notificator.Notify(ToastNotificationType.Success, "Wiishper", "Bienvenido a Wiishper", TimeSpan.FromSeconds(2));
                Navigation.InsertPageBefore(new ProfilePage(RestService.LoggedUser), this);
                await Navigation.PopAsync();
            }
            else
            {
                await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ooops, ocurrió un error en el registro", TimeSpan.FromSeconds(2));
            }
        }

        private async void SaveNewUser(User user)
        {
            var response = await App.Manager.SignUp(user);
            if (response != null)
            {
                await App.Manager.Login(user.email, user.password);
                user.apikey = RestService.LoggedUser.apikey;
                user.idusers = RestService.LoggedUser.idusers;
                user.password = RestService.userpass;
                if (picChanged)
                {
                    var imageStream = new MemoryStream();
                    StreamImageSource streamImageSource = (StreamImageSource)picture.Source;
                    System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
                    Task<Stream> task = streamImageSource.Stream(cancellationToken);
                    Stream stream = task.Result;
                    stream.CopyTo(imageStream);
                    bool uploaded = await ImageUploader.UploadPic(imageStream.ToArray(), user.idusers + ".png");
                    user.profilepic = uploaded ? "http://wiishper.com/profilepics/" + user.idusers + ".png" : "http://wiishper.com/profilepics/main.png";
                }
                else
                {
                    user.profilepic = user.profilepic == null ? "http://wiishper.com/profilepics/main.png" : user.profilepic;
                }
            }
            response = await App.Manager.UpdateUser(user);
            if (response != null)
            {
                notificator.Notify(ToastNotificationType.Success, "Wiishper", "Tus datos fueron actualizados", TimeSpan.FromSeconds(2));
                List<Taste> tastes = App.Database.GetProducts().ToList();
                foreach (Taste t in tastes)
                {
                    if (t.liked)
                        App.Manager.LikeProduct(t.idproducts);
                    else
                        App.Manager.RejectProduct(t.idproducts);
                }
                App.Database.CleanProducts();
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

		private async void OnLogOut(object sender, EventArgs e)
		{
			RestService.LoggedUser = null;
			Helpers.Settings.GeneralSettings = "used";
			Navigation.InsertPageBefore(new SignUp(), this);
			await Navigation.PopAsync();
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

            using (var memoryStream = new MemoryStream())
            {
                file.GetStream().CopyTo(memoryStream);
                file.Dispose();

                memoryStream.Position = 0;
                var byteArray = memoryStream.ToArray();
                picture.Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
            }

            picChanged = true;


        }
    }
}
