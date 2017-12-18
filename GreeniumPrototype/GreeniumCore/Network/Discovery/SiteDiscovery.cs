using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreeniumCore.Network.Discovery
{
    public static class SiteDiscovery {

        public static string FindDomain(string url)
        {
            if (url.Contains("//")) url = url.Remove(0, url.IndexOf("//") + 2);
            if (url.Contains("www.")) url = url.Remove(0, url.IndexOf(".") + 1);
            if (url.Contains("/")) url = url.Remove(url.IndexOf("/"));
            return url;
        }

        public static bool Discover(string url)
        {
            try
            {
                if (!Directory.Exists($"{System.AppDomain.CurrentDomain.BaseDirectory}/Data/Site/Cache/"))
                    Directory.CreateDirectory($"{System.AppDomain.CurrentDomain.BaseDirectory}/Data/Site/Cache");
                url = FindDomain(url);

                // Temporary Solution
                // Not GREEN at all
                var Path = $@"{System.AppDomain.CurrentDomain.BaseDirectory}\Data\Site\Cache\{url}.ico";

                if(!File.Exists(Path))
                    using (var client = new System.Net.WebClient())
                        client.DownloadFile($@"http://www.google.com/s2/favicons?domain={url}",
                            Path);

                return true;
            }
            catch{
                return false;
            }
        }
    }
}
