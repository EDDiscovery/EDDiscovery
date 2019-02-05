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
using EliteDangerousCore;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.EDDCommodityPrices)]
    public class JournalEDDCommodityPrices : JournalCommodityPricesBase
    {
        public JournalEDDCommodityPrices(JObject evt) : base(evt, JournalTypeEnum.EDDCommodityPrices)
        {
            Station = evt["station"].Str();
            Faction = evt["faction"].Str();
            Commodities = new List<CCommodities>();
           
            JArray jcommodities = null;
            if (!evt["commodities"].Empty())
                jcommodities = (JArray)evt["commodities"];

            if (jcommodities != null)
            {
                foreach (JObject commodity in jcommodities)
                {
                    CCommodities com = new CCommodities(commodity);
                    Commodities.Add(com);
                }

                Commodities.Sort((l, r) => l.locName.CompareTo(r.locName));
            }
        }
    }

    public class JournalCommodityPricesBase : JournalEntry
    {
        public JournalCommodityPricesBase(JObject evt, JournalTypeEnum en) : base(evt,en)
        {
        }

        public string Station { get; protected set; }
        public string Faction { get; protected set; }
        public string StarSystem { get; set; }
        public long? MarketID { get; set; }
        public List<CCommodities> Commodities { get; protected set; }   // never null

        public bool HasCommodity(string fdname) { return Commodities.FindIndex(x => x.fdname.Equals(fdname, System.StringComparison.InvariantCultureIgnoreCase)) >= 0; }
        public bool HasCommodityToBuy(string fdname) { return Commodities.FindIndex(x => x.fdname.Equals(fdname, System.StringComparison.InvariantCultureIgnoreCase) && x.stock > 0) >= 0; }

        public override void FillInformation(out string info, out string detailed)
        {
            FillInformation(out info, out detailed, Commodities.Count > 60 ? 2 : 1);
        }

        public void FillInformation(out string info, out string detailed, int maxcol)
        {

            info = BaseUtils.FieldBuilder.Build("Prices on ; items".Tx(typeof(JournalCommodityPricesBase),"PON"), Commodities.Count, 
                                                "< at ".Tx(typeof(JournalCommodityPricesBase), "CPBat"), Station , 
                                                "< in ".Tx(typeof(JournalCommodityPricesBase), "CPBin"), StarSystem);

            int col = 0;
            detailed = "Items to buy:".Tx(typeof(JournalCommodityPricesBase)) + System.Environment.NewLine;
            foreach (CCommodities c in Commodities)
            {
                if (c.buyPrice > 0)
                {
                    string name = MaterialCommodityData.GetNameByFDName(c.fdname);

                    if (c.sellPrice > 0)
                    {
                        detailed += string.Format("{0}: {1} sell {2} Diff {3} {4}%  ".Tx(typeof(JournalCommodityPricesBase), "CPBBuySell"),
                            name, c.buyPrice, c.sellPrice, c.buyPrice - c.sellPrice, 
                            ((double)(c.buyPrice - c.sellPrice) / (double)c.sellPrice * 100.0).ToString("0.#"));
                    }
                    else
                        detailed += string.Format("{0}: {1}  ".Tx(typeof(JournalCommodityPricesBase), "CPBBuy"), name, c.buyPrice);

                    if (++col == maxcol)
                    {
                        detailed += System.Environment.NewLine;
                        col = 0;
                    }
                }
            }

            if (col == maxcol - 1)
                detailed += System.Environment.NewLine;

            col = 0;
            detailed += "Sell only Items:".Tx(typeof(JournalCommodityPricesBase),"SO") + System.Environment.NewLine;
            foreach (CCommodities c in Commodities)
            {
                if (c.buyPrice <= 0)
                {
                    string name = MaterialCommodityData.GetNameByFDName(c.fdname);

                    detailed += string.Format("{0}: {1}  ".Tx(typeof(JournalCommodityPricesBase), "CPBBuy"), name, c.sellPrice);
                    if (++col == maxcol)
                    {
                        detailed += System.Environment.NewLine;
                        col = 0;
                    }
                }
            }
        }

    }


    //When written: by EDD when a user manually sets an item count (material or commodity)
    [JournalEntryType(JournalTypeEnum.EDDItemSet)]
    public class JournalEDDItemSet : JournalEntry, ICommodityJournalEntry, IMaterialJournalEntry
    {
        public JournalEDDItemSet(JObject evt) : base(evt, JournalTypeEnum.EDDItemSet)
        {
            Materials = new MaterialListClass(evt["Materials"]?.ToObjectProtected<MaterialItem[]>().ToList());
            Commodities = new CommodityListClass(evt["Commodities"]?.ToObjectProtected<CommodityItem[]>().ToList());
        }

        public MaterialListClass Materials { get; set; }             // FDNAMES
        public CommodityListClass Commodities { get; set; }

        public void UpdateMaterials(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Materials != null)
            {
                foreach (MaterialItem m in Materials.Materials)
                    mc.Set(m.Category, m.Name, m.Count, 0, conn);
            }
        }

        public void UpdateCommodities(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        { 
            if (Commodities != null)
            {
                foreach (CommodityItem m in Commodities.Commodities)
                    mc.Set(MaterialCommodityData.CommodityCategory, m.Name, m.Count, m.BuyPrice, conn);
            }
        }

        public override void FillInformation(out string info, out string detailed)
        {

            info = "";
            bool comma = false;
            if (Materials != null)
            {
                foreach (MaterialItem m in Materials.Materials)
                {
                    if (comma)
                        info += ", ";
                    comma = true;
                    info += BaseUtils.FieldBuilder.Build("Name:".Txb(this), MaterialCommodityData.GetNameByFDName(m.Name), "", m.Count);
                }
            }

            if (Commodities != null)
            {
                foreach (CommodityItem m in Commodities.Commodities)
                {
                    if (comma)
                        info += ", ";
                    comma = true;
                    info += BaseUtils.FieldBuilder.Build("Name:".Txb(this), MaterialCommodityData.GetNameByFDName(m.Name), "", m.Count);
                }
            }
            detailed = "";
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

        public class MaterialListClass
        {
            public MaterialListClass(System.Collections.Generic.List<MaterialItem> ma)
            {
                Materials = ma ?? new System.Collections.Generic.List<MaterialItem>();
                foreach (MaterialItem i in Materials)
                    i.Name = JournalFieldNaming.FDNameTranslation(i.Name);
            }

            public System.Collections.Generic.List<MaterialItem> Materials { get; protected set; }
        }

        public class CommodityListClass
        {
            public CommodityListClass(System.Collections.Generic.List<CommodityItem> ma)
            {
                Commodities = ma ?? new System.Collections.Generic.List<CommodityItem>();
                foreach (CommodityItem i in Commodities)
                    i.Name = JournalFieldNaming.FDNameTranslation(i.Name);
            }

            public System.Collections.Generic.List<CommodityItem> Commodities { get; protected set; }
        }
    }

}


