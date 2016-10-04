using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery2.HTTP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EDDiscovery.EDDN
{
    public class EDDNClass : HttpCom
    {
        public string commanderName;
        
        private readonly string fromSoftwareVersion;
        private readonly string fromSoftware;
        private readonly string EDDNServer = "http://eddn-gateway.elite-markets.net:8080/upload/";

        public EDDNClass()
        {
            fromSoftware = "EDDiscovery";
            var assemblyFullName = Assembly.GetExecutingAssembly().FullName;
            fromSoftwareVersion = assemblyFullName.Split(',')[1].Split('=')[1];
            commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;

            _serverAddress = EDDNServer;
        }

        private JObject Header()
        {
            JObject header = new JObject();

            header["uploaderID"] = commanderName;
            header["softwareName"] = fromSoftware;
            header["softwareVersion"] = fromSoftwareVersion;

            return header;
        }

        public JObject CreateEDDNMessage(JournalFSDJump journal)
        {
            if (!journal.HasCoordinate)
                return null;

            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = "http://schemas.elite-markets.net/eddn/journal/1/test";

            JObject message = new JObject();


            message["StarSystem"] = journal.StarSystem;
            message["Government"] = journal.Government;
            message["timestamp"] = journal.EventTimeUTC.ToString("yyyy-MM-ddTHH:mm:ssZ");
            message["Faction"] = journal.Faction;
            message["Allegiance"] = journal.Allegiance;
            message["StarPos"] = new JArray(new float[] { journal.StarPos.X, journal.StarPos.Y, journal.StarPos.Z });
            message["Security"] = journal.Security;
            message["event"] = journal.EventTypeStr;
            message["Economy"] = journal.Economy;

            msg["message"] = message;
            return msg;
        }

        public bool PostMessage(JObject msg)
        {

            ResponseData resp = RequestPost(msg.ToString(), "");

            return true;
        }
    }
}
