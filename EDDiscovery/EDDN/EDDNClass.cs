using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery2.HTTP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;

namespace EDDiscovery.EDDN
{
    public class EDDNClass : HttpCom
    {
        public string commanderName;
        
        private readonly string fromSoftwareVersion;
        private readonly string fromSoftware;
        private readonly string EDDNServer = "http://eddn-gateway.elite-markets.net:8080/upload/";

        public EDDNClass()
        {
            fromSoftware = "EDDiscovery";
            var assemblyFullName = Assembly.GetExecutingAssembly().FullName;
            fromSoftwareVersion = assemblyFullName.Split(',')[1].Split('=')[1];
            commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;

            _serverAddress = EDDNServer;
        }

        private JObject Header()
        {
            JObject header = new JObject();

            header["uploaderID"] = commanderName;
            header["softwareName"] = fromSoftware;
            header["softwareVersion"] = fromSoftwareVersion;

            return header;
        }


        private string GetEDDNSchemaRef()
        {
            if (commanderName.StartsWith("[BETA]"))
                return "http://schemas.elite-markets.net/eddn/journal/1/test";
            else
                return "http://schemas.elite-markets.net/eddn/journal/1";
        }

        private JObject RemoveCommonKeys(JObject obj)
        {
            foreach (JProperty prop in obj.Properties().ToList())
            {
                if (prop.Name.EndsWith("_Localised") || prop.Name.StartsWith("EDD"))
                {
                    obj.Remove(prop.Name);
                }
            }

            return obj;
        }

        public JObject CreateEDDNMessage(JournalFSDJump journal)
        {
            if (!journal.HasCoordinate || journal.StarPosFromEDSM)
                return null;

            JObject msg = new JObject();
            
            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNSchemaRef();

            JObject message = (JObject) JObject.Parse(journal.EventDataString);

            if (JSONHelper.IsNullOrEmptyT(message["FuelUsed"]))  // Old ED 2.1 messages has no Fuel used fields
                return null;


            message = RemoveCommonKeys(message);
            message.Remove("BoostUsed");
            message.Remove("JumpDist");
            message.Remove("FuelUsed");
            message.Remove("FuelLevel");
            message.Remove("StarPosFromEDSM");

            msg["message"] = message;
            return msg;
        }

        public JObject CreateEDDNMessage(JournalDocked journal, double x, double y, double z)
        {
            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNSchemaRef();

            JObject message = (JObject)JObject.Parse(journal.EventDataString);

            message = RemoveCommonKeys(message);
            message.Remove("CockpitBreach");

            message["StarPos"] = new JArray(new float[] { (float)x, (float)y, (float)z });

            msg["message"] = message;
            return msg;
        }

        public JObject CreateEDDNMessage(JournalScan journal, string starSystem, double x, double y, double z)
        {
            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNSchemaRef();

            JObject message = (JObject)JObject.Parse(journal.EventDataString);

            message["StarSystem"] = starSystem;
            message["StarPos"] = new JArray(new float[] { (float)x, (float)y, (float)z });

            if (!journal.BodyName.StartsWith(starSystem))  // For now test if its a different name ( a few exception for like sol system with named planets)  To catch a rare out of sync bug in historylist.
            {
                return null;
            }


            message = RemoveCommonKeys(message);
            msg["message"] = message;
            return msg;
        }


        public bool PostMessage(JObject msg)
        {

            ResponseData resp = RequestPost(msg.ToString(), "");

            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else return false;
        }



        static public bool CheckforEDMC()
        {
            string EDMCFileName = null;


            try
            {
                Process[] processes32 = Process.GetProcessesByName("EDMarketConnector");
               

                Process[] processes = processes32;

                if (processes == null)
                {
                    return  false;
                }
                else if (processes.Length == 0)
                {
                    return false;
                }
                else
                {
                    string processFilename = null;
                    try
                    {
                        int id = processes[0].Id;
                        processFilename = GetMainModuleFilepath(id);        // may return null if id not found (seen this)

                        if (processFilename != null)
                            EDMCFileName = processFilename;
                    }
                    catch (Win32Exception)
                    {
                    }

                    if (EDMCFileName != null)                                 // if found..
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
            }
            return false;
        }

        private static string GetMainModuleFilepath(int processId)
        {
            string wmiQueryString = "SELECT ProcessId, ExecutablePath FROM Win32_Process WHERE ProcessId = " + processId;

            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            {
                if (searcher != null)           // seen it return null
                {
                    using (var results = searcher.Get())
                    {
                        if (results != null)
                        {
                            foreach (ManagementObject mo in results)
                            {
                                if (mo != null)
                                {
                                    return (string)mo["ExecutablePath"];
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }




    }
}
