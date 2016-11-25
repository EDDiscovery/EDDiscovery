using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery2.DB
{
    public class MaterialCommodities
    {
        public long id { get; set; }
        public string category { get; set; }                // either Commodity, or one of the Category types from the MaterialCollected type.
        public string name { get; set; }                    // name of it in nice text
        public string fdname { get; set; }                  // fdnames
        public string type { get; set; }                    // and its type, for materials its rarity, for commodities its group ("Metals" etc).
        public string shortname { get; set; }               // short abv. name
        public Color colour { get; set; }                   // colour if its associated with one
        public int flags { get; set; }                      // 0 is automatically set, 1 is user edited (so don't override)

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
            fdname = String.Copy(c.fdname);
            type = String.Copy(c.type);
            shortname = c.shortname;
            colour = c.colour;
            flags = c.flags;
            count = c.count;
            price = c.price;
        }

        public MaterialCommodities(long i, string cs, string n, string fd, string t, string shortn, Color cl, int fl, int c = 0, double  p = 0)
        {
            id = i;
            category = cs;
            name = n;
            fdname = fd;
            type = t;
            shortname = shortn;
            colour = cl;
            flags = fl;
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
            using (DbCommand cmd = cn.CreateCommand("Insert into MaterialsCommodities (Category,Name,FDName,Type,ShortName,Colour,Flags) values (@category,@name,@fdname,@type,@shortname,@colour,@flags)"))
            {
                cmd.AddParameterWithValue("@category", category);
                cmd.AddParameterWithValue("@name", name);
                cmd.AddParameterWithValue("@fdname", fdname);
                cmd.AddParameterWithValue("@type", type);
                cmd.AddParameterWithValue("@shortname", shortname);
                cmd.AddParameterWithValue("@colour", colour.ToArgb());
                cmd.AddParameterWithValue("@flags", flags);
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
            using (DbCommand cmd = cn.CreateCommand("Update MaterialsCommodities set Category=@category, Name=@name, FDName=@fdname, Type=@type, ShortName=@shortname, Colour=@colour, Flags=@flags where ID=@id"))
            {
                cmd.AddParameterWithValue("@id", id);
                cmd.AddParameterWithValue("@category", category);
                cmd.AddParameterWithValue("@name", name);
                cmd.AddParameterWithValue("@fdname", fdname);
                cmd.AddParameterWithValue("@type", type);
                cmd.AddParameterWithValue("@shortname", shortname);
                cmd.AddParameterWithValue("@colour", colour.ToArgb());
                cmd.AddParameterWithValue("@flags", flags);

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

        public static MaterialCommodities GetCatFDName(string cat, string fdname)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(mode: EDDbAccessMode.Reader))
            {
                return GetCatFDName(cat, fdname, cn );
            }
        }

        // if cat is null, fdname only
        public static MaterialCommodities GetCatFDName(string cat , string fdname, SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("select Id,Category,Name,FDName,Type,ShortName,Colour,Flags from MaterialsCommodities WHERE FDName=@name"))
            {
                cmd.AddParameterWithValue("@name", fdname);

                if (cat != null)
                {
                    cmd.CommandText += " AND Category==@cat";
                    cmd.AddParameterWithValue("@cat", cat);
                }

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())           // already sorted, and already limited to max items
                    {
                        return new MaterialCommodities((long)reader[0], (string)reader[1], (string)reader[2], (string)reader[3], (string)reader[4], (string)reader[5], Color.FromArgb((int)reader[6]), (int)reader[7]);
                    }
                    else
                        return null;
                }
            }
        }

        public static MaterialCommodities GetCatName(string cat, string name)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(mode: EDDbAccessMode.Reader))
            {
                return GetCatName(cat, name, cn);
            }
        }

        public static MaterialCommodities GetCatName(string cat, string name, SQLiteConnectionUser cn)      // by NAME and CAT
        {
            using (DbCommand cmd = cn.CreateCommand("select Id,Category,Name,FDName,Type,ShortName,Colour,Flags from MaterialsCommodities WHERE Name=@name AND Category==@cat"))
            {
                cmd.AddParameterWithValue("@name", name);
                cmd.AddParameterWithValue("@cat", cat);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())           // already sorted, and already limited to max items
                    {
                        return new MaterialCommodities((long)reader[0], (string)reader[1], (string)reader[2], (string)reader[3], (string)reader[4], (string)reader[5], Color.FromArgb((int)reader[6]), (int)reader[7]);
                    }
                    else
                        return null;
                }
            }
        }

        public static List<MaterialCommodities> GetAll(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("select Id,Category,Name,FDName,Type,ShortName,Colour,Flags from MaterialsCommodities Order by Name"))
            {
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    List<MaterialCommodities> list = new List<MaterialCommodities>();

                    while (reader.Read())           // already sorted, and already limited to max items
                    {
                        list.Add(new MaterialCommodities((long)reader[0], (string)reader[1], (string)reader[2], (string)reader[3], (string)reader[4], (string)reader[5], Color.FromArgb((int)reader[6]), (int)reader[7]));
                    }

                    return list;
                }
            }
        }


        public static void AddNewType(SQLiteConnectionUser cn, string c, string namelist, string t)
        {
            AddNewTypeC(cn, c, Color.Green, namelist, t);
        }

        public static void AddNewTypeC(SQLiteConnectionUser cn, string c, Color cl, string namelist, string t, string sn = "")
        {
            string[] list = namelist.Split(';');

            foreach (string name in list)
            {
                if (name.Length > 0)   // just in case a semicolon slips thru
                {
                    string fdname = EDDiscovery.Tools.FDName(name);

                    MaterialCommodities mc = GetCatFDName(null, fdname, cn);

                    if (mc == null)
                    {
                        mc = new MaterialCommodities(0, c, name, fdname, t, sn, cl, 0);
                        mc.Add(cn);
                    }               // don't change any user changed fields
                    else if (mc.flags == 0 && (!mc.name.Equals(name) && !mc.shortname.Equals(sn) || !mc.category.Equals(c) || !mc.type.Equals(t) || mc.colour.ToArgb() != cl.ToArgb()))
                    {
                        mc.name = name;
                        mc.shortname = sn;          // So, name is there, update the others
                        mc.category = c;
                        mc.type = t;
                        mc.colour = cl;
                        mc.Update(cn);
                    }
                }
            }
        }

        public static bool ChangeDbText(string fdname, string name, string abv, string cat, string type)
        {
            MaterialCommodities mc = GetCatName(cat, name);             // is the name,cat duplex there?

            if (mc != null && !mc.fdname.Equals(fdname))                // yes, so entry is there with name/cat, and fdname is the same, ok.  else abort
                return false;                                           // if fdname is different to the one we want to modify, but cat/name is there, its a duplicate

            mc = GetCatFDName(null, fdname);                            // now pick it up by its primary key for frontier, the fdname

            if (mc != null)
            {
                mc.name = name;
                mc.shortname = abv;
                mc.category = cat;
                mc.type = type;
                mc.flags = 1;
                mc.Update();
                return true;
            }
            else
                return false;
        }

        public static void SetUpInitialTable()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                AddNewTypeC(cn, MaterialRawCategory, Color.Red, "Antimony", "Very Rare", "Sb");
                AddNewTypeC(cn, MaterialRawCategory, Color.Red, "Polonium", "Very Rare", "Po");
                AddNewTypeC(cn, MaterialRawCategory, Color.Red, "Ruthenium", "Very Rare", "Ru");
                AddNewTypeC(cn, MaterialRawCategory, Color.Red, "Technetium", "Very Rare", "Tc");
                AddNewTypeC(cn, MaterialRawCategory, Color.Red, "Tellurium", "Very Rare", "Te");
                AddNewTypeC(cn, MaterialRawCategory, Color.Red, "Yttrium", "Very Rare", "Y");

                AddNewTypeC(cn, MaterialRawCategory, Color.Yellow, "Cadmium", "Rare", "Cd");
                AddNewTypeC(cn, MaterialRawCategory, Color.Yellow, "Mercury", "Rare", "Hg");
                AddNewTypeC(cn, MaterialRawCategory, Color.Yellow, "Molybdenum", "Rare", "Mo");
                AddNewTypeC(cn, MaterialRawCategory, Color.Yellow, "Niobium", "Rare", "Nb");
                AddNewTypeC(cn, MaterialRawCategory, Color.Yellow, "Tin", "Rare", "Sn");
                AddNewTypeC(cn, MaterialRawCategory, Color.Yellow, "Tungsten", "Rare", "W");

                AddNewTypeC(cn, MaterialRawCategory, Color.Cyan, "Carbon", "Very Common", "C");
                AddNewTypeC(cn, MaterialRawCategory, Color.Cyan, "Iron", "Very Common", "Fe");
                AddNewTypeC(cn, MaterialRawCategory, Color.Cyan, "Nickel", "Very Common", "Ni");
                AddNewTypeC(cn, MaterialRawCategory, Color.Cyan, "Phosphorus", "Very Common", "P");
                AddNewTypeC(cn, MaterialRawCategory, Color.Cyan, "Sulphur", "Very Common", "S");

                AddNewTypeC(cn, MaterialRawCategory, Color.Green, "Arsenic", "Common", "As");
                AddNewTypeC(cn, MaterialRawCategory, Color.Green, "Chromium", "Common", "Cr");
                AddNewTypeC(cn, MaterialRawCategory, Color.Green, "Germanium", "Common", "Ge");
                AddNewTypeC(cn, MaterialRawCategory, Color.Green, "Manganese", "Common", "Mn");
                AddNewTypeC(cn, MaterialRawCategory, Color.Green, "Selenium", "Common", "Se");
                AddNewTypeC(cn, MaterialRawCategory, Color.Green, "Vanadium", "Common", "V");
                AddNewTypeC(cn, MaterialRawCategory, Color.Green, "Zinc", "Common", "Zn");
                AddNewTypeC(cn, MaterialRawCategory, Color.Green, "Zirconium", "Common", "Zr");

                AddNewType(cn, CommodityCategory, "Explosives;Hydrogen Fuel;Hydrogen Peroxide;Liquid Oxygen;Mineral Oil;Nerve Agents;Pesticides;Surface Stabilisers;Synthetic Reagents;Water", "Chemicals");
                AddNewType(cn, CommodityCategory, "Clothing;Consumer Technology;Domestic Appliances;Evacuation Shelter;Survival Equipment", "Consumer Items");
                AddNewType(cn, CommodityCategory, "Algae;Animal Meat;Coffee;Fish;Food Cartridges;Fruit and Vegetables;Grain;Synthetic Meat;Tea", "Foods");
                AddNewType(cn, CommodityCategory, "Ceramic Composites;CMM Composite;Insulating Membrane;Meta-Alloys;Micro-Weave Cooling Hoses;Neofabric Insulation;Polymers;Semiconductors;Superconductors", "Industrial Materials");
                AddNewType(cn, CommodityCategory, "Beer;Bootleg Liquor;Liquor;Narcotics;Tobacco;Wine", "Legal Drugs");
                AddNewType(cn, CommodityCategory, "Articulation Motors;Atmospheric Processors;Building Fabricators;Crop Harvesters;Emergency Power Cells;Energy Grid Assembly;Exhaust Manifold;Geological Equipment", "Machinery");
                AddNewType(cn, CommodityCategory, "Heatsink Interlink;HN Shock Mount;Ion Distributor;Magnetic Emitter Coil;Marine Equipment", "Machinery");
                AddNewType(cn, CommodityCategory, "Microbial Furnaces;Mineral Extractors;Modular Terminals;Power Converter;Power Generators;Power Transfer Bus", "Machinery");
                AddNewType(cn, CommodityCategory, "Radiation Baffle;Reinforced Mounting Plate;Skimmer Components;Thermal Cooling Units;Water Purifiers", "Machinery");
                AddNewType(cn, CommodityCategory, "Advanced Medicines;Agri-Medicines;Basic Medicines;Combat Stabilisers;Performance Enhancers;Progenitor Cells", "Medicines");
                AddNewType(cn, CommodityCategory, "Aluminium;Beryllium;Bismuth;Cobalt;Copper;Gallium;Gold;Hafnium 178;Indium;Lan;hanum;Lithium;Osmium;Palladium;Platinum;Praseodymium;Samarium;Silver;Tantalum;Thallium;Thorium;Titanium;Uranium", "Metals");
                AddNewType(cn, CommodityCategory, "Bauxite;Bertrandite;Bromellite;Coltan;Cryolite;Gallite;Goslarite", "Minerals");
                AddNewType(cn, CommodityCategory, "Indite;Jadeite;Lepidolite;Lithium Hydroxide;Low Temperature Diamonds;Methane ;lathrate;Methanol Monohydrate;Moissanite;Painite;Pyrophyllite;Rutile;Taaffeite;Uraninite", "Minerals");
                AddNewType(cn, CommodityCategory, "Ai Relics;Ancient Artefact;Antimatter Containment Unit;Antiquities;Assault Plans;Black Box;Commercial Samples;Data Core;Diplomatic Bag;Encrypted Correspondence;Encrypted Data Storage;Experimental Chemicals;Fossil Remnants", "Salvage");
                AddNewType(cn, CommodityCategory, "Galactic Travel Guide;Geological Samples;Hostage;Military Intelligence;Military Plans (USS Cargo);Mysterious Idol;Occupied CryoPod;Occupied Escape Pod;Personal Effects;Political Prisoner;Precious Gems;Prohibited Research Materials;Prototype Tech", "Salvage");
                AddNewType(cn, CommodityCategory, "Rare Artwork;Rebel Transmissions;Salvageable Wreckage;Sap 8 Core Container;Sc;entific Research;Scientific Samples;Space Pioneer Relics;Tactical Data;Technical Blueprints;Trade Data;Unknown Artefact;Unknown Probe;Unstable Data Core", "Salvage");
                AddNewType(cn, CommodityCategory, "Imperial Slaves;Slaves", "Slavery");
                AddNewType(cn, CommodityCategory, "Advanced Catalysers;Animal Monitors;Aquaponic Systems;Auto-Fabricators;Bioreducing Lichen;Computer Components", "Technology");
                AddNewType(cn, CommodityCategory, "H.E. Suits;Hardware Diagnostic Sensor;Land Enrichment Systems;Medical Diagnostic Equipment;Micro Controllers;Muon Imager", "Technology");
                AddNewType(cn, CommodityCategory, "Nanobreakers;Resonating Separators;Robotics;Structural Regulators;Telemetry Suite", "Technology");
                AddNewType(cn, CommodityCategory, "Conductive Fabrics;Leather;Military Grade Fabrics;Natural Fabrics;Synthetic Fabrics", "Textiles");
                AddNewType(cn, CommodityCategory, "Biowaste;Chemical Waste;Scrap;Toxic Waste", "Waste");
                AddNewType(cn, CommodityCategory, "Battle Weapons;Landmines;Non-lethal Weapons;Personal Weapons;Reactive Armour", "Weapons");

                AddNewType(cn, "Encoded", "Scan Data Banks;Disrupted Wake Echoes;Datamined Wake;Hyperspace Trajectories;Wake Solutions", "");
                AddNewType(cn, "Encoded", "Shield Density Reports;Shield Pattern Analysis;Shield Cycle Recordings;Emission Data;Bulk Scan Data;Consumer Firmware;Shield Soak Analysis;Legacy Firmware", "");
                AddNewType(cn, "Encoded", "Aberrant Shield Pattern Analysis;Abnormal Compact Emission Data;Adaptive Encryptors Capture;", "");
                AddNewType(cn, "Encoded", "Anomalous Bulk Scan Data;Anomalous FSD Telemetry;Atypical Disrupted Wake Echoes;Atypical Encryption Archives;Classified Scan Databanks", "");
                AddNewType(cn, "Encoded", "Classified Scan Fragment;Cracked Industrial Firmware;Datamined Wake Exceptions;Decoded Emission Data;Distorted Shield Cycle Recordings", "");
                AddNewType(cn, "Encoded", "Divergent Scan Data;Eccentric Hyperspace Trajectories;Exceptional Scrambled Emission Data;Inconsistent Shield Soak Analysis;Irregular Emission Data", "");
                AddNewType(cn, "Encoded", "Modified Consumer Firmware;Modified Embedded Firmware;Open Symmetric Keys;Peculiar Shield Frequency Data;Security Firmware Patch;Specialised Legacy Firmware", "");
                AddNewType(cn, "Encoded", "Strange Wake Solutions;Tagged Encryption Codes;Unexpected Emission Data;Unidentified Scan Archives;Untypical Shield Scans;Unusual Encrypted Files", "");

                AddNewType(cn, "Manufactured", "Uncut Focus Crystals;Basic Conductors;Biotech Conductors;Chemical Distillery;Chemical Manipulators;Chemical Processors;Chemical Storage Units", "");
                AddNewType(cn, "Manufactured", "Refined Focus Crystals;Compact Composites;Compound Shielding;Conductive Ceramics;Conductive Components;Conductive Polymers;Configurable Components", "");
                AddNewType(cn, "Manufactured", "Core Dynamics Composites;Crystal Shards;Electrochemical Arrays;Exquisite Focus Crystals;Filament Composites;Flawed Focus Crystals", "");
                AddNewType(cn, "Manufactured", "Focus Crystals;Galvanising Alloys;Grid Resistors;Heat Conduction Wiring;Heat Dispersion Plate;Heat Exchangers;Heat Resistant Ceramics", "");
                AddNewType(cn, "Manufactured", "Heat Vanes;High Density Composites;Hybrid Capacitors;Imperial Shielding;Improvised Components;Mechanical Components;Mechanical Equipment", "");
                AddNewType(cn, "Manufactured", "Mechanical Scrap;Military Grade Alloys;Military Supercapacitors;Pharmaceutical Isolators;Phase Alloys;Polymer Capacitors;Precipitated Alloys", "");
                AddNewType(cn, "Manufactured", "Proprietary Composites;Proto Heat Radiators;Proto Light Alloys;Proto Radiolic Alloys;Salvaged Alloys", "");
                AddNewType(cn, "Manufactured", "Shield Emitters;Shielding Sensors;Tempered Alloys;Thermic Alloys;Unknown Fragment;Worn Shield Emitters", "");

                loadedlist = GetAll(cn);
            }
        }

        static List<MaterialCommodities> loadedlist;

        public static void LoadCacheList()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                loadedlist = GetAll(cn);
            }
        }

        public static MaterialCommodities GetCachedMaterial(string fdname)
        {
            return loadedlist?.Find(x => x.fdname.Equals(fdname, StringComparison.InvariantCultureIgnoreCase));
        }

        public static List<MaterialCommodities> GetMaterialsCommoditiesList { get { return loadedlist; } }
    }


    public class MaterialCommoditiesList
    {
        private List<MaterialCommodities> list;

        public MaterialCommoditiesList()
        {
            list = new List<MaterialCommodities>();
        }

        public MaterialCommoditiesList Clone( bool clearzeromaterials, bool clearzerocommodities )       // returns a new copy of this class.. all items a copy
        {
            MaterialCommoditiesList mcl = new MaterialCommoditiesList();

            mcl.list = new List<MaterialCommodities>(list.Count);
            list.ForEach(item => 
            {
                bool commodity = item.category.Equals(MaterialCommodities.CommodityCategory);
                    // if items, or commodity and not clear zero, or material and not clear zero, add
                if ( item.count > 0 || ( commodity && !clearzerocommodities) || ( !commodity && !clearzeromaterials ))
                    mcl.list.Add(new MaterialCommodities(item));
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

        private MaterialCommodities EnsurePresent(string cat, string fdname, SQLiteConnectionUser conn , bool ignorecatonsearch = false)
        {
            MaterialCommodities mc = list.Find(x => x.fdname.Equals(fdname, StringComparison.InvariantCultureIgnoreCase) && (ignorecatonsearch || x.category.Equals(cat, StringComparison.InvariantCultureIgnoreCase)));

            if (mc == null)
            {
                MaterialCommodities mcdb = MaterialCommodities.GetCatFDName(cat, fdname, conn);    // look up in DB and see if we have a record of this type of item

                if (mcdb == null)             // no record of this, add as Unknown to db
                {
                    mcdb = new MaterialCommodities(0,cat, fdname, fdname, "","",Color.Green,0);
                    mcdb.Add();                 // and add to data base
                }

                mc = new MaterialCommodities(0,mcdb.category, mcdb.name, mcdb.fdname, mcdb.type,mcdb.shortname , mcdb.colour,0);        // make a new entry
                list.Add(mc);
            }

            return mc;
        }

        // ignore cat is only used if you don't know what it is 
        public void Change(string cat, string name, int num, long price, SQLiteConnectionUser conn, bool ignorecatonsearch = false)
        {
            MaterialCommodities mc = EnsurePresent(cat, name, conn, ignorecatonsearch);

            double costprev = mc.count * mc.price;
            double costnew = num * price;
            mc.count = Math.Max(mc.count + num, 0); ;

            if ( mc.count > 0 && num > 0 )      // if bought (defensive with mc.count)
                mc.price = (costprev + costnew) / mc.count;       // price is now a combination of the current cost and the new cost. in case we buy in tranches
        }

        public void Craft(string name, int num)
        {
            MaterialCommodities mc = list.Find(x => x.fdname.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if ( mc != null )               // if we find it, we remove count, else we don't worry since we may have not got the bought/collected event
                mc.count = Math.Max(mc.count - num, 0);
        }

        public void Set(string cat, string name, int num , double price , SQLiteConnectionUser conn, bool ignorecatonsearch = false)
        {
            MaterialCommodities mc = EnsurePresent(cat, name, conn);
            mc.count = num;
            if ( price>0 )
                mc.price = price;
        }

        static public MaterialCommoditiesList Process(JournalEntry je, MaterialCommoditiesList oldml, SQLiteConnectionUser conn , 
                                                        bool clearzeromaterials, bool clearzerocommodities)
        {
            MaterialCommoditiesList newmc = (oldml == null) ? new MaterialCommoditiesList() : oldml;

            Type jtype = JournalEntry.TypeOfJournalEntry(je.EventTypeStr);

            if (jtype != null)
            {
                System.Reflection.MethodInfo m = jtype.GetMethod("MaterialList"); // see if the class defines this function..

                if (m != null)                                      // event wants to change it
                {
                    newmc = newmc.Clone(clearzeromaterials,clearzerocommodities);          // so we need a new one

                    m.Invoke(Convert.ChangeType(je, jtype), new Object[] { newmc, conn });
                }
            }

            return newmc;
        }
    }

    public class MaterialCommoditiesLedger
    {
        public class Transaction
        {
            public long jid;
            public DateTime utctime;                               // when it was done.
            public JournalTypeEnum jtype;                          // what caused it..
            public string notes;                                   // notes about the entry
            public long cashadjust;                                // any profit, 0 if none (negative for cost, positive for profit)
            public double profitperunit;                             // profit per unit
            public long cash;                                      // cash total at this point

            public bool IsJournalEventInEventFilter(string[] events)
            {
                return events.Contains(EDDiscovery.Tools.SplitCapsWord(jtype.ToString()));
            }
        }

        private List<Transaction> transactions;
        public long CashTotal = 0;

        public MaterialCommoditiesLedger()
        {
            transactions = new List<Transaction>();
        }

        public List<Transaction> Transactions { get { return transactions; } }
        
        public MaterialCommodities GetMaterialCommodity(string cat, string fdname, SQLiteConnectionUser conn)
        {
            MaterialCommodities mcdb = MaterialCommodities.GetCatFDName(cat, fdname, conn);    // look up in DB and see if we have a record of this type of item

            MaterialCommodities mc = null;
            if (mcdb != null)
                mc = new MaterialCommodities(0, cat, mcdb.name, mcdb.fdname, mcdb.type, mcdb.shortname, Color.Green, 0);
            else
                mc = new MaterialCommodities(0, cat, fdname, fdname, "", "", Color.Green, 0);

            return mc;
        }

        public void AddEvent(long jidn, DateTime t, JournalTypeEnum j, string n, long? ca, double ppu = 0)
        {
            AddEventCash(jidn, t, j, n, ca.HasValue ? ca.Value : 0, ppu);
        }

        public void AddEventNoCash(long jidn, DateTime t, JournalTypeEnum j, string n)
        {
            AddEventCash(jidn, t, j, n, 0, 0);
        }

        public void AddEventCash(long jidn, DateTime t, JournalTypeEnum j, string n, long ca , double ppu = 0)
        {
            long newcashtotal = CashTotal + ca;
            //System.Diagnostics.Debug.WriteLine("{0} {1} {2} {3} = {4}", j.ToString(), n, CashTotal, ca , newcashtotal);
            CashTotal = newcashtotal;

            Transaction tr = new Transaction
            {
                jid = jidn,
                utctime = t,
                jtype = j,
                notes = n,
                cashadjust = ca,
                cash = CashTotal,
                profitperunit = ppu
            };

            transactions.Add(tr);
        }


        public void Process(JournalEntry je, SQLiteConnectionUser conn )
        {
            Type jtype = JournalEntry.TypeOfJournalEntry(je.EventTypeStr);

            if (jtype != null)
            {
                System.Reflection.MethodInfo m = jtype.GetMethod("Ledger"); // see if the class defines this function..

                if (m == null)
                    m = jtype.GetMethod("LedgerNC"); // see if the class defines this function..

                if (m!=null)
                    m.Invoke(Convert.ChangeType(je, jtype), new Object[] { this, conn });
            }
        }

    }
}
