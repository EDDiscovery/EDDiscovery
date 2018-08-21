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
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public class HttpCom
    {
        protected string httpserveraddress { get; set; }

        static public string LogPath = null;           // set path to cause logging to occur

        public String MimeType { get; set; } = "application/json; charset=utf-8";

        protected static string EscapeLongDataString(string str)
        {
            string ret = "";

            for (int p = 0; p < str.Length; p += 16384)
            {
                ret += Uri.EscapeDataString(str.Substring(p, Math.Min(str.Length - p, 16384)));
            }

            return ret;
        }

        protected ResponseData RequestPost(string postData, string action, NameValueCollection headers = null, bool handleException = false )
        {
            return Request("POST", postData, action, headers, handleException,0);
        }

        protected ResponseData RequestPatch(string postData, string action, NameValueCollection headers = null, bool handleException = false)
        {
            return Request("PATCH", postData, action, headers, handleException,0);
        }

        protected ResponseData RequestGet(string action, NameValueCollection headers = null, bool handleException = false, int timeout = 5000)
        {
            return Request("GET", "", action, headers, handleException, timeout);
        }

        // responsecallback is in TASK you must convert back to foreground
        protected void RequestGetAsync(string action, Action<ResponseData,Object> responsecallback, Object tag = null, NameValueCollection headers = null, bool handleException = false, int timeout = 5000)
        {
            Task.Factory.StartNew(() =>
            {
                ResponseData resp = Request("GET", "", action, headers, handleException, timeout);
                responsecallback(resp,tag);
            });
        }

        protected ResponseData Request(string method, string postData, string action, NameValueCollection headers, bool handleException,
                                        int timeout)
        {
            if (httpserveraddress == null || httpserveraddress.Length == 0)           // for debugging, set _serveraddress empty
            {
                System.Diagnostics.Trace.WriteLine(method + action);
                return new ResponseData(HttpStatusCode.Unauthorized);
            }

            try
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(httpserveraddress + action);
                    request.Method = method;
                    request.ContentType = MimeType;    //request.Headers.Add("Accept-Encoding", "gzip,deflate");

                    if (method == "GET")
                    {
                        request.Headers.Add("Accept-Encoding", "gzip,deflate");
                        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                        request.Timeout = timeout;
                    }
                    else
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(postData);  // Set the ContentType property of the WebRequest.
                        request.ContentLength = byteArray.Length;       // Set the ContentLength property of the WebRequest.
                        Stream dataStream = request.GetRequestStream();     // Get the request stream.
                        dataStream.Write(byteArray, 0, byteArray.Length);       // Write the data to the request stream.
                        dataStream.Close();     // Close the Stream object.
                    }

                    if (headers != null)
                        request.Headers.Add(headers);

                    System.Diagnostics.Trace.WriteLine("HTTP" + method + " TO " + (httpserveraddress + action) + " Thread" + System.Threading.Thread.CurrentThread.Name);
                    WriteLog(method + " " + request.RequestUri, postData);

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    var data = getResponseData(response);
                    response.Close();

                    System.Diagnostics.Trace.WriteLine("..HTTP" + method + " Response " + data.StatusCode);
                    WriteLog("Response", data.Body.Left(512));

                    return data;
                }
                catch (WebException ex)
                {
                    if (!handleException)
                    {
                        throw;
                    }

                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        var data = getResponseData(httpResponse);
                        System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                        System.Diagnostics.Trace.WriteLine("WebException : " + ex.Message);
                        if (httpResponse != null)
                        {
                            System.Diagnostics.Trace.WriteLine("Response code : " + httpResponse.StatusCode);
                            System.Diagnostics.Trace.WriteLine("Response body : " + data.Body);
                        }

                        System.Diagnostics.Trace.WriteLine(ex.StackTrace);

                        if (LogPath != null)
                        {
                            WriteLog("WebException" + ex.Message, "");
                            if (httpResponse != null)
                            {
                                WriteLog($"HTTP Error code: {httpResponse.StatusCode}", "");
                                WriteLog($"HTTP Error body: {data.Body}", "");
                            }
                        }
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!handleException)
                {
                    throw;
                }

                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);

                WriteLog("Exception" + ex.Message, "");
 
                return new ResponseData(HttpStatusCode.BadRequest);
            }
        }


        private static Object LOCK = new Object();
        static public  void WriteLog(string str1, string str2)
        {
            //System.Diagnostics.Debug.WriteLine("From:" + Environment.StackTrace.StackTrace("WriteLog",10) + Environment.NewLine + "HTTP:" + str1 + ":" + str2);

            if (LogPath == null || !Directory.Exists(LogPath))
                return;

            if (str1 != null && str1.ToLowerInvariant().Contains("password"))
                str1 = "** This string contains a password so not logging it.**";

            if (str2 != null && str2.ToLowerInvariant().Contains("password"))
                str2 = "** This string contains a password so not logging it.**";

            try
            {
                lock(LOCK)
                {
                    string filename = Path.Combine(LogPath, "HTTP_" + DateTime.Now.ToString("yyyy-MM-dd") + ".hlog");

                    using (StreamWriter w = File.AppendText(filename))
                    {
                        w.WriteLine(DateTime.UtcNow.ToStringZulu() + ": " + str1 + ": " + str2);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
        }

        private ResponseData getResponseData(HttpWebResponse response, bool? error = null)
        {
            if (response == null)
                return new ResponseData(HttpStatusCode.NotFound);

            var dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            var data = new ResponseData(response.StatusCode, reader.ReadToEnd(), response.Headers);
            reader.Close();
            dataStream.Close();
            return data;
        }

    }
}
