using EDDiscovery.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery
{
    public class EDSCClass
    {
        public static bool UseTest = false;

        private string EDSCRequest(string query, string action)
        {
            try
            {
                WebRequest request = WebRequest.Create("http://edstarcoordinator.com/api.asmx/" + action);
                // Set the Method property of the request to POST.
                request.Method = "POST";
                // Create POST data and convert it to a byte array.
                //WRITE JSON DATA TO VARIABLE D
                //string postData = "D={\"requests\":[{\"C\":\"Gpf_Auth_Service\", \"M\":\"authenticate\", \"fields\":[[\"name\",\"value\"],[\"Id\",\"\"],[\"username\",\"user@example.com\"],[\"password\",\"ab9ce908\"],[\"rememberMe\",\"Y\"],[\"language\",\"en-US\"],[\"roleType\",\"M\"]]}],\"C\":\"Gpf_Rpc_Server\", \"M\":\"run\"}";
                string postData = "{ \"data\": " + query + " }";


                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/json; charset=utf-8";
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

        public string RequestSystems(string date)
        {
            string query = "{\"ver\":\"2\", " + (UseTest ? "test: true," : "") + "  \"outputmode\":2, \"filter\": { \"knownstatus\":0, \"cr\":1,  \"date\":\"" + date + "\" }} ";

            return EDSCRequest(query, "GetSystems");
        }

        public string RequestDistances(string date)
        {
            string query = "{\"ver\":\"2\", " + (UseTest ? "test: true," : "") + " \"outputmode\":2, \"filter\": { \"knownstatus\":0, \"cr\":1,  \"date\":\"" + date + "\" }} ";

            return EDSCRequest(query, "GetDistances");
        }

        public string SubmitDistances(string cmdr, string from, string to, double dist)
        {
            return SubmitDistances(cmdr, from, new Dictionary<string, double> { { to, dist } });
        }

        public string SubmitDistances(string cmdr, string from, Dictionary<string, double> distances)
        {
            CultureInfo culture = new CultureInfo("en-US");
            string query = "{ver:2," + (UseTest ? " test: true," : "") + " commander:\"" + cmdr + "\", p0: { name: \"" + from + "\" },   refs: [";

            var counter = 0;
            foreach (var item in distances)
            {
                if (counter++ > 0)
                {
                    query += ",";
                }

                var to = item.Key;
                var distance = item.Value.ToString("0.00", culture);

                query += " { name: \"" + to + "\",  dist: " + distance + " } ";
            }


            query += "] } ";

            return EDSCRequest(query, "SubmitDistances");
        }


        public bool ShowDistanceResponce(string json, out string respstr)
        {
            bool retval = false;
            JObject edsc = null;

            respstr = "";

            try
            {
                if (json == null)
                    return false;

                edsc = (JObject)JObject.Parse(json);

                if (edsc == null)
                    return false;

                JObject edscdata = (JObject)edsc["d"];
                JObject status = (JObject)edscdata["status"];

                JArray input = (JArray)status["input"];
                if (input != null)
                    foreach (var st in input)
                    {
                        JObject inpstatus = (JObject)st["status"];
                        int statusnum = inpstatus["statusnum"].Value<int>();

                        if (statusnum == 0)
                            retval = true;

                        respstr += "Status " + statusnum.ToString() + " : " + inpstatus["msg"].Value<string>() + Environment.NewLine;

                    }


                JArray system = (JArray)status["system"];
                if (system != null)
                    foreach (var st in system)
                    {
                        JObject inpstatus = (JObject)st["status"];
                        int statusnum = inpstatus["statusnum"].Value<int>();


                        respstr += "System " + statusnum.ToString() + " : " + inpstatus["msg"].Value<string>() + Environment.NewLine;

                    }

                JArray dist = (JArray)status["dist"];
                if (dist != null)
                    foreach (var st in dist)
                    {
                        JObject inpstatus = (JObject)st["status"];
                        int statusnum = inpstatus["statusnum"].Value<int>();

                        respstr += "Dist " + statusnum.ToString() + " : " + inpstatus["msg"].Value<string>() + Environment.NewLine;

                    }
                JArray trilat = (JArray)status["trilat"];
                if (trilat != null)
                    foreach (var st in trilat)
                    {
                        JObject inpstatus = (JObject)st["status"];
                        int statusnum = inpstatus["statusnum"].Value<int>();

                        respstr += "Trilat " + statusnum.ToString() + " : " + inpstatus["msg"].Value<string>() + " : " + st["system"].Value<string>() + Environment.NewLine;

                    }
                return retval;
            }
            catch (Exception ex)
            {
                respstr += "Excpetion in ShowDistanceResponce: " + ex.Message;
                return false;
            }
        }



        internal string  EDSCGetNewSystems(SQLiteDBClass db)
        {
            string json;
            string date = "2010-01-01 00:00:00";
            string lstsyst = db.GetSettingString("EDSCLastSystems", "2010-01-01 00:00:00");

            string retstr="";
      

            Application.DoEvents();

            json = RequestSystems(lstsyst);

            db.GetAllSystems();

            List<SystemClass> listNewSystems = SystemClass.ParseEDSC(json, ref date);

            retstr = listNewSystems.Count.ToString() + " new systems from EDSC." + Environment.NewLine;
            Application.DoEvents();
            SystemClass.Store(listNewSystems);
            db.PutSettingString("EDSCLastSystems", date);

            return retstr;
        }


    }
}
