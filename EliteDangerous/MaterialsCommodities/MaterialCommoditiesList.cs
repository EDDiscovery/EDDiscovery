/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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

using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EliteDangerousCore
{
    [System.Diagnostics.DebuggerDisplay("Mat {Name} count {Count} left {scratchpad}")]
    public class MaterialCommodities               // in memory version of it
    {
        public int Count { get; set; }
        public double Price { get; set; }
        public MaterialCommodityData Details { get; set; }

        public int scratchpad { get; set; }        // for synthesis dialog..

        public MaterialCommodities(MaterialCommodityData c)
        {
            Count = scratchpad = 0;
            Price = 0;
            this.Details = c;
        }

        public MaterialCommodities(MaterialCommodities c)
        {
            Count = c.Count;        // clone these
            Price = c.Price;
            this.Details = c.Details;       // can copy this, its fixed
        }
    }


    public class MaterialCommoditiesList
    {
        private List<MaterialCommodities> list;

        // static BaseUtils.LogToFile log = new BaseUtils.LogToFile("c:\\code"); // debug

        public MaterialCommoditiesList()
        {
            list = new List<MaterialCommodities>();
        }

        public bool ContainsRares() // function on purpose
        {
            return list.FindIndex(x => x.Details.IsRareCommodity && x.Count > 0) != -1;
        }

        public MaterialCommoditiesList Clone()       // returns a new copy of this class.. all items a copy
        {
            MaterialCommoditiesList mcl = new MaterialCommoditiesList();

            list.ForEach(item =>
            {
                bool commodity = item.Details.IsCommodity;
                // if items, or commodity and not clear zero, or material and not clear zero, add
                if (item.Count > 0 )
                    mcl.list.Add(item);
            });

            return mcl;
        }

        public List<MaterialCommodities> List { get { return list; } }

        public MaterialCommodities Find(MaterialCommodityData other) { return list.Find(x => x.Details.FDName.Equals(other.FDName, StringComparison.InvariantCultureIgnoreCase)); }
        public MaterialCommodities FindFDName(string fdname) { return list.Find(x => x.Details.FDName.Equals(fdname, StringComparison.InvariantCultureIgnoreCase)); }

        public List<MaterialCommodities> Sort(bool commodity)
        {
            List<MaterialCommodities> ret = new List<MaterialCommodities>();

            if (commodity)
                ret = list.Where(x => x.Details.IsCommodity).OrderBy(x => x.Details.Type)
                           .ThenBy(x => x.Details.Name).ToList();
            else
                ret = list.Where(x => !x.Details.IsCommodity).OrderBy(x => x.Details.Name).ToList();

            return ret;
        }

        public int Count(string [] cats)    // for all types of cat, if item matches or does not, count
        {
            int total = 0;
            foreach (MaterialCommodities c in list)
            {
                if ( Array.IndexOf<string>(cats, c.Details.Category) != -1 )
                    total += c.Count;
            }

            return total;
        }

        public int DataCount { get { return Count(new string[] { MaterialCommodityData.MaterialEncodedCategory }); } }
        public int MaterialsCount { get { return Count(new string[] { MaterialCommodityData.MaterialRawCategory, MaterialCommodityData.MaterialManufacturedCategory }); } }
        public int CargoCount { get { return Count(new string[] { MaterialCommodityData.CommodityCategory }); } }

        public int DataHash() { return list.GetHashCode(); }

        void Dump()
        {
            System.Diagnostics.Debug.Write(list.GetHashCode() + " ");
            foreach ( MaterialCommodities m in list )
            {
                System.Diagnostics.Debug.Write( "{" + m.GetHashCode() + " " + m.Details.Category + " " + m.Details.FDName + " " + m.Count + "}");
            }
            System.Diagnostics.Debug.WriteLine("");
        }

        // ifnorecatonsearch is used if you don't know if its a material or commodity.. for future use.

        private MaterialCommodities GetNewCopyOf(string cat, string fdname, bool ignorecatonsearch = false)
        {
            int index = list.FindIndex(x => x.Details.FDName.Equals(fdname, StringComparison.InvariantCultureIgnoreCase) && (ignorecatonsearch || x.Details.Category.Equals(cat, StringComparison.InvariantCultureIgnoreCase)));

            if (index >= 0)
            {
                list[index] = new MaterialCommodities(list[index]);    // fresh copy..
                return list[index];
            }
            else
            {
                MaterialCommodityData mcdb = MaterialCommodityData.EnsurePresent(cat,fdname);    // get a MCDB of this
                MaterialCommodities mc = new MaterialCommodities(mcdb);        // make a new entry
                list.Add(mc);

                //log.WriteLine("MC Made:" + cat + " " + fdname + " >> " + mc.fdname + mc.name );

                return mc;
            }
        }

        // ignore cat is only used if you don't know what it is 
        public void Change(string cat, string fdname, int num, long price, bool ignorecatonsearch = false)
        {
            MaterialCommodities mc = GetNewCopyOf(cat, fdname, ignorecatonsearch);
       
            double costprev = mc.Count * mc.Price;
            double costnew = num * price;
            mc.Count = Math.Max(mc.Count + num, 0);

            if (mc.Count > 0 && num > 0)      // if bought (defensive with mc.count)
                mc.Price = (costprev + costnew) / mc.Count;       // price is now a combination of the current cost and the new cost. in case we buy in tranches

            //log.WriteLine("MC Change:" + cat + " " + fdname + " " + num + " " + mc.count);
        }

        public void Craft(string fdname, int num)
        {
            int index = list.FindIndex(x => x.Details.FDName.Equals(fdname, StringComparison.InvariantCultureIgnoreCase));

            if (index >= 0)
            {
                MaterialCommodities mc = new MaterialCommodities(list[index]);      // new clone of
                list[index] = mc;       // replace ours with new one
                mc.Count = Math.Max(mc.Count - num, 0);

                //log.WriteLine("MC Craft:" + fdname + " " + num + " " + mc.count);
            }
        }

        public void Died()
        {
            list.RemoveAll(x => x.Details.IsCommodity);      // empty the list of all commodities
        }

        public void Set(string cat, string fdname, int num, double price)
        {
            MaterialCommodities mc = GetNewCopyOf(cat, fdname);

            mc.Count = num;
            if (price > 0)
                mc.Price = price;

            //log.WriteLine("MC Set:" + cat + " " + fdname + " " + num + " " + mc.count);
        }

        public void Clear(bool commodity)
        {
            //log.Write("MC Clear");
            for (int i = 0; i < list.Count; i++)
            {
                MaterialCommodities mc = list[i];
                if (commodity == mc.Details.IsCommodity)
                {
                    list[i] = new MaterialCommodities(list[i]);     // new clone of it we can change..
                    list[i].Count = 0;  // and clear it
                    //log.Write(mc.fdname + ",");
                }
            }
            //log.Write("", true);
        }

        static public MaterialCommoditiesList Process(JournalEntry je, MaterialCommoditiesList oldml, SQLiteConnectionUser conn )
        {
            MaterialCommoditiesList newmc = (oldml == null) ? new MaterialCommoditiesList() : oldml;

            if (je is ICommodityJournalEntry || je is IMaterialJournalEntry)    // could be both
            {
                newmc = newmc.Clone();          // so we need a new one, makes a new list, but copies the items..

                if (je is ICommodityJournalEntry)
                {
                    ICommodityJournalEntry e = je as ICommodityJournalEntry;
                    e.UpdateCommodities(newmc, conn);
                }

                if (je is IMaterialJournalEntry)
                {
                    IMaterialJournalEntry e = je as IMaterialJournalEntry;
                    e.UpdateMaterials(newmc, conn);
                }
            }

            return newmc;
        }
    }
}
