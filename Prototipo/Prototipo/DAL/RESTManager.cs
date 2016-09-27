using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Prototipo
{
    public class RESTManager
    {
        IRestService restService;

        public RESTManager(IRestService service)
        {
            restService = service;
        }

        public Task<List<User>> GetFriends()
        {
            return restService.GetFriends();
        }

        public Task<List<Product>> GetProducts()
        {
            return restService.GetProducts();
        }

        public Task<string> SignUp(User user)
        {
            return restService.SignUp(user);
        }

        public Task AddFriend(int friendId)
        {
            return restService.AddFriend(friendId);
        }

        public Task<List<User>> GetUsers()
        {
            return restService.GetUsers();
        }

        public List<Product> products()
        {
            return RestService.Products;
        }

        public Task<User> Login(string username, string password)
        {
            return restService.Login(username, password);
        }
    }
}
