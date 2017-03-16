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
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    [JournalEntryType(JournalTypeEnum.MaterialDiscovered)]
    public class JournalMaterialDiscovered : JournalEntry
    {
        public JournalMaterialDiscovered(JObject evt ) : base(evt, JournalTypeEnum.MaterialDiscovered)
        {
            Category = evt["Category"].Str();
            Name = evt["Name"].Str();
            DiscoveryNumber = evt["DiscoveryNumber"].Int();
            FriendlyName = JournalFieldNaming.RMat(Name);
        }

        public string Category { get; set; }
        public string Name { get; set; }
        public int DiscoveryNumber { get; set; }
        public string FriendlyName { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.materialdiscovered; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("", FriendlyName );
            detailed = "";
        }
    }
}
