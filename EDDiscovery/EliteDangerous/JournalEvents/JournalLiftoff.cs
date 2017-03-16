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
//    When written: when taking off from planet surface
//Parameters:
//•	Latitude(only if player flying in ship)
//•	Longitude(only if player flying in ship)
//•	PlayerControlled: (bool) false if ship dismissed when player is in SRV, true if player is taking off


    [JournalEntryType(JournalTypeEnum.Liftoff)]
    public class JournalLiftoff : JournalEntry
    {
        public JournalLiftoff(JObject evt ) : base(evt, JournalTypeEnum.Liftoff)
        {
            Latitude = evt["Latitude"].Double();
            Longitude = evt["Longitude"].Double();
            PlayerControlled = evt["PlayerControlled"].BoolNull();
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool? PlayerControlled { get; set; }
        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.liftoff; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = JournalFieldNaming.RLat(Latitude) + " " + JournalFieldNaming.RLong(Longitude) + Tools.FieldBuilder(", NPC Controlled;", PlayerControlled);
            detailed = "";
        }
    }
}
