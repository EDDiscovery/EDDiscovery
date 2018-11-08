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
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
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

namespace EliteDangerousCore.EDDN
{
    public class EDDNClass : BaseUtils.HttpCom
    {
        public string commanderName;
        public bool isBeta;
        
        private readonly string fromSoftwareVersion;
        private readonly string fromSoftware;
        private readonly string EDDNServer = "https://eddn.edcd.io:4430/upload/";

        public EDDNClass()
        {
            fromSoftware = "EDDiscovery";
            var assemblyFullName = Assembly.GetEntryAssembly().FullName;
            fromSoftwareVersion = assemblyFullName.Split(',')[1].Split('=')[1];
            commanderName = EDCommander.Current.Name;

            httpserveraddress = EDDNServer;
        }

        private JObject Header()
        {
            JObject header = new JObject();

            header["uploaderID"] = commanderName;
            header["softwareName"] = fromSoftware;
            header["softwareVersion"] = fromSoftwareVersion;

            return header;
        }

        static public bool IsEDDNMessage( JournalTypeEnum EntryType, DateTime EventTimeUTC )
        {
            DateTime ed22 = new DateTime(2016, 10, 25, 12, 0, 0);
            if ((EntryType == JournalTypeEnum.Scan ||
                 EntryType == JournalTypeEnum.Docked ||
                 EntryType == JournalTypeEnum.FSDJump ||
                 EntryType == JournalTypeEnum.Location ||
                 EntryType == JournalTypeEnum.Market ||
                 EntryType == JournalTypeEnum.Shipyard ||
                 EntryType == JournalTypeEnum.Outfitting) && EventTimeUTC > ed22) return true;
            else return false;
        }

        private string GetEDDNJournalSchemaRef()
        {
            if (isBeta || commanderName.StartsWith("[BETA]"))
                return "https://eddn.edcd.io/schemas/journal/1/test";
            else
                return "https://eddn.edcd.io/schemas/journal/1";
        }

        private string GetEDDNCommoditySchemaRef()
        {
            if (isBeta || commanderName.StartsWith("[BETA]"))
                return "https://eddn.edcd.io/schemas/commodity/3/test";
            else
                return "https://eddn.edcd.io/schemas/commodity/3";
                //return "https://eddn.edcd.io/schemas/commodity/3/test"; // For testing now.
        }

        private string GetEDDNOutfittingSchemaRef()
        {
            if (isBeta || commanderName.StartsWith("[BETA]"))
                return "https://eddn.edcd.io/schemas/outfitting/2/test";
            else
                return "https://eddn.edcd.io/schemas/outfitting/2";
        }

