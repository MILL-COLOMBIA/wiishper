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
            dynamic data = new JObject();
            dynamic sendContent = CreateMessage(Constants.SHOW_FRIENDS, data);
            List<User> people = new List<User>();
            PrepareClient();
            Debug.WriteLine(sendContent.ToString());

            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                if(response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    Debug.WriteLine(result);
                    Friends = JsonConvert.DeserializeObject<List<User>>(sr.data.ToString());
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

            return Friends;
        }

        public async Task<List<Product>> GetProducts()
        {
            Products = new List<Product>();

            var uri = new Uri(Constants.RestURL);
            dynamic sentjson = new JObject();
            sendContent.control.operation = Constants.SHOW_RANDOM_PRODS;
            var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");
            PrepareClient();
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

        public async Task<List<User>> ShowPeople()
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            dynamic sendContent = CreateMessage(Constants.SEARCH_PEOPLE, data);
            List<User> people =  new List<User>();
            PrepareClient();
            Debug.WriteLine(sendContent.ToString());
            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    people = JsonConvert.DeserializeObject<List<User>>(sr.data.ToString());
                    return people;
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
                return null;
            }
        }

        public async Task<bool> IsFriend(int id)
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            data.friendee = id;
            dynamic sendContent = CreateMessage(Constants.IS_FRIEND, data);
            PrepareClient();
            Debug.WriteLine(sendContent.ToString());
            bool answer;
            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    answer = (bool)sr.data;
                    return answer;
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<string> AddFriend(int id)
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            data.friendee = id;
            dynamic sendContent = CreateMessage(Constants.ADD_FRIEND, data);
            PrepareClient();
            string answer;
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
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    answer = sr.data.ToString();
                    return answer;
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return "FAIL";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine(ex.Message);
                return "FAIL";
            }
        }

        public async Task<string> Unfriend(int id)
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            data.friendee = id;
            dynamic sendContent = CreateMessage(Constants.UNFRIEND, data);
            PrepareClient();
            string answer;
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
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    answer = sr.data.ToString();
                    return answer;
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return "FAIL";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine(ex.Message);
                return "FAIL";
            }
        }

        public async Task<string> LikeProduct(int id)
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            data.idproducts = id;
            data.liked = 1;
            dynamic sendContent = CreateMessage(Constants.LIKE_PROD, data);
            PrepareClient();
            Debug.WriteLine(sendContent.ToString());

            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri, content);
                byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                Debug.WriteLine(result);
                if(response.IsSuccessStatusCode)
                {
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    return sr.data.ToString();
                }
                else
                {
                    return "FAIL";
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine(e.Message);
                return "FAIL";
            }
        }

        public async Task<string> RejectProduct(int id)
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            data.idproducts = id;
            data.liked = 0;
            dynamic sendContent = CreateMessage(Constants.REJECT_PROD, data);
            PrepareClient();
            Debug.WriteLine(sendContent.ToString());

            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri, content);
                byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                Debug.WriteLine(result);
                if (response.IsSuccessStatusCode)
                {
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    return sr.data.ToString();
                }
                else
                {
                    return "FAIL";
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine(e.Message);
                return "FAIL";
            }
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
