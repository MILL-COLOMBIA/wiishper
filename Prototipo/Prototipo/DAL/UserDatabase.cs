using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;

namespace Prototipo
{
    public class UserDatabase
    {

        static object locker = new object();
        SQLiteConnection database;

        public UserDatabase()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<User>();
            database.CreateTable<Taste>();
        }

        public User GetUser(int id)
        {
            lock(locker)
            {
                return database.Table<User>().FirstOrDefault(x => x.idusers == id);
            }
        }

        public User GetUser(string username)
        {
            lock(locker)
            {
                return database.Table<User>().FirstOrDefault(x => x.username == username);
            }
        }

        public int SaveUser(User user)
        {
            lock (locker)
            {
                if(user.idusers != 0)
                {
                    database.Update(user);
                    return user.idusers;
                }
                else
                {
                    return database.Insert(user);
                }
            }
        }

        public int DeleteUser(int id)
        {
            lock(locker)
            {
                return database.Delete<User>(id);
            }
        }

        public IEnumerable<User> GetUsers()
        {
            return (from user in database.Table<User>() select user).ToList();
        }

        public int SaveProduct(Taste taste)
        {
            lock(locker)
            {
                if(taste.idproducts != 0)
                {
                    database.Update(taste);
                    return taste.idproducts;
                }
                else
                {
                    return database.Insert(taste);
                }
            }
        }
    }
}
