using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Prototipo
{
    class RestService : IRestService
    {
        HttpClient client;

        public List<User> Friends { get; private set; }
        public List<Products> _Products { get; private set; }

        public RestService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "e0d8b9bf0ec4908d130bc93130480eb8");
        }

        public async Task<List<User>> GetFriends()
        {
            Friends = new List<User>();

            var uri = new Uri(string.Format(Constants.RestURL, Constants.FriendsResource, string.Empty));
            try
            {
                var response = await client.GetAsync(uri);
                if(response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    Friends = JsonConvert.DeserializeObject<List<User>>(sr.data.ToString());
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //TODO exception handling
            }

            return Friends;
        }

        public async Task<List<Products>> GetProducts()
        {
            _Products = new List<Products>();

            var uri = new Uri(string.Format(Constants.RestURL, Constants.ProductsResource, string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    _Products = JsonConvert.DeserializeObject<List<Products>>(sr.data.ToString());
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //TODO exception handling
            }

            return _Products;
        }

        public async Task<string> SignUp(User user)
        {
            var uri = new Uri(string.Format(Constants.RestURL, Constants.UsersResource, Constants.SignUp));

            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;

                response = await client.PostAsync(uri, content);

                if(response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task AddFriend(int friendId)
        {
            var uri = new Uri(string.Format(Constants.RestURL, Constants.FriendsResource, Constants.Add));

            try
            {
                dynamic json = new JObject();
                json.friendee = friendId;
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    //TODO deal with return
                }
            }
            catch (Exception ex)
            {
                //TODO exception handling
            }
        }

        public async Task<List<User>> GetUsers()
        {
            Friends = new List<User>();

            var uri = new Uri(string.Format(Constants.RestURL, Constants.UsersResource, string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    Friends = JsonConvert.DeserializeObject<List<User>>(sr.data.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine(ex.Message);
                //TODO exception handling
            }

            return Friends;
        }
    }
}
