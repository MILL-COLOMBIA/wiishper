using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototipo
{
    public interface IRestService
    {
        Task<List<User>> GetFriends();
        Task<List<Product>> GetProducts();
        Task<string> SignUp(User user);
        Task<string> Update(User user);
        Task<List<User>> GetUsers();
        Task<User> Login(string username, string password);
        Task<List<User>> ShowPeople();
        Task<bool> IsFriend(int id);
        Task<string> AddFriend(int id);
        Task<string> Unfriend(int id);
        Task<string> LikeProduct(int id);
        Task<string> RejectProduct(int id);
        Task<List<Product>> ShowLikedProducts(int id);
        Task<List<Product>> ShowRejectedProducts(int id);
    }
}
