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
            return DownloadFile("http://robert.astronet.se/Elite/eddb/v4/systems_populated.json", systemFileName);
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

        static public bool DownloadFile(string url, string filename)
        {
            bool newfile = false;
            return DownloadFile(url, filename, out newfile);
        }

        private static T ProcessDownload<T>(string url, string filename, Action<bool, Stream> processor, Func<HttpWebRequest, Func<Func<HttpWebResponse>, bool>, T> doRequest)
        {
            var etagFilename = filename == null ? null : filename + ".etag";
            var tmpEtagFilename = filename == null ? null : etagFilename + ".tmp";

            HttpCom.WriteLog("DownloadFile", url);
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.UserAgent = "EDDiscovery v" + Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (filename != null && File.Exists(etagFilename))
            {
                var etag = File.ReadAllText(etagFilename);
                if (etag != "")
                {
                    request.Headers[HttpRequestHeader.IfNoneMatch] = etag;
                }
                else
                {
                    request.IfModifiedSince = File.GetLastWriteTimeUtc(etagFilename);
                }
            }

            return doRequest(request, (getResponse) =>
            {
                try
                {
                    using (var response = getResponse())
                    {
                        HttpCom.WriteLog("Response", response.StatusCode.ToString());

                        File.WriteAllText(tmpEtagFilename, response.Headers[HttpResponseHeader.ETag]);
                        using (var httpStream = response.GetResponseStream())
                        {
                            processor(true, httpStream);
                            File.Delete(etagFilename);
                            File.Move(tmpEtagFilename, etagFilename);
                            return true;
                        }
                    }
                }
                catch (WebException ex)
                {
                    var code = ((HttpWebResponse)ex.Response).StatusCode;
                    if (code == HttpStatusCode.NotModified)
                    {
                        System.Diagnostics.Trace.WriteLine("EDDB: " + filename + " up to date (etag).");
                        HttpCom.WriteLog(filename, "up to date (etag).");
                        using (FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            processor(false, stream);
                        }
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
            });
        }

        private static T DoDownloadFile<T>(string url, string filename, Func<string, string, Action<bool, Stream>, T> doDownload)
        {
            var tmpFilename = filename + ".tmp";
            return doDownload(url, filename, (n, s) =>
            {
                if (n)
                {
                    using (var destFileStream = File.Open(tmpFilename, FileMode.Create, FileAccess.Write))
                    {
                        s.CopyTo(destFileStream);
                    }
                    File.Delete(filename);
                    File.Move(tmpFilename, filename);
                }
            });
        }

        public static IAsyncResult BeginDownloadFile(string url, string filename, Action<bool, Stream> processor, Action<bool> callback)
        {
            return ProcessDownload(url, filename, processor, (request, doProcess) =>
            {
                return request.BeginGetResponse((ar) =>
                {
                    callback(doProcess(() => (HttpWebResponse)request.EndGetResponse(ar)));
                }, null);
            });
        }

        public static bool DownloadFile(string url, string filename, Action<bool, Stream> processor)
        {
            return ProcessDownload(url, filename, processor, (request, doProcess) =>
            {
                return doProcess(() => (HttpWebResponse)request.GetResponse());
            });
        }

        public static IAsyncResult BeginDownloadFile(string url, string filename, Action<bool, bool> callback)
        {
            bool _newfile = false;
            return DoDownloadFile(url, filename, (u, f, processor) =>
            {
                return BeginDownloadFile(u, f, (n, s) =>
                {
                    _newfile = n;
                    processor(n, s);
                }, (s) =>
                {
                    callback(s, _newfile);
                });
            });
        }

        static public bool DownloadFile(string url, string filename, out bool newfile)
        {
            bool _newfile = false;
            bool success = DoDownloadFile(url, filename, (u, f, processor) =>
            {
                return DownloadFile(u, f, (n, s) =>
                {
                    _newfile = n;
                    processor(n, s);
                });
            });

            newfile = _newfile && success;
            return success;
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
