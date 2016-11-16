using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototipo
{
    public class Taste
    {
        [PrimaryKey]
        public int idproducts { get; set; }
        public bool liked { get; set; }
        public DateTime inter_date { get; set; }
    }
}
