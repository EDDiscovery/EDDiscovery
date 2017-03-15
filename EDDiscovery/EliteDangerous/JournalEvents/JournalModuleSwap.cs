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
            FromSlot = JournalFieldNaming.GetBetterSlotName(JSONHelper.GetStringDef(evt["FromSlot"]));
            ToSlot = JournalFieldNaming.GetBetterSlotName(JSONHelper.GetStringDef(evt["ToSlot"]));
            FromItem = JournalFieldNaming.GetBetterItemNameEvents(JSONHelper.GetStringDef(evt["FromItem"]));
            FromItemLocalised = JSONHelper.GetStringDef(evt["FromItem_Localised"]);
            ToItem = JournalFieldNaming.GetBetterItemNameEvents(JSONHelper.GetStringDef(evt["ToItem"]));
            ToItemLocalised = JSONHelper.GetStringDef(evt["ToItem_Localised"]);
            Ship = JournalFieldNaming.GetBetterShipName(JSONHelper.GetStringDef(evt["Ship"]));
            ShipId = JSONHelper.GetInt(evt["ShipID"]);

        }
        public string FromSlot { get; set; }
        public string ToSlot { get; set; }
        public string FromItem { get; set; }
        public string FromItemLocalised { get; set; }
        public string ToItem { get; set; }
        public string ToItemLocalised { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.moduleswap; } }

        public void ShipInformation(ShipInformationList shp, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleSwap(this);
        }

    }
}
