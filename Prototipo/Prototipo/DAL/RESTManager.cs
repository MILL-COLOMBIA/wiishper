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
            Debug.WriteLine("________ GETFRIENDS _______________");
            return restService.GetFriends();
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
    }
}
