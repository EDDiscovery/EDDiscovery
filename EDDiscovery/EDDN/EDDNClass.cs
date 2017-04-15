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
using EDDiscovery.CompanionAPI;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;

using EDDiscovery.HTTP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
            commanderName = EDCommander.Current.Name;

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


        private string GetEDDNJournalSchemaRef()
        {
            if (commanderName.StartsWith("[BETA]"))
                return "http://schemas.elite-markets.net/eddn/journal/1/test";
            else
                return "http://schemas.elite-markets.net/eddn/journal/1";
        }

        private string GetEDDNCommoditySchemaRef()
        {
            if (commanderName.StartsWith("[BETA]"))
                return "http://schemas.elite-markets.net/eddn/commodity/3/test";
            else
                //return "http://schemas.elite-markets.net/eddn/commodity/3";
                return "http://schemas.elite-markets.net/eddn/commodity/3/test"; // For testing now.
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
            msg["$schemaRef"] = GetEDDNJournalSchemaRef();

            JObject message = journal.GetJson();

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
            msg["$schemaRef"] = GetEDDNJournalSchemaRef();

            JObject message = journal.GetJson();

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
            msg["$schemaRef"] = GetEDDNJournalSchemaRef();

            JObject message = journal.GetJson();

            message["StarSystem"] = starSystem;
            message["StarPos"] = new JArray(new float[] { (float)x, (float)y, (float)z });

            string bodydesig = journal.BodyDesignation ?? journal.BodyName;

            if (!bodydesig.StartsWith(starSystem))  // For now test if its a different name ( a few exception for like sol system with named planets)  To catch a rare out of sync bug in historylist.
            {
                return null;
            }


            message = RemoveCommonKeys(message);
            msg["message"] = message;
            return msg;
        }


        public JObject CreateEDDNCommodityMessage(List<CCommodities> commodities, string systemName, string stationName, DateTime time)
        {
            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNCommoditySchemaRef();

            JObject message = new JObject();

            message["systemName"] = systemName;
            message["stationName"] = stationName;
            message["timestamp"] = time.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);

            JArray JAcommodities = new JArray();

            foreach (var commodity in commodities)
            {
                JObject jo = new JObject();

                jo["demandBracket"] = commodity.demandBracket;
                jo["name"] = commodity.name;
                jo["buyPrice"] = commodity.buyPrice;
                jo["meanPrice"] = commodity.meanPrice;
                jo["stockBracket"] = commodity.stockBracket;
                jo["demand"] = commodity.demand;
                jo["sellPrice"] = commodity.sellPrice;
                jo["stock"] = commodity.stock;

                if (commodity.StatusFlags!=null && commodity.StatusFlags.Count > 0)
                {
                    jo["statusFlags"] = new JArray(commodity.StatusFlags);
                }

                JAcommodities.Add(jo);
            }

            message["commodities"] = JAcommodities;
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
