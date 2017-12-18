using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GreeniumCommon {
    public enum TargetType
    {
        UI = 0,
        BEHAVIORAL
    };

    public interface IPlugin{
        TargetType TargetType { get; set; }
        String Name { get; set; }
        String Image { get; set; }
        String Descriptor { get; set; }
        String Details { get; set; }

        Object Register();
        Object Execute(Object[] Parameters);
    }
    
}
