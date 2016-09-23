using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototipo
{
    public static class Constants
    {
        public static string RestURL = "http://mill.com.co/ws/wiishper/process";

        #region Operations
        public static int SIGNUP = 100;
        public static int LOGIN = 101;
        public static int REFRESH_SESSION = 102;
        public static int SHOW_FRIENDS = 200;
        public static int SEARCH_PEOPLE = 201;
        public static int ADD_FRIEND = 202;
        public static int UNFRIEND = 203;
        public static int SYNC_FRIENDS = 204;
        public static int SHOW_RANDOM_PRODS = 300;
        public static int LIKE_PROD = 301;
        public static int REJECT_PROD = 302;
        public static int FILTER_PRODS = 303;
        public static int SHOW_LIKED_PRODS = 304;
        public static int SHOW_REJECTED_PRODS = 305;
        public static int SYNC_PRODS = 306;
        #endregion
    }
}
