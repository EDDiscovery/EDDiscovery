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

namespace EDDiscovery.DB
{
   // [System.Diagnostics.DebuggerDisplay("MatDB {category} {name} {fdname} {type} {shortname}")]
    public class MaterialCommodityDB
    {
        public long id { get; set; }
        public string category { get; set; }                // either Commodity, or one of the Category types from the MaterialCollected type.
        public string name { get; set; }                    // name of it in nice text
        public string fdname { get; set; }                  // fdnames
        public string type { get; set; }                    // and its type, for materials its rarity, for commodities its group ("Metals" etc).
        public string shortname { get; set; }               // short abv. name
        public Color colour { get; set; }                   // colour if its associated with one
        public int flags { get; set; }                      // 0 is automatically set, 1 is user edited (so don't override)

        public static string CommodityCategory = "Commodity";
        public static string MaterialRawCategory = "Raw";
        public static string MaterialEncodedCategory = "Encoded";
        public static string MaterialManufacturedCategory = "Manufactured";

        private static Dictionary<string, MaterialCommodityDB> cachelist = new Dictionary<string, MaterialCommodityDB>();

        public static MaterialCommodityDB GetCachedMaterial(string fdname)
        {
            return cachelist.ContainsKey(fdname) ? cachelist[fdname] : null;
        }

        public static MaterialCommodityDB GetCachedMaterialByShortName(string shortname)
        {
            List<MaterialCommodityDB> lst = cachelist.Values.ToList();
            int i = lst.FindIndex(x => x.shortname.Equals(shortname));
            return i >= 0 ? lst[i] : null;
        }

        public MaterialCommodityDB()
        {
        }

        public MaterialCommodityDB(long i, string cs, string n, string fd, string t, string shortn, Color cl, int fl)
        {
            id = i;
            category = cs;
            name = n;
            fdname = fd;
            type = t;
            shortname = shortn;
            colour = cl;
            flags = fl;
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

                cachelist[this.fdname.ToLower()] = this;
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
                cachelist[this.fdname.ToLower()] = this;
                return true;
            }
        }

