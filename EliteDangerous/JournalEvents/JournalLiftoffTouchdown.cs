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
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.Liftoff)]
    public class JournalLiftoff : JournalEntry
    {
        public JournalLiftoff(JObject evt ) : base(evt, JournalTypeEnum.Liftoff)
        {
            Latitude = evt["Latitude"].Double();
            Longitude = evt["Longitude"].Double();
            PlayerControlled = evt["PlayerControlled"].BoolNull();
            NearestDestination = evt["NearestDestination"].StrNull();
            NearestDestination_Localised = JournalFieldNaming.CheckLocalisation(evt["NearestDestination_Localised"].StrNull(), NearestDestination);
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool? PlayerControlled { get; set; }
        public string NearestDestination { get; set; }
        public string NearestDestination_Localised { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = JournalFieldNaming.RLat(Latitude) + " " + JournalFieldNaming.RLong(Longitude) + BaseUtils.FieldBuilder.Build(", NPC Controlled;".T(EDTx.JournalEntry_NPCControlled), PlayerControlled, ", " + "Nearest:".T(EDTx.JournalEntry_Nearest), NearestDestination_Localised);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.Touchdown)]
    public class JournalTouchdown : JournalEntry
    {
        public JournalTouchdown(JObject evt) : base(evt, JournalTypeEnum.Touchdown)
        {
            Latitude = evt["Latitude"].Double();
            Longitude = evt["Longitude"].Double();
            PlayerControlled = evt["PlayerControlled"].BoolNull();
            NearestDestination = evt["NearestDestination"].StrNull();
            NearestDestination_Localised = JournalFieldNaming.CheckLocalisation(evt["NearestDestination_Localised"].StrNull(), NearestDestination);
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool? PlayerControlled { get; set; }
        public string NearestDestination { get; set; }
        public string NearestDestination_Localised { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = JournalFieldNaming.RLat(Latitude) + " " + JournalFieldNaming.RLong(Longitude) + BaseUtils.FieldBuilder.Build(", NPC Controlled;".T(EDTx.JournalEntry_NPCControlled), PlayerControlled, ", " + "Nearest:".T(EDTx.JournalEntry_Nearest), NearestDestination_Localised);
            detailed = "";
        }
    }

}
