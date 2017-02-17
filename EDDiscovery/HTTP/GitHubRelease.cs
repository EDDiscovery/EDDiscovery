/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
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
