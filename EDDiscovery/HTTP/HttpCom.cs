using EDDiscovery;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace EDDiscovery2.HTTP
{
    public struct ResponseData
    {
        public ResponseData(HttpStatusCode statusCode, string content)
        {
            StatusCode = statusCode;
            Content = content;
        }

        public HttpStatusCode StatusCode; // Sometimes you need the status code if you're in a
                                   // converstation with the server
        public string Content;
    }

    public class HttpCom
    {
        protected string _serverAddress;

        protected ResponseData RequestPost(string json, string action, bool authenticate=true)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_serverAddress + action);
                // Set the Method property of the request to POST.
                request.Method = "POST";
                if (authenticate)
                {
                    AddAuthHeaders(request);
                }
                // Create POST data and convert it to a byte array.
                //WRITE JSON DATA TO VARIABLE D
                //string postData = "D={\"requests\":[{\"C\":\"Gpf_Auth_Service\", \"M\":\"authenticate\", \"fields\":[[\"name\",\"value\"],[\"Id\",\"\"],[\"username\",\"user@example.com\"],[\"password\",\"ab9ce908\"],[\"rememberMe\",\"Y\"],[\"language\",\"en-US\"],[\"roleType\",\"M\"]]}],\"C\":\"Gpf_Rpc_Server\", \"M\":\"run\"}";
                string postData = json;


                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/json; charset=utf-8";
                //request.Headers.Add("Accept-Encoding", "gzip,deflate");
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                //request.Timeout = 740 * 1000;

                if (EDDiscoveryForm.EDDConfig.EDSMLog)
                    WriteEDSMLog("POST " + request.RequestUri, postData);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Display the status.
                //            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                var data = new ResponseData(response.StatusCode, reader.ReadToEnd());

                // Display the content.
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();

                if (EDDiscoveryForm.EDDConfig.EDSMLog)
                {
                    //TODO: Log Status Code too
                    WriteEDSMLog(data.Content, "");
                }

                return data;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                if (EDDiscoveryForm.EDDConfig.EDSMLog)
                {
                    WriteEDSMLog("Exception" + ex.Message, "");
                }

                return new ResponseData(HttpStatusCode.BadRequest, null);
            }
        }

        protected ResponseData RequestPatch(string json, string action)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_serverAddress + action);
                request.Method = "PATCH";
                string postData = json;


                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/json; charset=utf-8";
                //request.Headers.Add("Accept-Encoding", "gzip,deflate");
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                //request.Timeout = 740 * 1000;

                if (EDDiscoveryForm.EDDConfig.EDSMLog)
                    WriteEDSMLog("PATCH " + request.RequestUri, postData);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Display the status.
                //            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                var data = new ResponseData(response.StatusCode, reader.ReadToEnd());
                // Display the content.
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();

                if (EDDiscoveryForm.EDDConfig.EDSMLog)
                {
                    WriteEDSMLog(data.Content, "");
                }

                return data;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                if (EDDiscoveryForm.EDDConfig.EDSMLog)
                {
                    WriteEDSMLog("Exception" + ex.Message, "");
                }

                return new ResponseData(HttpStatusCode.BadRequest, null);
            }
        }



        protected ResponseData RequestGet(string action)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_serverAddress + action);
                // Set the Method property of the request to POST.
                request.Method = "GET";

                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/json; charset=utf-8";
                request.Headers.Add("Accept-Encoding", "gzip,deflate");
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                if (EDDiscoveryForm.EDDConfig.EDSMLog)
                    WriteEDSMLog("GET " + request.RequestUri, "");


                // Get the response.
                //request.Timeout = 740 * 1000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Display the status.
                //            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                var data = new ResponseData(response.StatusCode, reader.ReadToEnd());
                var statusCode = response.StatusCode;
                // Display the content.
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();

                if (EDDiscoveryForm.EDDConfig.EDSMLog)
                {
                    WriteEDSMLog(data.Content, "");
                }

                return data;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);

                if (EDDiscoveryForm.EDDConfig.EDSMLog)
                {
                    WriteEDSMLog("Exception" + ex.Message, "");
                }


                return new ResponseData(HttpStatusCode.BadRequest, null);
            }

        }


        protected ResponseData RequestDelete(string action)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_serverAddress + action);
                request.Method = "DELETE";

                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/json; charset=utf-8";
                request.Headers.Add("Accept-Encoding", "gzip,deflate");
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                if (EDDiscoveryForm.EDDConfig.EDSMLog)
                    WriteEDSMLog("DELETE " + request.RequestUri, "");


                // Get the response.
                //request.Timeout = 740 * 1000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Display the status.
                //            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                var data = new ResponseData(response.StatusCode, reader.ReadToEnd());
                // Display the content.
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();

                if (EDDiscoveryForm.EDDConfig.EDSMLog)
                {
                    WriteEDSMLog(data.Content, "");
                }

                return data;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);

                if (EDDiscoveryForm.EDDConfig.EDSMLog)
                {
                    WriteEDSMLog("Exception" + ex.Message, "");
                }


                return new ResponseData(HttpStatusCode.BadRequest, null);
            }

        }

        protected void AddAuthHeaders(WebRequest request)
        {
            // Override me
        }

        private void WriteEDSMLog(string str1, string str2)
        {
            try
            {
                string filename = Path.Combine(Tools.GetAppDataDirectory(), "Log", "edsm" + EDDiscoveryForm.EDDConfig.LogIndex + ".log");

                using (StreamWriter w = File.AppendText(filename))
                {
                    w.WriteLine(DateTime.Now.ToLongTimeString() + "; " + str1 + "; " + str2);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
        }

    }
}