        private bool Delete()
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
                cachelist.Remove(this.fdname.ToLower());
                return true;
            }
        }

        public static MaterialCommodityDB EnsurePresent(string cat, string fdname, SQLiteConnectionUser conn )  // By FDNAME
        {
            if (!cachelist.ContainsKey(fdname.ToLower()))
            {
                MaterialCommodityDB mcdb = new MaterialCommodityDB(0, cat, fdname.SplitCapsWordFull(), fdname, "", "", Color.Green, 0);
                //System.Diagnostics.Debug.WriteLine("Add new db " + cat + ": fdname=" + fdname + " " + mcdb.name);
                mcdb.Add();                 // and add to data base, and it adds to cache..
            }
            return cachelist[fdname.ToLower()];
        }


        #region Initial setup

        private static MaterialCommodityDB GetCatFDName(string cat, string fdname)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(mode: EDDbAccessMode.Reader))
            {
                return GetCatFDName(cat, fdname, cn);
            }
        }

        // if cat is null, fdname only
        private static MaterialCommodityDB GetCatFDName(string cat, string fdname, SQLiteConnectionUser cn)
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
                        return new MaterialCommodityDB((long)reader[0], (string)reader[1], (string)reader[2], (string)reader[3], (string)reader[4], (string)reader[5], Color.FromArgb((int)reader[6]), (int)reader[7]);
                    }
                    else
                        return null;
                }
            }
        }

        private static void AddNewType(SQLiteConnectionUser cn, string c, string namelist, string t)
        {
            AddNewTypeC(cn, c, Color.Green, namelist, t);
        }

        private static void AddNewTypeC(SQLiteConnectionUser cn, string c, Color cl, string namelist, string t, string sn = "")
        {
            string[] list = namelist.Split(';');

            foreach (string name in list)
            {
                if (name.Length > 0)   // just in case a semicolon slips thru
                {
                    string fdname = name.FDName();

                    MaterialCommodityDB mc = GetCatFDName(null, fdname, cn);

                    if (mc == null)
                    {
                        mc = new MaterialCommodityDB(0, c, name, fdname, t, sn, cl, 0);
                        mc.Add(cn);
                    }               // don't change any user changed fields
                    else if (mc.flags == 0 && (!mc.name.Equals(name) || !mc.shortname.Equals(sn) || !mc.category.Equals(c) || !mc.type.Equals(t) || !mc.colour.IsEqual(cl)))
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
                AddNewType(cn, CommodityCategory, "Beer;Bootleg Liquor;Liquor;Narcotics;Tobacco;Wine;Lavian Brandy", "Legal Drugs");
                AddNewType(cn, CommodityCategory, "Articulation Motors;Atmospheric Processors;Building Fabricators;Crop Harvesters;Emergency Power Cells;Energy Grid Assembly;Exhaust Manifold;Geological Equipment", "Machinery");
                AddNewType(cn, CommodityCategory, "Heatsink Interlink;HN Shock Mount;Ion Distributor;Magnetic Emitter Coil;Marine Equipment", "Machinery");
                AddNewType(cn, CommodityCategory, "Microbial Furnaces;Mineral Extractors;Modular Terminals;Power Converter;Power Generators;Power Transfer Bus", "Machinery");
                AddNewType(cn, CommodityCategory, "Radiation Baffle;Reinforced Mounting Plate;Skimmer Components;Thermal Cooling Units;Water Purifiers", "Machinery");
                AddNewType(cn, CommodityCategory, "Advanced Medicines;Agri-Medicines;Basic Medicines;Combat Stabilisers;Performance Enhancers;Progenitor Cells", "Medicines");
                AddNewType(cn, CommodityCategory, "Aluminium;Beryllium;Bismuth;Cobalt;Copper;Gallium;Gold;Hafnium 178;Indium;Lan;hanum;Lithium;Osmium;Palladium;Platinum;Praseodymium;Samarium;Silver;Tantalum;Thallium;Thorium;Titanium;Uranium", "Metals");
                AddNewType(cn, CommodityCategory, "Bauxite;Bertrandite;Bromellite;Coltan;Cryolite;Gallite;Goslarite", "Minerals");
                AddNewType(cn, CommodityCategory, "Indite;Jadeite;Lepidolite;Lithium Hydroxide;Low Temperature Diamonds;Methane ;lathrate;Methanol Monohydrate;Moissanite;Painite;Pyrophyllite;Rutile;Taaffeite;Uraninite", "Minerals");
                AddNewType(cn, CommodityCategory, "Ai Relics;Ancient Artefact;Antimatter Containment Unit;Antiquities;Assault Plans;Black Box;Commercial Samples;Data Core;Diplomatic Bag;Encrypted Correspondence;Encrypted Data Storage;Experimental Chemicals;Fossil Remnants;Ancient Orb;Ancient Casket;Ancient Relic", "Salvage");
                AddNewType(cn, CommodityCategory, "Galactic Travel Guide;Geological Samples;Hostage;Military Intelligence;Military Plans (USS Cargo);Mysterious Idol;Occupied CryoPod;Occupied Escape Pod;Personal Effects;Political Prisoner;Precious Gems;Prohibited Research Materials;Prototype Tech", "Salvage");
                AddNewType(cn, CommodityCategory, "Rare Artwork;Rebel Transmissions;Salvageable Wreckage;Sap 8 Core Container;Scientific Research;Scientific Samples;Space Pioneer Relics;Tactical Data;Technical Blueprints;Trade Data;Unknown Artefact;Unknown Probe;Unstable Data Core", "Salvage");
                AddNewType(cn, CommodityCategory, "Imperial Slaves;Slaves", "Slavery");
                AddNewType(cn, CommodityCategory, "Advanced Catalysers;Animal Monitors;Aquaponic Systems;Auto-Fabricators;Bioreducing Lichen;Computer Components", "Technology");
                AddNewType(cn, CommodityCategory, "H.E. Suits;Hardware Diagnostic Sensor;Land Enrichment Systems;Medical Diagnostic Equipment;Micro Controllers;Muon Imager", "Technology");
                AddNewType(cn, CommodityCategory, "Nanobreakers;Resonating Separators;Robotics;Structural Regulators;Telemetry Suite", "Technology");
                AddNewType(cn, CommodityCategory, "Conductive Fabrics;Leather;Military Grade Fabrics;Natural Fabrics;Synthetic Fabrics", "Textiles");
                AddNewType(cn, CommodityCategory, "Biowaste;Chemical Waste;Scrap;Toxic Waste", "Waste");
                AddNewType(cn, CommodityCategory, "Battle Weapons;Landmines;Non-lethal Weapons;Personal Weapons;Reactive Armour", "Weapons");

                AddNewType(cn, MaterialEncodedCategory, "Scan Data Banks;Disrupted Wake Echoes;Datamined Wake;Hyperspace Trajectories;Wake Solutions", "");
                AddNewType(cn, MaterialEncodedCategory, "Shield Density Reports;Shield Pattern Analysis;Shield Cycle Recordings;Emission Data;Bulk Scan Data;Consumer Firmware;Shield Soak Analysis;Legacy Firmware", "");
                AddNewType(cn, MaterialEncodedCategory, "Aberrant Shield Pattern Analysis;Abnormal Compact Emission Data;Adaptive Encryptors Capture;", "");
                AddNewType(cn, MaterialEncodedCategory, "Anomalous Bulk Scan Data;Anomalous FSD Telemetry;Atypical Disrupted Wake Echoes;Atypical Encryption Archives;Classified Scan Databanks", "");
                AddNewType(cn, MaterialEncodedCategory, "Classified Scan Fragment;Cracked Industrial Firmware;Datamined Wake Exceptions;Decoded Emission Data;Distorted Shield Cycle Recordings", "");
                AddNewType(cn, MaterialEncodedCategory, "Divergent Scan Data;Eccentric Hyperspace Trajectories;Exceptional Scrambled Emission Data;Inconsistent Shield Soak Analysis;Irregular Emission Data", "");
                AddNewType(cn, MaterialEncodedCategory, "Modified Consumer Firmware;Modified Embedded Firmware;Open Symmetric Keys;Peculiar Shield Frequency Data;Security Firmware Patch;Specialised Legacy Firmware", "");
                AddNewType(cn, MaterialEncodedCategory, "Strange Wake Solutions;Tagged Encryption Codes;Unexpected Emission Data;Unidentified Scan Archives;Untypical Shield Scans;Unusual Encrypted Files", "");
                AddNewType(cn, MaterialEncodedCategory, "Archived Emission Data;Encoded Scan Data;Scrambled Emission Data;Scan Archives;Encryption Codes", "");

                AddNewType(cn, MaterialManufacturedCategory, "Uncut Focus Crystals;Basic Conductors;Biotech Conductors;Chemical Distillery;Chemical Manipulators;Chemical Processors;Chemical Storage Units", "");
                AddNewType(cn, MaterialManufacturedCategory, "Core Dynamics Composites;Crystal Shards;Electrochemical Arrays;Exquisite Focus Crystals;Filament Composites;Flawed Focus Crystals", "");
                AddNewType(cn, MaterialManufacturedCategory, "Refined Focus Crystals;Compact Composites;Compound Shielding;Conductive Ceramics;Conductive Components;Conductive Polymers;Configurable Components", "");
                AddNewType(cn, MaterialManufacturedCategory, "Focus Crystals;Galvanising Alloys;Grid Resistors;Heat Conduction Wiring;Heat Dispersion Plate;Heat Exchangers;Heat Resistant Ceramics", "");
                AddNewType(cn, MaterialManufacturedCategory, "Heat Vanes;High Density Composites;Hybrid Capacitors;Imperial Shielding;Improvised Components;Mechanical Components;Mechanical Equipment", "");
                AddNewType(cn, MaterialManufacturedCategory, "Mechanical Scrap;Military Grade Alloys;Military Supercapacitors;Pharmaceutical Isolators;Phase Alloys;Polymer Capacitors;Precipitated Alloys", "");
                AddNewType(cn, MaterialManufacturedCategory, "Proprietary Composites;Proto Heat Radiators;Proto Light Alloys;Proto Radiolic Alloys;Salvaged Alloys", "");
                AddNewType(cn, MaterialManufacturedCategory, "Shield Emitters;Shielding Sensors;Tempered Alloys;Thermic Alloys;Unknown Fragment;Worn Shield Emitters", "");

                using (DbCommand cmd = cn.CreateCommand("select Id,Category,Name,FDName,Type,ShortName,Colour,Flags from MaterialsCommodities Order by Name"))
                {
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())           
                        {
                            MaterialCommodityDB mcdb = new MaterialCommodityDB((long)reader[0], (string)reader[1], (string)reader[2], (string)reader[3], (string)reader[4], (string)reader[5], Color.FromArgb((int)reader[6]), (int)reader[7]);
                            cachelist[mcdb.fdname.ToLower()] = mcdb;      // load up cache..
                           // System.Diagnostics.Debug.WriteLine("Cache " + mcdb.category + ": fd=" + mcdb.fdname + " name=" + mcdb.name);
                        }
                    }
                }
            }
        }

        #endregion
    }
}


