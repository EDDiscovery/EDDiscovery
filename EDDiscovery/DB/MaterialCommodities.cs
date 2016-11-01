using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery2.DB
{
    public class MaterialCommodities
    {
        public long id { get; set; }
        public string category { get; set; }                // either Commodity, or one of the Category types from the MaterialCollected type.
        public string name { get; set; }                    // name of it
        public string type { get; set; }                    // and its type, for materials its rarity, for commodities its group ("Metals" etc).
        public string shortname { get; set; }

        // Not in DB

        public int count { get; set; }
        public double price { get; set; }

        public static string CommodityCategory = "Commodity";
        public static string MaterialRawCategory = "Raw";

        // Helpers

        public MaterialCommodities()
        {
        }

        public MaterialCommodities(MaterialCommodities c)     // copy constructor, ensure a different copy of this
        {
            id = c.id;
            category = c.category;
            name = String.Copy(c.name);
            type = String.Copy(c.type);
            shortname = c.shortname;
            count = c.count;
            price = c.price;
        }

        public MaterialCommodities(long i, string cs, string n, string t, string s, int c = 0 , long p = 0)
        {
            id = i;
            category = cs;
            name = n;
            type = t;
            shortname = s;
            count = c;
            price = p;
        }

        public bool Add()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())      // open connection..
            {
                bool ret = Add(cn);
                return ret;
            }
        }

        private bool Add(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into MaterialsCommodities (Category,Name,Type,ShortName) values (@category,@name,@type,@shortname)"))
            {
                cmd.AddParameterWithValue("@category", category);
                cmd.AddParameterWithValue("@name", name);
                cmd.AddParameterWithValue("@type", type);
                cmd.AddParameterWithValue("@shortname", shortname);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from MaterialsCommodities"))
                {
                    id = (long)SQLiteDBClass.SQLScalar(cn, cmd2);
                }
                return true;
            }
        }

        public bool Update()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Update MaterialsCommodities set Category=@category, Name=@name, Type=@type, ShortName=@shortname where ID=@id"))
            {
                cmd.AddParameterWithValue("@id", id);
                cmd.AddParameterWithValue("@category", category);
                cmd.AddParameterWithValue("@name", name);
                cmd.AddParameterWithValue("@type", type);
                cmd.AddParameterWithValue("@shortname", shortname);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);
                return true;
            }
        }

        public bool Delete()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return Delete(cn);
            }
        }

        private bool Delete(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("DELETE FROM MaterialsCommodities WHERE id = @id"))
            {
                cmd.AddParameterWithValue("@id", id);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);
                return true;
            }
        }

        public static MaterialCommodities Get(string c, string name)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(mode: EDDbAccessMode.Reader))
            {
                return Get(c, name, cn);
            }
        }

        public static MaterialCommodities Get(string c, string name, SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("select Id,Category,Name,Type,ShortName from MaterialsCommodities WHERE Category = @category And Name==@name"))
            {
                cmd.AddParameterWithValue("@category", c);
                cmd.AddParameterWithValue("@name", name);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())           // already sorted, and already limited to max items
                    {
                        return new MaterialCommodities((long)reader[0],(string)reader[1], (string)reader[2], (string)reader[3],(string)reader[4]);
                    }
                    else
                        return null;
                }
            }
        }

        public static MaterialCommodities Get(string name, SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("select Id,Category,Name,Type,ShortName from MaterialsCommodities WHERE Name==@name"))
            {
                cmd.AddParameterWithValue("@name", name);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())           // already sorted, and already limited to max items
                    {
                        return new MaterialCommodities((long)reader[0], (string)reader[1], (string)reader[2], (string)reader[3], (string)reader[4]);
                    }
                    else
                        return null;
                }
            }
        }

        public static void AddNewType(string c, string namelist, string t, string sn = "")
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                string[] list = namelist.Split(';');

                foreach (string s in list)
                {
                    MaterialCommodities mc = Get(c, s, cn);

                    if (mc == null)
                    {
                        mc = new MaterialCommodities(0,c, s, t, sn);
                        mc.Add(cn);
                    }
                    else
                    {
                        mc.shortname = sn;          // So, category/name combo is there, only thing that can be updated is shortname and type..
                        mc.type = t;
                        mc.Update(cn);
                    }
                }
            }
        }

        public static void SetUpInitialTable()
        {
            AddNewType(MaterialRawCategory, "Antimony", "Very Rare","Sb");
            AddNewType(MaterialRawCategory, "Arsenic", "Common","As");
            AddNewType(MaterialRawCategory, "Cadmium", "Rare","Cd");
            AddNewType(MaterialRawCategory, "Carbon", "Very common","C");
            AddNewType(MaterialRawCategory, "Chromium", "Common","Cr");
            AddNewType(MaterialRawCategory, "Germanium", "Common","Ge");
            AddNewType(MaterialRawCategory, "Iron", "Very Common","Fe");
            AddNewType(MaterialRawCategory, "Manganese", "Common","Mn");
            AddNewType(MaterialRawCategory, "Mercury", "Rare","Hg");
            AddNewType(MaterialRawCategory, "Molybdenum", "Rare","Mo");
            AddNewType(MaterialRawCategory, "Nickel", "Very Common","Ni");
            AddNewType(MaterialRawCategory, "Niobium", "Rare","Nb");
            AddNewType(MaterialRawCategory, "Phosphorus", "Very Common","P");
            AddNewType(MaterialRawCategory, "Polonium", "Very Rare","Po");
            AddNewType(MaterialRawCategory, "Ruthenium", "Very Rare","Ru");
            AddNewType(MaterialRawCategory, "Selenium", "Common","Se");
            AddNewType(MaterialRawCategory, "Sulphur", "Very Common","S");
            AddNewType(MaterialRawCategory, "Technetium", "Very Rare","Tc");
            AddNewType(MaterialRawCategory, "Tellurium", "Very Rare","Te");
            AddNewType(MaterialRawCategory, "Tin", "Rare","Sn");
            AddNewType(MaterialRawCategory, "Tungsten", "Rare","W");
            AddNewType(MaterialRawCategory, "Vanadium", "Common","V");
            AddNewType(MaterialRawCategory, "Yttrium", "Very Rare","Y");
            AddNewType(MaterialRawCategory, "Zinc", "Common","Zn");
            AddNewType(MaterialRawCategory, "Zirconium", "Common","Zr");

            AddNewType(CommodityCategory, "Explosives;Hydrogen Fuel;Hydrogen Peroxide;Liquid Oxygen;Mineral Oil;Nerve Agents;Pesticides;Surface Stabilisers;Synthetic Reagents;Water", "Chemicals");
            AddNewType(CommodityCategory, "Clothing;Consumer Technology;Domestic Appliances;Evacuation Shelter;Survival Equipment", "Consumer Items");
            AddNewType(CommodityCategory, "Algae;Animal Meat;Coffee;Fish;Food Cartridges;Fruit and Vegetables;Grain;Synthetic Meat;Tea", "Foods");
            AddNewType(CommodityCategory, "Ceramic Composites;CMM Composite;Insulating Membrane;Meta-Alloys;Micro-Weave Cooling Hoses;Neofabric Insulation;Polymers;Semiconductors;Superconductors", "Industrial Materials");
            AddNewType(CommodityCategory, "Beer;Bootleg Liquor;Liquor;Narcotics;Tobacco;Wine", "Legal Drugs");
            AddNewType(CommodityCategory, "Articulation Motors;Atmospheric Processors;Building Fabricators;Crop Harvesters;Emergency Power Cells;Energy Grid Assembly;Exhaust Manifold;Geological Equipment", "Machinery");
            AddNewType(CommodityCategory, "Heatsink Interlink;HN Shock Mount;Ion Distributor;Magnetic Emitter Coil;Marine Equipment", "Machinery");
            AddNewType(CommodityCategory, "Microbial Furnaces;Mineral Extractors;Modular Terminals;Power Converter;Power Generators;Power Transfer Bus", "Machinery");
            AddNewType(CommodityCategory, "Radiation Baffle;Reinforced Mounting Plate;Skimmer Components;Thermal Cooling Units;Water Purifiers", "Machinery");
            AddNewType(CommodityCategory, "Advanced Medicines;Agri-Medicines;Basic Medicines;Combat Stabilisers;Performance Enhancers;Progenitor Cells", "Medicines");
            AddNewType(CommodityCategory, "Aluminium;Beryllium;Bismuth;Cobalt;Copper;Gallium;Gold;Hafnium 178;Indium;Lan;hanum;Lithium;Osmium;Palladium;Platinum;Praseodymium;Samarium;Silver;Tantalum;Thallium;Thorium;Titanium;Uranium", "Metals");
            AddNewType(CommodityCategory, "Bauxite;Bertrandite;Bromellite;Coltan;Cryolite;Gallite;Goslarite", "Minerals");
            AddNewType(CommodityCategory, "Indite;Jadeite;Lepidolite;Lithium Hydroxide;Low Temperature Diamonds;Methane ;lathrate;Methanol Monohydrate;Moissanite;Painite;Pyrophyllite;Rutile;Taaffeite;Uraninite", "Minerals");
            AddNewType(CommodityCategory, "Ai Relics;Ancient Artefact;Antimatter Containment Unit;Antiquities;Assault Plans;Black Box;Commercial Samples;Data Core;Diplomatic Bag;Encrypted Correspondence;Encrypted Data Storage;Experimental Chemicals;Fossil Remnants", "Salvage");
            AddNewType(CommodityCategory, "Galactic Travel Guide;Geological Samples;Hostage;Military Intelligence;Military Plans (USS Cargo);Mysterious Idol;Occupied CryoPod;Occupied Escape Pod;Personal Effects;Political Prisoner;Precious Gems;Prohibited Research Materials;Prototype Tech", "Salvage");
            AddNewType(CommodityCategory, "Rare Artwork;Rebel Transmissions;Salvageable Wreckage;Sap 8 Core Container;Sc;entific Research;Scientific Samples;Space Pioneer Relics;Tactical Data;Technical Blueprints;Trade Data;Unknown Artefact;Unknown Probe;Unstable Data Core", "Salvage");
            AddNewType(CommodityCategory, "Imperial Slaves;Slaves", "Slavery");
            AddNewType(CommodityCategory, "Advanced Catalysers;Animal Monitors;Aquaponic Systems;Auto-Fabricators;Bioreducing Lichen;Computer Components", "Technology");
            AddNewType(CommodityCategory, "H.E. Suits;Hardware Diagnostic Sensor;Land Enrichment Systems;Medical Diagnostic Equipment;Micro Controllers;Muon Imager", "Technology");
            AddNewType(CommodityCategory, "Nanobreakers;Resonating Separators;Robotics;Structural Regulators;Telemetry Suite", "Technology");
            AddNewType(CommodityCategory, "Conductive Fabrics;Leather;Military Grade Fabrics;Natural Fabrics;Synthetic Fabrics", "Textiles");
            AddNewType(CommodityCategory, "Biowaste;Chemical Waste;Scrap;Toxic Waste", "Waste");
            AddNewType(CommodityCategory, "Battle Weapons;Landmines;Non-lethal Weapons;Personal Weapons;Reactive Armour", "Weapons");
        }
    }


    public class MaterialCommoditiesList
    {
        private List<MaterialCommodities> list;

        public MaterialCommoditiesList()
        {
            list = new List<MaterialCommodities>();
        }

        public MaterialCommoditiesList Clone()       // returns a new copy of this class.. all items a copy
        {
            MaterialCommoditiesList mcl = new MaterialCommoditiesList();
            mcl.list = new List<MaterialCommodities>(list.Count);
            list.ForEach(item => { mcl.list.Add(new MaterialCommodities(item)); });
            return mcl;
        }

        private MaterialCommodities EnsurePresent(string cat, string name)
        {
            MaterialCommodities mc = list.Find(x => x.name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && x.category.Equals(cat, StringComparison.InvariantCultureIgnoreCase));

            if (mc == null)
            {
                MaterialCommodities mcdb = MaterialCommodities.Get(cat, name);    // look up in DB and see if we have a record of this type of item

                if (mcdb == null)             // no record of this, add as Unknown to db
                {
                    mcdb = new MaterialCommodities(0,cat, name, "Unknown","");
                    mcdb.Add();                 // and add to data base
                }

                mc = new MaterialCommodities(0,cat, name, mcdb.type,mcdb.shortname);        // make a new entry
                list.Add(mc);
            }

            return mc;
        }

        public void MaterialObtained(string cat, string name, int num)      // material
        {
            MaterialCommodities mc = EnsurePresent(cat, name);
            mc.count += num;
        }

        public void MaterialDiscarded(string cat, string name, int num)     // material
        {
            MaterialCommodities mc = EnsurePresent(cat, name);
            mc.count = Math.Max(mc.count - num, 0);
        }

        public void Bought(string name, int num, long price)         // commodity
        {
            MaterialCommodities mc = EnsurePresent(MaterialCommodities.CommodityCategory, name);

            double costprev = mc.count * mc.price;
            double costnew = num * price;
            mc.count += num;
            mc.price = (costprev + costnew) / mc.count;       // price is now a combination of the current cost and the new cost. in case we buy in tranches
        }

        public void Collected(string name, int num)         // same as buying at price 0
        {
            Bought(name, num, 0);
        }

        public int Sold(string name, int num, long price)   // commodity
        {
            MaterialCommodities mc = EnsurePresent(MaterialCommodities.CommodityCategory, name);
            mc.count = Math.Max(mc.count - num, 0);
            return (int)(num * (price - mc.price));           // profit is this.. rounded to the credit point
        }

        public void Ejected(string name, int num)
        {
            Sold(name, num, 0);
        }

        public void Crafted(string name, int num)           // craft uses up either materials or commodities..
        {
            MaterialCommodities mc = list.Find(x => x.name.Equals(name, StringComparison.InvariantCultureIgnoreCase)); // name only..

            if (mc == null)                                 // if we have a record..
                mc.count = Math.Max(mc.count - num, 0);     // decrement count.. if we don't have a record, ignore it
        }


        static public MaterialCommoditiesList Process(JournalEntry je, MaterialCommoditiesList oldml)
        {
            MaterialCommoditiesList newmc = (oldml == null) ? new MaterialCommoditiesList() : oldml;

            switch (je.EventTypeID)
            {
                case JournalTypeEnum.MaterialCollected:
                    newmc = newmc.Clone();
                    JournalMaterialCollected ji = (JournalMaterialCollected)je;
                    newmc.MaterialObtained(ji.Category, ji.Name, ji.Count);
                    break;

                case JournalTypeEnum.MaterialDiscarded:
                    newmc = newmc.Clone();
                    JournalMaterialDiscarded jd = (JournalMaterialDiscarded)je;
                    newmc.MaterialDiscarded(jd.Category, jd.Name, jd.Count);
                    break;

                case JournalTypeEnum.CollectCargo:
                    newmc = newmc.Clone();
                    JournalCollectCargo jcc = (JournalCollectCargo)je;
                    newmc.Collected(jcc.Type, 1);
                    break;

                case JournalTypeEnum.EjectCargo:
                    newmc = newmc.Clone();
                    JournalEjectCargo jce = (JournalEjectCargo)je;
                    newmc.Ejected(jce.Type, jce.Count);
                    break;

                case JournalTypeEnum.MiningRefined:
                    newmc = newmc.Clone();
                    JournalMiningRefined jmr = (JournalMiningRefined)je;
                    newmc.Collected(jmr.Type, 1);
                    break;

                case JournalTypeEnum.MissionCompleted:      // Commodities can be rewarded - never seen. leave for now. question is out on forums
                    //newmc = newmc.Clone();
                    //JournalMissionCompleted jmc = (JournalMissionCompleted)je;
                    // TBD
                    break;

                case JournalTypeEnum.MarketBuy:
                    newmc = newmc.Clone();
                    JournalMarketBuy jmb = (JournalMarketBuy)je;
                    newmc.Bought(jmb.Type, jmb.Count, jmb.BuyPrice);
                    break;

                case JournalTypeEnum.MarketSell:
                    newmc = newmc.Clone();
                    JournalMarketSell jms = (JournalMarketSell)je;
                    newmc.Sold(jms.Type, jms.Count, jms.SellPrice);
                    break;

                case JournalTypeEnum.EngineerCraft: // i'm guessing commodities also can be used..
                    newmc = newmc.Clone();
                    JournalEngineerCraft jec = (JournalEngineerCraft)je;
                    // TBD  Crafted
                    break;
            }

            return newmc;
        }
    }

    public class MaterialCommoditiesLedger
    {
        public class Transaction
        {
            public DateTime utctime;                               // when it was done.
            public JournalTypeEnum jtype;                          // what caused it..
            public MaterialCommodities materialcommoditity;        // holds material/commoditity 
            public long profit;                                     // any profit?
        }

        private List<Transaction> transactions;

        public MaterialCommoditiesLedger()
        {
            transactions = new List<Transaction>();
        }

        void AddCommmodityEvent( DateTime t, JournalTypeEnum j, string name, int count, long buyprice, long pft = 0 )
        {
            MaterialCommodities mcdb = MaterialCommodities.Get(MaterialCommodities.CommodityCategory, name);    // look up in DB and see if we have a record of this type of item
            MaterialCommodities mc = new MaterialCommodities(0,MaterialCommodities.CommodityCategory, name, mcdb!=null ? mcdb.type : "Unknown", mcdb != null ? mcdb.shortname : "", count, buyprice);

            Transaction tr = new Transaction
            {
                utctime = t,
                jtype = j,
                materialcommoditity = mc,
                profit = pft
            };

            transactions.Add(tr);
        }

        void AddMaterialEvent(DateTime t, JournalTypeEnum j, string cat, string name, int count)
        {
            MaterialCommodities mcdb = MaterialCommodities.Get(cat, name);    // look up in DB and see if we have a record of this type of item
            MaterialCommodities mc = new MaterialCommodities(0,cat, name, mcdb != null ? mcdb.type : "Unknown", mcdb != null ? mcdb.shortname : "", count);

            Transaction tr = new Transaction
            {
                utctime = t,
                jtype = j,
                materialcommoditity = mc,
            };

            transactions.Add(tr);
        }

        public void Process(JournalEntry je)
        {
            switch (je.EventTypeID)
            {
                case JournalTypeEnum.MaterialCollected:
                    JournalMaterialCollected ji = (JournalMaterialCollected)je;
                    AddMaterialEvent(je.EventTimeUTC, je.EventTypeID, ji.Category, ji.Name, ji.Count);
                    break;

                case JournalTypeEnum.MaterialDiscarded:
                    JournalMaterialDiscarded jd = (JournalMaterialDiscarded)je;
                    AddMaterialEvent(je.EventTimeUTC, je.EventTypeID, jd.Category, jd.Name, jd.Count);
                    break;

                case JournalTypeEnum.CollectCargo:
                    JournalCollectCargo jcc = (JournalCollectCargo)je;
                    AddCommmodityEvent(je.EventTimeUTC, je.EventTypeID, jcc.Type, 1, 0);
                    break;

                case JournalTypeEnum.EjectCargo:
                    JournalEjectCargo jce = (JournalEjectCargo)je;
                    AddCommmodityEvent(je.EventTimeUTC, je.EventTypeID, jce.Type, jce.Count, 0);
                    break;

                case JournalTypeEnum.MiningRefined:
                    JournalMiningRefined jmr = (JournalMiningRefined)je;
                    AddCommmodityEvent(je.EventTimeUTC, je.EventTypeID, jmr.Type, 1, 0);
                    break;

                case JournalTypeEnum.MissionCompleted:      // Commodities can be rewarded - never seen. leave for now. question is out on forums
                    //JournalMissionCompleted jmc = (JournalMissionCompleted)je;
                    //TBD
                    break;

                case JournalTypeEnum.MarketBuy:
                    JournalMarketBuy jmb = (JournalMarketBuy)je;
                    AddCommmodityEvent(je.EventTimeUTC, je.EventTypeID, jmb.Type, jmb.Count, jmb.BuyPrice);
                    break;

                case JournalTypeEnum.MarketSell:
                    JournalMarketSell jms = (JournalMarketSell)je;
                    AddCommmodityEvent(je.EventTimeUTC, je.EventTypeID, jms.Type, jms.Count, jms.SellPrice, (jms.SellPrice-jms.AvgPricePaid)*jms.Count);
                    break;

                case JournalTypeEnum.EngineerCraft: // i'm guessing commodities also can be used..
                    JournalEngineerCraft jec = (JournalEngineerCraft)je;
                    // TBD  Crafted
                    break;
            }
        }

    }
}
