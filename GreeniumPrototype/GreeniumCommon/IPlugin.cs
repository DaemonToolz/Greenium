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

        //bool IsFreemium { get; set; }
        //bool IsUnlocked { get; set; }

        Object Register();
        Object Execute(Object[] Parameters);
    }

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class AssemblySecurityAttribute : Attribute {

        // Methods
        public AssemblySecurityAttribute(string SecurityInfo) {
            this.SecurityInfo = SecurityInfo;
        }
        // Properties
        public string SecurityInfo { get; private set; }
        // Fields
    }
    

}
