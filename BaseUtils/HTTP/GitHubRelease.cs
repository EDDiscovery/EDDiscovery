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

namespace BaseUtils
{ 
    public class GitHubRelease
    {
        JObject jo;

        public GitHubRelease(JObject jo)
        {
            this.jo = jo;
        }

        public string ReleaseName { get { return  jo["name"].Str(); } }

        public string ReleaseVersion {
            get
            {
                string str = jo["tag_name"].Str();
                int indexof = str.IndexOfAny("0123456789".ToCharArray());
                if (indexof >= 0)
                    return str.Substring(indexof);
                else
                    return "";
            }
        }

        public string HtmlURL { get { return jo["html_url"].Str(); } }
        public string Time { get { return jo["created_at"].Str(); } }

        public string Description { get { return jo["body"].Str(); } }


        public string ExeInstallerLink
        {
            get
            {
                var asset = jo["assets"].FirstOrDefault(j => j["name"].Str().ToLowerInvariant().EndsWith(".exe"));
                if (asset != null)
                {
                    string url = asset["browser_download_url"].Str();
                    return url;
                }
                return null;
            }
        }
        public string MsiInstallerLink
        {
            get
            {
                var asset = jo["assets"].FirstOrDefault(j => j["name"].Str().ToLowerInvariant().EndsWith(".msi"));
                if (asset != null)
                {
                    string url = asset["browser_download_url"].Str();
                    return url;
                }
                return null;
            }
        }
        public string PortableInstallerLink
        {
            get
            {
                var asset = jo["assets"].FirstOrDefault(j => j["name"].Str().ToLowerInvariant().EndsWith(".zip") && j["name"].Str().ToLowerInvariant().Contains("portable"));
                if (asset != null)
                {
                    string url = asset["browser_download_url"].Str();
                    return url;
                }
                return null;
            }
        }

    }
}
