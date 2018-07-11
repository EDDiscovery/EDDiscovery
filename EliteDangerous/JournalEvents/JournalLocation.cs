/*
 * Copyright © 2016-2018 EDDiscovery development team
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

        public override string FillSummary { get
            {
                if (Docked)
                    return string.Format("At {0}".Tx(this, "AtStat"), StationName);
                else if (Latitude.HasValue && Longitude.HasValue)
                    return string.Format("Landed on {0}".Tx(this, "LND"), Body);
                else
                    return string.Format("At {0}".Tx(this, "AtStar"), StarSystem);
            } }

        public override void FillInformation(out string info, out string detailed) 
        {
            if (Docked)
            {
                info = BaseUtils.FieldBuilder.Build("Type ".Txb(this), StationType, "< in system ".Txb(this), StarSystem);
                detailed = BaseUtils.FieldBuilder.Build("<;(Wanted) ".Txb(this), Wanted, "Allegiance:".Txb(this), Allegiance, "Economy:".Txb(this), Economy_Localised, "Government:".Txb(this), Government_Localised, "Security:".Txb(this), Security_Localised);

                if (Factions != null)
                    foreach (FactionInformation f in Factions)
                    {
                        detailed += Environment.NewLine;
                        detailed += BaseUtils.FieldBuilder.Build("", f.Name, "State:".Txb(this), f.FactionState, "Government:".Txb(this), f.Government, "Inf:;%".Txb(this), (int)(f.Influence * 100), "Allegiance:".Txb(this), f.Allegiance);

                        if (f.PendingStates != null)
                        {
                            detailed += BaseUtils.FieldBuilder.Build(",", "Pending State:".Txb(this));
                            foreach (JournalLocation.PowerStatesInfo state in f.PendingStates)
                                detailed += BaseUtils.FieldBuilder.Build(",", state.State, "", state.Trend);

                        }

                        if (f.RecoveringStates != null)
                        {
                            detailed += BaseUtils.FieldBuilder.Build(",", "Recovering State:".Txb(this));
                            foreach (JournalLocation.PowerStatesInfo state in f.RecoveringStates)
                                detailed += BaseUtils.FieldBuilder.Build(",", state.State, "", state.Trend);
                        }
     
                    }
            }
            else if (Latitude.HasValue && Longitude.HasValue)
            {
                info = "At " + JournalFieldNaming.RLat(Latitude.Value) + " " + JournalFieldNaming.RLong(Longitude.Value);
                detailed = "";
            }
            else
            {
                info = BaseUtils.FieldBuilder.Build("In space near ".Txb(this), Body, "< of type ".Txb(this), BodyType);
                detailed = "";
            }
        }
    }
}
