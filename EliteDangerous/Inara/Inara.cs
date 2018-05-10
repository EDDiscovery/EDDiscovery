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
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web;
using System.Linq;

namespace EliteDangerousCore.Inara
{
    public partial class InaraClass : BaseUtils.HttpCom
    {
        // use if you need an API/name pair to get info from Inara.  Not all queries need it
        public bool ValidCredentials { get { return !string.IsNullOrEmpty(commanderName) && !string.IsNullOrEmpty(apiKey); } }

        private string commanderName;
        private string apiKey;

        private readonly string fromSoftwareVersion;
        private readonly string fromSoftware;
        static private Dictionary<long, List<JournalScan>> DictEDSMBodies = new Dictionary<long, List<JournalScan>>();

        private string InaraAPI = "inapi/v1/";      // Action end point

        public InaraClass()
        {
            fromSoftware = "EDDiscovery";
            var assemblyFullName = Assembly.GetEntryAssembly().FullName;
            fromSoftwareVersion = assemblyFullName.Split(',')[1].Split('=')[1];

            base.httpserveraddress = @"https://inara.cz/";

            apiKey = EDCommander.Current.InaraAPIKey;
            commanderName = string.IsNullOrEmpty(EDCommander.Current.InaraName) ? EDCommander.Current.Name : EDCommander.Current.InaraName;

            MimeType = "application/json";      // sets Content-type
        }

        public InaraClass(EDCommander cmdr) : this()
        {
            if (cmdr != null)
            {
                apiKey = cmdr.InaraAPIKey;
                commanderName = string.IsNullOrEmpty(cmdr.InaraName) ? cmdr.Name : cmdr.InaraName;
            }
        }

        #region Formatters

        static private JToken Event(string eventname, JToken eventData)       // an event
        {
            JObject jo = new JObject();
            jo["eventName"] = eventname;
            jo["eventTimestamp"] = DateTime.UtcNow.ToStringZulu();
            jo["eventData"] = eventData;
            return jo;
        }

        static private JToken Event(string eventname, DateTime utc, JToken eventData)       // an event
        {
            JObject jo = new JObject();
            jo["eventName"] = eventname;
            jo["eventTimestamp"] = utc;
            jo["eventData"] = eventData;
            return jo;
        }

        private JToken Header()         // Inara header
        {
            JObject jo = new JObject();
            jo["appName"] = fromSoftware;
            jo["appVersion"] = fromSoftwareVersion;
            jo["isDeveloped"] = true;
            jo["APIkey"] = apiKey;
            jo["commanderName"] = commanderName;
            return jo;
        }

        static private JArray EventArray(params JToken[] events)           // group events
        {
            JArray earray = new JArray();
            foreach (JToken t in events)
                earray.Add(t);
            return earray;
        }

        private JToken Request(JToken singleevent)              // request for a single event
        {
            JObject jouter = new JObject();
            jouter["header"] = Header();
            JArray ja = new JArray();
            ja.Add(singleevent);
            jouter["events"] = ja;
            return jouter;
        }

        private JToken Request(JToken[] events)                 // request for multi events
        {
            JObject jouter = new JObject();
            jouter["header"] = Header();
            jouter["events"] = EventArray(events);
            return jouter;
        }

        #endregion

        #region Send and receive

