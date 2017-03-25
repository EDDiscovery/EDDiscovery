﻿/*
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
//    When written: when storing a module in Outfitting
//    Parameters:
//•	Slot
//•	Ship
//•	ShipID
//•	StoredItem
//•	EngineerModifications: name of modification blueprint, if any
//•	ReplacementItem(if a core module)
//•	Cost(if any)
    [JournalEntryType(JournalTypeEnum.ModuleStore)]
    public class JournalModuleStore : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalModuleStore(JObject evt) : base(evt, JournalTypeEnum.ModuleStore)
        {
            Slot = JournalFieldNaming.GetBetterSlotName(evt["Slot"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(evt["Ship"].Str());
            ShipId = evt["ShipID"].Int();
            StoredItem = JournalFieldNaming.GetBetterItemNameEvents(evt["StoredItem"].Str());
            StoredItemLocalised = evt["StoredItem_Localised"].Str();
            EngineerModifications = evt["EngineerModifications"].StrNull().SplitCapsWordFull();
            ReplacementItem = JournalFieldNaming.GetBetterItemNameEvents(evt["ReplacementItem"].Str());
            ReplacementItemLocalised = evt["ReplacementItem_Localised"].Str();
            Cost = evt["Cost"].LongNull();
        }
        public string Slot { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public string StoredItem { get; set; }
        public string StoredItemLocalised { get; set; }
        public string EngineerModifications { get; set; }
        public string ReplacementItem { get; set; }
        public string ReplacementItemLocalised { get; set; }
        public long? Cost { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.modulestore; } }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (StoredItemLocalised.Length > 0) ? StoredItemLocalised : StoredItem;
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s +" on " + Ship, -Cost);
        }

        public void ShipInformation(ShipInformationList shp, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleStore(this);
        }

        public override void FillInformation(out string summary, out string info, out string detailed)  //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("", StoredItemLocalised.Alt(StoredItem), "<from ", Slot , "Cost:" , Cost);
            if (ReplacementItem.Length > 0)
                info = ", " + Tools.FieldBuilder("Replaced by:", ReplacementItemLocalised.Alt(ReplacementItem));
            detailed = Tools.FieldBuilder("Modifications:", EngineerModifications);
        }
    }
}
