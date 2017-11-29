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

namespace EliteDangerousCore.JournalEvents
{
//    When written: landing on a planet surface
//    Parameters:
//•	Latitude(only if player is landing)
//•	Longitude(only if player is landing)
//•	PlayerControlled: (bool) false if ship was recalled from SRV, true if player is landing

    [JournalEntryType(JournalTypeEnum.Touchdown)]
    public class JournalTouchdown : JournalEntry
    {
        public JournalTouchdown(JObject evt ) : base(evt, JournalTypeEnum.Touchdown)
        {
            Latitude = evt["Latitude"].Double();
            Longitude = evt["Longitude"].Double();
            PlayerControlled = evt["PlayerControlled"].BoolNull();
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool? PlayerControlled { get; set; }


        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = JournalFieldNaming.RLat(Latitude) + " " + JournalFieldNaming.RLong(Longitude) + BaseUtils.FieldBuilder.Build(", NPC Controlled;", PlayerControlled);
            detailed = "";
        }
    }
}
