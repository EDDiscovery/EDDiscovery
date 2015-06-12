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

        public bool GetSystems()
        {
            try
            {
                if (File.Exists(systemFileNameTemp))
                    File.Delete(systemFileNameTemp);

                WebClient webClient = new WebClient();
                webClient.DownloadFile("http://eddb.io/archive/v3/systems.json", systemFileNameTemp);

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


        public bool GetStationsLite()
        {

            try
            {
                if (File.Exists(stationFileNameTemp))
                    File.Delete(stationFileNameTemp);

                WebClient webClient = new WebClient();
                webClient.DownloadFile("http://eddb.io/archive/v3/stations_lite.json", stationFileNameTemp);

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



    }
}
