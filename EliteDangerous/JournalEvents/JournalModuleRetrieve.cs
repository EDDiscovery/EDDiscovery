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
    [JournalEntryType(JournalTypeEnum.ModuleRetrieve)]
    public class JournalModuleRetrieve : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalModuleRetrieve(JObject evt) : base(evt, JournalTypeEnum.ModuleRetrieve)
        {
            SlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["Slot"].Str());
            Slot = JournalFieldNaming.GetBetterSlotName(SlotFD);

            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipId = evt["ShipID"].Int();

            RetrievedItemFD = JournalFieldNaming.NormaliseFDItemName(evt["RetrievedItem"].Str());
            RetrievedItem = JournalFieldNaming.GetBetterItemName(RetrievedItemFD);
            RetrievedItemLocalised = JournalFieldNaming.CheckLocalisation(evt["RetrievedItem_Localised"].Str(),RetrievedItem);

            EngineerModifications = evt["EngineerModifications"].Str().SplitCapsWordFull();

            SwapOutItemFD = JournalFieldNaming.NormaliseFDItemName(evt["SwapOutItem"].Str());
            SwapOutItem = JournalFieldNaming.GetBetterItemName(SwapOutItemFD);
            SwapOutItemLocalised = JournalFieldNaming.CheckLocalisation(evt["SwapOutItem_Localised"].Str(),SwapOutItem);

            Cost = evt["Cost"].Long();

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
        public string RetrievedItem { get; set; }
        public string RetrievedItemFD { get; set; }
        public string RetrievedItemLocalised { get; set; }
        public string EngineerModifications { get; set; }
        public string SwapOutItem { get; set; }
        public string SwapOutItemFD { get; set; }
        public string SwapOutItemLocalised { get; set; }
        public long Cost { get; set; }
        public double? Quality { get; set; }
        public int? Level { get; set; }
        public bool? Hot { get; set; }
        public long? MarketID { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (RetrievedItemLocalised.Length > 0) ? RetrievedItemLocalised : RetrievedItem;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s + " on " + Ship, -Cost);
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleRetrieve(this);
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            
            info = BaseUtils.FieldBuilder.Build("", RetrievedItemLocalised, "< into ".Txb(this), Slot ,";(Hot)".Txb(this), Hot );
            if ( Cost>0)
                info += " " + BaseUtils.FieldBuilder.Build("Cost:; cr;N0".Txb(this), Cost);

            if (SwapOutItem.Length > 0)
                info += ", " + BaseUtils.FieldBuilder.Build("Stored:".Txb(this), SwapOutItemLocalised);
            detailed = "";
        }

    }
}
