using System;
using SQLite;


namespace Prototipo
{
    public class User
    {
        public User()
        {

        }

        [PrimaryKey, AutoIncrement]
        public int idusers { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public DateTime birthdate { get; set; }
        public DateTime entrydate { get; set; }
        public string phone { get; set; }
        public string profilepic { get; set; }
        public string apikey { get; set;}
        public string password { get; set; }
        public string gender { get; set; }
        public int followers { get; set; }
        public int following { get; set; }
        public int wishcount { get; set; }
        public bool isfriend { get; set; }
        public string completename { get { return name + " " + surname; } }

        public override string ToString()
        {
            return "[ " + name + " " + surname + "]";
        }

    }
}
