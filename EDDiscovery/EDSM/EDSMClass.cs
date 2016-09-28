using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using EDDiscovery2.EDDB;
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
    class EDSMClass : HttpCom
    {
        public string commanderName;
        public string apiKey;

        private readonly string fromSoftwareVersion;
        private readonly string fromSoftware;
        private string EDSMDistancesFileName;

        public EDSMClass()
        {
            fromSoftware = "EDDiscovery";
            _serverAddress = "https://www.edsm.net/";
            EDSMDistancesFileName = Path.Combine(Tools.GetAppDataDirectory(), "EDSMDistances.json");

            var assemblyFullName = Assembly.GetExecutingAssembly().FullName;
            fromSoftwareVersion = assemblyFullName.Split(',')[1].Split('=')[1];
        }
        
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

            var response = RequestPost("{ \"data\": " + query + " }", "api-v1/submit-distances");
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
            var response = RequestGet(query);
            if ((int)response.StatusCode >= 400)
                return null;

            return response.Body;
        }

        public string RequestSystems(string date)
        {
            DateTime dtDate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToLocalTime();

            if (dtDate.Subtract(new DateTime(2015, 5, 10)).TotalDays < 0)
                date = "2015-05-10 00:00:00";

            string query = "api-v1/systems" + "?startdatetime=" + HttpUtility.UrlEncode(date) + "&coords=1&submitted=1&known=1&showId=1";
            var response = RequestGet(query);
            return response.Body;
        }

        public string RequestDistances(string date)
        {
            string query;
            query = "?showId=1 & submitted=1 & startdatetime=" + HttpUtility.UrlEncode(date);

            var response = RequestGet("api-v1/distances" + query );
            return response.Body;
        }

        public string GetEDSMDistances()            // download a file of distances..
        {
            if (File.Exists(EDSMDistancesFileName))
                File.Delete(EDSMDistancesFileName);
            if (File.Exists(EDSMDistancesFileName + ".etag"))
                File.Delete(EDSMDistancesFileName + ".etag");

            if (EDDBClass.DownloadFile("https://www.edsm.net/dump/distances.json", EDSMDistancesFileName))
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

            if (SystemClass.IsSystemsTableEmpty())
            {
                lstsystdate = gammadate;
            }
            else
            {
                // Get the most recent modify time returned from EDSM
                lstsystdate = SystemClass.GetLastSystemModifiedTimeFast() - TimeSpan.FromSeconds(1);

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

                updates += SystemClass.ParseEDSMUpdateSystemsString(json, ref lstsyst, false, discoveryform, cancelRequested, reportProgress, false);
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
                EDDBClass.DownloadFile("https://www.edsm.net/api-v1/hidden-systems?showId=1", edsmhiddensystems, out newfile);

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

        public List<DistanceClass> GetDistances(string systemname)
        {
            List<DistanceClass> listDistances = new List<DistanceClass>();
            try
            {
                string query;
                query = "?sysname=" + HttpUtility.UrlEncode(systemname) + "&coords=1&distances=1&submitted=1";

                var response = RequestGet("api-v1/system" + query);

                if ((int)response.StatusCode >= 400)
                    return listDistances;


                var json = response.Body;

                //https://www.edsm.net/api-v1/system?sysname=Col+359+Sector+CP-Y+c1-18&coords=1&include_hidden=1&distances=1&submitted=1

                if (json.Length > 1)
                {
                    JObject ditancesresp = (JObject)JObject.Parse(json);

                    JArray distances = (JArray)ditancesresp["distances"];

                    if (distances != null)
                    {
                        foreach (JObject jo in distances)
                        {
                            DistanceClass dc = new DistanceClass();

                            dc.NameA = systemname;
                            dc.NameB = jo["name"].Value<string>();
                            dc.Dist = jo["dist"].Value<float>();
//                            dc.CommanderCreate = jo[]

                            listDistances.Add(dc);
                        }
                    }
                }
            }
            catch
            {
            }
            return listDistances;
        }

        public int GetComments(DateTime starttime, out List<SystemNoteClass> notes)
        {
            notes = new List<SystemNoteClass>();

            var json = GetComments(starttime);
            if (json == null)
            {
                return 0;
            }

            JObject msg = JObject.Parse(json);
            int msgnr = msg["msgnum"].Value<int>();

            JArray comments = (JArray)msg["comments"];
            if (comments != null)
            {
                foreach (JObject jo in comments)
                {
                    SystemNoteClass note = new SystemNoteClass();
                    note.Name = jo["system"].Value<string>();
                    note.Note = jo["comment"].Value<string>();
                    note.Time = DateTime.ParseExact(jo["lastUpdate"].Value<string>(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToLocalTime();
                    notes.Add(note);
                }
            }

            return msgnr;
        }

        public string GetComments(DateTime starttime)
        {
            string query = "get-comments?startdatetime=" + HttpUtility.UrlEncode(starttime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)) + "&apiKey=" + apiKey + "&commanderName=" + HttpUtility.UrlEncode(commanderName);
            //string query = "get-comments?apiKey=" + apiKey + "&commanderName=" + HttpUtility.UrlEncode(commanderName);
            var response = RequestGet("api-logs-v1/" + query);

            if ((int)response.StatusCode >= 400)
                return null;


            return response.Body;
        }


        public string GetComment(string systemName)
        {
            string query;
            query = "get-comment?systemName=" + HttpUtility.UrlEncode(systemName);

            var response = RequestGet("api-logs-v1/" + query);

            if ((int)response.StatusCode >= 400)
                return null;

            return response.Body;
        }

        public string SetComment(SystemNoteClass sn)
        {
            string query;
            query = "set-comment?systemName=" + HttpUtility.UrlEncode(sn.Name) + "&commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey + "&comment=" + HttpUtility.UrlEncode(sn.Note);
            var response = RequestGet("api-logs-v1/" + query);

            if ((int)response.StatusCode >= 400)
                return null;

            return response.Body;
        }

        public string SetLog(string systemName, DateTime dateVisited)
        {
            string query;
            query = "set-log?systemName=" + HttpUtility.UrlEncode(systemName) + "&commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey +
                 "&fromSoftware=" + HttpUtility.UrlEncode(fromSoftware) + "&fromSoftwareVersion=" + HttpUtility.UrlEncode(fromSoftwareVersion) +
                  "&dateVisited=" + HttpUtility.UrlEncode(dateVisited.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            var response = RequestGet("api-logs-v1/" + query);

            if ((int)response.StatusCode >= 400)
                return null;

            return response.Body;
        }

        public string SetLogWithPos(string systemName, DateTime dateVisited, double x, double y, double z)
        {
            string query;
            query = "set-log?systemName=" + HttpUtility.UrlEncode(systemName) + "&commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey +
                 "&fromSoftware=" + HttpUtility.UrlEncode(fromSoftware) + "&fromSoftwareVersion=" + HttpUtility.UrlEncode(fromSoftwareVersion) +
                 "&x=" + HttpUtility.UrlEncode(x.ToString(CultureInfo.InvariantCulture)) + "&y=" + HttpUtility.UrlEncode(y.ToString(CultureInfo.InvariantCulture)) + "&z=" + HttpUtility.UrlEncode(z.ToString(CultureInfo.InvariantCulture)) +
                  "&dateVisited=" + HttpUtility.UrlEncode(dateVisited.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            var response = RequestGet("api-logs-v1/" + query);

            if ((int)response.StatusCode >= 400)
                return null;

            return response.Body;
        }

        public int GetLogs(DateTime starttime, out List<HistoryEntry> log)
        {
            log = new List<HistoryEntry>();

            string query = "get-logs?startdatetime=" + HttpUtility.UrlEncode(starttime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)) + "&apiKey=" + apiKey + "&commanderName=" + HttpUtility.UrlEncode(commanderName);
            //string query = "get-logs?apiKey=" + apiKey + "&commanderName=" + HttpUtility.UrlEncode(commanderName);
            var response = RequestGet("api-logs-v1/" + query);

            if ((int)response.StatusCode >= 400)
                return 0;

            var json = response.Body;

            if (json == null)
                return 0;

            JObject msg = JObject.Parse(json);
            int msgnr = msg["msgnum"].Value<int>();

            JArray logs = (JArray)msg["logs"];

            if (logs != null)
            {
                foreach (JObject jo in logs)
                {
                    string name = jo["system"].Value<string>();
                    string ts = jo["date"].Value<string>();
                    DateTime et = DateTime.ParseExact(ts, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToLocalTime();

                    SystemClass sc = new SystemClass(name);

                    HistoryEntry he = new HistoryEntry();
                    he.MakeVSEntry(sc, et, EDDConfig.Instance.DefaultMapColour, "","");       // FSD jump entry
                    log.Add(he);
                }
            }

            return msgnr;
        }

        public bool IsKnownSystem(string sysName)
        {
            string query = "system?sysname=" + HttpUtility.UrlEncode(sysName) + "&commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey;
            var response = RequestGet("api-v1/" + query);
            var json = response.Body;
            if (json == null)
                return false;

            return (json.ToString() != "[]");
        }

        public List<String> GetPushedSystems()
        {
            List<String> systems = new List<string>();
            string query = "api-v1/systems?pushed=1";

            var response = RequestGet(query);
            if ((int)response.StatusCode >= 400)
                return null;


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
                string query = "system?sysname=" + encodedSys + "&commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey + "&showId=1";
                var response = RequestGet("api-v1/" + query);
                var json = response.Body;
                if (json == null || json.ToString() == "[]")
                    return "";

                JObject msg = JObject.Parse(json);
                sysID = msg["id"].Value<string>();
            }

            string url = "https://www.edsm.net/show-system/index/id/" + sysID + "/name/" + encodedSys;
            return url;
        }

    }
}
