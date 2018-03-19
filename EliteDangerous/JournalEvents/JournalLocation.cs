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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
    //When written: at startup, or when being resurrected at a station
    //Parameters:
    //•	StarSystem: name of destination starsystem
    //•	StarPos: star position, as a Json array [x, y, z], in light years
    //•	Body: star’s body name
    //•	Docked: true (if docked)
    //•	StationName: station name, (if docked)
    //•	StationType: (if docked)
    //•	Faction: star system controlling faction
    //•	FactionState
    //•	Allegiance
    //•	Economy
    //•	Government
    //•	Security
    [JournalEntryType(JournalTypeEnum.Location)]
    public class JournalLocation : JournalLocOrJump, ISystemStationEntry, IBodyNameAndID
    {
        public JournalLocation(JObject evt) : base(evt, JournalTypeEnum.Location)      // all have evidence 16/3/2017
        {
            // base class does StarSystem/StarPos/Faction/Powerplay

            Docked = evt.Value<bool?>("Docked") ?? false;
            StationName = evt["StationName"].Str();
            StationType = evt["StationType"].Str().SplitCapsWord();
            Body = evt["Body"].Str();
            BodyID = evt["BodyID"].IntNull();
            BodyType = evt["BodyType"].Str();

            Latitude = evt["Latitude"].DoubleNull();
            Longitude = evt["Longitude"].DoubleNull();

            MarketID = evt["MarketID"].LongNull();
        }

        public bool Docked { get; set; }
        public string StationName { get; set; }
        public string StationType { get; set; }
        public string Body { get; set; }
        public int? BodyID { get; set; }
        public string BodyType { get; set; }
        public string BodyDesignation { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public long? MarketID { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            if (Docked)
            {
                summary = "At " + StationName;
                info = BaseUtils.FieldBuilder.Build("Type ", StationType, "< in system ", StarSystem);
                detailed = BaseUtils.FieldBuilder.Build(";Wanted", Wanted, "Allegiance:", Allegiance, "Economy:", Economy_Localised, "Government:", Government_Localised, "Security:", Security_Localised);

                if (Factions != null)
                    foreach (FactionInformation f in Factions)
                    {
                        detailed += Environment.NewLine;
                        detailed += BaseUtils.FieldBuilder.Build("", f.Name, "State:", f.FactionState, "Gov:", f.Government, "Inf:;%", (int)(f.Influence * 100), "Allegiance:", f.Allegiance);

                        if (f.PendingStates != null)
                        {
                            detailed += BaseUtils.FieldBuilder.Build(",", "Pending State:");
                            foreach (JournalLocation.PowerStatesInfo state in f.PendingStates)
                                detailed += BaseUtils.FieldBuilder.Build(",", state.State, "", state.Trend);

                        }

                        if (f.RecoveringStates != null)
                        {
                            detailed += BaseUtils.FieldBuilder.Build(",", "Recovering State:");
                            foreach (JournalLocation.PowerStatesInfo state in f.RecoveringStates)
                                detailed += BaseUtils.FieldBuilder.Build(",", state.State, "", state.Trend);
                        }
     
                    }
            }
            else if (Latitude.HasValue && Longitude.HasValue)
            {
                summary = "Landed on " + Body;
                info = "At " + JournalFieldNaming.RLat(Latitude.Value) + " " + JournalFieldNaming.RLong(Longitude.Value);
                detailed = "";
            }
            else
            {
                summary = "At " + StarSystem;
                info = BaseUtils.FieldBuilder.Build("In space near ", Body, "< of type ", BodyType);
                detailed = "";
            }
        }
    }
}
