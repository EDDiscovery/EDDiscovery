using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace EDDiscovery2.HTTP
{
    class DownloadFileHandler
    {
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

        private static T ProcessDownload<T>(string url, string filename, Action<bool, Stream, Func<bool>> processor, Func<HttpWebRequest, Func<Func<HttpWebResponse>, Func<bool>, bool>, T> doRequest)
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

            return doRequest(request, (getResponse, cancelRequested) =>
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
                            processor(true, httpStream, cancelRequested);
                            if (!cancelRequested())
                            {
                                File.Delete(etagFilename);
                                File.Move(tmpEtagFilename, etagFilename);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    if ((HttpWebResponse)ex.Response == null)
                        return false;

                    var code = ((HttpWebResponse)ex.Response).StatusCode;
                    if (code == HttpStatusCode.NotModified)
                    {
                        System.Diagnostics.Trace.WriteLine("DownloadFile: " + filename + " up to date (etag).");
                        HttpCom.WriteLog(filename, "up to date (etag).");
                        using (FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            processor(false, stream, cancelRequested);
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

        private static T DoDownloadFile<T>(string url, string filename, Action<bool, Stream, Func<bool>> processor, Func<string, string, Action<bool, Stream, Func<bool>>, T> doDownload)
        {
            var tmpFilename = filename + ".tmp";
            return doDownload(url, filename, (n, s, c) =>
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
                            processor(true, cacheStream, c);
                        }
                    }
                    if (!c())
                    {
                        File.Delete(filename);
                        File.Move(tmpFilename, filename);
                    }
                }
                else if (processor != null)
                {
                    processor(false, s, c);
                }
            });
        }

        public static Task<bool> DownloadFileAsync(string url, string filename, Action<bool, Stream, Func<bool>> processor, Func<bool> cancelRequested)
        {
            return ProcessDownload<Task<bool>>(url, filename, processor, (request, doProcess) =>
            {
                return request.GetResponseAsync().ContinueWith(ar =>
                {
                    bool success = false;
                    if (!cancelRequested())
                    {
                        success = doProcess(() => (HttpWebResponse)ar.Result, cancelRequested);
                    }
                    return success;
                });
            });
        }

        public static bool DownloadFile(string url, string filename, Action<bool, Stream, Func<bool>> processor)
        {
            return ProcessDownload(url, filename, processor, (request, doProcess) =>
            {
                return doProcess(() => (HttpWebResponse)request.GetResponse(), () => false);
            });
        }

        public static Task<bool> DownloadFileAsync(string url, string filename, Action<bool> callback, Func<bool> cancelRequested, Action<bool, Stream, Func<bool>> processor = null)
        {
            bool _newfile = false;
            return DoDownloadFile(url, filename, processor, (u, f, p) =>
            {
                return DownloadFileAsync(u, f, (n, s, c) =>
                {
                    _newfile = n;
                    p?.Invoke(n, s, c);
                    callback(_newfile);
                }, cancelRequested);
            });
        }

        static public bool DownloadFile(string url, string filename, out bool newfile, Action<bool, Stream> processor = null)
        {
            if ( !url.Contains("http"))
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Not valid url (Debug) {0} ", url));
                newfile = false;
                return false;
            }

            bool _newfile = false;
            bool success = DoDownloadFile(url, filename, (n, s, c) => processor(n, s), (u, f, p) =>
            {
                return DownloadFile(u, f, (n, s, c) =>
                {
                    _newfile = n;
                    p?.Invoke(n, s, c);
                });
            });

            newfile = _newfile && success;
            return success;
        }


    }
}
