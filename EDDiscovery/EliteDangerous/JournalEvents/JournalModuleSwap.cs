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
    //When Written: when moving a module to a different slot on the ship
    //Parameters:
    //•	FromSlot
    //•	ToSlot
    //•	FromItem
    //•	ToItem
    //•	Ship
    [JournalEntryType(JournalTypeEnum.ModuleSwap)]
    public class JournalModuleSwap : JournalEntry, IShipInformation
    {
        public JournalModuleSwap(JObject evt ) : base(evt, JournalTypeEnum.ModuleSwap)
        {
            FromSlot = JournalFieldNaming.GetBetterSlotName(evt["FromSlot"].Str());
            ToSlot = JournalFieldNaming.GetBetterSlotName(evt["ToSlot"].Str());
            FromItem = JournalFieldNaming.GetBetterItemNameEvents(evt["FromItem"].Str());
            FromItemLocalised = evt["FromItem_Localised"].Str();
            ToItem = JournalFieldNaming.GetBetterItemNameEvents(evt["ToItem"].Str());
            ToItemLocalised = evt["ToItem_Localised"].Str();
            Ship = JournalFieldNaming.GetBetterShipName(evt["Ship"].Str());
            ShipId = evt["ShipID"].Int();

        }
        public string FromSlot { get; set; }
        public string ToSlot { get; set; }
        public string FromItem { get; set; }
        public string FromItemLocalised { get; set; }
        public string ToItem { get; set; }
        public string ToItemLocalised { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.moduleswap; } }

        public void ShipInformation(ShipInformationList shp, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleSwap(this);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("From ", FromSlot , "<to " , ToSlot , "Item:" , FromItemLocalised.Alt(FromItem));
            if (ToItem.Length > 0 && !ToItem.Equals("Null"))            // Null if a frontier thing in 2.3 beta
                info += ", Swapped with " + ToItemLocalised.Alt(ToItem);
            detailed = "";
        }
    }
}
