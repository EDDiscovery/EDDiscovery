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
    [JournalEntryType(JournalTypeEnum.EngineerContribution)]
    public class JournalEngineerContribution : JournalEntry , ILedgerJournalEntry , IMaterialCommodityJournalEntry
    {
        public JournalEngineerContribution(JObject evt ) : base(evt, JournalTypeEnum.EngineerContribution)
        {
            Engineer = evt["Engineer"].Str();
            EngineerID = evt["EngineerID"].LongNull();
            Type = evt["Type"].Str();

            if (Type.Equals("Commodity") || Type.Equals("Materials") || Type.Equals("Credits") || Type.Equals("Bond") || Type.Equals("Bounty"))
                unknownType = false;
            else
                unknownType = true;

            Commodity = evt["Commodity"].Str();
            Commodity = JournalFieldNaming.FDNameTranslation(Commodity);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyCommodity = JournalFieldNaming.RMat(Commodity);
            Commodity_Localised = JournalFieldNaming.CheckLocalisation(evt["Commodity_Localised"].Str(),FriendlyCommodity);

            Material = evt["Material"].Str();
            Material = JournalFieldNaming.FDNameTranslation(Material);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyMaterial = JournalFieldNaming.RMat(Material);
            Material_Localised = JournalFieldNaming.CheckLocalisation(evt["Material_Localised"].Str(),FriendlyMaterial);

            Quantity = evt["Quantity"].Int();
            TotalQuantity = evt["TotalQuantity"].Int();
        }

        private bool unknownType;
        public string Engineer { get; set; }
        public long? EngineerID { get; set; }
        public string Type { get; set; }

        public string FriendlyCommodity { get; set; }
        public string Commodity { get; set; }
        public string Commodity_Localised { get; set; }     // always set

        public string FriendlyMaterial { get; set; }
        public string Material { get; set; }
        public string Material_Localised { get; set; }      // always set

        public int Quantity { get; set; }
        public int TotalQuantity { get; set; }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Type.Equals("Commodity"))
                mc.Change(MaterialCommodityData.CommodityCategory, Commodity, -Quantity, 0, conn);
            else if (Type.Equals("Materials"))
                mc.Change(MaterialCommodityData.MaterialRawCategory, Material, -Quantity, 0, conn, true);
        }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            if ( Type.Equals("Credits"))
                mcl.AddEvent(Id, EventTimeUTC, EventTypeID, "Engineer Contribution Credits", -Quantity);
        }

        protected override JournalTypeEnum IconEventType { get { return unknownType ? JournalTypeEnum.EngineerContribution_Unknown : JournalTypeEnum.EngineerContribution_MatCommod; } }

        public override void FillInformation(out string info, out string detailed) 
        {
            if (unknownType==true)
                info = "Report to EDDiscovery team an unknown EngineerContribution type: " + Type;
             else
                info = BaseUtils.FieldBuilder.Build("", Engineer, "Type:".Txb(this), Type, "Commodity:".Txb(this), Commodity_Localised, 
                    "Material:".Txb(this), Material_Localised, "Quantity:".Txb(this), Quantity, "TotalQuantity:".Txb(this), TotalQuantity);
            detailed = "";
        }
    }
}
