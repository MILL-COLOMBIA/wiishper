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

        dynamic sendContent;

        public List<User> Friends { get; private set; }
        public static List<Product> Products { get; private set; }
        public static User LoggedUser { get; set; }

        public RestService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "82991d5c121abd6985314d7df5708260");
            Products = new List<Product>();
            Products.Add(new Product() { name = "producto 1", image = "profilepic.png" });
            sendContent = new JObject();
            sendContent.data = new JObject();
            sendContent.control = new JObject();
        }

        public async Task<List<User>> GetFriends()
        {
            Friends = new List<User>();

            var uri = new Uri(Constants.RestURL);
            try
            {
                var response = await client.GetAsync(uri);
                if(response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
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

        public async Task<List<Product>> GetProducts()
        {
            Products = new List<Product>();

            var uri = new Uri(Constants.RestURL);
            dynamic sentjson = new JObject();
            sendContent.control.operation = Constants.SHOW_RANDOM_PRODS;
            var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    Debug.WriteLine("********************************------------------------------------****************************");
                    Products = JsonConvert.DeserializeObject<List<Product>>(sr.data.ToString());
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
                return new List<Product>();
            }

            return Products;
        }

        public async Task<string> SignUp(User user)
        {
            var uri = new Uri(Constants.RestURL);
            var data = JsonConvert.SerializeObject(user);
            dynamic sendContent = CreateMessage(Constants.SIGNUP, data);
            sendContent.data = JsonConvert.SerializeObject(user);
            Debug.WriteLine(sendContent.ToString());
            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;

                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return result;
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task AddFriend(int friendId)
        {
            var uri = new Uri(Constants.RestURL);

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

            var uri = new Uri(Constants.RestURL);

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

        public async Task<User> Login(string username, string password)
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            data.password = password;
            data.email = username;
            dynamic sendContent = CreateMessage(Constants.LOGIN, data);

            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                if(response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    LoggedUser = JsonConvert.DeserializeObject<User>(sr.data.ToString());
                    return LoggedUser;
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine(e.Message);
            }

            return LoggedUser;
        }

        private dynamic CreateMessage(int operation, dynamic data)
        {
            dynamic message = new JObject();
            message.control = new JObject();
            message.data = new JObject();

            message.control.operation = operation;
            message.data = data;

            return message;
        }

        private void PrepareClient()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", LoggedUser.apikey);
        }
    }
}
