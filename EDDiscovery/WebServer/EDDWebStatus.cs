/*
 * Copyright © 2019-2021 EDDiscovery development team
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

using QuickJSON;
using BaseUtils.WebServer;
using EliteDangerousCore;
using System;
using System.Linq;
using System.Net;

namespace EDDiscovery.WebServer
{
    public class StatusRequest : IJSONNode
    {
        private EDDiscoveryForm discoveryform;

        public StatusRequest(EDDiscoveryForm f)
        {
            discoveryform = f;
        }

        public JToken Refresh(int entry)        // -1 mean latest
        {
            return MakeResponse(entry, "status");
        }

        public JToken Push()                                    // push latest entry
        {
            return MakeResponse(-1, "status");
        }

        public JToken Response(string key, JToken message, HttpListenerRequest request)
        {
            System.Diagnostics.Debug.WriteLine("Status Request " + key + " Fields " + message.ToString());
            int entry = message["entry"].Int(0);
            return MakeResponse(entry, "status");
        }

        private JToken MakeResponse(int entry, string rt)
        {
            if (discoveryform.InvokeRequired)
            {
                return (JToken)discoveryform.Invoke(new Func<JToken>(() => MakeResponse(entry, rt)));
            }
            else
            {
                JToken response = null;

                var hl = discoveryform.history;
                if (hl.Count == 0)
                {
                    response = new JObject();
                    response["responsetype"] = rt;
                    response["entry"] = -1;
                }
                else
                {
                    if (entry < 0 || entry >= hl.Count)
                        entry = hl.Count - 1;

                    response = NewSRec(hl, rt, entry);
                }

                return response;
            }
        }

        public JToken NewSRec(EliteDangerousCore.HistoryList hl, string type, int entry)       // entry = -1 means latest
        {
            HistoryEntry he = hl.EntryOrder()[entry];

            JObject response = new JObject();
            response["responsetype"] = type;
            response["entry"] = entry;

            JObject systemdata = new JObject();
            systemdata["System"] = he.System.Name;
            systemdata["SystemAddress"] = he.System.SystemAddress;
            systemdata["PosX"] = he.System.X.ToStringInvariant("0.00");
            systemdata["PosY"] = he.System.Y.ToStringInvariant("0.00");
            systemdata["PosZ"] = he.System.Z.ToStringInvariant("0.00");
            systemdata["EDSMID"] = he.System.EDSMID.ToStringInvariant();
            systemdata["VisitCount"] = hl.GetVisitsCount(he.System.Name);
            response["SystemData"] = systemdata;

            JObject sysstate = new JObject();

            hl.ReturnSystemInfo(he, out string allegiance, out string economy, out string gov, out string faction, out string factionstate, out string security);
            sysstate["State"] = factionstate;
            sysstate["Allegiance"] = allegiance;
            sysstate["Gov"] = gov;
            sysstate["Economy"] = economy;
            sysstate["Faction"] = faction;
            sysstate["Security"] = security;
            sysstate["MarketID"] = he.MarketID;
            response["EDDB"] = sysstate;

            var mcl = hl.MaterialCommoditiesMicroResources.Get(he.MaterialCommodity);

            var counts = MaterialCommoditiesMicroResourceList.Count(mcl);
            int cargocount = counts[(int)MaterialCommodityMicroResourceType.CatType.Commodity];
            string shipname = "N/A", fuel = "N/A", range = "N/A", tanksize = "N/A";
            string cargo = cargocount.ToStringInvariant();

            ShipInformation si = he.ShipInformation;
            if (si != null)
            {
                shipname = si.ShipFullInfo(cargo: false, fuel: false);
                if (si.FuelLevel > 0)
                    fuel = si.FuelLevel.ToStringInvariant("0.#");
                if (si.FuelCapacity > 0)
                    tanksize = si.FuelCapacity.ToStringInvariant("0.#");

                EliteDangerousCalculations.FSDSpec fsd = si.GetFSDSpec();
                if (fsd != null)
                {
                    EliteDangerousCalculations.FSDSpec.JumpInfo ji = fsd.GetJumpInfo(cargocount,
                                                                si.ModuleMass() + si.HullMass(), si.FuelLevel, si.FuelCapacity / 2, he.Status.CurrentBoost);
                    range = ji.cursinglejump.ToString("N2") + "ly";
                }

                int cargocap = si.CargoCapacity();

                if (cargocap > 0)
                    cargo += "/" + cargocap.ToStringInvariant();
            }

            JObject ship = new JObject();
            ship["Ship"] = shipname;
            ship["Fuel"] = fuel;
            ship["Range"] = range;
            ship["TankSize"] = tanksize;
            ship["Cargo"] = cargo;
            ship["Data"] = counts[(int)MaterialCommodityMicroResourceType.CatType.Encoded].ToStringInvariant();
            ship["Materials"] = (counts[(int)MaterialCommodityMicroResourceType.CatType.Raw] + counts[(int)MaterialCommodityMicroResourceType.CatType.Manufactured]).ToStringInvariant();
            ship["MicroResources"] = (counts[(int)MaterialCommodityMicroResourceType.CatType.Data] + counts[(int)MaterialCommodityMicroResourceType.CatType.Component] +
                                       counts[(int)MaterialCommodityMicroResourceType.CatType.Item] + counts[(int)MaterialCommodityMicroResourceType.CatType.Consumable]).ToStringInvariant();
            response["Ship"] = ship;

            JObject travel = new JObject();

            if (he.isTravelling)
            {
                travel["Dist"] = he.TravelledDistance.ToStringInvariant("0.0");
                travel["Jumps"] = he.Travelledjumps.ToStringInvariant();
                travel["UnknownJumps"] = he.TravelledMissingjump.ToStringInvariant();
                travel["Time"] = he.TravelledSeconds.ToString();
            }
            else
                travel["Time"] = travel["Jumps"] = travel["Dist"] = "";

            response["Travel"] = travel;

            response["Bodyname"] = he.WhereAmI;

            if (he.System.HasCoordinate)         // cursystem has them?
            {
                response["HomeDist"] = he.System.Distance(EDCommander.Current.HomeSystemIOrSol).ToString("0.##");
                response["SolDist"] = he.System.Distance(0, 0, 0).ToString("0.##");
            }
            else
                response["SolDist"] = response["HomeDist"] = "-";

            response["GameMode"] = he.GameModeGroup;
            response["Credits"] = he.Credits.ToStringInvariant();
            response["Commander"] = EDCommander.Current.Name;
            response["Mode"] = he.TravelState.ToString();

            return response;
        }
    }

}



