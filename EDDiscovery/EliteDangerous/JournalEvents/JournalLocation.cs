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

namespace EDDiscovery.EliteDangerous.JournalEvents
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
    public class JournalLocation : JournalLocOrJump
    {
        public JournalLocation(JObject evt ) : base(evt, JournalTypeEnum.Location)      // all have evidence 16/3/2017
        {
            Docked = evt.Value<bool?>("Docked") ?? false;
            StationName = evt["StationName"].Str();
            StationType = evt["StationType"].Str();
            Body = evt["Body"].Str();
            BodyType = evt["BodyType"].Str();
            Faction = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemFaction", "Faction" });

            Allegiance = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemAllegiance", "Allegiance"});
            Economy = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemEconomy", "Economy" });
            Economy_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemEconomy_Localised", "Economy_Localised" });
            Government = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemGovernment", "Government" });
            Government_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemGovernment_Localised", "Government_Localised" });
            Security = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemSecurity", "Security" });
            Security_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemSecurity_Localised", "Security_Localised" });

            Latitude = evt["Latitude"].DoubleNull();
            Longitude = evt["Longitude"].DoubleNull();

            Factions = evt["Factions"]?.ToObject<FactionInfo[]>();

            PowerplayState = evt["PowerplayState"].Str();            // NO evidence
            if (!evt["Powers"].Empty())
                Powers = evt.Value<JArray>("Powers").Values<string>().ToArray();
        }

        public bool Docked { get; set; }
        public string StationName { get; set; }
        public string StationType { get; set; }
        public string Body { get; set; }
        public string BodyType { get; set; }
        public string Faction { get; set; }

        public string Allegiance { get; set; }
        public string Economy { get; set; }
        public string Economy_Localised { get; set; }
        public string Government { get; set; }
        public string Government_Localised { get; set; }
        public string Security { get; set; }
        public string Security_Localised { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string PowerplayState { get; set; }
        public string[] Powers { get; set; }

        public FactionInfo[] Factions { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            if (Docked)
            {
                summary = "At " + StationName;
                info = Tools.FieldBuilder("Type ", StationType, "<in system ", StarSystem);
                detailed = Tools.FieldBuilder("Allegiance:", Allegiance, "Economy:", Economy_Localised.Alt(Economy), "Government:", Government_Localised.Alt(Government) , "Security:" , Security_Localised.Alt(Security));
                
                if ( Factions != null )
                    foreach( FactionInfo f in Factions)
                    {
                        detailed += Environment.NewLine;
                        detailed += Tools.FieldBuilder("", f.Name, "State:" , f.FactionState, "Gov:" , f.Government , "Inf:;%" , (int)(f.Influence*100), "Allegiance:" , f.Allegiance);
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
                info = Tools.FieldBuilder("In space near ", Body, "<of type ", BodyType);
                detailed = "";
            }
        }

            

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.location; } }

        public class FactionInfo
        {
            public string Name { get; set; }
            public string FactionState { get; set; }
            public string Government { get; set; }
            public double Influence { get; set; }
            public string Allegiance { get; set; }
        }


    }
}
