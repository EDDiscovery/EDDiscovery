/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web;
using System.Linq;

namespace EliteDangerousCore.EDSM
{
    public partial class EDSMClass : BaseUtils.HttpCom
    {
        public string commanderName;
        public string apiKey;
        public bool IsApiKeySet { get { return !(string.IsNullOrEmpty(commanderName) || string.IsNullOrEmpty(apiKey)); } }

        private readonly string fromSoftwareVersion;
        private readonly string fromSoftware;
        static private Dictionary<long, List<JournalScan>> DictEDSMBodies = new Dictionary<long, List<JournalScan>>();

        public EDSMClass()
        {
            fromSoftware = "EDDiscovery";
            var assemblyFullName = Assembly.GetEntryAssembly().FullName;
            fromSoftwareVersion = assemblyFullName.Split(',')[1].Split('=')[1];

            _serverAddress = ServerAddress;

            apiKey = EDCommander.Current.APIKey;
            commanderName = EDCommander.Current.EdsmName;
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

            MimeType = "application/json; charset=utf-8";
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
            query = "?showId=1&submitted=1&startdatetime=" + HttpUtility.UrlEncode(date);

            var response = RequestGet("api-v1/distances" + query, handleException: true);
            if (response.Error)
                return null;
            return response.Body;
        }

        
        internal long GetNewSystems(Func<bool> cancelRequested, Action<int, string> reportProgress, Action<string> logLine)
        {
            string lstsyst;

            DateTime lstsystdate;
            // First system in EDSM is from 2015-05-01 00:39:40
            DateTime gammadate = new DateTime(2015, 5, 1, 0, 0, 0, DateTimeKind.Utc);
            bool outoforder = SQLiteConnectionSystem.GetSettingBool("EDSMSystemsOutOfOrder", true);

            if (SystemClassDB.IsSystemsTableEmpty())
            {
                lstsystdate = gammadate;
            }
            else
            {
                // Get the most recent modify time returned from EDSM
                DateTime lastmod = outoforder ? SystemClassDB.GetLastSystemModifiedTime() : SystemClassDB.GetLastSystemModifiedTimeFast();
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

                logLine($"Downloading systems from {lstsystdate.ToLocalTime().ToString()} to {enddate.ToLocalTime().ToString()}");
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
                        logLine($"Download of EDSM systems from the server failed ({status}), will try next time program is run");
                    }
                    else
                    {
                        logLine($"Download of EDSM systems from the server failed ({ex.Status.ToString()}), will try next time program is run");
                    }
                    break;
                }
                catch (Exception ex)
                {
                    reportProgress(-1, $"EDSM request failed");
                    logLine($"Download of EDSM systems from the server failed ({ex.Message}), will try next time program is run");
                    break;
                }

                if (json == null)
                {
                    reportProgress(-1, "EDSM request failed");
                    logLine("Download of EDSM systems from the server failed (no data returned), will try next time program is run");
                    break;
                }

                updates += SystemClassEDSM.ParseEDSMUpdateSystemsString(json, ref lstsyst, ref outoforder, false, cancelRequested, reportProgress, false);
                lstsystdate += TimeSpan.FromHours(12);
            }
            logLine($"System download complete");

