using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreeniumPrototype.Models {
    public class LogModel {
        public string ID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public int XP { get; set; }
        public int Level { get; set; }
        public string Creation { get; set; }
        public string[] Emails { get; set; }

        public bool Online { get; set; }

    }
}
