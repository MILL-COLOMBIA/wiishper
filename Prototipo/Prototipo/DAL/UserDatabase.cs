using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using System.Diagnostics;

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
            database.DeleteAll<Taste>();
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
                User u = GetUser(user.idusers);
                if (u == null)
                    return database.Insert(user);
                else
                    return database.Update(user);
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

        public Taste GetProduct(int idproducts)
        {
            lock (locker)
            {
                return database.Table<Taste>().FirstOrDefault(x => x.idproducts == idproducts);
            }
        }

        public int SaveProduct(Taste taste)
        {
            lock(locker)
            {
                Taste t = GetProduct(taste.idproducts);
                if (t == null)
                    return database.Insert(taste);
                else
                    return database.Update(taste);
            }
        }

        public void CleanProducts()
        {
            database.DeleteAll<Taste>();
        }

        public IEnumerable<Taste> GetProducts()
        {
            return (from taste in database.Table<Taste>() select taste).ToList();
        }

        public void PrintTastes()
        {
            Debug.WriteLine("**********************||||||||||||||||||||||||||||||***************************");
            List<Taste> tastes = (from taste in database.Table<Taste>() select taste).ToList();
            foreach( Taste t in tastes)
            {
                Debug.WriteLine(t.idproducts);
            }
        }
    }
}
