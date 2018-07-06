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
using EliteDangerousCore.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.FSDJump)]
    public class JournalFSDJump : JournalLocOrJump, IShipInformation, ISystemStationEntry
    {
        public JournalFSDJump(JObject evt) : base(evt, JournalTypeEnum.FSDJump)
        {
            RealJournalEvent = evt["FuelUsed"].Empty(); // Old pre ED 2.2 messages has no Fuel used fields

            // base class does StarSystem/StarPos/Faction/Powerplay

            JumpDist = evt["JumpDist"].Double();
            FuelUsed = evt["FuelUsed"].Double();
            FuelLevel = evt["FuelLevel"].Double();
            BoostUsed = evt["BoostUsed"].Bool();
            BoostValue = evt["BoostUsed"].Int();

            JToken jm = evt["EDDMapColor"];
            MapColor = jm.Int(EliteDangerousCore.EliteConfigInstance.InstanceConfig.DefaultMapColour);
            if (jm.Empty())
                evt["EDDMapColor"] = EliteDangerousCore.EliteConfigInstance.InstanceConfig.DefaultMapColour;      // new entries get this default map colour if its not already there

            EDSMFirstDiscover = evt["EDD_EDSMFirstDiscover"].Bool(false);
        }

        public JournalFSDJump(DateTime utc, ISystem sys, int colour, bool first, int synced) : base(utc, sys, synced, JournalTypeEnum.FSDJump)
        {
            MapColor = colour;
            EDSMFirstDiscover = first;
        }

        public double JumpDist { get; set; }
        public double FuelUsed { get; set; }
        public double FuelLevel { get; set; }
        public bool BoostUsed { get; set; }
        public int BoostValue { get; set; }
        public int MapColor { get; set; }
        public bool RealJournalEvent { get; private set; } // True if real ED 2.2+ journal event and not pre 2.2 imported.
        public bool EDSMFirstDiscover { get; set; }

        public override string FillSummary { get { return "Jump to " + StarSystem; } }

        public override void FillInformation(out string info, out string detailed)  //V
        {
            StringBuilder sb = new StringBuilder();
            if (JumpDist > 0)
                sb.Append(JumpDist.ToString("0.00") + " ly");
            if (FuelUsed > 0)
                sb.Append(", Fuel " + FuelUsed.ToString("0.0") + "t");
            if (FuelLevel > 0)
                sb.Append(" left " + FuelLevel.ToString("0.0") + "t");

            string econ = Economy_Localised.Alt(Economy);
            if (econ.Equals("None"))
                econ = "";

            sb.Append(" ");
            sb.Append(BaseUtils.FieldBuilder.Build("Faction:", Faction, "<;(Wanted) ", Wanted, "State:", FactionState, "Allegiance:", Allegiance, "Economy:", econ, "Population:", Population));
            info = sb.ToString();

            sb.Clear();

            if ( Factions != null )
            {
                foreach (FactionInformation i in Factions)
                {
                    sb.Append(BaseUtils.FieldBuilder.Build("", i.Name, "State:", i.FactionState, "Gov:", i.Government, "Inf:;%", (i.Influence * 100.0).ToString("0.0"), "Alg:", i.Allegiance));
                    if (i.PendingStates != null)
                    {
                        sb.Append(BaseUtils.FieldBuilder.Build(",", "Pending State:"));
                        foreach (JournalLocation.PowerStatesInfo state in i.PendingStates)
                            sb.Append(BaseUtils.FieldBuilder.Build(",", state.State, "", state.Trend));

                    }

                    if (i.RecoveringStates != null)
                    {
                        sb.Append(BaseUtils.FieldBuilder.Build(",", "Recovering State:"));
                        foreach (JournalLocation.PowerStatesInfo state in i.RecoveringStates)
                            sb.Append(BaseUtils.FieldBuilder.Build(",", state.State, "", state.Trend));
                    }
                    sb.Append(Environment.NewLine);

                }
            }

            detailed = sb.ToString();
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.FSDJump(this);
        }

        public void UpdateMapColour(int mapcolour)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                JObject jo = GetJson(Id, cn);

                if (jo != null)
                {
                    jo["EDDMapColor"] = mapcolour;
                    UpdateJsonEntry(jo, cn);
                    MapColor = mapcolour;
                }
            }
        }

        public void UpdateFirstDiscover(bool value, SQLiteConnectionUser cn = null, DbTransaction txnl = null)
        {
            JObject jo = cn == null ? GetJson(Id) : GetJson(Id,cn,txnl);

            if (jo != null)
            {
                jo["EDD_EDSMFirstDiscover"] = value;
                UpdateJsonEntry(jo, cn, txnl);
                EDSMFirstDiscover = value;
            }
        }

        public JObject CreateFSDJournalEntryJson()          // minimal version, not the whole schebang
        {
            JObject jo = new JObject();
            jo["timestamp"] = EventTimeUTC.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
            jo["event"] = "FSDJump";
            jo["StarSystem"] = StarSystem;
            jo["StarPos"] = new JArray(StarPos.X, StarPos.Y, StarPos.Z);
            jo["EDDMapColor"] = MapColor;
            jo["EDD_EDSMFirstDiscover"] = EDSMFirstDiscover;
            return jo;
        }

    }
}
