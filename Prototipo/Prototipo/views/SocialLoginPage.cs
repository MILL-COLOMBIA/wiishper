using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using Plugin.Toasts;

namespace Prototipo
{
    public class SocialLoginPage : ContentPage
    {
        public enum SocialNetwork
        {
            Facebook, Google
        };

        private string ClientIdFacebook = "397763907223862";
        private string ClientIdGoogle = "840940483248-u78r3nli6opsrt71t3d4ii1nfo1bsllv.apps.googleusercontent.com";
        private string ClientSecretGoogle = "LFzqV-LCjPUlZE3ocZGOVzq0";
        private IToastNotificator notificator;
        private SocialNetwork network;
        public SocialLoginPage(SocialNetwork network)
        {
            this.network = network;

            notificator = DependencyService.Get<IToastNotificator>();

            var apiRequest = "";

            switch(this.network)
            {
                case SocialNetwork.Facebook:
                    apiRequest = "https://www.facebook.com/dialog/oauth?client_id=" + ClientIdFacebook + "&display=popup&response_type=token&redirect_uri=http://www.facebook.com/connect/login_success.html&scope=email";
                    break;
                case SocialNetwork.Google:
                    apiRequest = "https://accounts.google.com/o/oauth2/v2/auth?response_type=code&scope=openid+email&redirect_uri=http://wiishper.com&client_id=" + ClientIdGoogle;
                    break;
            }
            

            var webView = new WebView
            {
                Source = apiRequest,
                HeightRequest = 1
            };

            if (network == SocialNetwork.Facebook)
                webView.Navigated += WebViewOnNavigatedFacebook;
            else if (network == SocialNetwork.Google)
                webView.Navigated += WebViewOnNavigatedGoogle;

            Content = webView;
        }

        private async void WebViewOnNavigatedFacebook(object sender, WebNavigatedEventArgs e)
        {
            var accessToken = ExtractAccessTokenFromUrl(e.Url);

            if (accessToken != "")
            {
                FacebookServices fbServices = new FacebookServices();
                Facebook.FacebookProfile profile = await fbServices.GetFacebookProfileAsync(accessToken);

                User user = new User
                {
                    name = profile.FirstName,
                    surname = profile.LastName,
                    gender = profile.Gender.Equals("male") ? "M" : "F",
                    email = profile.Email,
                    profilepic = profile.Picture.Data.Url
                };

                user = await App.Manager.ValidateUser(user);

                if (user != null)
                {
                    Navigation.InsertPageBefore(new ProfilePage(RestService.LoggedUser), this);
                    await Navigation.PopAsync();
                }
                else
                {
                    await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ocurrió un error obteniendo la información de usuario", TimeSpan.FromSeconds(2));
                    Navigation.InsertPageBefore(new SignUp(), this);
                    await Navigation.PopAsync();
                }
            }
        }

        private string ExtractAccessTokenFromUrl(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                var at = url.Replace("https://www.facebook.com/connect/login_success.html#access_token=", "");

                if (Xamarin.Forms.Device.OS == TargetPlatform.WinPhone || Xamarin.Forms.Device.OS == TargetPlatform.Windows)
                {
                    at = url.Replace("http://www.facebook.com/connect/login_success.html#access_token=", "");
                }

                var accessToken = at.Remove(at.IndexOf("&expires_in="));

                return accessToken;
            }

            return string.Empty;
        }

        private async void WebViewOnNavigatedGoogle(object sender, WebNavigatedEventArgs e)
        {
            var code = ExtractCodeFromUrl(e.Url);

            if(code != "")
            {
                GoogleServices services = new GoogleServices();
                Google.GoogleProfile profile = await services.GetGoogleProfileAsync(code);

                if (profile == null)
                {
                    await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ocurrió un error obteniendo la información de usuario", TimeSpan.FromSeconds(2));
                    Navigation.InsertPageBefore(new SignUp(), this);
                    await Navigation.PopAsync();
                }

                User user = new User
                {
                    name = profile.Name.GivenName,
                    surname = profile.Name.FamilyName,
                    //gender = profile.Gender.Equals("male") ? "M" : "F",
                    email = profile.Emails[0].Value,
                    profilepic = profile.Image.Url
                };

                user = await App.Manager.ValidateUser(user);

                if (user != null)
                {
                    Navigation.InsertPageBefore(new ProfilePage(RestService.LoggedUser), this);
                    await Navigation.PopAsync();
                }
                else
                {
                    await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ocurrió un error obteniendo la información de usuario", TimeSpan.FromSeconds(2));
                    Navigation.InsertPageBefore(new SignUp(), this);
                    await Navigation.PopAsync();
                }
            }
        }

        private string ExtractCodeFromUrl(string url)
        {
            if(url.Contains("code="))
            {
                var attributes = url.Split('&');
                var code = attributes.FirstOrDefault(s => s.Contains("code=")).Split('=')[1];
                return code;
            }
            return string.Empty;
        }
    }

}
