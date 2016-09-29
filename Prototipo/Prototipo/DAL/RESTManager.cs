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

        public Task<List<User>> ShowPeople()
        {
            return restService.ShowPeople();
        }

        public Task<bool> IsFriend(int id)
        {
            return restService.IsFriend(id);
        }

        public Task<string> AddFriend(int id)
        {
            return restService.AddFriend(id);
        }

        public Task<string> Unfriend(int id)
        {
            return restService.Unfriend(id);
        }

        public Task<string> LikeProduct(int id)
        {
            return restService.LikeProduct(id);
        }

        public Task<string> RejectProduct(int id)
        {
            return restService.RejectProduct(id);
        }

        public Task<List<Product>> ShowLikedProducts(int id)
        {
            return restService.ShowLikedProducts(id);
        }

        public Task<List<Product>> ShowRejectedProducts(int id)
        {
            return restService.ShowRejectedProducts(id);
        }
    }
}
