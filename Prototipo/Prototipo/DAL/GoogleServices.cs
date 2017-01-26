using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Plugin.Toasts;

using Prototipo.Google;

using Xamarin.Forms;

namespace Prototipo
{
    public class GoogleServices
    {
        public string ClientSecret = "LFzqV-LCjPUlZE3ocZGOVzq0";
        public string ClientId = "840940483248-u78r3nli6opsrt71t3d4ii1nfo1bsllv.apps.googleusercontent.com";
        private IToastNotificator notificator;

        public GoogleServices()
        {
            notificator = DependencyService.Get<IToastNotificator>();
        }

        public async Task<GoogleProfile> GetGoogleProfileAsync(string code)
        {
            var requestToken = "https://www.googleapis.com/oauth2/v4/token?code=" + code + "&client_secret=" + ClientSecret + "&redirect_uri=http://wiishper.com&client_id=" + ClientId + "&grant_type=authorization_code";

            var httpClient = new HttpClient();

            var response = await httpClient.PostAsync(requestToken, null);

            var json = await response.Content.ReadAsStringAsync();

            var accessToken = JsonConvert.DeserializeObject<JObject>(json).Value<string>("access_token");

            if(string.IsNullOrEmpty(accessToken))
            {
                await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ocurrió un error obteniendo el token de acceso con Google", TimeSpan.FromSeconds(2));
                return null;
            }

            var requestProfile = "https://www.googleapis.com/plus/v1/people/me?access_token=" + accessToken;

            var userJson = await httpClient.GetStringAsync(requestProfile);

            Debug.WriteLine(userJson);

            var googleProfile = JsonConvert.DeserializeObject<GoogleProfile>(userJson);
            var emails = JsonConvert.DeserializeObject<JObject>(userJson);

            Debug.WriteLine(emails["emails"][0]["value"].ToString());

            googleProfile.Emails.Add(new Email { Value = emails["emails"][0]["value"].ToString() });

            return googleProfile;
        }
    }
}
