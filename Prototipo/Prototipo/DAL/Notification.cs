using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototipo
{
    public class Notification
    {
        public string subject { get; set; }
        public string action { get; set; }
        public string objeto { get; set; }
        public DateTime timestamp { get; set; }
        public string mainimage { get; set; }
    }
}
