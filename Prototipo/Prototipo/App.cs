using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Xamarin.Forms;

namespace Prototipo
{
    public class App : Application
    {
        public static RESTManager Manager { get; private set; }

        static UserDatabase database;
        public static bool IsLoggedIn { get; set; }
        public static string ApiKey { get; set; }
        public static UserDatabase Database
        {
            get
            {
                if (database == null) database = new UserDatabase();
                return database;
            }
        }
        public App()
        {
            Manager = new RESTManager(new RestService());
            MainPage = new NavigationPage(new SignUp()) { BarBackgroundColor = Color.FromRgb(0, 0, 0) };
                        
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void LoadUsers()
        {
            string[] names = { "Andrés", "Felipe", "Cristian", "Mónica", "Juan", "Renzo", "Jonathan", "Miguel", "Jeny", "Ginna" };
            string[] surnames = { "Mejía", "Perry", "Hincapié", "Contreras", "Saravia", "Sesana", "Álvarez", "Acosta", "Beltrán", "Barrera" };
            Random r = new Random();
            for(int i=0; i<15; i++)
            {
                string name = names[r.Next(0, names.Length)];
                string surname = surnames[r.Next(0, surnames.Length)];
                User user = new User()
                {
                    name = name,
                    surname = surname,
                    username = name + "_" + surname,
                    email = name.Substring(0, 1) + "." + surname + "@" + "mill.com.co",
                    birthdate = new DateTime(r.Next(1977, 1996), r.Next(1, 13), r.Next(1, 29)),
                    entrydate = new DateTime(r.Next(2015, 2017), r.Next(1, 13), r.Next(1, 29))
                };
                Database.SaveUser(user);
                Debug.WriteLine(user);
            }      
        }

        private void UpdateUsers()
        {
            List<User> users = Database.GetUsers().ToList();
            foreach(User user in users)
            {
                user.profilepic = "profilepic.png";
                Database.SaveUser(user);
            }
        }
    }
}
