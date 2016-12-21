using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Prototipo
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int idproducts { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string price { get; set; }
        public DateTime publishdate { get; set; }
        public string store { get; set; }
    }
}
