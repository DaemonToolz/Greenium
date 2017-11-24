using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using GreeniumCore.FileTracker.FileTracker;

namespace GreeniumCore.FileTracker {
    public class XmlUserSettingsProvider : XmlProvider {
        public XmlUserSettingsProvider(string src) : base(src)
        {
        }


        public XmlUserSettingsProvider(String src, String trackedFile) : base(src)
        {
            FileParser = "_usrstg{0}";
            TrackedFile = trackedFile;
            KnownNodes = new Dictionary<int, string[]>() {
                { 0, new string[]{"root"}},
                { 1, new string[]{"settings", "version"}},
                { 2, new string[]{"setting", "version_data"}},
                { 3, new string[]{
                    "username","emails", "autolog",
                    "idsave","defaultbrowser","ishost",
                    "isfixedseed", "seed", "privacy",
                    "p2pstatus", "p2presolver", "p2pprotocol"} }
            };
        }

        public IEnumerable<String> ReadConfig(){
            return ReadNode("settings").Any() ?
                ReadNode("settings").Select(
                    settings =>
                        settings.Children
                            .Select(setting => $"{setting.Key}:{setting.Value}")
                            .Aggregate((@out, @in) => @out + ";" + @in))
                : new List<string>();
        }

        public bool InsertOrUpdateConfig(Dictionary<String,String> config)
        {
            try
            {
                return UpdateMultipleNodes(ReadNode("settings").First().Attributes.First().Value, "setting", config, true).All(val => val == true);
            }
            catch
            {
                var newComment = new XmlElementProvider()
                {
                    Key = "setting",
                    Value = "",
                    Attributes = new List<XAttribute>() {new XAttribute("id", Guid.NewGuid().ToString())},

                    Children = config.Select(value => new XmlElementProvider(){
                        Key = value.Key,
                        Value = value.Value
                    }).ToList()
                };

                return AddNode("settings", newComment);
            }
        }
    }
}
