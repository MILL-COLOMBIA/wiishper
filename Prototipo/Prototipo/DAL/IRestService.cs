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
        Task AddFriend(int idFriend);
        Task<List<User>> GetUsers();
        Task<User> Login(string username, string password);
        Task<List<User>> ShowPeople();
    }
}
