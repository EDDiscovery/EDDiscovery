﻿/*
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
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when landing at landing pad in a space station, outpost, or surface settlement
    //Parameters:
    //•	StationName: name of station
    //•	StationType: type of station
    //•	StarSystem: name of system
    //•	CockpitBreach:true (only if landing with breached cockpit)
    //•	Faction: station’s controlling faction
    //•	FactionState
    //•	Allegiance
    //•	Economy
    //•	Government
    //•	Security
    [JournalEntryType(JournalTypeEnum.Docked)]
    public class JournalDocked : JournalEntry
    {


        public JournalDocked(JObject evt ) : base(evt, JournalTypeEnum.Docked)
        {
            StationName = JSONHelper.GetStringDef(evt["StationName"]);
            StationType = JSONHelper.GetStringDef(evt["StationType"]);
            StarSystem = JSONHelper.GetStringDef(evt["StarSystem"]);
            CockpitBreach = JSONHelper.GetBool(evt["CockpitBreach"]);

            Faction = JSONHelper.GetMultiStringDef(evt, new string[] { "StationFaction", "Faction" });
            FactionState = JSONHelper.GetStringDef(evt["FactionState"]);

            Allegiance = JSONHelper.GetMultiStringDef(evt, new string[] { "StationAllegiance", "Allegiance" });
            Economy = JSONHelper.GetMultiStringDef(evt, new string[] { "StationEconomy", "Economy" });
            Economy_Localised = JSONHelper.GetMultiStringDef(evt, new string[] { "StationEconomy_Localised", "Economy_Localised" });
            Government = JSONHelper.GetMultiStringDef(evt, new string[] { "StationGovernment", "Government" });
            Government_Localised = JSONHelper.GetMultiStringDef(evt, new string[] { "StationGovernment_Localised", "Government_Localised" });

            //Security = JSONHelper.GetMultiStringDef(evt["Security"]);
            //Security_Localised = JSONHelper.GetMultiStringDef(evt["Security_Localised"]);
        }

        public string StationName { get; set; }
        public string StationType { get; set; }
        public string StarSystem { get; set; }
        public bool CockpitBreach { get; set; }
        public string Faction { get; set; }
        public string FactionState { get; set; }
        public string Allegiance { get; set; }
        public string Economy { get; set; }
        public string Economy_Localised { get; set; }
        public string Government { get; set; }
        public string Government_Localised { get; set; }
        //public string Security { get; set; }
        //public string Security_Localised { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.Stationenter; } }

        public override void FillInformation(out string summary, out string info, out string detailed)
        {
            base.FillInformation(out summary, out info, out detailed);
            summary = $"Docked at {StationName}";
        }

    }
}
