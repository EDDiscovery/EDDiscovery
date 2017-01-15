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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery.HTTP;
using EDDiscovery2.HTTP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace EDDiscovery
{
    public class GitHubClass : HttpCom
    {
        public string commanderName;

        private readonly string fromSoftwareVersion;
//        private readonly string fromSoftware;
        private readonly string githubServer = "https://api.github.com/repos/EDDiscovery/EDDiscovery/";

        public GitHubClass()
        {
//            fromSoftware = "EDDiscovery";
            var assemblyFullName = Assembly.GetExecutingAssembly().FullName;
            fromSoftwareVersion = assemblyFullName.Split(',')[1].Split('=')[1];
            commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.EdsmName;

            _serverAddress = githubServer;
        }

      

        public JArray GetAllReleases()
        {

            try
            {
                HttpWebRequest request = WebRequest.Create("https://api.github.com/repos/EDDiscovery/EDDiscovery/releases") as HttpWebRequest;
                request.UserAgent = "TestApp";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string content1 = reader.ReadToEnd();
                    JArray ja = JArray.Parse(content1);
                    return ja;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception: {ex.Message}");
                Trace.WriteLine($"ETrace: {ex.StackTrace}");
                return null;
            }
            
        }

        public GitHubRelease GetLatestRelease()
        {

            try
            {
                HttpWebRequest request = WebRequest.Create("https://api.github.com/repos/EDDiscovery/EDDiscovery/releases/latest") as HttpWebRequest;
                request.UserAgent = "TestApp";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string content1 = reader.ReadToEnd();
                    JObject ja = JObject.Parse(content1);

                    if (ja != null)
                    {
                        GitHubRelease rel = new GitHubRelease(ja);
                        return rel;
                    }
                    else
                        return null; ;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception: {ex.Message}");
                Trace.WriteLine($"ETrace: {ex.StackTrace}");
                return null;
            }

        }



    }
}
