using EDDiscovery.EliteDangerous;
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
            commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.EdsmName;

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


        private string GetEDDNSchemaRef()
        {
            if (commanderName.StartsWith("[BETA]"))
                return "http://schemas.elite-markets.net/eddn/journal/1/test";
            else
                return "http://schemas.elite-markets.net/eddn/journal/1";
        }



        public JObject CreateEDDNMessage(JournalFSDJump journal)
        {
            if (!journal.HasCoordinate || journal.StarPosFromEDSM)
                return null;

            JObject msg = new JObject();
            
            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNSchemaRef();

            JObject message = (JObject) JObject.Parse(journal.EventDataString);

            if (Tools.IsNullOrEmptyT(message["FuelUsed"]))  // Old ED 2.1 messages has no Fuel used fields
                return null;


            message.Remove("Economy_Localised");
            message.Remove("Government_Localised");
            message.Remove("Security_Localised");
            message.Remove("BoostUsed");
            message.Remove("JumpDist");
            message.Remove("FuelUsed");
            message.Remove("FuelLevel");
            message.Remove("EDDMapColor");
            message.Remove("StarPosFromEDSM");

            msg["message"] = message;
            return msg;
        }

        public JObject CreateEDDNMessage(JournalDocked journal, double x, double y, double z)
        {
            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNSchemaRef();

            JObject message = (JObject)JObject.Parse(journal.EventDataString);

            message.Remove("Economy_Localised");
            message.Remove("Government_Localised");
            message.Remove("Security_Localised");
            message.Remove("CockpitBreach");

            message["StarPos"] = new JArray(new float[] { (float)x, (float)y, (float)z });

            msg["message"] = message;
            return msg;
        }

        public JObject CreateEDDNMessage(JournalScan journal, string starSystem, double x, double y, double z)
        {
            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNSchemaRef();

            JObject message = (JObject)JObject.Parse(journal.EventDataString);

            message["StarSystem"] = starSystem;
            message["StarPos"] = new JArray(new float[] { (float)x, (float)y, (float)z });

            msg["message"] = message;
            return msg;
        }


        public bool PostMessage(JObject msg)
        {

            ResponseData resp = RequestPost(msg.ToString(), "");

            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else return false;
        }
    }
}
