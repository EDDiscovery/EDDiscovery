using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.HTTP
{
    public class GitHubRelease
    {
        JObject jo;

        public GitHubRelease(JObject jo)
        {
            this.jo = jo;
        }

        public string ReleaseName { get { return  Tools.GetStringDef(jo["name"]); } }

        public string ReleaseVersion {
            get
            {
                string str = Tools.GetStringDef(jo["name"]);

                return new string(str.Where(p => char.IsDigit(p) || p=='.').ToArray());
            }
        }

        public string HtmlURL { get { return Tools.GetStringDef(jo["html_url"]); } }
        public string Time { get { return Tools.GetStringDef(jo["created_at"]); } }

        public string Description { get { return Tools.GetStringDef(jo["body"]); } }


        public string ExeInstallerLink
        {
            get
            {
                var asset = jo["assets"].FirstOrDefault(j => Tools.GetStringDef(j["name"]).EndsWith(".exe"));
                if (asset != null)
                {
                    string url = Tools.GetStringDef(asset["browser_download_url"]);
                    return url;
                }
                return null;
            }
        }
        public string MsiInstallerLink
        {
            get
            {
                var asset = jo["assets"].FirstOrDefault(j => Tools.GetStringDef(j["name"]).EndsWith(".msi"));
                if (asset != null)
                {
                    string url = Tools.GetStringDef(asset["browser_download_url"]);
                    return url;
                }
                return null;
            }
        }
        public string PortableInstallerLink
        {
            get
            {
                var asset = jo["assets"].FirstOrDefault(j => Tools.GetStringDef(j["name"]).EndsWith(".Portable.zip"));
                if (asset != null)
                {
                    string url = Tools.GetStringDef(asset["browser_download_url"]);
                    return url;
                }
                return null;
            }
        }

    }
}
