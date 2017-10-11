using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GreeniumCore.FileTracker.FileTracker{
    public class XmlElementProvider{
        public String Key { get; set; }
        public Object Value { get; set; }
        public List<XmlElementProvider> Children { get; set; }
        public List<XAttribute> Attributes { get; set; }

        public XmlElementProvider(){
            Children = new List<XmlElementProvider>();
            Attributes = new List<XAttribute>();
        }

        public static XElement Recursive_ConvertToXElement(XmlElementProvider originalNode){
            if (originalNode.Key == null && !originalNode.Key.Trim().Any() && originalNode.Value == null)
                return null;

            var xElement = new XElement(originalNode.Key, originalNode.Value, originalNode.Attributes);

            if (originalNode.Children.Any())
                foreach (var node in originalNode.Children)
                    xElement.Add(Recursive_ConvertToXElement(node));

            return xElement;
        }

        public static XmlElementProvider Recursive_XmlElementProvider(XElement originalNode, bool isRoot = true)
        {

            if (originalNode == null)
                return null;

            var xElement = new XmlElementProvider(){
                Key = originalNode.Name.ToString(),
                Value = !isRoot ? originalNode.Value : "",
                Attributes = originalNode.Attributes().ToList(),
                Children = new List<XmlElementProvider>()
            };

            if(originalNode.Nodes().Any())
                foreach(var node in originalNode.Elements())
                    xElement.Children.Add(Recursive_XmlElementProvider(node, false));

            return xElement;
        }
    }
}
