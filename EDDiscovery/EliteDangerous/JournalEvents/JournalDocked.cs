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
            StationName = evt["StationName"].Str();
            StationType = evt["StationType"].Str();
            StarSystem = evt["StarSystem"].Str();
            CockpitBreach = evt["CockpitBreach"].Bool();

            Faction = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "StationFaction", "Faction" });
            FactionState = evt["FactionState"].Str().SplitCapsWord();

            Allegiance = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "StationAllegiance", "Allegiance" });
            Economy = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "StationEconomy", "Economy" });
            Economy_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "StationEconomy_Localised", "Economy_Localised" });
            Government = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "StationGovernment", "Government" });
            Government_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "StationGovernment_Localised", "Government_Localised" });
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

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.Stationenter; } }


        public override void FillInformation(out string summary, out string info, out string detailed)      //V
        {
            summary = $"At {StationName}";
            info = Tools.FieldBuilder("Type ", StationType, "<in system ", StarSystem, "Faction:", Faction, "<in state ", FactionState);
            detailed = Tools.FieldBuilder("Allegiance:", Allegiance, "Economy:", Economy_Localised.Alt(Economy), "Government:", Government_Localised.Alt(Government));
        }

    }
}
