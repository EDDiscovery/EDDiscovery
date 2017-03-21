using EDDiscovery.DB;
using EDDiscovery2.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EliteDangerous
{
    public struct MaterialCommodities               // in memory version of it
    {
        public int count { get; set; }
        public double price { get; set; }
        public MaterialCommodityDB Details { get; set; }

        public MaterialCommodities(MaterialCommodityDB c)
        {
            count = 0;
            price = 0;
            this.Details = c;
        }

        public long id { get { return Details.id; } }
        public string category { get { return Details.category; } }
        public string name { get { return Details.name; } }
        public string fdname { get { return Details.fdname; } }
        public string type { get { return Details.type; } }
        public string shortname { get { return Details.shortname; } }
        public Color colour { get { return Details.colour; } }

        #region Static properties and methods linking to MaterialCommodity
        public static string CommodityCategory { get { return MaterialCommodityDB.CommodityCategory; } }
        public static string MaterialRawCategory { get { return MaterialCommodityDB.MaterialRawCategory; } }
        public static string MaterialEncodedCategory { get { return MaterialCommodityDB.MaterialEncodedCategory; } }
        public static string MaterialManufacturedCategory { get { return MaterialCommodityDB.MaterialManufacturedCategory; } }
        #endregion
    }


    public class MaterialCommoditiesList
    {
        private List<MaterialCommodities> list;

        public MaterialCommoditiesList()
        {
            list = new List<MaterialCommodities>();
        }

        public MaterialCommoditiesList Clone(bool clearzeromaterials, bool clearzerocommodities)       // returns a new copy of this class.. all items a copy
        {
            MaterialCommoditiesList mcl = new MaterialCommoditiesList();

            mcl.list = new List<MaterialCommodities>(list.Count);
            list.ForEach(item =>
            {
                bool commodity = item.category.Equals(MaterialCommodities.CommodityCategory);
                // if items, or commodity and not clear zero, or material and not clear zero, add
                if (item.count > 0 || (commodity && !clearzerocommodities) || (!commodity && !clearzeromaterials))
                    mcl.list.Add(item);
            });

            return mcl;
        }

        public List<MaterialCommodities> Sort(bool commodity)
        {
            List<MaterialCommodities> ret = new List<MaterialCommodities>();

            if (commodity)
                ret = list.Where(x => x.category.Equals(MaterialCommodities.CommodityCategory)).OrderBy(x => x.type)
                           .ThenBy(x => x.name).ToList();
            else
                ret = list.Where(x => !x.category.Equals(MaterialCommodities.CommodityCategory)).OrderBy(x => x.name).ToList();

            return ret;
        }

        // ifnorecatonsearch is used if you don't know if its a material or commodity.. for future use.

        private int EnsurePresent(string cat, string fdname, SQLiteConnectionUser conn, bool ignorecatonsearch = false)
        {
            int index = list.FindIndex(x => x.fdname.Equals(fdname, StringComparison.InvariantCultureIgnoreCase) && (ignorecatonsearch || x.category.Equals(cat, StringComparison.InvariantCultureIgnoreCase)));

            if (index >= 0)
            {
                return index;
            }
            else
            {
                MaterialCommodityDB mcdb = MaterialCommodityDB.EnsurePresent(cat,fdname, conn);    // get a MCDB of this
                MaterialCommodities mc = new MaterialCommodities(mcdb);        // make a new entry
                list.Add(mc);
                return list.Count - 1;
            }
        }

        // ignore cat is only used if you don't know what it is 
        public void Change(string cat, string fdname, int num, long price, SQLiteConnectionUser conn, bool ignorecatonsearch = false)
        {
            int index = EnsurePresent(cat, fdname, conn, ignorecatonsearch);
            MaterialCommodities mc = list[index];

            double costprev = mc.count * mc.price;
            double costnew = num * price;
            mc.count = Math.Max(mc.count + num, 0); ;

            if (mc.count > 0 && num > 0)      // if bought (defensive with mc.count)
                mc.price = (costprev + costnew) / mc.count;       // price is now a combination of the current cost and the new cost. in case we buy in tranches

            list[index] = mc;
        }

        public void Craft(string fdname, int num)
        {
            int index = list.FindIndex(x => x.fdname.Equals(fdname, StringComparison.InvariantCultureIgnoreCase));

            if (index >= 0)
            {
                MaterialCommodities mc = list[index];
                mc.count = Math.Max(mc.count - num, 0);
                list[index] = mc;
            }
        }

        public void Set(string cat, string fdname, int num, double price, SQLiteConnectionUser conn, bool ignorecatonsearch = false)
        {
            int index = EnsurePresent(cat, fdname, conn);
            MaterialCommodities mc = list[index];

            mc.count = num;
            if (price > 0)
                mc.price = price;

            list[index] = mc;
        }

        static public MaterialCommoditiesList Process(JournalEntry je, MaterialCommoditiesList oldml, SQLiteConnectionUser conn,
                                                        bool clearzeromaterials, bool clearzerocommodities)
        {
            MaterialCommoditiesList newmc = (oldml == null) ? new MaterialCommoditiesList() : oldml;

            if (je is IMaterialCommodityJournalEntry)
            {
                IMaterialCommodityJournalEntry e = je as IMaterialCommodityJournalEntry;
                newmc = newmc.Clone(clearzeromaterials, clearzerocommodities);          // so we need a new one
                e.MaterialList(newmc, conn);
            }

            return newmc;
        }
    }

}
