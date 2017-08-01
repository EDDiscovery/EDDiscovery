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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace BaseUtils
{
    public class DownloadFileHandler
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

        private static T ProcessDownload<T>(string url, string filename, Action<bool, Stream> processor, Func<HttpWebRequest, Func<Func<HttpWebResponse>, bool>, T> doRequest, Func<bool> cancelRequested)
        {
            var etagFilename = filename == null ? null : filename + ".etag";
            var tmpEtagFilename = filename == null ? null : etagFilename + ".tmp";

            BaseUtils.HttpCom.WriteLog("DownloadFile", url);
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.UserAgent = BrowserInfo.UserAgent;
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
                        BaseUtils.HttpCom.WriteLog("Response", response.StatusCode.ToString());

                        if (cancelRequested?.Invoke() == true)
                            return false;

                        File.WriteAllText(tmpEtagFilename, response.Headers[HttpResponseHeader.ETag]);
                        File.SetLastWriteTimeUtc(tmpEtagFilename, response.LastModified.ToUniversalTime());
                        using (var httpStream = response.GetResponseStream())
                        {
                            processor(true, httpStream);
                        if (cancelRequested?.Invoke() == true)
                            return false;

                            File.Delete(etagFilename);
                            File.Move(tmpEtagFilename, etagFilename);
                            return true;
                        }
                    }
                }
                catch (WebException ex)
                {
                    if ((HttpWebResponse)ex.Response == null)
                        return false;

                    if (cancelRequested?.Invoke() == true)
                        return false;

                    var code = ((HttpWebResponse)ex.Response).StatusCode;
                    if (code == HttpStatusCode.NotModified)
                    {
                        System.Diagnostics.Trace.WriteLine("DownloadFile: " + filename + " up to date (etag).");
                        BaseUtils.HttpCom.WriteLog(filename, "up to date (etag).");
                        using (FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            processor(false, stream);
                        }
                        return true;
                    }
                    System.Diagnostics.Trace.WriteLine("DownloadFile Exception:" + ex.Message);
                    System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                    BaseUtils.HttpCom.WriteLog("Exception", ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    if (cancelRequested?.Invoke() == true)
                        return false;

                    System.Diagnostics.Trace.WriteLine("DownloadFile Exception:" + ex.Message);
                    System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                    BaseUtils.HttpCom.WriteLog("DownloadFile Exception", ex.Message);
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

        public static Task<bool> BeginDownloadFile(string url, string filename, Action<bool, Stream> processor, Func<bool> cancelRequested)
        {
            return ProcessDownload(url, filename, processor, (request, doProcess) =>
            {
                return Task<bool>.Factory.FromAsync(request.BeginGetResponse, (ar) =>
                {
                    bool success = doProcess(() => (HttpWebResponse)request.EndGetResponse(ar));
                    return success;
                }, null);
            }, cancelRequested);
        }


        public static bool DownloadFile(string url, string filename, Action<bool, Stream> processor, Func<bool> cancelRequested)
        {
            return ProcessDownload(url, filename, processor, (request, doProcess) =>
            {
                return doProcess(() => (HttpWebResponse)request.GetResponse());
            }, cancelRequested);
        }

        public static Task<bool> BeginDownloadFile(string url, string filename, Action<bool> callback, Func<bool> cancelRequested, Action<bool, Stream> processor = null)
        {
            bool _newfile = false;
            return DoDownloadFile(url, filename, processor, (u, f, p) =>
            {
                return BeginDownloadFile(u, f, (n, s) =>
                {
                    _newfile = n;
                    p(n, s);
                    callback(_newfile);
                }, cancelRequested);
            });
        }

        static public bool DownloadFile(string url, string filename, out bool newfile, Action<bool, Stream> processor = null, Func<bool> cancelRequested = null)
        {
            if ( !url.Contains("http"))
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Not valid url (Debug) {0} ", url));
                newfile = false;
                return false;
            }

            bool _newfile = false;
            bool success = DoDownloadFile(url, filename, processor, (u, f, p) =>
            {
                return DownloadFile(u, f, (n, s) =>
                {
                    _newfile = n;
                    p(n, s);
                }, cancelRequested);
            });

            newfile = _newfile && success;
            return success;
        }


    }
}