            return updates;
        }


        public string GetHiddenSystems()
        {
            try
            {
                string edsmhiddensystems = Path.Combine(EliteConfigInstance.InstanceOptions.AppDataDirectory, "edsmhiddensystems.json");
                bool newfile = false;
                BaseUtils.DownloadFileHandler.DownloadFile(_serverAddress + "api-v1/hidden-systems?showId=1", edsmhiddensystems, out newfile);

                string json = BaseUtils.FileHelpers.TryReadAllTextFromFile(edsmhiddensystems);

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
            query = "systemName=" + HttpUtility.UrlEncode(systemName) + "&commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey + "&comment=" + HttpUtility.UrlEncode(note);

            if (edsmid > 0)
            {
                // For future use when EDSM adds the ability to link a comment to a system by EDSM ID
                query += "&systemId=" + edsmid;
            }

            MimeType = "application/x-www-form-urlencoded";
            var response = RequestPost(query, "api-logs-v1/set-comment", handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        public string SetLog(string systemName, DateTime dateVisitedutc)
        {
            if (!IsApiKeySet)
                return null;

            string query;
            query = "systemName=" + HttpUtility.UrlEncode(systemName) + "&commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey +
                 "&fromSoftware=" + HttpUtility.UrlEncode(fromSoftware) + "&fromSoftwareVersion=" + HttpUtility.UrlEncode(fromSoftwareVersion) +
                  "&dateVisited=" + HttpUtility.UrlEncode(dateVisitedutc.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            MimeType = "application/x-www-form-urlencoded";
            var response = RequestPost(query, "api-logs-v1/set-log", handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        public string SetLogWithPos(string systemName, DateTime dateVisitedutc, double x, double y, double z)
        {
            if (!IsApiKeySet)
                return null;

            string query;
            query = "systemName=" + HttpUtility.UrlEncode(systemName) + "&commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey +
                 "&fromSoftware=" + HttpUtility.UrlEncode(fromSoftware) + "&fromSoftwareVersion=" + HttpUtility.UrlEncode(fromSoftwareVersion) +
                 "&x=" + HttpUtility.UrlEncode(x.ToString(CultureInfo.InvariantCulture)) + "&y=" + HttpUtility.UrlEncode(y.ToString(CultureInfo.InvariantCulture)) + "&z=" + HttpUtility.UrlEncode(z.ToString(CultureInfo.InvariantCulture)) +
                  "&dateVisited=" + HttpUtility.UrlEncode(dateVisitedutc.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

            MimeType = "application/x-www-form-urlencoded";
            var response = RequestPost(query, "api-logs-v1/set-log", handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        public bool SendTravelLog(string name, DateTime timeutc, bool coord, double x, double y, double z, out string error, out int errno, out bool firstdiscover, out int edsmid)
        {
            firstdiscover = false;
            edsmid = 0;

            if (!IsApiKeySet)
            {
                errno = -1;
                error = "EDSM API Key not set";
                return false;
            }

            error = "";
            errno = 0;

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
                errno = -1;
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
                    firstdiscover = msg["systemCreated"].Bool(false);
                    edsmid = msg["systemId"].Int(0);

                    return true;
                }
                else
                {
                    error = "EDSM sync error submitting system (" + name + "):" + msgnum.ToString() + ":" + msgstr;
                    errno = msgnum;
                    System.Diagnostics.Trace.WriteLine("Error sync:" + msgnum.ToString() + " : " + name);
                }
            }

            return false;
        }

        public int GetLogs(DateTime? starttimeutc, DateTime? endtimeutc, out List<HistoryEntry> log, out DateTime logstarttime, out DateTime logendtime)
        {
            log = new List<HistoryEntry>();
            logstarttime = DateTime.MaxValue;
            logendtime = DateTime.MinValue;

            if (!IsApiKeySet)
                return 0;

            string query = "get-logs?showId=1&apiKey=" + apiKey + "&commanderName=" + HttpUtility.UrlEncode(commanderName);

            if (starttimeutc != null)
                query += "&startDateTime=" + HttpUtility.UrlEncode(starttimeutc.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

            if (endtimeutc != null)
                query += "&endDateTime=" + HttpUtility.UrlEncode(endtimeutc.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

            var response = RequestGet("api-logs-v1/" + query, handleException: true);

            if (response.Error)
                return 0;

            var json = response.Body;

            if (json == null)
                return 0;

            JObject msg = JObject.Parse(json);
            int msgnr = msg["msgnum"].Int(0);

            JArray logs = (JArray)msg["logs"];

            if (logs != null)
            {
                string startdatestr = msg["startDateTime"].Value<string>();
                string enddatestr = msg["endDateTime"].Value<string>();
                if (startdatestr == null || !DateTime.TryParseExact(startdatestr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out logstarttime))
                    logstarttime = DateTime.MaxValue;
                if (enddatestr == null || !DateTime.TryParseExact(enddatestr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out logendtime))
                    logendtime = DateTime.MinValue;

                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    foreach (JObject jo in logs)
                    {
                        string name = jo["system"].Value<string>();
                        string ts = jo["date"].Value<string>();
                        long id = jo["systemId"].Value<long>();
                        bool firstdiscover = jo["firstDiscover"].Value<bool>();
                        DateTime etutc = DateTime.ParseExact(ts, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal|DateTimeStyles.AssumeUniversal); // UTC time

                        ISystem sc = SystemClassDB.GetSystem(id, cn, SystemClassDB.SystemIDType.EdsmId);
                        if (sc == null)
                        {
                            if (DateTime.UtcNow.Subtract(etutc).TotalHours < 6) // Avoid running into the rate limit
                                sc = GetSystemsByName(name)?.FirstOrDefault(s => s.id_edsm == id);

                            if (sc == null)
                            {
                                sc = new SystemClass(name)
                                {
                                    id_edsm = id
                                };
                            }
                        }

                        HistoryEntry he = HistoryEntry.MakeVSEntry(sc, etutc, EliteConfigInstance.InstanceConfig.DefaultMapColour, "", "", firstdiscover: firstdiscover);       // FSD jump entry
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

        public List<Tuple<ISystem,double>> GetSphereSystems(String systemName, double radius)
        {
            string query = String.Format("api-v1/sphere-systems?systemName={0}&radius={1}&showCoordinates=1&showId=1", Uri.EscapeDataString(systemName), radius);

            var response = RequestGet(query, handleException: true);
            if (response.Error)
                return null;

            var json = response.Body;
            if (json == null)
                return null;

            JArray msg = JArray.Parse(json);

            List<Tuple<ISystem,double>> systems = new List<Tuple<ISystem,double>>();

            if (msg != null)
            {
                foreach (JObject sysname in msg)
                {
                    ISystem sys = new SystemClass();
                    sys.name = sysname["name"].Str("Unknown");
                    sys.id_edsm = sysname["id"].Long(0);
                    JObject co = (JObject)sysname["coords"];
                    if ( co != null )
                    {
                        sys.x = co["x"].Double();
                        sys.y = co["y"].Double();
                        sys.z = co["z"].Double();
                    }
                    systems.Add(new Tuple<ISystem, double>(sys, sysname["distance"].Double()));
                }
            }
            return systems;
        }


        public List<ISystem> GetSystemsByName(string systemName, bool uselike = false)
        {
            string query = String.Format("api-v1/systems?systemName={0}&showCoordinates=1&showId=1&showInformation=1&showPermit=1", Uri.EscapeDataString(systemName));

            var response = RequestGet(query, handleException: true);
            if (response.Error)
                return null;

            var json = response.Body;
            if (json == null)
                return null;

            JArray msg = JArray.Parse(json);

            List<ISystem> systems = new List<ISystem>();

            if (msg != null)
            {
                foreach (JObject sysname in msg)
                {
                    ISystem sys = new SystemClass();
                    sys.name = sysname["name"].Str("Unknown");
                    sys.id_edsm = sysname["id"].Long(0);
                    JObject co = (JObject)sysname["coords"];

                    if (co != null)
                    {
                        sys.x = co["x"].Double();
                        sys.y = co["y"].Double();
                        sys.z = co["z"].Double();
                    }

                    sys.needs_permit = sysname["requirePermit"].Bool(false) ? 1 : 0;

                    JObject info = sysname["information"] as JObject;

                    if (info != null)
                    {
                        sys.population = info["population"].Long(0);
                        sys.faction = info["faction"].StrNull();
                        EDAllegiance allegiance = EDAllegiance.None;
                        EDGovernment government = EDGovernment.None;
                        EDState state = EDState.None;
                        EDEconomy economy = EDEconomy.None;
                        EDSecurity security = EDSecurity.Unknown;
                        sys.allegiance = Enum.TryParse(info["allegiance"].Str(), out allegiance) ? allegiance : EDAllegiance.None;
                        sys.government = Enum.TryParse(info["government"].Str(), out government) ? government : EDGovernment.None;
                        sys.state = Enum.TryParse(info["factionState"].Str(), out state) ? state : EDState.None;
                        sys.primary_economy = Enum.TryParse(info["economy"].Str(), out economy) ? economy : EDEconomy.None;
                        sys.security = Enum.TryParse(info["security"].Str(), out security) ? security : EDSecurity.Unknown;
                    }

                    if (uselike ? sys.name.StartsWith(systemName, StringComparison.InvariantCultureIgnoreCase) : sys.name.Equals(systemName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        systems.Add(sys);
                    }
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
            var response = RequestGet("api-system-v1/" + query, handleException: true);
            if (response.Error)
                return null;

            var json = response.Body;
            if (json == null || json.ToString() == "[]")
                return null;

            JObject msg = JObject.Parse(json);
            return msg;
        }

        public JObject GetBodies(long edsmID)
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

        public  static List<JournalScan> GetBodiesList(long edsmid)
        {
            try
            {
                if (DictEDSMBodies!=null &&  DictEDSMBodies.ContainsKey(edsmid))  // Cache EDSM bidies during run of EDD.
                    return DictEDSMBodies[edsmid];

                List<JournalScan> bodies = new List<JournalScan>();

                EDSMClass edsm = new EDSMClass();

                JObject jo = edsm.GetBodies(edsmid);  // Colonia 

                if (jo != null)
                {
                    foreach (JObject edsmbody in jo["bodies"])
                    {
                        try
                        {
                            JObject jbody = EDSMClass.ConvertFromEDSMBodies(edsmbody);
                            JournalScan js = new JournalScan(jbody);
                            js.EdsmID = edsmid;

                            bodies.Add(js);
                        }
                        catch (Exception ex)
                        {
                            BaseUtils.HttpCom.WriteLog($"Exception Loop: {ex.Message}", "");
                            BaseUtils.HttpCom.WriteLog($"ETrace: {ex.StackTrace}", "");
                            Trace.WriteLine($"Exception Loop: {ex.Message}");
                            Trace.WriteLine($"ETrace: {ex.StackTrace}");
                        }
                    }
                    DictEDSMBodies[edsmid] = bodies;
                    return bodies;
                }

                DictEDSMBodies[edsmid] = null;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception: {ex.Message}");
                Trace.WriteLine($"ETrace: {ex.StackTrace}");
                return null;

            }
            return null;
        }


        private static JObject ConvertFromEDSMBodies(JObject jo)
        {
            JObject jout = new JObject
            {
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture),
                ["event"] = "Scan",
                ["EDDFromEDSMBodie"] = true,
                ["BodyName"] = jo["name"],
            };

            if (jo["orbitalInclination"] != null) jout["OrbitalInclination"] = jo["orbitalInclination"];
            if (jo["orbitalEccentricity"] != null) jout["Eccentricity"] = jo["orbitalEccentricity"];
            if (jo["argOfPeriapsis"] != null) jout["Periapsis"] = jo["argOfPeriapsis"];
            if (jo["semiMajorAxis"].Double() != 0) jout["SemiMajorAxis"] = jo["semiMajorAxis"].Double() * JournalScan.oneAU_m; // AU -> metres
            if (jo["orbitalPeriod"].Double() != 0) jout["OrbitalPeriod"] = jo["orbitalPeriod"].Double() * JournalScan.oneDay_s; // days -> seconds
            if (jo["rotationalPeriodTidallyLocked"] != null) jout["TidalLock"] = jo["rotationalPeriodTidallyLocked"];
            if (jo["axialTilt"] != null) jout["AxialTilt"] = jo["axialTilt"].Double() * Math.PI / 180.0; // degrees -> radians
            if (jo["rotationalPeriod"].Double() != 0) jout["RotationalPeriod"] = jo["rotationalPeriod"].Double() * JournalScan.oneDay_s; // days -> seconds
            if (jo["surfaceTemperature"] != null) jout["SurfaceTemperature"] = jo["surfaceTemperature"];

            if (!jo["type"].Empty())
            {
                if (jo["type"].Value<string>().Equals("Star"))
                {
                    jout["StarType"] = EDSMStar2JournalName(jo["subType"].StrNull());           // pass thru null to code, it will cope with it
                    jout["Age_MY"] = jo["age"];
                    jout["StellarMass"] = jo["solarMasses"];
                    jout["Radius"] = jo["solarRadius"].Double() * JournalScan.solarRadius_m; // solar-rad -> metres
                }
                else if (jo["type"].Value<string>().Equals("Planet"))
                {
                    jout["Landable"] = jo["isLandable"];
                    jout["MassEM"] = jo["earthMasses"];
                    jout["Volcanism"] = jo["volcanismType"];
                    jout["Atmosphere"] = jo["atmosphereType"];
                    jout["Radius"] = jo["radius"].Double() * 1000.0; // km -> metres
                    jout["PlanetClass"] = EDSMPlanet2JournalName(jo["subType"].Str());
                    if (jo["terraformingState"] != null) jout["TerraformState"] = jo["terraformingState"];
                    if (jo["surfacePressure"] != null) jout["SurfacePressure"] = jo["surfacePressure"].Double() * 101325; // atmospheres -> pascals
                    if (jout["TerraformState"].Str() == "Candidate for terraforming")
                        jout["TerraformState"] = "Terraformable";
                }
            }


            JArray rings = (jo["belts"] ?? jo["rings"]) as JArray;

            if (!rings.Empty())
            {
                JArray jring = new JArray();

                foreach (JObject ring in rings)
                {
                    jring.Add(new JObject
                    {
                        ["InnerRad"] = ring["innerRadius"].Double() * 1000,
                        ["OuterRad"] = ring["outerRadius"].Double() * 1000,
                        ["MassMT"] = ring["mass"],
                        ["RingClass"] = ring["type"],
                        ["Name"] = ring["name"]
                    });
                }

                jout["Rings"] = jring;
            }

            if (!jo["Materials"].Empty())  // Check if matieals has null
            {
                Dictionary<string, double?> mats;
                Dictionary<string, double> mats2;
                mats = jo["Materials"]?.ToObject<Dictionary<string, double?>>();
                mats2 = new Dictionary<string, double>();

                foreach (string key in mats.Keys)
                    if (mats[key] == null)
                        mats2[key.ToLower()] = 0.0;
                    else
                        mats2[key.ToLower()] = mats[key].Value;

                jout["Materials"] = JObject.FromObject(mats2);
            }

            return jout;
        }

        private static Dictionary<string, string> EDSM2PlanetNames = new Dictionary<string, string>()
        {
            // EDSM name    (lower case)            Journal name                  
            { "rocky ice world",                    "Rocky ice body" },
            { "high metal content world" ,          "High metal content body"},
            { "class i gas giant",                  "Sudarsky class I gas giant"},
            { "class ii gas giant",                 "Sudarsky class II gas giant"},
            { "class iii gas giant",                "Sudarsky class III gas giant"},
            { "class iv gas giant",                 "Sudarsky class IV gas giant"},
            { "class v gas giant",                  "Sudarsky class V gas giant"},
            { "earth-like world",                   "Earthlike body" },
        };

        private static Dictionary<string, string> EDSM2StarNames = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            // EDSM name (lower case)               Journal name
            { "a (blue-white super giant) star", "A_BlueWhiteSuperGiant" },
            { "f (white super giant) star", "F_WhiteSuperGiant" },
            { "k (yellow-orange giant) star", "K_OrangeGiant" },
            { "m (red giant) star", "M_RedGiant" },
            { "m (red super giant) star", "M_RedSuperGiant" },
            { "black hole", "H" },
            { "c star", "C" },
            { "cj star", "CJ" },
            { "cn star", "CN" },
            { "herbig ae/be star", "AeBe" },
            { "ms-type star", "MS" },
            { "neutron star", "N" },
            { "s-type star", "S" },
            { "t tauri star", "TTS" },
            { "wolf-rayet c star", "WC" },
            { "wolf-rayet n star", "WN" },
            { "wolf-rayet nc star", "WNC" },
            { "wolf-rayet o star", "WO" },
            { "wolf-rayet star", "W" },
        };

        private static string EDSMPlanet2JournalName(string inname)
        {
            return EDSM2PlanetNames.ContainsKey(inname.ToLower()) ? EDSM2PlanetNames[inname.ToLower()] : inname;
        }

        private static string EDSMStar2JournalName(string startype)
        {
            if (startype == null)
                startype = "Unknown";
            else if (EDSM2StarNames.ContainsKey(startype))
                startype = EDSM2StarNames[startype];
            else   // Remove extra text from EDSM   ex  "F (White) Star" -> "F"
            {
                int index = startype.IndexOf("(");
                if (index > 0)
                    startype = startype.Substring(0, index).Trim();
            }
            return startype;
        }

        public string SetRanks(int combat_rank, int combat_progress, int trade_rank, int trade_progress,
            int explore_rank, int explore_progress, int cqc_rank, int cqc_progress,
            int federation_rank, int federation_progress, int empire_rank, int empire_progress)
        {
            if (!IsApiKeySet)
                return null;

            string query;
            query = "commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey;
            query = query + "&Combat=" + combat_rank.ToString() + ";" + combat_progress.ToString();
            query = query + "&Trade=" + trade_rank.ToString() + ";" + trade_progress.ToString();
            query = query + "&Explore=" + explore_rank.ToString() + ";" + explore_progress.ToString();
            query = query + "&CQC=" + cqc_rank.ToString() + ";" + cqc_progress.ToString();
            query = query + "&Federation=" + federation_rank.ToString() + ";" + federation_progress.ToString();
            query = query + "&Empire=" + empire_rank.ToString() + ";" + empire_progress.ToString();


            MimeType = "application/x-www-form-urlencoded";
            var response = RequestPost(query, "api-commander-v1/set-ranks", handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        public string SetCredits(long credits, long loan)
        {
            if (!IsApiKeySet)
                return null;

            string query;
            query = "commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey;
            query = query + "&balance=" + credits.ToString();
            query = query + "&loan=" + loan.ToString();


            MimeType = "application/x-www-form-urlencoded";
            var response = RequestPost(query, "api-commander-v1/set-credits" + query, handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        public string SetInventoryMaterials(Dictionary<string, int> matcounts)
        {
            JObject jo = new JObject();
            foreach (KeyValuePair<string, int> kvp in matcounts)
            {
                jo[kvp.Key] = kvp.Value;
            }

            string query = "commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey;
            query += "&type=materials";
            query += "&values=" + HttpUtility.UrlEncode(jo.ToString());

            MimeType = "application/x-www-form-urlencoded";
            var response = RequestPost(query, "api-commander-v1/set-materials", handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        public string SetInventoryData(Dictionary<string, int> matcounts)
        {
            JObject jo = new JObject();
            foreach (KeyValuePair<string, int> kvp in matcounts)
            {
                jo[kvp.Key] = kvp.Value;
            }

            string query = "commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey;
            query += "&type=data";
            query += "&values=" + HttpUtility.UrlEncode(jo.ToString());

            MimeType = "application/x-www-form-urlencoded";
            var response = RequestPost(query, "api-commander-v1/set-materials", handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        public string SetInventoryCargo(Dictionary<string, int> matcounts)
        {
            JObject jo = new JObject();
            foreach (KeyValuePair<string, int> kvp in matcounts)
            {
                jo[kvp.Key] = kvp.Value;
            }

            string query = "commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey;
            query += "&type=cargo";
            query += "&values=" + HttpUtility.UrlEncode(jo.ToString());

            MimeType = "application/x-www-form-urlencoded";
            var response = RequestPost(query, "api-commander-v1/set-materials", handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        public string CommanderUpdateShip(int shipId, string type, EliteDangerousCore.ShipInformation shipinfo = null, int cargoqty = -1)
        {
            if (!IsApiKeySet)
                return null;

            string query;
            query = "commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey;
            query = query + "&shipId=" + shipId.ToString();
            query = query + "&type=" + Uri.EscapeDataString(type);

            if (shipinfo != null)
            {
                int cargocap = shipinfo.CargoCapacity();
                if (cargocap != 0)
                    query += "&cargoCapacity=" + cargocap.ToString();
                if (cargoqty >= 0)
                    query += "&cargoQty=" + cargoqty.ToString();
                double fuelcap = shipinfo.FuelCapacity;
                if (fuelcap != 0)
                    query += "&fuelMainCapacity=" + fuelcap.ToString();
                double fuellevel = shipinfo.FuelLevel;
                if (fuellevel != 0)
                    query += "&fuelMainLevel=" + fuellevel.ToString();
                string ident = shipinfo.ShipUserIdent;
                if (!string.IsNullOrWhiteSpace(ident))
                    query += "&shipIdent=" + Uri.EscapeDataString(ident);
                string name = shipinfo.ShipUserName;
                if (!string.IsNullOrWhiteSpace(name))
                    query += "&shipName=" + Uri.EscapeDataString(name);
                string paintjob = shipinfo.Modules.ContainsKey("Paint Job") ? shipinfo.Modules["Paint Job"].ItemFD : null;
                if (!string.IsNullOrWhiteSpace(paintjob))
                    query += "&paintJob=" + Uri.EscapeDataString(paintjob);
                string errstr;
                string coriolisurl = shipinfo.GetCoriolisUrl(out errstr);
                query += "&linkToCoriolis=" + Uri.EscapeDataString(coriolisurl);
            }

            MimeType = "application/x-www-form-urlencoded";
            var response = RequestPost(query, "api-commander-v1/update-ship", handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        public string CommanderSetCurrentShip(int shipId)
        {
            if (!IsApiKeySet)
                return null;

            string query;
            query = "commanderName=" + HttpUtility.UrlEncode(commanderName) + "&apiKey=" + apiKey;
            query = query + "&shipId=" + shipId.ToString();

            MimeType = "application/x-www-form-urlencoded";
            var response = RequestPost(query, "api-commander-v1/set-ship-id", handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        static private ShipInformation LastShipInfo = null;     // send with some caching
        static private long LastEDSMCredits = -1;
        static private long LastShipID = -1;
        static JournalProgress LastProgress = null;
        static JournalRank LastRank = null;
        static MaterialCommoditiesList LastMats = null;
        static Object LockShipInfo = new object();

        public void SendShipInfo(ShipInformation si, MaterialCommoditiesList matcommod, int cargo, ShipInformation sicurrent, long cash, long loan, 
                                    JournalProgress progress, JournalRank rank  // both may be null
                                )
        {
            lock (LockShipInfo) // lets not double send in different threads.
            {
                if (!si.Equals(LastShipInfo))   // if we are sending new ship info..
                {
                    System.Diagnostics.Trace.WriteLine("Update EDSM with ship info " + si.ID + " " + si.ShipType + " " + cargo);
                    CommanderUpdateShip(si.ID, si.ShipType.Alt("Unknown"), si, cargo);
                    LastShipInfo = si;
                }

                if (LastShipID != sicurrent.ID) // if we have a new current ship
                {
                    System.Diagnostics.Trace.WriteLine("Update EDSM with current ship " + sicurrent.ID);
                    CommanderSetCurrentShip(sicurrent.ID);
                    LastShipID = sicurrent.ID;
                }

                if (LastEDSMCredits != cash)    // if our cash has changed..
                {
                    System.Diagnostics.Trace.WriteLine("Update EDSM with credits " + cash);
                    SetCredits(cash, loan);
                    LastEDSMCredits = cash;
                }

                if ( progress != null && rank != null && (!Object.ReferenceEquals(progress,LastProgress) || !Object.ReferenceEquals(rank,LastRank)) )
                {
                    System.Diagnostics.Trace.WriteLine("Update EDSM with ranks");
                    SetRanks((int)rank.Combat, progress.Combat, (int)rank.Trade, progress.Trade, (int)rank.Explore, progress.Explore, (int)rank.CQC, progress.CQC, (int)rank.Federation, progress.Federation, (int)rank.Empire, progress.Empire);
                    LastProgress = progress;
                    LastRank = rank;
                }

                if (matcommod != null && matcommod != LastMats)
                {
                    System.Diagnostics.Trace.WriteLine("Update EDSM with materials and cargo");
                    List<MaterialCommodities> lmats = matcommod.Sort(false);
                    List<MaterialCommodities> lcargo = matcommod.Sort(true);
                    List<MaterialCommodities> ldata = lmats.Where(m => m.category == MaterialCommodities.MaterialEncodedCategory).ToList();
                    lmats = lmats.Where(m => m.category != MaterialCommodities.MaterialEncodedCategory).ToList();
                    SetInventoryMaterials(lmats.Where(m => m.count > 0).ToDictionary(m => m.fdname, m => m.count));
                    SetInventoryData(ldata.Where(m => m.count > 0).ToDictionary(m => m.fdname, m => m.count));
                    SetInventoryCargo(lcargo.Where(m => m.count > 0).ToDictionary(m => m.fdname, m => m.count));
                    LastMats = matcommod;
                }
            }
        }
    }
}
