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
    [JournalEntryType(JournalTypeEnum.ModuleStore)]
    public class JournalModuleStore : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalModuleStore(JObject evt) : base(evt, JournalTypeEnum.ModuleStore)
        {
            SlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["Slot"].Str());
            Slot = JournalFieldNaming.GetBetterSlotName(SlotFD);

            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipId = evt["ShipID"].Int();

            StoredItemFD = JournalFieldNaming.NormaliseFDItemName(evt["StoredItem"].Str());
            StoredItem = JournalFieldNaming.GetBetterItemName(StoredItemFD);
            StoredItemLocalised = JournalFieldNaming.CheckLocalisation(evt["StoredItem_Localised"].Str(),StoredItem);

            EngineerModifications = evt["EngineerModifications"].StrNull().SplitCapsWordFull();

            ReplacementItemFD = JournalFieldNaming.NormaliseFDItemName(evt["ReplacementItem"].Str());
            ReplacementItem = JournalFieldNaming.GetBetterItemName(ReplacementItemFD);
            ReplacementItemLocalised = JournalFieldNaming.CheckLocalisation(evt["ReplacementItem_Localised"].Str(),ReplacementItem);

            Cost = evt["Cost"].LongNull();

            Hot = evt["Hot"].BoolNull();
            Level = evt["Level"].IntNull();
            Quality = evt["Quality"].DoubleNull();

            MarketID = evt["MarketID"].LongNull();
        }

        public string Slot { get; set; }
        public string SlotFD { get; set; }
        public string Ship { get; set; }
        public string ShipFD { get; set; }
        public int ShipId { get; set; }
        public string StoredItem { get; set; }
        public string StoredItemFD { get; set; }
        public string StoredItemLocalised { get; set; }
        public string EngineerModifications { get; set; }
        public string ReplacementItem { get; set; }
        public string ReplacementItemFD { get; set; }
        public string ReplacementItemLocalised { get; set; }
        public long? Cost { get; set; }
        public double? Quality { get; set; }
        public int? Level { get; set; }
        public bool? Hot { get; set; }
        public long? MarketID { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (StoredItemLocalised.Length > 0) ? StoredItemLocalised : StoredItem;
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s +" on ".Txb(this) + Ship, -Cost);
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleStore(this);
        }

        public override void FillInformation(out string info, out string detailed)  
        {
            
            info = BaseUtils.FieldBuilder.Build("", StoredItemLocalised, "< from ".Txb(this), Slot , ";(Hot)".Txb(this), Hot, "Cost:; cr;N0".Txb(this), Cost);
            if (ReplacementItem.Length > 0)
                info = ", " + BaseUtils.FieldBuilder.Build("Replaced by:".Txb(this), ReplacementItemLocalised);
            detailed = BaseUtils.FieldBuilder.Build("Modifications:".Txb(this), EngineerModifications);
        }
    }
}
