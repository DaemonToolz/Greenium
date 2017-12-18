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
            Image = "./Assets/gamemode.png";
            Name = "Greenium Gaming";
            Descriptor = "Plateforme de distribution de jeux en P2P";
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
            return new Object[]{
                typeof(Calculator)
            };
        }

        public object Execute(object[] Parameters) {
            FunctionToExecute();
            return null;
        }

        public void FunctionToExecute()
        {
            Console.WriteLine("I AM EXECUTED !");
        }
    }
}
