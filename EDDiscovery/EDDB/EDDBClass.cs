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

        private class FileCachedReadStream : Stream
        {
            private Stream innerStream;
            private Stream fileStream;

            public override bool CanRead { get { return innerStream.CanRead; } }
            public override bool CanSeek { get { return false; } }
            public override bool CanTimeout { get { return innerStream.CanTimeout; } }
            public override bool CanWrite { get { return false; } }
            public override long Length { get { return innerStream.Length; } }
            public override long Position { get { return innerStream.Position; } set { throw new InvalidOperationException(); } }
            public override int ReadTimeout { get { return innerStream.ReadTimeout; } }
            public override int WriteTimeout { get { return innerStream.WriteTimeout; } }
            public override long Seek(long offset, SeekOrigin origin) { throw new InvalidOperationException(); }
            public override void Flush() { }
            public override void SetLength(long value) { throw new InvalidOperationException(); }
            public override void Write(byte[] buffer, int offset, int count) { throw new InvalidOperationException(); }
            public override int Read(byte[] buffer, int offset, int count)
            {
                int length = innerStream.Read(buffer, offset, count);
                if (length > 0)
                {
                    fileStream.Write(buffer, offset, length);
                }
                return length;
            }

            public FileCachedReadStream(Stream instream, Stream filestream)
            {
                innerStream = instream;
                fileStream = filestream;
            }
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
                        File.SetLastWriteTimeUtc(tmpEtagFilename, response.LastModified.ToUniversalTime());
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

        private static T DoDownloadFile<T>(string url, string filename, Action<bool, Stream> processor, Func<string, string, Action<bool, Stream>, T> doDownload)
        {
            var tmpFilename = filename + ".tmp";
            return doDownload(url, filename, (n, s) =>
            {
                if (n)
                {
                    using (var destFileStream = File.Open(tmpFilename, FileMode.Create, FileAccess.Write))
                    {
                        if (processor == null)
                        {
                            s.CopyTo(destFileStream);
                        }
                        else
                        {
                            var cacheStream = new FileCachedReadStream(s, destFileStream);
                            processor(true, cacheStream);
                        }
                    }
                    File.Delete(filename);
                    File.Move(tmpFilename, filename);
                }
                else if (processor != null)
                {
                    processor(false, s);
                }
            });
        }

        public static Task<bool> BeginDownloadFile(string url, string filename, Action<bool, Stream> processor, Action<Action> registerCancelCallback)
        {
            bool completed = false;
            return ProcessDownload(url, filename, processor, (request, doProcess) =>
            {
                registerCancelCallback(() => { if (!completed) request.Abort(); });
                return Task<bool>.Factory.FromAsync(request.BeginGetResponse, (ar) =>
                {
                    bool success = doProcess(() => (HttpWebResponse)request.EndGetResponse(ar));
                    completed = true;
                    return success;
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

        public static Task<bool> BeginDownloadFile(string url, string filename, Action<bool> callback, Action<Action> registerCancelCallback, Action<bool, Stream> processor = null)
        {
            bool _newfile = false;
            return DoDownloadFile(url, filename, processor, (u, f, p) =>
            {
                return BeginDownloadFile(u, f, (n, s) =>
                {
                    _newfile = n;
                    p(n, s);
                    callback(_newfile);
                }, registerCancelCallback);
            });
        }

        static public bool DownloadFile(string url, string filename, out bool newfile, Action<bool, Stream> processor = null)
        {
            bool _newfile = false;
            bool success = DoDownloadFile(url, filename, processor, (u, f, p) =>
            {
                return DownloadFile(u, f, (n, s) =>
                {
                    _newfile = n;
                    p(n, s);
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
