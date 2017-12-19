using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GreeniumCommon {
    public class MyPluginDescriptor : IPlugin
    {
        public TargetType TargetType { get; set; }
        public string Name { get; set; }
        public string Descriptor{ get; set; }
        public string Details { get; set; }

        public String Image { get; set; }

        public MyPluginDescriptor()
        {
            Image = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Extensions/Assets/greeniummovies.png";
            Name = "Greenium Movies";
            Descriptor = "Plateforme de critique Greenium";
            Details = "Développé par Greenium Group";
            TargetType = TargetType.UI;
        }

        public MyPluginDescriptor(String Image, String Descriptor, String Details, TargetType Target){
            this.Image = Image;
            this.Descriptor = Descriptor;
            this.Details = Details;
            this.TargetType = Target;
        }

        public object Register()
        {
            return null;
        }

        public object Execute(object[] Parameters) {
            
            return null;
        }

    
    }
}
