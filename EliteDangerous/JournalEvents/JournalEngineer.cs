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
using System.Collections.Generic;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.EngineerApply)]
    public class JournalEngineerApply : JournalEntry
    {
        public JournalEngineerApply(JObject evt ) : base(evt, JournalTypeEnum.EngineerApply)
        {
            Engineer = evt["Engineer"].Str();
            Blueprint = evt["Blueprint"].Str().SplitCapsWordFull();
            Level = evt["Level"].Int();
            Override = evt["Override"].Str();
        }

        public string Engineer { get; set; }
        public string Blueprint { get; set; }
        public int Level { get; set; }
        public string Override { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            
            info = BaseUtils.FieldBuilder.Build("", Engineer, "Blueprint:".Txb(this), Blueprint, "Level:".Txb(this), Level, "Override:".Txb(this), Override);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.EngineerContribution)]
    public class JournalEngineerContribution : JournalEntry, ILedgerJournalEntry, ICommodityJournalEntry, IMaterialJournalEntry
    {
        public JournalEngineerContribution(JObject evt) : base(evt, JournalTypeEnum.EngineerContribution)
        {
            Engineer = evt["Engineer"].Str();
            EngineerID = evt["EngineerID"].LongNull();
            Type = evt["Type"].Str();

            Commodity = evt["Commodity"].Str();
            Commodity = JournalFieldNaming.FDNameTranslation(Commodity);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyCommodity = MaterialCommodityData.GetNameByFDName(Commodity);
            Commodity_Localised = JournalFieldNaming.CheckLocalisationTranslation(evt["Commodity_Localised"].Str(), FriendlyCommodity);

            Material = evt["Material"].Str();
            Material = JournalFieldNaming.FDNameTranslation(Material);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyMaterial = MaterialCommodityData.GetNameByFDName(Material);
            Material_Localised = JournalFieldNaming.CheckLocalisationTranslation(evt["Material_Localised"].Str(), FriendlyMaterial);

            Quantity = evt["Quantity"].Int();
            TotalQuantity = evt["TotalQuantity"].Int();
        }

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

        public void UpdateMaterials(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Type.Equals("Materials"))
                mc.Change(MaterialCommodityData.MaterialRawCategory, Material, -Quantity, 0, conn, true);
        }

        public void UpdateCommodities(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Type.Equals("Commodity"))
                mc.Change(MaterialCommodityData.CommodityCategory, Commodity, -Quantity, 0, conn);
        }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            if (Type.Equals("Credits"))
                mcl.AddEvent(Id, EventTimeUTC, EventTypeID, "Engineer Contribution Credits", -Quantity);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Engineer, "Type:".Txb(this), Type, "Commodity:".Txb(this), Commodity_Localised,
                    "Material:".Txb(this), Material_Localised, "Quantity:".Txb(this), Quantity, "TotalQuantity:".Txb(this), TotalQuantity);
            detailed = "";
        }
    }



    // Base class used for craft and legacy

    public class JournalEngineerCraftBase : JournalEntry, IMaterialJournalEntry, IShipInformation
    {
        public JournalEngineerCraftBase(JObject evt, JournalTypeEnum en) : base(evt, en)
        {
            SlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["Slot"].Str());
            Slot = JournalFieldNaming.GetBetterSlotName(SlotFD);

            ModuleFD = JournalFieldNaming.NormaliseFDItemName(evt["Module"].Str());
            Module = JournalFieldNaming.GetBetterItemName(ModuleFD);

            Engineering = new ShipModule.EngineeringData(evt);

            IsPreview = evt["IsPreview"].BoolNull();
            JToken mats = (JToken)evt["Ingredients"];

            if (mats != null)
            {
                Ingredients = new Dictionary<string, int>();

                if (mats.Type == JTokenType.Object)
                {
                    Dictionary<string, int> temp = mats?.ToObjectProtected<Dictionary<string, int>>();

                    if (temp != null)
                    {
                        foreach (string key in temp.Keys)
                            Ingredients[JournalFieldNaming.FDNameTranslation(key)] = temp[key];
                    }
                }
                else
                {
                    foreach (JObject jo in (JArray)mats)
                    {
                        Ingredients[JournalFieldNaming.FDNameTranslation((string)jo["Name"])] = jo["Count"].Int();
                    }
                }
            }

        }

        public string Slot { get; set; }
        public string SlotFD { get; set; }
        public string Module { get; set; }
        public string ModuleFD { get; set; }

        public ShipModule.EngineeringData Engineering { get; set; }

        public bool? IsPreview { get; set; }            // Only for legacy convert

        public Dictionary<string, int> Ingredients { get; set; }        // not for legacy convert

        public void UpdateMaterials(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Ingredients != null)
            {
                foreach (KeyValuePair<string, int> k in Ingredients)        // may be commodities or materials but mostly materials so we put it under this
                    mc.Craft(k.Key, k.Value);
            }
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            if ((IsPreview == null || IsPreview.Value == false) && Engineering != null)
            {
                shp.EngineerCraft(this);
            }
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("In Slot:".Txb(this), Slot, "", Module, "By:".Txb(this), Engineering.Engineer, "Blueprint:".Txb(this), Engineering.FriendlyBlueprintName, "Level:".Txb(this), Engineering.Level);

            detailed = "";
            if (Ingredients != null)
            {
                foreach (KeyValuePair<string, int> k in Ingredients)        // may be commodities or materials
                    detailed += BaseUtils.FieldBuilder.Build("", MaterialCommodityData.GetNameByFDName(k.Key), "", k.Value) + "; ";
            }

            if (Engineering != null)
                detailed = detailed.AppendPrePad( Engineering.ToString() , System.Environment.NewLine);
        }
    }

    [JournalEntryType(JournalTypeEnum.EngineerCraft)]
    public class JournalEngineerCraft : JournalEngineerCraftBase
    {
        public JournalEngineerCraft(JObject evt) : base(evt, JournalTypeEnum.EngineerCraft)
        {
        }
    }


    [JournalEntryType(JournalTypeEnum.EngineerLegacyConvert)]
    public class JournalLegacyConvert : JournalEngineerCraftBase
    {
        public JournalLegacyConvert(JObject evt) : base(evt, JournalTypeEnum.EngineerLegacyConvert)     // same as craft.
        {
        }
    }


    [JournalEntryType(JournalTypeEnum.EngineerProgress)]
    public class JournalEngineerProgress : JournalEntry
    {
        public class ProgressInformation
        {
            public string Engineer { get; set; }
            public long EngineerID { get; set; }
            public int? Rank { get; set; }       // only when unlocked
            public string Progress { get; set; }
            public int? RankProgress { get; set; }  // newish 3.x only when unlocked
        }

        public JournalEngineerProgress(JObject evt) : base(evt, JournalTypeEnum.EngineerProgress)
        {
            Engineers = evt["Engineers"]?.ToObjectProtected<ProgressInformation[]>().OrderBy(x => x.Engineer)?.ToArray();       // 3.3 introduced this at startup

            if (Engineers == null)
            {
                Engineers = new ProgressInformation[1];
                Engineers[0] = new ProgressInformation();
                Engineers[0].Engineer = evt["Engineer"].Str();
                Engineers[0].EngineerID = evt["EngineerID"].Long();
                Engineers[0].Rank = evt["Rank"].IntNull();
                Engineers[0].Progress = evt["Progress"].Str();
                Engineers[0].RankProgress = evt["RankProgress"].IntNull();
            }
        }

        public ProgressInformation[] Engineers { get; set; }      // may be NULL if not startup or pre 3.3

        public override void FillInformation(out string info, out string detailed)
        {
            string enginfo = "";
            foreach (var p in Engineers)
            {
                enginfo = enginfo.AppendPrePad(BaseUtils.FieldBuilder.Build("", p.Engineer, "", p.Progress, "Rank:".Tx(this), p.Rank, ";%",p.RankProgress), System.Environment.NewLine);
            }

            detailed = "";

            if (Engineers.Length == 1)
                info = enginfo;
            else
            {
                info = BaseUtils.FieldBuilder.Build("Progress on ; Engineers".Tx(this), Engineers.Length);
                detailed = enginfo;
            }
        }
    }

}