        private string GetEDDNShipyardSchemaRef()
        {
            if (isBeta || commanderName.StartsWith("[BETA]"))
                return "https://eddn.edcd.io/schemas/shipyard/2/test";
            else
                return "https://eddn.edcd.io/schemas/shipyard/2";
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

        private JObject RemoveFactionReputation(JObject obj)
        {
            JArray factions = obj["Factions"] as JArray;

            if (factions != null)
            {
                foreach (JObject faction in factions)
                {
                    faction.Remove("MyReputation");
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

            if (message["FuelUsed"].Empty())  // Old ED 2.1 messages has no Fuel used fields
                return null;


            message = RemoveCommonKeys(message);
            message = RemoveFactionReputation(message);
            message.Remove("BoostUsed");
            message.Remove("MyReputation"); 
            message.Remove("JumpDist");
            message.Remove("FuelUsed");
            message.Remove("FuelLevel");
            message.Remove("StarPosFromEDSM");
            message.Remove("ActiveFine");

            msg["message"] = message;
            return msg;
        }

        public JObject CreateEDDNMessage(JournalLocation journal)
        {
            if (!journal.HasCoordinate || journal.StarPosFromEDSM)
                return null;

            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNJournalSchemaRef();

            JObject message = journal.GetJson();

            message = RemoveCommonKeys(message);
            message = RemoveFactionReputation(message);
            message.Remove("StarPosFromEDSM");
            message.Remove("Latitude");
            message.Remove("Longitude");
            message.Remove("MyReputation");
            message.Remove("ActiveFine");

            msg["message"] = message;
            return msg;
        }

        public JObject CreateEDDNMessage(JournalDocked journal, ISystem system)
        {
            if (!String.Equals(system.Name, journal.StarSystem, StringComparison.InvariantCultureIgnoreCase))
                return null;

            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNJournalSchemaRef();

            JObject message = journal.GetJson();

            message = RemoveCommonKeys(message);
            message.Remove("CockpitBreach");
            message.Remove("Wanted");
            message.Remove("ActiveFine");

            message["StarPos"] = new JArray(new float[] { (float)system.X, (float)system.Y, (float)system.Z });

            if (system.SystemAddress != null && message["SystemAddress"] == null)
                message["SystemAddress"] = system.SystemAddress;

            msg["message"] = message;
            return msg;
        }

        public JObject CreateEDDNJournalMessage(JournalOutfitting journal, double x, double y, double z, long? systemAddress)
        {
            if (journal.ItemList.Items == null)
                return null;

            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNJournalSchemaRef();

            JObject message = journal.GetJson();

            message = RemoveCommonKeys(message);

            message["StarPos"] = new JArray(new float[] { (float)x, (float)y, (float)z });

            if (systemAddress != null)
                message["SystemAddress"] = systemAddress;

            msg["message"] = message;
            return msg;
        }

        public JObject CreateEDDNOutfittingMessage(JournalOutfitting journal, ISystem system = null)
        {
            if (journal.ItemList.Items == null)
                return null;

            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNOutfittingSchemaRef();

            JObject message = new JObject
            {
                ["timestamp"] = journal.EventTimeUTC.ToString("yyyy-MM-ddTHH:mm:ss'Z'"),
                ["systemName"] = journal.ItemList.StarSystem,
                ["stationName"] = journal.ItemList.StationName,
                ["marketId"] = journal.MarketID,
                ["modules"] = new JArray(journal.ItemList.Items.Select(m => JournalFieldNaming.NormaliseFDItemName(m.FDName)))
            };

            //if (systemAddress != null)
            //    message["systemAddress"] = systemAddress;

            msg["message"] = message;
            return msg;
        }

        public JObject CreateEDDNJournalMessage(JournalShipyard journal, double x, double y, double z, long? systemAddress)
        {
            if (journal.Yard.Ships == null)
                return null;

            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNJournalSchemaRef();

            JObject message = journal.GetJson();

            message = RemoveCommonKeys(message);

            message["StarPos"] = new JArray(new float[] { (float)x, (float)y, (float)z });

            if (systemAddress != null)
                message["SystemAddress"] = systemAddress;

            msg["message"] = message;
            return msg;
        }

        public JObject CreateEDDNShipyardMessage(JournalShipyard journal, ISystem system = null)
        {
            if (journal.Yard.Ships == null)
                return null;

            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNShipyardSchemaRef();

            JObject message = new JObject
            {
                ["timestamp"] = journal.EventTimeUTC.ToString("yyyy-MM-ddTHH:mm:ss'Z'"),
                ["systemName"] = journal.Yard.StarSystem,
                ["stationName"] = journal.Yard.StationName,
                ["marketId"] = journal.MarketID,
                ["ships"] = new JArray(journal.Yard.Ships.Select(m => m.FDShipType))
            };

            //if (systemAddress != null)
            //    message["SystemAddress"] = systemAddress;

            msg["message"] = message;
            return msg;
        }

        public JObject CreateEDDNJournalMessage(JournalMarket journal, double x, double y, double z, long? systemAddress)
        {
            if (journal.Commodities == null || journal.Commodities.Count == 0)
                return null;

            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNJournalSchemaRef();

            JObject message = journal.GetJson();

            message = RemoveCommonKeys(message);

            message["StarPos"] = new JArray(new float[] { (float)x, (float)y, (float)z });

            if (systemAddress != null)
                message["SystemAddress"] = systemAddress;

            msg["message"] = message;
            return msg;
        }

        public JObject CreateEDDNMessage(JournalScan journal, ISystem system)
        {
            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNJournalSchemaRef();

            JObject message = journal.GetJson();

            message["StarSystem"] = system.Name;
            message["StarPos"] = new JArray(new float[] { (float)system.X, (float)system.Y, (float)system.Z });

            if (system.SystemAddress != null)
                message["SystemAddress"] = system.SystemAddress;

            if (message["Materials"] != null && message["Materials"] is JArray)
            {
                foreach (JObject mmat in message["Materials"])
                {
                    mmat.Remove("Name_Localised");
                }
            }

            string bodydesig = journal.BodyDesignation ?? journal.BodyName;

            if (!bodydesig.StartsWith(system.Name, StringComparison.InvariantCultureIgnoreCase))  // For now test if its a different name ( a few exception for like sol system with named planets)  To catch a rare out of sync bug in historylist.
            {
                return null;
            }


            message = RemoveCommonKeys(message);
            msg["message"] = message;
            return msg;
        }


        public JObject CreateEDDNCommodityMessage(List<CCommodities> commodities, string systemName, string stationName, long? marketID, DateTime time)
        {
            if (commodities == null || commodities.Count == 0)
                return null;
                
            JObject msg = new JObject();

            msg["header"] = Header();
            msg["$schemaRef"] = GetEDDNCommoditySchemaRef();

            JObject message = new JObject();

            message["systemName"] = systemName;
            message["stationName"] = stationName;
            message["marketId"] = marketID;
            message["timestamp"] = time.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);

            JArray JAcommodities = new JArray();

            foreach (var commodity in commodities)
            {
                if (commodity.category.IndexOf("NonMarketable", StringComparison.InvariantCultureIgnoreCase)>=0)
                {
                    continue;
                }

                JObject jo = new JObject();

                jo["name"] = commodity.fdname;
                jo["meanPrice"] = commodity.meanPrice;
                jo["buyPrice"] = commodity.buyPrice;
                jo["stock"] = commodity.stock;
                jo["stockBracket"] = commodity.stockBracket;
                jo["sellPrice"] = commodity.sellPrice;
                jo["demand"] = commodity.demand;
                jo["demandBracket"] = commodity.demandBracket;

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
            try
            {
                BaseUtils.ResponseData resp = RequestPost(msg.ToString(), "");

                if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else return false;
            }
            catch (System.Net.WebException ex)
            {
                System.Net.HttpWebResponse response = ex.Response as System.Net.HttpWebResponse;
                string responsetext = null;
                using (var responsestream = response.GetResponseStream())
                {
                    using (var reader = new System.IO.StreamReader(responsestream))
                    {
                        responsetext = reader.ReadToEnd();
                    }
                }

                System.Diagnostics.Trace.WriteLine($"EDDN message post failed - status: {response?.StatusCode.ToString() ?? ex.Status.ToString()}\nResponse: {responsetext}\nEDDN Message: {msg.ToString()}");
                return false;
            }
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
