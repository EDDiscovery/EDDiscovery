using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;
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

        private string stationTempFileName;
        private string systemTempFileName;
        private string commoditiesTempFileName;

        public EDDBClass()
        {
            stationFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbstationslite.json");
            systemFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbsystems.json");
            commoditiesFileName = Path.Combine(Tools.GetAppDataDirectory(), "commodities.json");

            stationTempFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbstationslite_temp.json");
            systemTempFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbsystems_temp.json");
            commoditiesTempFileName = Path.Combine(Tools.GetAppDataDirectory(), "commodities_temp.json");
        
        }

        public bool GetSystems()
        {
            if (File.Exists(stationTempFileName)) File.Delete(stationTempFileName); // migration - remove obsolete file
            return DownloadFile("http://robert.astronet.se/Elite/eddb/systems.json", systemFileName);
        }

        public bool GetCommodities()
        {
            if (File.Exists(systemTempFileName)) File.Delete(systemTempFileName); // migration - remove obsolete file
            return DownloadFile("http://robert.astronet.se/Elite/eddb/commodities.json", commoditiesFileName);
        }


        public bool GetStationsLite()
        {
            if (File.Exists(commoditiesTempFileName)) File.Delete(commoditiesTempFileName); // migration - remove obsolete file
            return DownloadFile("http://robert.astronet.se/Elite/eddb/stations_lite.json", stationFileName);
        }

        private bool DownloadFile(string url, string filename)
        {
            var etagFilename = filename + ".etag";
            var tmpFilename = filename + ".tmp";
            var tmpEtagFilename = etagFilename + ".tmp";
            
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

                return true;
            }
            catch (WebException ex)
            {
                var code = ((HttpWebResponse)ex.Response).StatusCode;
                if (code == HttpStatusCode.NotModified)
                {
                    System.Diagnostics.Trace.WriteLine("EDDB: " + filename + " up to date (etag).");
                    return true;
                }
                System.Diagnostics.Trace.WriteLine("Exception:" + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("DownloadFile Exception:" + ex.Message);
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

            //int lastupdated =  db.QueryValueInt("SELECT Max(eddb_updated_at ) FROM Systems", -1);

            //var result = from a in eddbsystems where a.eddb_updated_at > lastupdated  orderby a.eddb_updated_at  select a;

            //if (result.Count() == 0)
            //    return true ;


            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                cn.Open();
                int nr=0;

                using (var tra = cn.BeginTransaction())
                {
                    try
                    {
                        foreach (SystemClass sys in eddbsystems)
                        {
                            SystemClass sysdb = SystemData.GetSystem(sys.name);

                            if (sysdb != null)  // Update system
                            {
                                if (sysdb.eddb_updated_at != sys.eddb_updated_at)
                                {
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


                                    sysdb.Update(cn, sysdb.id, tra);
                                    nr++;
                                }
                            }
                            else
                            {
                                System.Diagnostics.Trace.WriteLine("New system " + sys.name);
                            }
                        }
                        System.Diagnostics.Trace.WriteLine("Add2DB  " + nr.ToString() + " eddb systems: " + sw.Elapsed.TotalSeconds.ToString("0.000s"));
                        tra.Commit();
                        sw.Stop();
                        System.Diagnostics.Trace.WriteLine("Add2DB  " + nr.ToString() + " eddb systems: " + sw.Elapsed.TotalSeconds.ToString("0.000s"));
                    }
                    catch (Exception ex)
                    {
                        tra.Rollback();
                        System.Diagnostics.Trace.WriteLine("Add2DB error: {0}" + ex.Message);
                        throw;
                    }

                }
            }

            return true;
        }

    }
}
