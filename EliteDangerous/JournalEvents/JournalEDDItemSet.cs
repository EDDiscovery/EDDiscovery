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

namespace EliteDangerousCore.JournalEvents
{
    //When written: by EDD when a user manually sets an item count (material or commodity)
    [JournalEntryType(JournalTypeEnum.EDDItemSet)]
    public class JournalEDDItemSet : JournalEntry, IMaterialCommodityJournalEntry
    {
        public JournalEDDItemSet(JObject evt) : base(evt, JournalTypeEnum.EDDItemSet)
        {
            Materials = new MaterialList(evt["Materials"]?.ToObject<MaterialItem[]>().ToList());
            Commodities = new CommodityList(evt["Commodities"]?.ToObject<CommodityItem[]>().ToList());
        }

        public MaterialList Materials { get; set; }             // FDNAMES
        public CommodityList Commodities { get; set; }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Materials != null)
            {
                foreach (MaterialItem m in Materials.Materials)
                    mc.Set(m.Category, m.Name, m.Count, 0, conn);
            }

            if (Commodities != null)
            {
                foreach (CommodityItem m in Commodities.Commodities)
                    mc.Set(MaterialCommodities.CommodityCategory, m.Name, m.Count, m.BuyPrice, conn);
            }
        }

        public override System.Drawing.Bitmap Icon { get { return EliteDangerous.Properties.Resources.genericevent; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";
            bool comma = false;
            if (Materials != null)
            {
                foreach (MaterialItem m in Materials.Materials)
                {
                    if (comma)
                        info += ", ";
                    comma = true;
                    info += BaseUtils.FieldBuilder.Build("Name:", JournalFieldNaming.RMat(m.Name), "", m.Count);
                }
            }

            if (Commodities != null)
            {
                foreach (CommodityItem m in Commodities.Commodities)
                {
                    if (comma)
                        info += ", ";
                    comma = true;
                    info += BaseUtils.FieldBuilder.Build("Name:", JournalFieldNaming.RMat(m.Name), "", m.Count);
                }
            }
            detailed = "";
        }

    }

    public class MaterialItem
    {
        public string Name;     //FDNAME
        public string Category;
        public int Count;
    }

    public class CommodityItem
    {
        public string Name;     //FDNAME
        public int Count;
        public double BuyPrice;
    }

    public class MaterialList
    {
        public MaterialList(System.Collections.Generic.List<MaterialItem> ma )
        {
            Materials = ma ?? new System.Collections.Generic.List<MaterialItem>();
            foreach (MaterialItem i in Materials)
                i.Name = JournalFieldNaming.FDNameTranslation(i.Name);
        }

        public System.Collections.Generic.List<MaterialItem> Materials { get; protected set; }
    }

    public class CommodityList
    {
        public CommodityList(System.Collections.Generic.List<CommodityItem> ma)
        {
            Commodities = ma ?? new System.Collections.Generic.List<CommodityItem>();
            foreach (CommodityItem i in Commodities)
                i.Name = JournalFieldNaming.FDNameTranslation(i.Name);
        }

        public System.Collections.Generic.List<CommodityItem> Commodities { get; protected set; }
    }

}


