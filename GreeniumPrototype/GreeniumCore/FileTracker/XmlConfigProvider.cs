using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;


namespace GreeniumCore.FileTracker.FileTracker
{
    public class XmlConfigProvider : XmlProvider {
        public XmlConfigProvider(string src) : base(src)
        {
        }


        public XmlConfigProvider(String src, String trackedFile) : base(src)
        {
            FileParser = "_iden{0}" ;
            TrackedFile = trackedFile;
            KnownNodes = new Dictionary<int, string[]>() {
                    { 0, new string[]{"root"}},
                    { 1, new string[]{"identifiers", "version"}},
                    { 2, new string[]{"id", "version_data"}},
                    { 3, new string[]{"uid", "name"}}
            };
        }

      
        public bool AddNode(String uid, String name)
        {

            var newComment = new XmlElementProvider(){
                Key = "id",
                Value = "",
                Attributes = new List<XAttribute>() {new XAttribute("id", Guid.NewGuid().ToString())},

                Children = new List<XmlElementProvider>()
                {
                    new XmlElementProvider()
                    {
                        Key = "uid",
                        Value = uid
                    },

                    new XmlElementProvider()
                    {
                        Key = "name",
                        Value = name
                    }
                }

            };

            return AddNode("identifiers", newComment);
        }

        public bool DeleteComment(String id)
        {
            return DeleteNode(id, "id");
        }

        public IEnumerable<string> ReadIdentifiers()
        {
            return ReadNode("identifiers").Any() ? ReadNode("identifiers").Reverse().Select(comment => $"{comment.Children[0].Value.ToString()};{comment.Children[1].Value.ToString()}") : new List<string>();
        }

    }
}