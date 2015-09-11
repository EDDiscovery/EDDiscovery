using EDDiscovery.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery2.EDSM
{
    class EDSMClass
    {
        private string RequestPost(string json, string action)
        {
            try
            {
                WebRequest request = WebRequest.Create("http://www.edsm.net/api-v1/" + action);
                // Set the Method property of the request to POST.
                request.Method = "POST";
                // Create POST data and convert it to a byte array.
                //WRITE JSON DATA TO VARIABLE D
                //string postData = "D={\"requests\":[{\"C\":\"Gpf_Auth_Service\", \"M\":\"authenticate\", \"fields\":[[\"name\",\"value\"],[\"Id\",\"\"],[\"username\",\"user@example.com\"],[\"password\",\"ab9ce908\"],[\"rememberMe\",\"Y\"],[\"language\",\"en-US\"],[\"roleType\",\"M\"]]}],\"C\":\"Gpf_Rpc_Server\", \"M\":\"run\"}";
                string postData = "{ \"data\": " + json + " }";


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
                WebResponse response = request.GetResponse();
                // Display the status.
                //            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();

                return responseFromServer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);

                //MessageBox.Show("Exception in EDSCRequest: " + ex.Message);
                return null;
            }
        }


        private string RequestGet(string action)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.edsm.net/api-v1/" + action);
                // Set the Method property of the request to POST.
                request.Method = "GET";


                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/json; charset=utf-8";
                request.Headers.Add("Accept-Encoding", "gzip,deflate");
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;



                // Get the response.
                //request.Timeout = 740 * 1000;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                // Display the status.
                //            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();

                return responseFromServer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);

                //MessageBox.Show("Exception in EDSCRequest: " + ex.Message);
                return null;
            }

        }


    

        public string SubmitDistances(string cmdr, string from, string to, double dist)
        {
            return SubmitDistances(cmdr, from, new Dictionary<string, double> { { to, dist } });
        }

        public string SubmitDistances(string cmdr, string from, Dictionary<string, double> distances)
        {
            CultureInfo culture = new CultureInfo("en-US");
            string query = "{\"ver\":2," + " \"commander\":\"" + cmdr + "\", \"p0\": { \"name\": \"" + from + "\" },   \"refs\": [";

            var counter = 0;
            foreach (var item in distances)
            {
                if (counter++ > 0)
                {
                    query += ",";
                }

                var to = item.Key;
                var distance = item.Value.ToString("0.00", culture);

                query += " { \"name\": \"" + to + "\",  \"dist\": " + distance + " } ";
            }


            query += "] } ";

            return RequestPost(query, "submit-distances");
        }


        public bool ShowDistanceResponse(string json, out string respstr, out Boolean trilOK)
        {
            bool retval = true;
            JObject edsc = null;
            trilOK = false;

            respstr = "";

            try
            {
                if (json == null)
                    return false;

                edsc = (JObject)JObject.Parse(json);

                if (edsc == null)
                    return false;

                JObject basesystem = (JObject)edsc["basesystem"];
                JArray distances = (JArray)edsc["distances"];


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
                respstr += "Excpetion in ShowDistanceResponse: " + ex.Message;
                return false;
            }
        }

        

     public string RequestSystems(string date)
        {
            string query;

            //string datestr = date.ToString("yyyy-MM-dd hh:mm:ss");

            query = "?startdatetime=" + WebUtility.HtmlEncode(date);
            return RequestGet("systems" + query + "&coords=1&submitted=1");
        }

        public string RequestDistances(string date)
        {
            string query;
            query = "?startdatetime=" + WebUtility.HtmlEncode(date);

            return RequestGet("distances" + query + "coords=1 & submitted=1");
        }



        internal string GetNewSystems(SQLiteDBClass db)
        {
            string json;
            string date = "2010-01-01 00:00:00";
            string lstsyst;

            string retstr = "";


            Application.DoEvents();

            db.GetAllSystems();

            //if (lstsys)


            var system = SQLiteDBClass.globalSystems.Max(x => x.UpdateDate);
            lstsyst = system.ToString("yyyy-MM-dd HH:mm:ss");
            lstsyst = db.GetSettingString("EDSMLastSystems", lstsyst);

            if (lstsyst.Equals("2010-01-01 00:00:00"))
                lstsyst = system.ToString("yyyy-MM-dd HH:mm:ss");

            json = RequestSystems(lstsyst);


            List<SystemClass> listNewSystems = SystemClass.ParseEDSM(json, ref date);

            retstr = listNewSystems.Count.ToString() + " new systems from EDSM." + Environment.NewLine;
            Application.DoEvents();
            SystemClass.Store(listNewSystems);
            db.PutSettingString("EDSMLastSystems", date);

            return retstr;
        }


        public List<DistanceClass> GetDistances(string systemname)
        {
            List<DistanceClass> listDistances = new List<DistanceClass>();
            try
            {
                string json;


                string query;
                query = "?sysname=" + WebUtility.HtmlEncode(systemname) + "&coords=1&distances=1&submitted=1";

                json = RequestGet("system" + query);

                //http://www.edsm.net/api-v1/system?sysname=Col+359+Sector+CP-Y+c1-18&coords=1&include_hidden=1&distances=1&submitted=1

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


    }
}
