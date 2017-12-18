using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreeniumCommon;

namespace GreeniumCore.Addons
{
    public class ModuleModel {
        public String Image { get; set; }
        public String Name { get; set; }
        public String Descriptor { get; set; }
        public String Details { get; set; }
        public TargetType TargetType { get; set; }

    }
}
