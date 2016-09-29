using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using EDDiscovery2.HTTP;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDDiscovery2.EDDB
{
    public class EDDBClass
    {
        private string stationFileName;
        private string systemFileName;
        private string commoditiesFileName;

        private string stationTempFileName;
        private string systemTempFileName;
        private string commoditiesTempFileName;

        public static Dictionary<int, Commodity> commodities;

        public string SystemFileName { get { return systemFileName; } }

        public EDDBClass()
        {
            stationFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbstations.json");
            systemFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbsystems.json");
            commoditiesFileName = Path.Combine(Tools.GetAppDataDirectory(), "commodities.json");

            stationTempFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbstationslite_temp.json");
            systemTempFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbsystems_temp.json");
            commoditiesTempFileName = Path.Combine(Tools.GetAppDataDirectory(), "commodities_temp.json");

            if (commodities == null)
                ReadCommodities();
        }

        public bool GetSystems()
        {
            if (File.Exists(stationTempFileName)) File.Delete(stationTempFileName); // migration - remove obsolete file
            return DownloadFileHandler.DownloadFile("http://robert.astronet.se/Elite/eddb/v4/systems_populated.json", systemFileName);
        }


        public bool GetCommodities()
        {
            if (File.Exists(systemTempFileName)) File.Delete(systemTempFileName); // migration - remove obsolete file
            return DownloadFileHandler.DownloadFile("http://robert.astronet.se/Elite/eddb/v4/commodities.json", commoditiesFileName);
        }


        public bool GetStationsLite()
        {
            if (File.Exists(commoditiesTempFileName)) File.Delete(commoditiesTempFileName); // migration - remove obsolete file
            return DownloadFileHandler.DownloadFile("http://robert.astronet.se/Elite/eddb/v4/stations.json", stationFileName);
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

        public void  ReadCommodities()
        {
            Dictionary<int, Commodity> eddbcommodities = new Dictionary<int, Commodity>();
            string json;

            try
            {
                json = ReadJson(commoditiesFileName);

                if (json == null)
                    return;

                JArray jcommodities = (JArray)JArray.Parse(json);

                if (jcommodities != null)
                {
                    foreach (JObject jo in jcommodities)
                    {
                        Commodity com = new Commodity(jo);

                        if (com != null)
                            eddbcommodities[com.id] = com;
                    }
                }

                commodities = eddbcommodities;

            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.WriteLine("ReadCommodities error: {0}" + ex.Message);
            }
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
                    StationClass sys = new StationClass(jo, EDDiscovery2.DB.SystemInfoSource.EDDB);

                    if (sys != null)
                        eddbstations.Add(sys);
                }
            }

            return eddbstations;
        }
    }
}
