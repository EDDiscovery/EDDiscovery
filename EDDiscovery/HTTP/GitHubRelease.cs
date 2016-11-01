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

        public string ReleaseName { get { return  JSONHelper.GetStringDef(jo["name"]); } }

        public string ReleaseVersion {
            get
            {
                string str = JSONHelper.GetStringDef(jo["name"]);

                return new string(str.Where(p => char.IsDigit(p) || p=='.').ToArray());
            }
        }

        public string HtmlURL { get { return JSONHelper.GetStringDef(jo["html_url"]); } }
        public string Time { get { return JSONHelper.GetStringDef(jo["created_at"]); } }

        public string Description { get { return JSONHelper.GetStringDef(jo["body"]); } }


        public string ExeInstallerLink
        {
            get
            {
                var asset = jo["assets"].FirstOrDefault(j => JSONHelper.GetStringDef(j["name"]).EndsWith(".exe"));
                if (asset != null)
                {
                    string url = JSONHelper.GetStringDef(asset["browser_download_url"]);
                    return url;
                }
                return null;
            }
        }
        public string MsiInstallerLink
        {
            get
            {
                var asset = jo["assets"].FirstOrDefault(j => JSONHelper.GetStringDef(j["name"]).EndsWith(".msi"));
                if (asset != null)
                {
                    string url = JSONHelper.GetStringDef(asset["browser_download_url"]);
                    return url;
                }
                return null;
            }
        }
        public string PortableInstallerLink
        {
            get
            {
                var asset = jo["assets"].FirstOrDefault(j => JSONHelper.GetStringDef(j["name"]).EndsWith(".Portable.zip"));
                if (asset != null)
                {
                    string url = JSONHelper.GetStringDef(asset["browser_download_url"]);
                    return url;
                }
                return null;
            }
        }

    }
}
