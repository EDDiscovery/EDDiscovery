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

namespace EDDiscovery2.EDDB
{
    public class EDDBClass
    {
        private string stationFileName;
        private string systemFileName;
        private string commoditiesFileName;
        private string EDSMDistancesFileName;

        private string stationTempFileName;
        private string systemTempFileName;
        private string commoditiesTempFileName;

        public static Dictionary<int, Commodity> commodities;


        public EDDBClass()
        {
            stationFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbstations.json");
            systemFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbsystems.json");
            commoditiesFileName = Path.Combine(Tools.GetAppDataDirectory(), "commodities.json");
            EDSMDistancesFileName = Path.Combine(Tools.GetAppDataDirectory(), "EDSMDistances.json");

            stationTempFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbstationslite_temp.json");
            systemTempFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbsystems_temp.json");
            commoditiesTempFileName = Path.Combine(Tools.GetAppDataDirectory(), "commodities_temp.json");

            if (commodities == null)
                ReadCommodities();
        }

        public bool GetSystems()
        {
            if (File.Exists(stationTempFileName)) File.Delete(stationTempFileName); // migration - remove obsolete file
            return DownloadFile("http://robert.astronet.se/Elite/eddb/v4/systems.json", systemFileName);
        }

        public bool GetEDSMDistances()
        {
            if (File.Exists(EDSMDistancesFileName))
                File.Delete(EDSMDistancesFileName);
            if (File.Exists(EDSMDistancesFileName + ".etag"))
                File.Delete(EDSMDistancesFileName + ".etag");
            return DownloadFile("http://robert.astronet.se/Elite/edsm/edsmdist.json", EDSMDistancesFileName);


        }

        public bool GetCommodities()
        {
            if (File.Exists(systemTempFileName)) File.Delete(systemTempFileName); // migration - remove obsolete file
            return DownloadFile("http://robert.astronet.se/Elite/eddb/v4/commodities.json", commoditiesFileName);
        }


        public bool GetStationsLite()
        {
            if (File.Exists(commoditiesTempFileName)) File.Delete(commoditiesTempFileName); // migration - remove obsolete file
            return DownloadFile("http://robert.astronet.se/Elite/eddb/v4/stations.json", stationFileName);
        }

        private bool DownloadFile(string url, string filename)
        {
            bool newfile = false;
            return DownloadFile(url, filename, out newfile);
        }

        static public bool DownloadFile(string url, string filename, out bool newfile)
        {
            var etagFilename = filename + ".etag";
            var tmpFilename = filename + ".tmp";
            var tmpEtagFilename = etagFilename + ".tmp";
            newfile = false;

            HttpCom.WriteLog("DownloadFile", url);
            var request = (HttpWebRequest) HttpWebRequest.Create(url);
            request.UserAgent = "EDDiscovery v" + Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            
            if (File.Exists(etagFilename))
            {
                var etag = File.ReadAllText(etagFilename);
                if (etag != "")
                {
                    request.Headers[HttpRequestHeader.IfNoneMatch] = etag;
                }
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                HttpCom.WriteLog("Response", response.StatusCode.ToString());

                File.WriteAllText(tmpEtagFilename, response.Headers[HttpResponseHeader.ETag]);
                var destFileStream = File.Open(tmpFilename, FileMode.Create, FileAccess.Write);
                response.GetResponseStream().CopyTo(destFileStream);

                destFileStream.Close();
                response.Close();

                if (File.Exists(filename))
                    File.Delete(filename);
                if (File.Exists(etagFilename))
                    File.Delete(etagFilename);

                File.Move(tmpFilename, filename);
                File.Move(tmpEtagFilename, etagFilename);

                newfile = true;
                return true;
            }
            catch (WebException ex)
            {
                var code = ((HttpWebResponse)ex.Response).StatusCode;
                if (code == HttpStatusCode.NotModified)
                {
                    System.Diagnostics.Trace.WriteLine("EDDB: " + filename + " up to date (etag).");
                    HttpCom.WriteLog(filename, "up to date (etag).");
                    return true;
                }
                System.Diagnostics.Trace.WriteLine("DownloadFile Exception:" + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                HttpCom.WriteLog("Exception", ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("DownloadFile Exception:" + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                HttpCom.WriteLog("DownloadFile Exception", ex.Message);
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

        public long UpdateSystems()
        {
            long updates = 0;

            StreamReader sr = new StreamReader(systemFileName);         // read directly from file..

            if (sr == null)
                return 0;

            JsonTextReader jr = new JsonTextReader(sr);               

            if (jr == null)
                return 0;

            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))  // open the db
            {
                cn.Open();

                SQLiteCommand cmd = new SQLiteCommand("select * from Systems where name = @name limit 1", cn);   // 1 return matching name

                List<SystemClass> toupdate = new List<SystemClass>();

                while (jr.Read())
                {
                    if (jr.TokenType == JsonToken.StartObject)
                    {
                        JObject jo = JObject.Load(jr);

                        SystemClass system = new SystemClass(jo, EDDiscovery2.DB.SystemInfoSource.EDDB);

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("name", system.name);

                        SQLiteDataReader reader1 = cmd.ExecuteReader();              // case insensitive
                        if (reader1.Read())                                          // its there..
                        {
                            SystemClass dbsys = new SystemClass(reader1);

                            if (dbsys.eddb_updated_at != system.eddb_updated_at || dbsys.population != system.population)
                            {
                                dbsys.id_eddb = system.id_eddb;
                                dbsys.faction = system.faction;
                                dbsys.population = system.population;
                                dbsys.government = system.government;
                                dbsys.allegiance = system.allegiance;
                                dbsys.state = system.state;
                                dbsys.security = system.security;
                                dbsys.primary_economy = system.primary_economy;
                                dbsys.needs_permit = system.needs_permit;
                                dbsys.eddb_updated_at = system.eddb_updated_at;

                                toupdate.Add(dbsys);                                // add to update list
                            }
                        }

                        cmd.Reset();
                    }
                }

                SQLiteTransaction transaction = cn.BeginTransaction();

                updates = toupdate.Count;

                foreach (SystemClass sys in toupdate)                               // commit updates
                {
                    sys.Update(cn, sys.id, transaction);
                }

                transaction.Commit();

                cn.Close();

                GC.Collect();
            }

            return updates;
        }

    }
}
