using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery2.HTTP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        private readonly string fromSoftware;
        private readonly string githubServer = "https://api.github.com/repos/EDDiscovery/EDDiscovery/";

        public GitHubClass()
        {
            fromSoftware = "EDDiscovery";
            var assemblyFullName = Assembly.GetExecutingAssembly().FullName;
            fromSoftwareVersion = assemblyFullName.Split(',')[1].Split('=')[1];
            commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;

            _serverAddress = githubServer;
        }

      

        public JArray GetAllReleases()
        {

            HttpWebRequest request = WebRequest.Create("https://api.github.com/repos/EDDiscovery/EDDiscovery/releases") as HttpWebRequest;
            request.UserAgent = "TestApp";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string content1 = reader.ReadToEnd();
            }



            ResponseData resp = RequestGet("releases");

            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string text = resp.Body;
                JArray ja = JArray.Parse(text);
                return ja;
            }
            else return null;
        }
    }
}
