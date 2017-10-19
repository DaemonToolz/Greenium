using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;


namespace GreeniumCore.FileTracker.FileTracker
{
    public class XmlBookProvider : XmlProvider {
        public XmlBookProvider(string src) : base(src)
        {
        }


        public XmlBookProvider(String src, String trackedFile) : base(src)
        {
            FileParser = "_bookmark{0}" ;
            TrackedFile = trackedFile;
            KnownNodes = new Dictionary<int, string[]>() {
                    { 0, new string[]{"root"}},
                    { 1, new string[]{"bookmarks", "version"}},
                    { 2, new string[]{"bookmark", "version_data"}},
                    { 3, new string[]{"name", "url"}}
            };
        }

      
        public bool AddNode(String name, String url)
        {

            var newComment = new XmlElementProvider(){
                Key = "bookmark",
                Value = "",
                Attributes = new List<XAttribute>() {new XAttribute("bookmark", Guid.NewGuid().ToString())},

                Children = new List<XmlElementProvider>()
                {
                    new XmlElementProvider()
                    {
                        Key = "name",
                        Value = name
                    },

                    new XmlElementProvider()
                    {
                        Key = "url",
                        Value = url
                    }
                }

            };

            return AddNode("bookmarks", newComment);
        }

        public bool DeleteBookmark(String name)
        {
            return DeleteNode(name, "bookmark");
        }

        public IEnumerable<string> ReadBookmarks()
        {
            return ReadNode("bookmarks").Any() ? ReadNode("bookmarks").Reverse().Select(comment => comment.Children[1].Value.ToString()) : new List<string>();
        }

    }
}