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
        // use if you need an API/name pair to get info from EDSM.  Not all queries need it
        public bool ValidCredentials { get { return !string.IsNullOrEmpty(commanderName) && !string.IsNullOrEmpty(apiKey); } }

        private string commanderName;
        private string apiKey;

        private readonly string fromSoftwareVersion;
        private readonly string fromSoftware;
        static private Dictionary<long, List<JournalScan>> DictEDSMBodies = new Dictionary<long, List<JournalScan>>();
        static private Dictionary<long, List<JournalScan>> DictEDSMBodiesByID64 = new Dictionary<long, List<JournalScan>>();

        public EDSMClass()
        {
            fromSoftware = "EDDiscovery";
            var assemblyFullName = Assembly.GetEntryAssembly().FullName;
            fromSoftwareVersion = assemblyFullName.Split(',')[1].Split('=')[1];

            base.httpserveraddress = ServerAddress;

            apiKey = EDCommander.Current.EDSMAPIKey;
            commanderName = string.IsNullOrEmpty(EDCommander.Current.EdsmName) ? EDCommander.Current.Name : EDCommander.Current.EdsmName;
        }

        public EDSMClass(EDCommander cmdr) : this()
        {
            if (cmdr != null)
            {
                apiKey = cmdr.EDSMAPIKey;
                commanderName = string.IsNullOrEmpty(cmdr.EdsmName) ? cmdr.Name : cmdr.EdsmName;
            }
        }


        static string edsm_server_address = "https://www.edsm.net/";
        public static string ServerAddress { get { return edsm_server_address; } set { edsm_server_address = value; } }
        public static bool IsServerAddressValid { get { return edsm_server_address.Length > 0; } }

        #region For Trilateration

        public string SubmitDistances(string from, Dictionary<string, double> distances)
        {
            string query = "{\"ver\":2," + " \"commander\":\"" + commanderName + "\", \"fromSoftware\":\"" + fromSoftware + "\",  \"fromSoftwareVersion\":\"" + fromSoftwareVersion + "\", \"p0\": { \"name\": \"" + from + "\" },   \"refs\": [";

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

            return json.ToString().Contains("\"name\":");
        }

        public List<string> CheckForNewCoordinates(List<string> sysNames)
        {
            List<string> nowKnown = new List<string>();
            string query = "api-v1/systems?onlyKnownCoordinates=1&";
            bool first = true;
            foreach (string s in sysNames)
            {
                if (first) first = false;
                else query = query + "&";
                query = query + $"systemName[]={HttpUtility.UrlEncode(s)}";
            }

            var response = RequestGet(query, handleException: true);
            if (response.Error)
                return nowKnown;

            var json = response.Body;
            if (json == null)
                return nowKnown;

            JArray msg = JArray.Parse(json);

            if (msg != null)
            {
                foreach (JObject sysname in msg)
                {
                    nowKnown.Add(sysname["name"].ToString());
                }
            }

            return nowKnown;
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

        #endregion

        #region For System DB update

        public BaseUtils.ResponseData RequestSystemsData(DateTime startdate, DateTime enddate, int timeout = 5000)      // protect yourself against JSON errors!
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
            return RequestGet(query, handleException: true, timeout: timeout);
        }

        public string RequestSystems(DateTime startdate, DateTime enddate, int timeout = 5000)
        {
            var response = RequestSystemsData(startdate, enddate, timeout);
            if (response.Error)
                return null;

            return response.Body;
        }

        public string GetHiddenSystems()   // protect yourself against JSON errors!
        {
            try
            {
                string edsmhiddensystems = Path.Combine(EliteConfigInstance.InstanceOptions.AppDataDirectory, "edsmhiddensystems.json");

                if (BaseUtils.DownloadFile.HTTPDownloadFile(base.httpserveraddress + "api-v1/hidden-systems?showId=1", edsmhiddensystems, false, out bool newfile))
                {
                    string json = BaseUtils.FileHelpers.TryReadAllTextFromFile(edsmhiddensystems);
                    return json;
                }
                else
                    return null;
            }
            
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception: {ex.Message}");
                Trace.WriteLine($"ETrace: {ex.StackTrace}");
                return null;
            }
        
        }

        #endregion

        #region Comment sync

        private string GetComments(DateTime starttime)
        {
            if (!ValidCredentials)
                return null;

            string query = "get-comments?startdatetime=" + HttpUtility.UrlEncode(starttime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)) + "&apiKey=" + apiKey + "&commanderName=" + HttpUtility.UrlEncode(commanderName) + "&showId=1";
            //string query = "get-comments?apiKey=" + apiKey + "&commanderName=" + HttpUtility.UrlEncode(commanderName);
            var response = RequestGet("api-logs-v1/" + query, handleException: true);

            if (response.Error)
                return null;

            return response.Body;
        }

        public void GetComments(Action<string> logout = null)           // Protected against bad JSON
        {
            var json = GetComments(new DateTime(2011, 1, 1));

            if (json != null)
            {
                try
                {
                    JObject msg = JObject.Parse(json);                  // protect against bad json - seen in the wild
                    int msgnr = msg["msgnum"].Value<int>();

                    JArray comments = (JArray)msg["comments"];
                    if (comments != null)
                    {
                        int commentsadded = 0;

                        foreach (JObject jo in comments)
                        {
                            string name = jo["system"].Value<string>();
                            string note = jo["comment"].Value<string>();
                            string utctime = jo["lastUpdate"].Value<string>();
                            int edsmid = 0;

                            if (!Int32.TryParse(jo["systemId"].Str("0"), out edsmid))
                                edsmid = 0;

                            DateTime localtime = DateTime.ParseExact(utctime, "yyyy-MM-dd HH:mm:ss",
                                        CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToLocalTime();

                            SystemNoteClass curnote = SystemNoteClass.GetNoteOnSystem(name, edsmid);

                            if (curnote != null)                // curnote uses local time to store
                            {
                                if (localtime.Ticks > curnote.Time.Ticks)   // if newer, add on (verified with EDSM 29/9/2016)
                                {
                                    curnote.UpdateNote(curnote.Note + ". EDSM: " + note, true, localtime, edsmid, true);
                                    commentsadded++;
                                }
                            }
                            else
                            {
                                SystemNoteClass.MakeSystemNote(note, localtime, name, 0, edsmid, true);   // new one!  its an FSD one as well
                                commentsadded++;
                            }
                        }

                        logout?.Invoke(string.Format("EDSM Comments downloaded/updated {0}", commentsadded));
                    }
                }
                catch ( Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Failed due to " + e.ToString());
                }
            }
        }

        private string SetComment(string systemName, string note, long edsmid = 0)
        {
            if (!ValidCredentials)
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

        public static void SendComments(string star, string note, long edsmid = 0, EDCommander cmdr = null) // (verified with EDSM 29/9/2016)
        {
            System.Diagnostics.Debug.WriteLine("Send note to EDSM " + star + " " + edsmid + " " + note);
            EDSMClass edsm = new EDSMClass(cmdr);

            if (!edsm.ValidCredentials)
                return;

            System.Threading.Tasks.Task taskEDSM = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                edsm.SetComment(star, note, edsmid);
            });
        }

        #endregion

        #region Log Sync for log fetcher

        // Protected against bad JSON

        public int GetLogs(DateTime? starttimeutc, DateTime? endtimeutc, out List<JournalFSDJump> log, out DateTime logstarttime, out DateTime logendtime, out BaseUtils.ResponseData response)
        {
            log = new List<JournalFSDJump>();
            logstarttime = DateTime.MaxValue;
            logendtime = DateTime.MinValue;
            response = new BaseUtils.ResponseData { Error = true, StatusCode = HttpStatusCode.Unauthorized };

            if (!ValidCredentials)
                return 0;

            string query = "get-logs?showId=1&apiKey=" + apiKey + "&commanderName=" + HttpUtility.UrlEncode(commanderName);

            if (starttimeutc != null)
                query += "&startDateTime=" + HttpUtility.UrlEncode(starttimeutc.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

            if (endtimeutc != null)
                query += "&endDateTime=" + HttpUtility.UrlEncode(endtimeutc.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

            response = RequestGet("api-logs-v1/" + query, handleException: true);

            if (response.Error)
            {
                if ((int)response.StatusCode == 429)
                    return 429;
                else
                    return 0;
            }

            var json = response.Body;

            if (json == null)
                return 0;

            try
            {

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
                            DateTime etutc = DateTime.ParseExact(ts, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal); // UTC time

                            ISystem sc = SystemClassDB.GetSystem(id, cn, SystemClassDB.SystemIDType.EdsmId, name: name);
                            if (sc == null)
                            {
                                if (DateTime.UtcNow.Subtract(etutc).TotalHours < 6) // Avoid running into the rate limit
                                    sc = GetSystemsByName(name)?.FirstOrDefault(s => s.EDSMID == id);

                                if (sc == null)
                                {
                                    sc = new SystemClass(name)
                                    {
                                        EDSMID = id
                                    };
                                }
                            }

                            JournalFSDJump fsd = new JournalFSDJump(etutc, sc, EliteConfigInstance.InstanceConfig.DefaultMapColour, firstdiscover, true);
                            log.Add(fsd);
                        }
                    }
                }

                return msgnr;
            }
            catch ( Exception e )
            {
                System.Diagnostics.Debug.WriteLine("Failed due to " + e.ToString());
                return 499;     // BAD JSON
            }
        }

        private List<ISystem> GetSystemsByName(string systemName, bool uselike = false)     // Protect yourself against bad JSON
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
                    sys.Name = sysname["name"].Str("Unknown");
                    sys.EDSMID = sysname["id"].Long(0);
                    JObject co = (JObject)sysname["coords"];

                    if (co != null)
                    {
                        sys.X = co["x"].Double();
                        sys.Y = co["y"].Double();
                        sys.Z = co["z"].Double();
                    }

                    sys.NeedsPermit = sysname["requirePermit"].Bool(false) ? 1 : 0;

                    JObject info = sysname["information"] as JObject;

                    if (info != null)
                    {
                        sys.Population = info["population"].Long(0);
                        sys.Faction = info["faction"].StrNull();
                        EDAllegiance allegiance = EDAllegiance.None;
                        EDGovernment government = EDGovernment.None;
                        EDState state = EDState.None;
                        EDEconomy economy = EDEconomy.None;
                        EDSecurity security = EDSecurity.Unknown;
                        sys.Allegiance = Enum.TryParse(info["allegiance"].Str(), out allegiance) ? allegiance : EDAllegiance.None;
                        sys.Government = Enum.TryParse(info["government"].Str(), out government) ? government : EDGovernment.None;
                        sys.State = Enum.TryParse(info["factionState"].Str(), out state) ? state : EDState.None;
                        sys.PrimaryEconomy = Enum.TryParse(info["economy"].Str(), out economy) ? economy : EDEconomy.None;
                        sys.Security = Enum.TryParse(info["security"].Str(), out security) ? security : EDSecurity.Unknown;
                    }

                    if (uselike ? sys.Name.StartsWith(systemName, StringComparison.InvariantCultureIgnoreCase) : sys.Name.Equals(systemName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        systems.Add(sys);
                    }
                }
            }

            return systems;
        }

        #endregion

        #region System Information

        // protected against bad JSON

        public List<Tuple<ISystem,double>> GetSphereSystems(String systemName, double maxradius, double minradius)      // may return null
        {
            string query = String.Format("api-v1/sphere-systems?systemName={0}&radius={1}&minRadius={2}&showCoordinates=1&showId=1", Uri.EscapeDataString(systemName), maxradius , minradius);

            var response = RequestGet(query, handleException: true);
            if (response.Error)
                return null;

            var json = response.Body;
            if (json != null)
            {
                try
                {
                    List<Tuple<ISystem, double>> systems = new List<Tuple<ISystem, double>>();

                    JArray msg = JArray.Parse(json);        // allow for crap from EDSM or empty list

                    if (msg != null)
                    {
                        foreach (JObject sysname in msg)
                        {
                            ISystem sys = new SystemClass();
                            sys.Name = sysname["name"].Str("Unknown");
                            sys.EDSMID = sysname["id"].Long(0);
                            JObject co = (JObject)sysname["coords"];
                            if (co != null)
                            {
                                sys.X = co["x"].Double();
                                sys.Y = co["y"].Double();
                                sys.Z = co["z"].Double();
                            }
                            systems.Add(new Tuple<ISystem, double>(sys, sysname["distance"].Double()));
                        }

                        return systems;
                    }
                }
                catch( Exception e)      // json may be garbage
                {
                    System.Diagnostics.Debug.WriteLine("Failed due to " + e.ToString());
                }
            }

            return null;
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

            if (id_edsm != null && id_edsm > 0)
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
                if (json == null || !json.ToString().Contains("\"name\":"))
                    return "";

                JObject msg = JObject.Parse(json);
                sysID = msg["id"].Value<string>();
            }

            string url = base.httpserveraddress + "system/id/" + sysID + "/name/" + encodedSys;
            return url;
        }

        // https://www.edsm.net/api-system-v1/bodies?systemName=Colonia
        // https://www.edsm.net/api-system-v1/bodies?systemId=27

        #endregion

        #region Body info

        private JObject GetBodies(string sysName)       // protect yourself from bad JSON
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

        private JObject GetBodiesByID64(long id64)       // protect yourself from bad JSON
        {
            string query = "bodies?systemId64=" + id64.ToString();
            var response = RequestGet("api-system-v1/" + query, handleException: true);
            if (response.Error)
                return null;

            var json = response.Body;
            if (json == null || json.ToString() == "[]")
                return null;

            JObject msg = JObject.Parse(json);
            return msg;
        }

        private JObject GetBodies(long edsmID)          // protect yourself from bad JSON
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

        // Example ASYNC call

        public void GetBodiesAsync(long edsmID, Action<JObject> ret)        // ret is called in thread, must deal with that
        {
            string query = "bodies?systemId=" + edsmID.ToString();
            RequestGetAsync("api-system-v1/" + query,
                (response, tag) =>
                {
                    JObject msg = null;
                    if (!response.Error)
                    {
                        var json = response.Body;
                        if (json != null && json.ToString() != "[]")
                        {
                            msg = JObject.Parse(json);
                        }
                    }

                    (tag as Action<JObject>).Invoke(msg);
                }
                , ret, handleException: true);
        }

        public static List<JournalScan> GetBodiesList(long edsmid, bool edsmweblookup = true, long? id64 = null, string sysname = null) // get this edsmid,  optionally lookup web protected against bad json
        {
            try
            {
                if (DictEDSMBodies!=null && edsmid > 0 && DictEDSMBodies.ContainsKey(edsmid))  // Cache EDSM bidies during run of EDD.
                {
                   // System.Diagnostics.Debug.WriteLine(".. found EDSM Lookup bodies from cache " + edsmid);
                    return DictEDSMBodies[edsmid];
                }
                else if (DictEDSMBodiesByID64 != null && id64 != null && id64 > 0 && DictEDSMBodiesByID64.ContainsKey(id64.Value))
                {
                    return DictEDSMBodiesByID64[id64.Value];
                }

                if (!edsmweblookup)      // must be set for a web lookup
                    return null;

                List<JournalScan> bodies = new List<JournalScan>();

                EDSMClass edsm = new EDSMClass();

                //System.Diagnostics.Debug.WriteLine("EDSM Web Lookup bodies " + edsmid);

                JObject jo = null;

                if (edsmid > 0)
                    jo = edsm.GetBodies(edsmid);  // Colonia 
                else if (id64 != null && id64 > 0)
                    jo = edsm.GetBodiesByID64(id64.Value);
                else if (sysname != null)
                    jo = edsm.GetBodies(sysname);

                if (jo != null && jo["bodies"] != null )
                {
                    foreach (JObject edsmbody in jo["bodies"])
                    {
                        try
                        {
                            JObject jbody = EDSMClass.ConvertFromEDSMBodies(edsmbody);
                            JournalScan js = new JournalScan(jbody, edsmid);

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

                    if (edsmid > 0)
                    {
                        DictEDSMBodies[edsmid] = bodies;
                    }

                    if (id64 != null && id64 > 0)
                    {
                        DictEDSMBodiesByID64[id64.Value] = bodies;
                    }

                    return bodies;
                }

                if (edsmid > 0)
                {
                    DictEDSMBodies[edsmid] = null;
                }

                if (id64 != null && id64 > 0)
                {
                    DictEDSMBodiesByID64[id64.Value] = null;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception: {ex.Message}");
                Trace.WriteLine($"ETrace: {ex.StackTrace}");
                return null;

            }
            return null;
        }

        private static JObject ConvertFromEDSMBodies(JObject jo)        // protect yourself against bad JSON
        {
            JObject jout = new JObject
            {
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture),
                ["event"] = "Scan",
                ["EDDFromEDSMBodie"] = true,
                ["BodyName"] = jo["name"],
            };

            if (jo["discovery"] != null && jo["discovery"].HasValues)       // much more defense around this.. EDSM gives discovery=null back
            {
                jout["discovery"] = jo["discovery"];
            }

            if (jo["orbitalInclination"] != null) jout["OrbitalInclination"] = jo["orbitalInclination"];
            if (jo["orbitalEccentricity"] != null) jout["Eccentricity"] = jo["orbitalEccentricity"];
            if (jo["argOfPeriapsis"] != null) jout["Periapsis"] = jo["argOfPeriapsis"];
            if (jo["semiMajorAxis"].Double() != 0) jout["SemiMajorAxis"] = jo["semiMajorAxis"].Double() * JournalScan.oneAU_m; // AU -> metres
            if (jo["orbitalPeriod"].Double() != 0) jout["OrbitalPeriod"] = jo["orbitalPeriod"].Double() * JournalScan.oneDay_s; // days -> seconds
            if (jo["rotationalPeriodTidallyLocked"] != null) jout["TidalLock"] = jo["rotationalPeriodTidallyLocked"];
            if (jo["axialTilt"] != null) jout["AxialTilt"] = jo["axialTilt"].Double() * Math.PI / 180.0; // degrees -> radians
            if (jo["rotationalPeriod"].Double() != 0) jout["RotationalPeriod"] = jo["rotationalPeriod"].Double() * JournalScan.oneDay_s; // days -> seconds
            if (jo["surfaceTemperature"] != null) jout["SurfaceTemperature"] = jo["surfaceTemperature"];
            if (jo["distanceToArrival"] != null) jout["DistanceFromArrivalLS"] = jo["distanceToArrival"];
            if (jo["parents"] != null) jout["Parents"] = jo["parents"];
            if (jo["id64"] != null) jout["BodyID"] = jo["id64"].Long() >> 55;

            if (!jo["type"].Empty())
            {
                if (jo["type"].Value<string>().Equals("Star"))
                {
                    jout["StarType"] = EDSMStar2JournalName(jo["subType"].StrNull());           // pass thru null to code, it will cope with it
                    jout["Age_MY"] = jo["age"];
                    jout["StellarMass"] = jo["solarMasses"];
                    jout["Radius"] = jo["solarRadius"].Double() * JournalScan.oneSolRadius_m; // solar-rad -> metres
                }
                else if (jo["type"].Value<string>().Equals("Planet"))
                {
                    jout["Landable"] = jo["isLandable"];
                    jout["MassEM"] = jo["earthMasses"];
                    jout["Volcanism"] = jo["volcanismType"];
                    string atmos = jo["atmosphereType"].StrNull();
                    if ( atmos != null && atmos.IndexOf("atmosphere",StringComparison.InvariantCultureIgnoreCase)==-1)
                        atmos += " atmosphere";
                    jout["Atmosphere"] = atmos;
                    jout["Radius"] = jo["radius"].Double() * 1000.0; // km -> metres
                    jout["PlanetClass"] = EDSMPlanet2JournalName(jo["subType"].Str());
                    if (jo["terraformingState"] != null) jout["TerraformState"] = jo["terraformingState"];
                    if (jo["surfacePressure"] != null) jout["SurfacePressure"] = jo["surfacePressure"].Double() * JournalScan.oneAtmosphere_Pa; // atmospheres -> pascals
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

            if (!jo["materials"].Empty())  // Check if materials has null
            {
                Dictionary<string, double?> mats;
                Dictionary<string, double> mats2;
                mats = jo["materials"]?.ToObjectProtected<Dictionary<string, double?>>();
                mats2 = new Dictionary<string, double>();

                foreach (string key in mats.Keys)
                {
                    if (mats[key] == null)
                        mats2[key.ToLowerInvariant()] = 0.0;
                    else
                        mats2[key.ToLowerInvariant()] = mats[key].Value;
                }

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
            return EDSM2PlanetNames.ContainsKey(inname.ToLowerInvariant()) ? EDSM2PlanetNames[inname.ToLowerInvariant()] : inname;
        }

        private static string EDSMStar2JournalName(string startype)
        {
            if (startype == null)
                startype = "Unknown";
            else if (EDSM2StarNames.ContainsKey(startype))
                startype = EDSM2StarNames[startype];
            else if (startype.StartsWith("White Dwarf (", StringComparison.InvariantCultureIgnoreCase))
            {
                int start = startype.IndexOf("(") + 1;
                int len = startype.IndexOf(")") - start;
                if (len > 0)
                    startype = startype.Substring(start, len);
            }
            else   // Remove extra text from EDSM   ex  "F (White) Star" -> "F"
            {
                int index = startype.IndexOf("(");
                if (index > 0)
                    startype = startype.Substring(0, index).Trim();
            }
            return startype;
        }

        #endregion

        #region Journal Events

        public List<string> GetJournalEventsToDiscard()     // protect yourself against bad JSON
        {
            string action = "api-journal-v1/discard";
            var response = RequestGet(action);
            return JArray.Parse(response.Body).Select(v => v.Str()).ToList();
        }

        public List<JObject> SendJournalEvents(List<JObject> entries, out string errmsg)    // protected against bad JSON
        {
            JArray message = new JArray(entries);

            string postdata = "commanderName=" + Uri.EscapeDataString(commanderName) +
                              "&apiKey=" + Uri.EscapeDataString(apiKey) +
                              "&fromSoftware=" + Uri.EscapeDataString(fromSoftware) +
                              "&fromSoftwareVersion=" + Uri.EscapeDataString(fromSoftwareVersion) +
                              "&message=" + EscapeLongDataString(message.ToString(Newtonsoft.Json.Formatting.None));

          //  BaseUtils.HttpCom.WriteLog(message.ToString(Newtonsoft.Json.Formatting.Indented), "");

            MimeType = "application/x-www-form-urlencoded";
            var response = RequestPost(postdata, "api-journal-v1", handleException: true);

            if (response.Error)
            {
                errmsg = response.StatusCode.ToString();
                return null;
            }

            try
            {

                JObject resp = JObject.Parse(response.Body);
                errmsg = resp["msg"]?.ToString();

                int msgnr = resp["msgnum"].Int();

                if (msgnr >= 200 || msgnr < 100)
                {
                    return null;
                }

                return resp["events"].Select(e => (JObject)e).ToList();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed due to " + e.ToString());
                errmsg = e.ToString();
                return null;
            }
        }

        #endregion
    }
}
