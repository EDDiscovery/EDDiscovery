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

        public JObject CreateEDDNMessage(JournalDocked journal)
        {
            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = "http://schemas.elite-markets.net/eddn/journal/1/test";

            JObject message = new JObject();

            message["StarSystem"] = journal.StarSystem;
            message["StationName"] = journal.StationName;
            message["StationType"] = journal.StationType;
            message["Faction"] = journal.Faction;
            message["Government"] = journal.Government;
            message["timestamp"] = journal.EventTimeUTC.ToString("yyyy-MM-ddTHH:mm:ssZ");
            message["Allegiance"] = journal.Allegiance;
            message["Security"] = journal.Security;
            message["event"] = journal.EventTypeStr;
            message["Economy"] = journal.Economy;

            msg["message"] = message;
            return msg;
        }

        public JObject CreateEDDNMessage(JournalScan journal, string starSystem)
        {
            JObject msg = new JObject();

           

            msg["header"] = Header();
            msg["$schemaRef"] = "http://schemas.elite-markets.net/eddn/journal/1/test";

            JObject message = new JObject();

            message["timestamp"] = journal.EventTimeUTC.ToString("yyyy-MM-ddTHH:mm:ssZ");
            message["event"] = journal.EventTypeStr;
            message["StarSystem"] = starSystem;

            if (journal.StarType != null && !journal.StarType.Equals("")) // check if star.
            {
                message["Bodyname"] = journal.BodyName;
                message["DistanceFromArrivalLS"] = journal.DistanceFromArrivalLS;
                message["StarType"] = journal.StarType;
                message["StellarMass"] = journal.StellarMass;
                message["Radius"] = journal.Radius;
                message["AbsoluteMagnitude"] = journal.AbsoluteMagnitude;
                message["OrbitalPeriod"] = journal.OrbitalPeriod;
                message["RotationPeriod"] = journal.RotationPeriod;

                if (journal.Rings!= null && journal.Rings.Length > 0)
                {
                    message["Rings"] = JArray.FromObject(journal.Rings);
                }
            }
            else
            {
                message["Bodyname"] = journal.BodyName;
                message["DistanceFromArrivalLS"] = journal.DistanceFromArrivalLS;
                message["TidalLock"] = journal.TidalLock;
                message["TerraformState"] = journal.TerraformState;
                message["PlanetClass"] = journal.PlanetClass;
                message["Atmosphere"] = journal.Atmosphere;
                message["Volcanism"] = journal.Volcanism;
                message["SurfaceGravity"] = journal.SurfaceGravity;
                message["SurfaceTemperature"] = journal.SurfaceTemperature;
                message["SurfacePressure"] = journal.SurfacePressure;
                message["Landable"] = journal.Landable;
                message["OrbitalPeriod"] = journal.OrbitalPeriod;
                message["RotationPeriod"] = journal.RotationPeriod;

                if (journal.Rings != null && journal.Rings.Length > 0)
                {
                    message["Rings"] = JArray.FromObject(journal.Rings);
                }

                if (journal.Materials.Count > 0)
                {
                    message["Materials"] = JObject.Parse(journal.MaterialsString);
                }
            }


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
