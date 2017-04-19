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
                    AddNewTypeF(cn, c, cl, name, t, sn);
                }
            }
        }

        private static void AddNewTypeF(SQLiteConnectionUser cn, string c, Color cl, string name, string t, string sn = "", string fdName = "")
        {
            string fdn = (fdName.Length > 0) ? fdName : name.FDName();
            
            MaterialCommodityDB mc = GetCatFDName(null, fdn, cn);

            if (mc == null)
            {
                mc = new MaterialCommodityDB(0, c, name, fdn, t, sn, cl, 0);
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

                // very common data
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Cyan, "Anomalous Bulk Scan Data", "Very Common", "ABSD", "bulkscandata");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Cyan, "Atypical Disrupted Wake Echoes", "Very Common", "ADWE", "disruptedwakeechoes");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Cyan, "Distorted Shield Cycle Recordings", "Very Common", "DSCR", "shieldcyclerecordings");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Cyan, "Exceptional Scrambled Emission Data", "Very Common", "ESED", "scrambledemissiondata");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Cyan, "Specialised Legacy Firmware", "Very Common", "SLF", "legacyfirmware");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Cyan, "Unusual Encrypted Files", "Very Common", "UEF", "encryptedfiles");
                // common data
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Green, "Anomalous FSD Telemetry", "Common", "AFT", "fsdtelemetry");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Green, "Inconsistent Shield Soak Analysis", "Common", "ISSA", "shieldsoakanalysis");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Green, "Irregular Emission Data", "Common", "IED", "archivedemissiondata");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Green, "Modified Consumer Firmware", "Common", "MCF", "consumerfirmware");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Green, "Tagged Encryption Codes", "Common", "TEC", "encryptioncodes");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Green, "Unidentified Scan Archives", "Common", "USA", "scanarchives");
                // standard data
                AddNewTypeF(cn, MaterialEncodedCategory, Color.SandyBrown, "Classified Scan Databanks", "Standard", "CSD", "scandatabanks");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.SandyBrown, "Cracked Industrial Firmware", "Standard", "CIF", "industrialfirmware");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.SandyBrown, "Open Symmetric Keys", "Standard", "OSK", "symmetrickeys");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.SandyBrown, "Strange Wake Solutions", "Standard", "SWS", "wakesolutions");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.SandyBrown, "Unexpected Emission Data", "Standard", "UED", "emissiondata");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.SandyBrown, "Untypical Shield Scans", "Standard", "USS", "shielddensityreports");
                // rare data
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Yellow, "Aberrant Shield Pattern Analysis", "Rare", "ASPA", "shieldpatternanalysis");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Yellow, "Atypical Encryption Archives", "Rare", "AEA", "encryptionarchives");
                AddNewTypeC(cn, MaterialEncodedCategory, Color.Yellow, "Decoded Emission Data", "Rare", "DED");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Yellow, "Divergent Scan Data", "Rare", "DSD", "encodedscandata");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Yellow, "Eccentric Hyperspace Trajectories", "Rare", "EHT", "hyperspacetrajectories");
                AddNewTypeC(cn, MaterialEncodedCategory, Color.Yellow, "Pattern Alpha Obelisk Data", "Rare", "ODA");
                AddNewTypeC(cn, MaterialEncodedCategory, Color.Yellow, "Pattern Beta Obelisk Data", "Rare", "ODB");
                AddNewTypeC(cn, MaterialEncodedCategory, Color.Yellow, "Pattern Gamma Obelisk Data", "Rare", "ODG");
                AddNewTypeC(cn, MaterialEncodedCategory, Color.Yellow, "Pattern Delta Obelisk Data", "Rare", "ODD");
                AddNewTypeC(cn, MaterialEncodedCategory, Color.Yellow, "Pattern Epsilon Obelisk Data", "Rare", "ODE");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Yellow, "Security Firmware Patch", "Rare", "SFP", "securityfirmware");
                AddNewTypeC(cn, MaterialEncodedCategory, Color.Yellow, "Unknown Ship Signature", "Rare", "USSig");
                AddNewTypeC(cn, MaterialEncodedCategory, Color.Yellow, "Unknown Wake Data", "Rare", "UWD");
                // very rare data
                AddNewTypeC(cn, MaterialEncodedCategory, Color.Red, "Abnormal Compact Emission Data", "Very Rare", "ACED");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Red, "Adaptive Encryptors Capture", "Very Rare", "AEC", "adaptiveencryptors");
                AddNewTypeC(cn, MaterialEncodedCategory, Color.Red, "Classified Scan Fragment", "Very Rare", "CSF");
                AddNewTypeF(cn, MaterialEncodedCategory, Color.Red, "Datamined Wake Exceptions", "Very Rare", "DWEx", "dataminedwake");
                AddNewTypeC(cn, MaterialEncodedCategory, Color.Red, "Modified Embedded Firmware", "Very Rare", "MEF");
                AddNewTypeC(cn, MaterialEncodedCategory, Color.Red, "Peculiar Shield Frequency Data", "Very Rare", "PSFD");

                //very common manufactured
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Cyan, "Basic Conductors", "Very Common", "BaC");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Cyan, "Chemical Storage Units", "Very Common", "CSU");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Cyan, "Compact Composites", "Very Common", "CC");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Cyan, "Crystal Shards", "Very Common", "CS");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Cyan, "Grid Resistors", "Very Common", "GR");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Cyan, "Heat Conduction Wiring", "Very Common", "HCW");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Cyan, "Mechanical Scrap", "Very Common", "MS");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Cyan, "Salvaged Alloys", "Very Common", "SAll");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Cyan, "Tempered Alloys", "Very Common", "TeA");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Cyan, "Worn Shield Emitters", "Very Common", "WSE");
                // common manufactured
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Green, "Chemical Processors", "Common", "CP");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Green, "Conductive Components", "Common", "CCo");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Green, "Filament Composites", "Common", "FiC");
                AddNewTypeF(cn, MaterialManufacturedCategory, Color.Green, "Flawed Focus Crystals", "Common", "FFC", "uncutfocuscrystals");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Green, "Galvanising Alloys", "Common", "GA");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Green, "Heat Dispersion Plate", "Common", "HDP");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Green, "Heat Resistant Ceramics", "Common", "HRC");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Green, "Hybrid Capacitors", "Common", "HC");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Green, "Mechanical Equipment", "Common", "ME");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Green, "Shield Emitters", "Common", "SE");
                // standard manufactured
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.SandyBrown, "Chemical Distillery", "Standard", "CD");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.SandyBrown, "Conductive Ceramics", "Standard", "CCe");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.SandyBrown, "Electrochemical Arrays", "Standard", "EA");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.SandyBrown, "Focus Crystals", "Standard", "FoC");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.SandyBrown, "Heat Exchangers", "Standard", "HE");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.SandyBrown, "High Density Composites", "Standard", "HDC");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.SandyBrown, "Mechanical Components", "Standard", "MC");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.SandyBrown, "Phase Alloys", "Standard", "PA");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.SandyBrown, "Precipitated Alloys", "Standard", "PAll");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.SandyBrown, "Shielding Sensors", "Standard", "SS");
                // rare manufactured
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Yellow, "Chemical Manipulators", "Rare", "CM");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Yellow, "Compound Shielding", "Rare", "CoS");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Yellow, "Conductive Polymers", "Rare", "CPo");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Yellow, "Configurable Components", "Rare", "CCom");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Yellow, "Heat Vanes", "Rare", "HV");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Yellow, "Polymer Capacitors", "Rare", "PCa");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Yellow, "Proprietary Composites", "Rare", "PCo");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Yellow, "Proto Light Alloys", "Rare", "PLA");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Yellow, "Refined Focus Crystals", "Rare", "RFC");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Yellow, "Thermic Alloys", "Rare", "ThA");
                // very rare manufactured
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Red, "Biotech Conductors", "Very Rare", "BiC");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Red, "Core Dynamics Composites", "Very Rare", "CDC");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Red, "Exquisite Focus Crystals", "Very Rare", "EFC");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Red, "Imperial Shielding", "Very Rare", "IS");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Red, "Improvised Components", "Very Rare", "IC");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Red, "Military Grade Alloys", "Very Rare", "MGA");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Red, "Military Supercapacitors", "Very Rare", "MSC");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Red, "Pharmaceutical Isolators", "Very Rare", "PI");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Red, "Proto Heat Radiators", "Very Rare", "PHR");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Red, "Proto Radiolic Alloys", "Very Rare", "PRA");
                AddNewTypeC(cn, MaterialManufacturedCategory, Color.Red, "Unknown Fragment", "Very Rare", "UF");
                

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