        public string Send(List<JToken> events)
        {
            if (!ValidCredentials)
                return null;

            JToken finaljson = Request(events.ToArray());
            string request = finaljson.ToString(Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(@"c:\code\json.txt", request);

            return "";
        }

        //example..
        public string getCommanderProfile(string commandername)
        {
            if (!ValidCredentials)
                return null;

            JObject eventData = new JObject();
            eventData["searchName"] = "Robbie";
            JToken finaljson = Request(Event("getCommanderProfile", eventData));
            string request = finaljson.ToString(Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(@"c:\code\json.txt", request);

            var response = RequestPost(request, InaraAPI, handleException: true);

            if (response.Error)
                return null;

            return response.Body; 
        }

        #endregion

        #region Formatters

        static public JToken addCommanderPermit(string starsystem, DateTime dt)
        {
            JObject eventData = new JObject();
            eventData["starsystemName"] = starsystem;
            return Event("addCommanderPermit", dt, eventData);
        }

        static public JToken setCommanderCredits(long credits, DateTime dt )
        {
            JObject eventData = new JObject();
            eventData["commanderCredits"] = credits;
            return Event("setCommanderCredits", dt, eventData);
        }

        static public JToken setCommanderGameStatistics(JObject jsonfromstats, DateTime dt)
        {
            jsonfromstats.Remove("event");
            jsonfromstats.Remove("timestamp");
            return Event("setCommanderGameStatistics", dt, jsonfromstats);
        }

        static public JToken setCommanderRankEngineer(string name, string progress, int value, DateTime dt)
        {
            JObject eventData = new JObject();
            eventData["engineerName"] = name;
            eventData["rankStage"] = progress;
            eventData["rankValue"] = value;
            return Event("setCommanderRankEngineer", dt, eventData);
        }

        static public JToken setCommanderRankPilot(string name, int value, int progress, DateTime dt)   // progress is in journal units (0-100)
        {
            JObject eventData = new JObject();
            eventData["rankName"] = name;
            eventData["rankValue"] = value;
            if (progress != -1)
                eventData["rankProgress"] = progress / 100.0;
            return Event("setCommanderRankPilot", dt, eventData);
        }

        static public JToken setCommanderRankPower(string name, int value, DateTime dt)  
        {
            JObject eventData = new JObject();
            eventData["powerName"] = name;
            eventData["rankValue"] = value;
            return Event("setCommanderRankPower", dt, eventData);
        }

        static public JToken setCommanderReputationMajorFaction(string name, double value, DateTime dt)     // value is in journal units (0-100)
        {
            JObject eventData = new JObject();
            eventData["majorfactionName"] = name;
            eventData["majorfactionReputation"] = value / 100.0;
            return Event("setCommanderReputationMajorFaction", dt, eventData);
        }

        static public JToken setCommanderInventoryCargo(IEnumerable<Tuple<string,int>> list, DateTime dt)
        {
            JArray items = new JArray();
            if (list != null)
            {
                foreach (var x in list)
                {
                    JObject data = new JObject();
                    data["itemName"] = x.Item1;
                    data["itemCount"] = x.Item2;
                    items.Add(data);
                }
            }
            return Event("setCommanderInventoryCargo", dt, items);
        }

        static public JToken setCommanderInventoryCargoItem(string fdname, int count, bool? isstolen, DateTime dt)
        {
            JObject eventData = new JObject();
            eventData["itemName"] = fdname;
            eventData["itemCount"] = count;
            if (isstolen.HasValue)
                eventData["isStolen"] = isstolen.Value;
            return Event("setCommanderInventoryCargoItem", dt, eventData);
        }

        static public JToken setCommanderInventoryMaterials(IEnumerable<Tuple<string, int>> list, DateTime dt)
        {
            JArray items = new JArray();
            if (list != null)
            {
                foreach (var x in list)
                {
                    JObject data = new JObject();
                    data["itemName"] = x.Item1;
                    data["itemCount"] = x.Item2;
                    items.Add(data);
                }
            }
            return Event("setCommanderInventoryMaterials", dt, items);
        }

        static public JToken setCommanderInventoryMaterialsItem(string fdname, int count, DateTime dt)
        {
            JObject eventData = new JObject();
            eventData["itemName"] = fdname;
            eventData["itemCount"] = count;
            return Event("setCommanderInventoryMaterialsItem", dt, eventData);
        }

        static public JToken setCommanderInventoryItem(MaterialCommoditiesList list, string name, DateTime dt)
        {
            var item = list.Find(name);
            if (item != null)
            {
                if (item.category.Equals(MaterialCommodities.CommodityCategory))
                    return setCommanderInventoryMaterialsItem(item.fdname, item.count, dt);
                else
                    return setCommanderInventoryCargoItem(item.fdname, item.count, null, dt);
            }
            else
                return null;
            
        }

        static public JToken setCommanderStorageModules(IEnumerable<ModulesInStore.StoredModule> list, DateTime dt)
        {
            JObject eventData = new JObject();
            JArray items = new JArray();
            foreach (var x in list)
            {
                JObject data = new JObject();
                data["itemName"] = x.NameFD;
                data["itemValue"] = x.BuyPrice;
                data["isHot"] = x.Hot;
                data["starsystemName"] = x.StarSystem;
                data["marketID"] = x.MarketID;
                if (!x.EngineerModifications.IsEmpty())
                {
                    JObject eng = new JObject();
                    eng["blueprintName"] = x.EngineerModifications;
                    eng["blueprintLevel"] = x.Level;
                    eng["blueprintQuality"] = x.Quality;
                    eng["experimentalEffect"] = "";
                    data["engineering"] = eng;
                }

                items.Add(data);
            }
            return Event("setCommanderStorageModules", dt, items);
        }

        static public JToken addCommanderShip(string fdname, int id, string starsystem, string station, DateTime dt)
        {
            JObject eventData = new JObject();
            eventData["shipType"] = fdname;
            eventData["shipGameID"] = id;
            eventData["starsystemName"] = starsystem;
            eventData["stationName"] = station;
            return Event("addCommanderShip", dt, eventData);
        }

        static public JToken delCommanderShip(string fdname, int id, DateTime dt)
        {
            JObject eventData = new JObject();
            eventData["shipType"] = fdname;
            eventData["shipGameID"] = id;
            return Event("delCommanderShip", dt, eventData);
        }

        static public JToken setCommanderShip(string fdname, int id, DateTime dt,
                                              string username = null, string userid = null, bool? curship = null,
                                              bool? ishot = null,
                                              long? shipHullValue = null, long? shipModulesValue = null, long? shipRebuyCost = null,
                                              string starsystemName = null, string stationName = null, long? MarketID = null)
        {
            JObject eventData = new JObject();
            eventData["shipType"] = fdname;
            eventData["shipGameID"] = id;
            if ( username!=null )
                eventData["shipName"] = username;
            if ( userid != null )
                eventData["shipIdent"] = userid;
            if ( curship != null )
                eventData["isCurrentShip"] = curship.Value;
            if ( ishot.HasValue )
                eventData["isHot"] = ishot.Value;
            if (shipHullValue.HasValue)
                eventData["shipHullValue"] = shipHullValue.Value;
            if (shipModulesValue.HasValue)
                eventData["shipModulesValue"] = shipModulesValue.Value;
            if (shipRebuyCost.HasValue)
                eventData["shipRebuyCost"] = shipRebuyCost.Value;
            if (starsystemName != null)
                eventData["starsystemName"] = starsystemName;
            if (stationName != null)
                eventData["stationName"] = stationName;
            if (MarketID != null)
                eventData["marketID"] = MarketID;

            return Event("setCommanderShip", dt, eventData);
        }

        static public JToken setCommanderShipLoadout(string fdname, int id, IEnumerable<ShipModule> list, DateTime dt)
        {
            JArray items = new JArray();
            foreach (var x in list)
            {
                JObject data = new JObject();
                data["slotName"] = x.SlotFD;
                data["itemName"] = x.ItemFD;
                if ( x.Value.HasValue )
                    data["itemValue"] = x.Value.Value;
                if ( x.Health.HasValue)
                    data["itemHealth"] = x.Health.Value;
                if (x.Enabled.HasValue)
                    data["isOn"] = x.Enabled.Value;
                if (x.Priority.HasValue)
                    data["itemPriority"] = x.Priority.Value;
                if (x.AmmoClip.HasValue)
                    data["itemAmmoClip"] = x.AmmoClip.Value;
                if (x.AmmoHopper.HasValue)
                    data["itemAmmoHopper"] = x.AmmoHopper.Value;
                if (x.Engineering != null )
                {
                    JObject eng = new JObject();
                    eng["blueprintName"] = x.Engineering.BlueprintName ?? "";
                    eng["blueprintLevel"] = x.Engineering.Level;
                    eng["blueprintQuality"] = x.Engineering.Quality;
                    eng["experimentalEffect"] = x.Engineering.ExperimentalEffect??"";

                    if ( x.Engineering.Modifiers != null )
                    {
                        JArray mods = new JArray();

                        foreach( var y in x.Engineering.Modifiers )
                        {
                            JObject mod = new JObject();
                            eng["name"] = y.Label;
                            if (y.ValueStr != null)
                                eng["value"] = y.ValueStr;
                            else
                            {
                                eng["value"] = y.Value;
                                eng["originalValue"] = y.OriginalValue;
                                eng["lessIsGood"] = y.LessIsGood;
                            }

                            mods.Add(mod);
                        }

                        eng["modifiers"] = mods;
                    }

                    data["engineering"] = eng;
                }

                items.Add(data);
            }

            return Event("setCommanderShipLoadout", dt, items);
        }

        static public JToken setCommanderShipTransfer(string fdname, int id, string starsystem, string station, long? marketid, int transfertimesec, DateTime dt)
        {
            JObject eventData = new JObject();
            eventData["shipType"] = fdname;
            eventData["shipGameID"] = id;
            eventData["starsystemName"] = starsystem;
            eventData["stationName"] = station;
            if (marketid != null)
                eventData["marketID"] = marketid;
            if (transfertimesec > 0)
                eventData["transferTime"] = transfertimesec;
            return Event("setCommanderShipTransfer", dt, eventData);
        }

        static public JToken addCommanderTravelDock(string fdname, int id, string starsystem, string station, DateTime dt)
        {
            JObject eventData = new JObject();
            eventData["shipType"] = fdname;
            eventData["shipGameID"] = id;
            eventData["starsystemName"] = starsystem;
            eventData["stationName"] = station;
            return Event("addCommanderTravelDock", dt, eventData);
        }

        static public JToken addCommanderTravelFSDJump(string fdname, int id, string starsystem, double distance, DateTime dt)
        {
            JObject eventData = new JObject();
            eventData["shipType"] = fdname;
            eventData["shipGameID"] = id;
            eventData["starsystemName"] = starsystem;
            eventData["jumpDistance"] = distance;
            return Event("addCommanderTravelFSDJump", dt, eventData);
        }

        static public JToken addCommanderTravelLocation(string starsystem, string station, long? marketid, DateTime dt)
        {
            JObject eventData = new JObject();
            eventData["starsystemName"] = starsystem;
            if (station != null)
                eventData["stationName"] = station;
            if (marketid != null)
                eventData["marketID"] = marketid;
            return Event("addCommanderTravelLocation", dt, eventData);
        }

        static public JToken addCommanderMission(JournalMissionAccepted mission)
        {
            JObject eventData = new JObject();
            return Event("addCommanderMission", mission.EventTimeUTC, eventData);
        }

        static public JToken setCommanderMissionAbandoned(JournalMissionAbandoned mission)
        {
            JObject eventData = new JObject();
            return Event("setCommanderMissionAbandoned", mission.EventTimeUTC, eventData);
        }

        static public JToken setCommanderMissionCompleted(JournalMissionCompleted mission)
        {
            JObject eventData = new JObject();
            return Event("setCommanderMissionCompleted", mission.EventTimeUTC, eventData);
        }

        static public JToken setCommanderMissionCompleted(JournalMissionFailed mission)
        {
            JObject eventData = new JObject();
            return Event("setCommanderMissionFailed", mission.EventTimeUTC, eventData);
        }

        static public JToken addCommanderCombatDeath(string starsystem, string[] killers, bool isplayer, DateTime dt)
        {
            JObject eventData = new JObject();
            return Event("addCommanderCombatDeath", null, eventData);
        }

        static public JToken addCommanderCombatInterdicted(string starsystem, string opponent, bool isplayer, bool issubmit, DateTime dt)
        {
            JObject eventData = new JObject();
            return Event("addCommanderCombatInterdicted", null, eventData);
        }

        static public JToken addCommanderCombatInterdiction(string starsystem, string opponent, bool isplayer, bool issuccess, DateTime dt)
        {
            JObject eventData = new JObject();
            return Event("addCommanderCombatInterdiction", null, eventData);
        }

        static public JToken addCommanderCombatInterdictionEscape(string starsystem, string opponent, bool isplayer, DateTime dt)
        {
            JObject eventData = new JObject();
            return Event("addCommanderCombatInterdictionEscape", null, eventData);
        }

        static public JToken addCommanderCombatKill(string starsystem, string opponent, DateTime dt)
        {
            JObject eventData = new JObject();
            return Event("addCommanderCombatKill", null, eventData);
        }

        #endregion
    }
}
