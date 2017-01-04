using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery2.DB;
using EDDiscovery2.HTTP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace EDDiscovery2.EDSM
{
    public class EDSMClass : HttpCom
    {
        public string commanderName;
        public string apiKey;
        public bool IsApiKeySet { get { return !(string.IsNullOrEmpty(commanderName) || string.IsNullOrEmpty(apiKey)); } }

        private readonly string fromSoftwareVersion;
        private readonly string fromSoftware;
        private string EDSMDistancesFileName;
        static private Dictionary<int, List<JournalScan>> DictEDSMBodies = new Dictionary<int, List<JournalScan>>();

        public EDSMClass()
        {
            fromSoftware = "EDDiscovery";
            var assemblyFullName = Assembly.GetExecutingAssembly().FullName;
            fromSoftwareVersion = assemblyFullName.Split(',')[1].Split('=')[1];

            _serverAddress = ServerAddress;

            EDSMDistancesFileName = Path.Combine(Tools.GetAppDataDirectory(), "EDSMDistances.json");

            apiKey = EDDiscoveryForm.EDDConfig.CurrentCommander.APIKey;
            commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.EdsmName;
        }

        static string edsm_server_address = "https://www.edsm.net/";
        public static string ServerAddress { get { return edsm_server_address; } set { edsm_server_address = value; } }
        public static bool IsServerAddressValid { get { return edsm_server_address.Length > 0; } }

        public string SubmitDistances(string cmdr, string from, string to, double dist)
        {
            return SubmitDistances(cmdr, from, new Dictionary<string, double> { { to, dist } });
        }

        public string SubmitDistances(string cmdr, string from, Dictionary<string, double> distances)
        {
            string query = "{\"ver\":2," + " \"commander\":\"" + cmdr + "\", \"fromSoftware\":\"" + fromSoftware + "\",  \"fromSoftwareVersion\":\"" + fromSoftwareVersion + "\", \"p0\": { \"name\": \"" + from + "\" },   \"refs\": [";

            var counter = 0;
            foreach (var item in distances)
            {
                if (counter++ > 0)
                {
                    query += ",";
                }

                var to = item.Key;
                var distance = item.Value.ToString("0.00", CultureInfo.InvariantCulture);

                query += " { \"name\": \"" + to + "\",  \"dist\": " + distance + " } ";
            }


            query += "] } ";

            var response = RequestPost("{ \"data\": " + query + " }", "api-v1/submit-distances", handleException: true);
            if (response.Error)
                return null;
            var data = response.Body;
            return response.Body;
        }


        public bool ShowDistanceResponse(string json, out string respstr, out Boolean trilOK)
        {
            bool retval = true;
            JObject edsm = null;
            trilOK = false;

            respstr = "";

            try
            {
                if (json == null)
                    return false;

                edsm = (JObject)JObject.Parse(json);

                if (edsm == null)
                    return false;

                JObject basesystem = (JObject)edsm["basesystem"];
                JArray distances = (JArray)edsm["distances"];


                if (distances != null)
                    foreach (var st in distances)
                    {
                        int statusnum = st["msgnum"].Value<int>();

                        if (statusnum == 201)
                            retval = false;

                        respstr += "Status " + statusnum.ToString() + " : " + st["msg"].Value<string>() + Environment.NewLine;

                    }

                if (basesystem != null)
                {

                    int statusnum = basesystem["msgnum"].Value<int>();

                    if (statusnum == 101)
                        retval = false;

                    if (statusnum == 102 || statusnum == 104)
                        trilOK = true;

                    respstr += "System " + statusnum.ToString() + " : " + basesystem["msg"].Value<string>() + Environment.NewLine;
                }

                return retval;
            }
            catch (Exception ex)
            {
                respstr += "Exception in ShowDistanceResponse: " + ex.Message;
                return false;
            }
        }

        public string RequestSystems(DateTime startdate, DateTime enddate)
        {
            DateTime gammadate = new DateTime(2015, 5, 10, 0, 0, 0, DateTimeKind.Utc);
            if (startdate < gammadate)
            {
                startdate = gammadate;
            }

            string query = "api-v1/systems" +
                "?startdatetime=" + HttpUtility.UrlEncode(startdate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)) +
                "&enddatetime=" + HttpUtility.UrlEncode(enddate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)) +
                "&coords=1&submitted=1&known=1&showId=1";
            var response = RequestGet(query, handleException: true);
            if (response.Error)
                return null;

            return response.Body;
        }

        public string RequestSystems(string date)
        {
            DateTime dtDate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToLocalTime();

            if (dtDate.Subtract(new DateTime(2015, 5, 10)).TotalDays < 0)
                date = "2015-05-10 00:00:00";

            string query = "api-v1/systems" + "?startdatetime=" + HttpUtility.UrlEncode(date) + "&coords=1&submitted=1&known=1&showId=1";
            var response = RequestGet(query, handleException: true);
            if (response.Error)
                return null;
            return response.Body;
        }

        public string RequestDistances(string date)
        {
            string query;
            query = "?showId=1 & submitted=1 & startdatetime=" + HttpUtility.UrlEncode(date);

            var response = RequestGet("api-v1/distances" + query, handleException: true);
            if (response.Error)
                return null;
            return response.Body;
        }

        public string GetEDSMDistances()            // download a file of distances..
        {
            if (File.Exists(EDSMDistancesFileName))
                File.Delete(EDSMDistancesFileName);
            if (File.Exists(EDSMDistancesFileName + ".etag"))
                File.Delete(EDSMDistancesFileName + ".etag");

            if (DownloadFileHandler.DownloadFile(_serverAddress + "dump/distances.json", EDSMDistancesFileName))
                return EDSMDistancesFileName;
            else
                return null;
        }
        
        internal long GetNewSystems(EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress)
        {
            string lstsyst;

            DateTime lstsystdate;
            // First system in EDSM is from 2015-05-01 00:39:40
            DateTime gammadate = new DateTime(2015, 5, 1, 0, 0, 0, DateTimeKind.Utc);
            bool outoforder = SQLiteConnectionSystem.GetSettingBool("EDSMSystemsOutOfOrder", true);

            if (SystemClass.IsSystemsTableEmpty())
            {
                lstsystdate = gammadate;
            }
            else
            {
                // Get the most recent modify time returned from EDSM
                DateTime lastmod = outoforder ? SystemClass.GetLastSystemModifiedTime() : SystemClass.GetLastSystemModifiedTimeFast();
                lstsystdate = lastmod - TimeSpan.FromSeconds(1);

                if (lstsystdate < gammadate)
                {
                    lstsystdate = gammadate;
                }
            }

            lstsyst = lstsystdate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            Console.WriteLine("EDSM Check date: " + lstsyst);

            long updates = 0;

            while (lstsystdate < DateTime.UtcNow)
            {
                if (cancelRequested())
                    return updates;

                DateTime enddate = lstsystdate + TimeSpan.FromHours(12);
                if (enddate > DateTime.UtcNow)
                {
                    enddate = DateTime.UtcNow;
                }

                discoveryform.LogLine($"Downloading systems from {lstsystdate.ToLocalTime().ToString()} to {enddate.ToLocalTime().ToString()}");
                reportProgress(-1, "Requesting systems from EDSM");
                string json = null;

                try
                {
                    json = RequestSystems(lstsystdate, enddate);
                }
                catch (WebException ex)
                {
                    reportProgress(-1, $"EDSM request failed");
                    if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null && ex.Response is HttpWebResponse)
                    {
                        string status = ((HttpWebResponse)ex.Response).StatusDescription;
                        discoveryform.LogLine($"Download of EDSM systems from the server failed ({status}), will try next time program is run");
                    }
                    else
                    {
                        discoveryform.LogLine($"Download of EDSM systems from the server failed ({ex.Status.ToString()}), will try next time program is run");
                    }
                    break;
                }
                catch (Exception ex)
                {
                    reportProgress(-1, $"EDSM request failed");
                    discoveryform.LogLine($"Download of EDSM systems from the server failed ({ex.Message}), will try next time program is run");
                    break;
                }

                if (json == null)
                {
                    reportProgress(-1, "EDSM request failed");
                    discoveryform.LogLine("Download of EDSM systems from the server failed (no data returned), will try next time program is run");
                    break;
                }

                updates += SystemClass.ParseEDSMUpdateSystemsString(json, ref lstsyst, ref outoforder, false, discoveryform, cancelRequested, reportProgress, false);
                lstsystdate += TimeSpan.FromHours(12);
            }
            discoveryform.LogLine($"System download complete");

            return updates;
        }


        internal string GetHiddenSystems()
        {
            try
            {
                string edsmhiddensystems = Path.Combine(Tools.GetAppDataDirectory(), "edsmhiddensystems.json");
                bool newfile = false;
                DownloadFileHandler.DownloadFile(_serverAddress + "api-v1/hidden-systems?showId=1", edsmhiddensystems, out newfile);

                string json = EDDiscovery.EDDiscoveryForm.LoadJsonFile(edsmhiddensystems);

                return json;
            }
            
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception: {ex.Message}");
                Trace.WriteLine($"ETrace: {ex.StackTrace}");
                return null;
            }
        
        }


        public string GetComments(DateTime starttime)
        {
            if (!IsApiKeySet)
                return null;

            string query = "get-comments?startdatetime=" + HttpUtility.UrlEncode(starttime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)) + "&apiKey=" + apiKey + "&commanderName=" + HttpUtility.UrlEncode(commanderName) + "&showId=1";
            //string query = "get-comments?apiKey=" + apiKey + "&commanderName=" + HttpUtility.UrlEncode(commanderName);
            var response = RequestGet("api-logs-v1/" + query, handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }


        public string GetComment(string systemName, long edsmid = 0)
        {
            if (!IsApiKeySet)
                return null;

            string query;
            query = "get-comment?systemName=" + HttpUtility.UrlEncode(systemName) + "&commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey;
            if (edsmid > 0)
                query += "&systemId=" + edsmid;

            var response = RequestGet("api-logs-v1/" + query, handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        public string SetComment(string systemName, string note, long edsmid = 0)
        {
            if (!IsApiKeySet)
                return null;

            string query;
            query = "set-comment?systemName=" + HttpUtility.UrlEncode(systemName) + "&commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey + "&comment=" + HttpUtility.UrlEncode(note);

            if (edsmid > 0)
            {
                // For future use when EDSM adds the ability to link a comment to a system by EDSM ID
                query += "&systemId=" + edsmid;
            }

            var response = RequestGet("api-logs-v1/" + query, handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        public string SetLog(string systemName, DateTime dateVisitedutc)
        {
            if (!IsApiKeySet)
                return null;

            string query;
            query = "set-log?systemName=" + HttpUtility.UrlEncode(systemName) + "&commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey +
                 "&fromSoftware=" + HttpUtility.UrlEncode(fromSoftware) + "&fromSoftwareVersion=" + HttpUtility.UrlEncode(fromSoftwareVersion) +
                  "&dateVisited=" + HttpUtility.UrlEncode(dateVisitedutc.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            var response = RequestGet("api-logs-v1/" + query, handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        public string SetLogWithPos(string systemName, DateTime dateVisitedutc, double x, double y, double z)
        {
            if (!IsApiKeySet)
                return null;

            string query;
            query = "set-log?systemName=" + HttpUtility.UrlEncode(systemName) + "&commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey +
                 "&fromSoftware=" + HttpUtility.UrlEncode(fromSoftware) + "&fromSoftwareVersion=" + HttpUtility.UrlEncode(fromSoftwareVersion) +
                 "&x=" + HttpUtility.UrlEncode(x.ToString(CultureInfo.InvariantCulture)) + "&y=" + HttpUtility.UrlEncode(y.ToString(CultureInfo.InvariantCulture)) + "&z=" + HttpUtility.UrlEncode(z.ToString(CultureInfo.InvariantCulture)) +
                  "&dateVisited=" + HttpUtility.UrlEncode(dateVisitedutc.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

            var response = RequestGet("api-logs-v1/" + query, handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        public bool SendTravelLog(string name, DateTime timeutc, bool coord, double x, double y, double z, out string error)
        {
            if (!IsApiKeySet)
            {
                error = "EDSM API Key not set";
                return false;
            }

            error = "";

            string json = null;

            try
            {
                if (!coord)
                    json = SetLog(name, timeutc);
                else
                    json = SetLogWithPos(name, timeutc, x, y, z);
            }
            catch (Exception ex)
            {
                error = "EDSM sync error, connection to server failed";
                System.Diagnostics.Trace.WriteLine("EDSM Sync error:" + ex.ToString());
            }

            if (json != null)
            {
                JObject msg = (JObject)JObject.Parse(json);

                int msgnum = msg["msgnum"].Value<int>();
                string msgstr = msg["msg"].Value<string>();

                if (msgnum == 100 || msgnum == 401 || msgnum == 402 || msgnum == 403)
                {
                    return true;
                }
                else
                {
                    error = "EDSM sync ERROR:" + msgnum.ToString() + ":" + msgstr;
                    System.Diagnostics.Trace.WriteLine("Error sync:" + msgnum.ToString() + " : " + name);
                }
            }

            return false;
        }

        public int GetLogs(DateTime starttimeutc, out List<HistoryEntry> log)     
        {
            log = new List<HistoryEntry>();

            if (!IsApiKeySet)
                return 0;

            string query = "get-logs?showId=1&startdatetime=" + HttpUtility.UrlEncode(starttimeutc.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)) + "&apiKey=" + apiKey + "&commanderName=" + HttpUtility.UrlEncode(commanderName);
            //string query = "get-logs?apiKey=" + apiKey + "&commanderName=" + HttpUtility.UrlEncode(commanderName);

            var response = RequestGet("api-logs-v1/" + query, handleException: true);

            if (response.Error)
                return 0;

            var json = response.Body;

            if (json == null)
                return 0;

            JObject msg = JObject.Parse(json);
            int msgnr = msg["msgnum"].Value<int>();

            JArray logs = (JArray)msg["logs"];

            if (logs != null)
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    foreach (JObject jo in logs)
                    {
                        string name = jo["system"].Value<string>();
                        string ts = jo["date"].Value<string>();
                        long id = jo["systemId"].Value<long>();
                        DateTime etutc = DateTime.ParseExact(ts, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal|DateTimeStyles.AssumeUniversal); // UTC time

                        SystemClass sc = SystemClass.GetSystem(id, cn, SystemClass.SystemIDType.EdsmId);
                        if (sc == null)
                            sc = new SystemClass(name)
                            {
                                id_edsm = id
                            };

                        HistoryEntry he = new HistoryEntry();
                        he.MakeVSEntry(sc, etutc, EDDConfig.Instance.DefaultMapColour, "", "");       // FSD jump entry
                        log.Add(he);
                    }
                }
            }

            return msgnr;
        }

        public bool IsKnownSystem(string sysName)
        {
            string query = "system?sysname=" + HttpUtility.UrlEncode(sysName);
            string json = null;
            var response = RequestGet("api-v1/" + query, handleException: true);
            if (response.Error)
                return false;
            json = response.Body;

            if (json == null)
                return false;

            return (json.ToString() != "[]");
        }

        public List<String> GetPushedSystems()
        {
            List<String> systems = new List<string>();
            string query = "api-v1/systems?pushed=1";

            var response = RequestGet(query, handleException: true);
            if (response.Error)
                return systems;

            var json = response.Body;
            if (json == null)
                return systems;

            JArray msg = JArray.Parse(json);

            if (msg != null)
            {
                foreach (JObject sysname in msg)
                {
                    systems.Add(sysname["name"].ToString());
                }
            }

            return systems;
        }

        public bool ShowSystemInEDSM(string sysName, long? id_edsm = null)
        {
            string url = GetUrlToEDSMSystem(sysName, id_edsm);
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            else
            {
                System.Diagnostics.Process.Start(url);
            }
            return true;
        }

        public string GetUrlToEDSMSystem(string sysName, long? id_edsm = null)
        {
            string sysID;
            string encodedSys = HttpUtility.UrlEncode(sysName);

            if (id_edsm != null)
            {
                sysID = id_edsm.ToString();
            }
            else
            {
                string query = "system?sysname=" + encodedSys + "&showId=1";
                var response = RequestGet("api-v1/" + query, handleException: true);
                if (response.Error)
                    return "";

                var json = response.Body;
                if (json == null || json.ToString() == "[]")
                    return "";

                JObject msg = JObject.Parse(json);
                sysID = msg["id"].Value<string>();
            }

            string url = _serverAddress + "system/id/" + sysID + "/name/" + encodedSys;
            return url;
        }

        // https://www.edsm.net/api-system-v1/bodies?systemName=Colonia
        // https://www.edsm.net/api-system-v1/bodies?systemId=27


        public JObject GetBodies(string sysName)
        {
            string encodedSys = HttpUtility.UrlEncode(sysName);

            string query = "bodies?systemName=" + sysName;
            var response = RequestGet("api-v1/" + query, handleException: true);
            if (response.Error)
                return null;

            var json = response.Body;
            if (json == null || json.ToString() == "[]")
                return null;

            JObject msg = JObject.Parse(json);
            return msg;
        }

        public JObject GetBodies(int edsmID)
        {
            string query = "bodies?systemId=" + edsmID.ToString();
            var response = RequestGet("api-system-v1/" + query, handleException: true);
            if (response.Error)
                return null;

            var json = response.Body;
            if (json == null || json.ToString() == "[]")
                return null;

            JObject msg = JObject.Parse(json);
            return msg;
        }

        public  static List<JournalScan> GetBodiesList(int edsmid)
        {
            if (DictEDSMBodies.ContainsKey(edsmid))  // Cache EDSM bidies during run of EDD.
                return DictEDSMBodies[edsmid];

            List<JournalScan> bodies = new List<JournalScan>();

            EDSMClass edsm = new EDSMClass();

            JObject jo = edsm.GetBodies(edsmid);  // Colonia 

            if (jo != null)
            {
                foreach (JObject bodie in jo["bodies"])
                {
                    EDSMClass.ConvertFromEDSMBodies(bodie);
                    JournalScan js = new JournalScan(bodie);
                    js.EdsmID = edsmid;
                    
                    bodies.Add(js);
                }
                DictEDSMBodies[edsmid] = bodies;
                return bodies;
            }

            DictEDSMBodies[edsmid] = null;
            return null;
        }


        public static JObject ConvertFromEDSMBodies(JObject jo)
        {
            jo["timestamp"] = DateTime.UtcNow.ToString("yyyy -MM-ddTHH:mm:ssZ");
            jo["EDDFromEDSMBodie"] = true;

            JSONHelper.Rename(jo["name"], "BodyName");
            if (jo["type"].Value<string>().Equals("Star"))
            {
                JSONHelper.Rename(jo["subType"], "StarType");   // Remove extra text from EDSM   ex  "F (White) Star" -> "F"
                string startype = jo["StarType"].Value<string>();
                if (startype == null)
                    startype = "unknown";
                int index = startype.IndexOf("(");
                if (index > 0)
                    startype = startype.Substring(0, index).Trim();
                jo["StarType"] = startype;

                JSONHelper.Rename(jo["age"], "Age_MY");
                JSONHelper.Rename(jo["solarMasses"], "StellarMass");
                JSONHelper.Rename(jo["solarRadius"], "Radius");
                JSONHelper.Rename(jo["orbitalEccentricity"], "Eccentricity");
                JSONHelper.Rename(jo["argOfPeriapsis"], "Periapsis");
                JSONHelper.Rename(jo["rotationalPeriod"], "RotationPeriod");
                JSONHelper.Rename(jo["rotationalPeriodTidallyLocked"], "TidalLock");
            }

            if (jo["type"].Value<string>().Equals("Planet"))
            {
                JSONHelper.Rename(jo["isLandable"], "Landable");
                JSONHelper.Rename(jo["earthMasses"], "MassEM");
                JSONHelper.Rename(jo["volcanismType"], "Volcanism");
                JSONHelper.Rename(jo["atmosphereType"], "Atmosphere");
                JSONHelper.Rename(jo["orbitalEccentricity"], "Eccentricity");
                JSONHelper.Rename(jo["argOfPeriapsis"], "Periapsis");
                JSONHelper.Rename(jo["rotationalPeriod"], "RotationPeriod");
                JSONHelper.Rename(jo["rotationalPeriodTidallyLocked"], "TidalLock");
                JSONHelper.Rename(jo["subType"], "PlanetClass");
                JSONHelper.Rename(jo["radius"], "Radius");
            }

            JSONHelper.Rename(jo["belts"], "Rings");
            JSONHelper.Rename(jo["rings"], "Rings");
            JSONHelper.Rename(jo["semiMajorAxis"], "SemiMajorAxis");
            JSONHelper.Rename(jo["surfaceTemperature"], "SurfaceTemperature");
            JSONHelper.Rename(jo["orbitalPeriod"], "OrbitalPeriod");
            JSONHelper.Rename(jo["semiMajorAxis"], "SemiMajorAxis");
            JSONHelper.Rename(jo["surfaceTemperature"], "SurfaceTemperature");
            JSONHelper.Rename(jo["surfacePressure"], "SurfacePressure");
            JSONHelper.Rename(jo["orbitalInclination"], "OrbitalInclination");
            JSONHelper.Rename(jo["materials"], "Materials");
            JSONHelper.Rename(jo["surfaceTemperature"], "SurfaceTemperature");
            JSONHelper.Rename(jo["surfaceTemperature"], "SurfaceTemperature");
            JSONHelper.Rename(jo["surfaceTemperature"], "SurfaceTemperature");
            JSONHelper.Rename(jo["surfaceTemperature"], "SurfaceTemperature");
            JSONHelper.Rename(jo["surfaceTemperature"], "SurfaceTemperature");



            if (!JSONHelper.IsNullOrEmptyT(jo["Rings"]))
            {
                foreach (JObject ring in jo["Rings"])
                {
                    JSONHelper.Rename(ring["innerRadius"], "InnerRad");
                    JSONHelper.Rename(ring["outerRadius"], "OuterRad");
                    JSONHelper.Rename(ring["mass"], "MassMT");
                    JSONHelper.Rename(ring["type"], "RingClass");
                }
            }


            if (!JSONHelper.IsNullOrEmptyT(jo["Materials"]))  // Check if matieals has null
            {
                Dictionary<string, double?> mats;
                Dictionary<string, double> mats2;
                mats = jo["Materials"]?.ToObject<Dictionary<string, double?>>();
                mats2 = new Dictionary<string, double>();

                foreach (string key in mats.Keys)
                    if (mats[key] == null)
                        mats2[key] = 0.0;
                    else
                        mats2[key] = mats[key].Value;

                jo["Materials"] = JObject.FromObject(mats2);
            }


            return jo;

        }

    }



}
