using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace EDDiscovery2.EDDB
{
    public class EDDBClass
    {
          private string stationFileNameTemp = "eddbstationslite_temp.json";
        private string stationFileName = "eddbstationslite.json";

        private string systemFileNameTemp = "eddbsystems_temp.json";
        private string systemFileName = "eddbsystems.json";

        private string commoditiesFileNameTemp = "commodities_temp.json";
        private string commoditiesFileName = "commodities.json";	

        public bool GetSystems()
        {
            try
            {
                if (File.Exists(systemFileNameTemp))
                    File.Delete(systemFileNameTemp);

                WebClient webClient = new WebClient();
                webClient.DownloadFile("http://robert.astronet.se/Elite/eddb/systems.json", systemFileNameTemp);

                if (File.Exists(systemFileName))
                    File.Delete(systemFileName);

                File.Copy(systemFileNameTemp, systemFileName);

                return true;
            }

            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception:" + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public bool GetCommodities()
        {
            try
            {
                if (File.Exists(commoditiesFileNameTemp))
                    File.Delete(commoditiesFileNameTemp);

                WebClient webClient = new WebClient();
                webClient.DownloadFile("http://robert.astronet.se/Elite/eddb/commodities.json", commoditiesFileNameTemp);

                if (File.Exists(commoditiesFileName))
                    File.Delete(commoditiesFileName);

                File.Copy(commoditiesFileNameTemp, commoditiesFileName);

                return true;
            }

            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception:" + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                return false;
            }
        }


        public bool GetStationsLite()
        {

            try
            {
                if (File.Exists(stationFileNameTemp))
                    File.Delete(stationFileNameTemp);

                WebClient webClient = new WebClient();
                webClient.DownloadFile("http://robert.astronet.se/Elite/eddb/stations_lite.json", stationFileNameTemp);

                if (File.Exists(stationFileName))
                    File.Delete(stationFileName);

                File.Copy(stationFileNameTemp, stationFileName);

                return true;
            }

            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception:" + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                return false;
            }
        }


        private string ReadJson(string filename)
        {
            string json = null;

            try
            {
                if (!File.Exists(filename))
                    return null;

                StreamReader reader = new StreamReader(filename);
                json = reader.ReadToEnd();
                reader.Close();

                return json;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception:" + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public List<SystemClass> ReadSystems()
        {
            List<SystemClass> eddbsystems = new List<SystemClass>();
            string json;

            json = ReadJson(systemFileName);

            if (json == null)
                return eddbsystems;

            JArray systems = (JArray)JArray.Parse(json);

            if (systems!=null)
            {
                foreach (JObject jo in systems)
                {
                    SystemClass sys = new SystemClass(jo, EDDiscovery.SystemInfoSource.EDDB);

                    if (sys != null)
                        eddbsystems.Add(sys);
                }
            }

            return eddbsystems;
        }

        public List<StationClass> ReadStations()
        {
            List<StationClass> eddbstations = new List<StationClass>();
            string json;

            json = ReadJson(stationFileName);

            if (json == null)
                return eddbstations;

            JArray systems = (JArray)JArray.Parse(json);

            if (systems != null)
            {
                foreach (JObject jo in systems)
                {
                    StationClass sys = new StationClass(jo, EDDiscovery.SystemInfoSource.EDDB);

                    if (sys != null)
                        eddbstations.Add(sys);
                }
            }

            return eddbstations;
        }

        public bool Add2DB(List<SystemClass> eddbsystems, List<StationClass> eddbstations)
        {
            SQLiteDBClass db = new SQLiteDBClass();

            db.Connect2DB();

            int lastupdated =  db.QueryValueInt("SELECT Max(eddb_updated_at ) FROM Systems", -1);

            var result = from a in eddbsystems where a.eddb_updated_at > lastupdated  orderby a.eddb_updated_at  select a;

            foreach (SystemClass sys in result)
            {
                SystemClass sysdb =  SystemData.GetSystem(sys.name);

                if (sysdb != null)  // Update system
                {
                    System.Diagnostics.Trace.WriteLine("Update system " + sys.name);
                    sysdb.id_eddb = sys.id_eddb;
                    sysdb.faction = sys.faction;
                    sysdb.population = sys.population;
                    sysdb.government = sys.government;
                    sysdb.allegiance = sys.allegiance;
                    sysdb.state = sys.state;
                    sysdb.security = sys.security;
                    sysdb.primary_economy = sys.primary_economy;
                    sysdb.needs_permit = sys.needs_permit;
                    sysdb.eddb_updated_at = sys.eddb_updated_at;

                    if (sys.government != EDGovernment.Unknown)
                        System.Diagnostics.Trace.WriteLine("Gov " + sys.government);
                    sysdb.Store();

                }
                else
                {
                    System.Diagnostics.Trace.WriteLine("New system " + sys.name);
                }

            }

            return true;
        }

    }
}
