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

namespace EDDiscovery.EliteDangerous
{
    // [System.Diagnostics.DebuggerDisplay("MatDB {category} {name} {fdname} {type} {shortname}")]
    public class MaterialCommodityDB
    {
        public string category { get; set; }                // either Commodity, or one of the Category types from the MaterialCollected type.
        public string name { get; set; }                    // name of it in nice text
        public string fdname { get; set; }                  // fdnames
        public string type { get; set; }                    // and its type, for materials its rarity, for commodities its group ("Metals" etc).
        public string shortname { get; set; }               // short abv. name
        public Color colour { get; set; }                   // colour if its associated with one
        public int flags { get; set; }                      // now out of date.. keep for now to limit code changes

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

        public MaterialCommodityDB(string cs, string n, string fd, string t, string shortn, Color cl, int fl)
        {
            category = cs;
            name = n;
            fdname = fd;
            type = t;
            shortname = shortn;
            colour = cl;
            flags = fl;
        }

        public void SetCache()
        {
            cachelist[this.fdname.ToLower()] = this;
        }

        public static MaterialCommodityDB EnsurePresent(string cat, string fdname, SQLiteConnectionUser conn)  // By FDNAME
        {
            if (!cachelist.ContainsKey(fdname.ToLower()))
            {
                MaterialCommodityDB mcdb = new MaterialCommodityDB(cat, fdname.SplitCapsWordFull(), fdname, "", "", Color.Green, 0);
                mcdb.SetCache();
                System.Diagnostics.Debug.WriteLine("Material not present: " + cat + "," + fdname );
            }

            return cachelist[fdname.ToLower()];
        }


        #region Initial setup

        private static bool AddNewType(string c, string namelist, string t)
        {
            return AddNewTypeC(c, Color.Green, namelist, t);
        }

        private static bool AddNewTypeC(string c, Color cl, string namelist, string t, string sn = "")
        {
            string[] list = namelist.Split(';');

            foreach (string name in list)
            {
                if (name.Length > 0)   // just in case a semicolon slips thru
                {
                    if (!AddNewTypeF(c, cl, name, t, sn))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool AddNewTypeF(string c, Color cl, string name, string t, string sn = "", string fdName = "")
        {
            string fdn = (fdName.Length > 0) ? fdName : name.FDName();
            MaterialCommodityDB mc = new MaterialCommodityDB(c, name, fdn, t, sn, cl, 0);
            mc.SetCache();
            return true;
        }

        public static void SetUpInitialTable()
        {
            AddNewTypeC(MaterialRawCategory, Color.Red, "Antimony", "Very Rare", "Sb");
            AddNewTypeC(MaterialRawCategory, Color.Red, "Polonium", "Very Rare", "Po");
            AddNewTypeC(MaterialRawCategory, Color.Red, "Ruthenium", "Very Rare", "Ru");
            AddNewTypeC(MaterialRawCategory, Color.Red, "Technetium", "Very Rare", "Tc");
            AddNewTypeC(MaterialRawCategory, Color.Red, "Tellurium", "Very Rare", "Te");
            AddNewTypeC(MaterialRawCategory, Color.Red, "Yttrium", "Very Rare", "Y");

            AddNewTypeC(MaterialRawCategory, Color.Yellow, "Cadmium", "Rare", "Cd");
            AddNewTypeC(MaterialRawCategory, Color.Yellow, "Mercury", "Rare", "Hg");
            AddNewTypeC(MaterialRawCategory, Color.Yellow, "Molybdenum", "Rare", "Mo");
            AddNewTypeC(MaterialRawCategory, Color.Yellow, "Niobium", "Rare", "Nb");
            AddNewTypeC(MaterialRawCategory, Color.Yellow, "Tin", "Rare", "Sn");
            AddNewTypeC(MaterialRawCategory, Color.Yellow, "Tungsten", "Rare", "W");

            AddNewTypeC(MaterialRawCategory, Color.Cyan, "Carbon", "Very Common", "C");
            AddNewTypeC(MaterialRawCategory, Color.Cyan, "Iron", "Very Common", "Fe");
            AddNewTypeC(MaterialRawCategory, Color.Cyan, "Nickel", "Very Common", "Ni");
            AddNewTypeC(MaterialRawCategory, Color.Cyan, "Phosphorus", "Very Common", "P");
            AddNewTypeC(MaterialRawCategory, Color.Cyan, "Sulphur", "Very Common", "S");

            AddNewTypeC(MaterialRawCategory, Color.Green, "Arsenic", "Common", "As");
            AddNewTypeC(MaterialRawCategory, Color.Green, "Chromium", "Common", "Cr");
            AddNewTypeC(MaterialRawCategory, Color.Green, "Germanium", "Common", "Ge");
            AddNewTypeC(MaterialRawCategory, Color.Green, "Manganese", "Common", "Mn");
            AddNewTypeC(MaterialRawCategory, Color.Green, "Selenium", "Common", "Se");
            AddNewTypeC(MaterialRawCategory, Color.Green, "Vanadium", "Common", "V");
            AddNewTypeC(MaterialRawCategory, Color.Green, "Zinc", "Common", "Zn");
            AddNewTypeC(MaterialRawCategory, Color.Green, "Zirconium", "Common", "Zr");

            AddNewType(CommodityCategory, "Explosives;Hydrogen Fuel;Hydrogen Peroxide;Liquid Oxygen;Mineral Oil;Nerve Agents;Pesticides;Surface Stabilisers;Synthetic Reagents;Water", "Chemicals");
            AddNewType(CommodityCategory, "Clothing;Consumer Technology;Domestic Appliances;Evacuation Shelter;Survival Equipment", "Consumer Items");
            AddNewType(CommodityCategory, "Algae;Animal Meat;Coffee;Fish;Food Cartridges;Fruit and Vegetables;Grain;Synthetic Meat;Tea", "Foods");
            AddNewType(CommodityCategory, "Ceramic Composites;CMM Composite;Insulating Membrane;Meta-Alloys;Micro-Weave Cooling Hoses;Cooling Hoses;Neofabric Insulation;Polymers;Semiconductors;Superconductors", "Industrial Materials");
            AddNewType(CommodityCategory, "Beer;Bootleg Liquor;Liquor;Narcotics;Tobacco;Wine;Lavian Brandy", "Legal Drugs");
            AddNewType(CommodityCategory, "Articulation Motors;Atmospheric Processors;Building Fabricators;Crop Harvesters;Emergency Power Cells;Energy Grid Assembly;Exhaust Manifold;Geological Equipment", "Machinery");
            AddNewType(CommodityCategory, "Heatsink Interlink;HN Shock Mount;Ion Distributor;Magnetic Emitter Coil;Marine Equipment", "Machinery");
            AddNewType(CommodityCategory, "Microbial Furnaces;Mineral Extractors;Modular Terminals;Power Converter;Power Generators;Power Transfer Bus", "Machinery");
            AddNewType(CommodityCategory, "Radiation Baffle;Reinforced Mounting Plate;Skimmer Components;Thermal Cooling Units;Water Purifiers", "Machinery");
            AddNewType(CommodityCategory, "Advanced Medicines;Agri-Medicines;Basic Medicines;Combat Stabilisers;Performance Enhancers;Progenitor Cells", "Medicines");
            AddNewType(CommodityCategory, "Aluminium;Beryllium;Bismuth;Cobalt;Copper;Gallium;Gold;Hafnium 178;Indium;Lan;hanum;Lithium;Osmium;Palladium;Platinum;Praseodymium;Samarium;Silver;Tantalum;Thallium;Thorium;Titanium;Uranium", "Metals");
            AddNewType(CommodityCategory, "Bauxite;Bertrandite;Bromellite;Coltan;Cryolite;Gallite;Goslarite", "Minerals");
            AddNewType(CommodityCategory, "Indite;Jadeite;Lepidolite;Lithium Hydroxide;Low Temperature Diamonds;Methane ;lathrate;Methanol Monohydrate;Moissanite;Painite;Pyrophyllite;Rutile;Taaffeite;Uraninite", "Minerals");
            AddNewType(CommodityCategory, "Ai Relics;Ancient Artefact;Antimatter Containment Unit;Antiquities;Assault Plans;Black Box;Commercial Samples;Data Core;Diplomatic Bag;Encrypted Correspondence;Encrypted Data Storage;Experimental Chemicals;Fossil Remnants;Ancient Orb;Ancient Casket;Ancient Relic", "Salvage");
            AddNewType(CommodityCategory, "Galactic Travel Guide;Geological Samples;Hostage;Military Intelligence;Military Plans (USS Cargo);Mysterious Idol;Occupied CryoPod;Occupied Escape Pod;Personal Effects;Political Prisoner;Precious Gems;Prohibited Research Materials;Prototype Tech", "Salvage");
            AddNewType(CommodityCategory, "Rare Artwork;Rebel Transmissions;Salvageable Wreckage;Sap 8 Core Container;Scientific Research;Scientific Samples;Space Pioneer Relics;Tactical Data;Technical Blueprints;Trade Data;Unknown Artefact;Unknown Probe;Unstable Data Core", "Salvage");
            AddNewType(CommodityCategory, "Imperial Slaves;Slaves", "Slavery");
            AddNewType(CommodityCategory, "Advanced Catalysers;Animal Monitors;Aquaponic Systems;Auto-Fabricators;Bioreducing Lichen;Computer Components", "Technology");
            AddNewType(CommodityCategory, "H.E. Suits;Hardware Diagnostic Sensor;Land Enrichment Systems;Medical Diagnostic Equipment;Micro Controllers;Muon Imager", "Technology");
            AddNewType(CommodityCategory, "Nanobreakers;Resonating Separators;Robotics;Structural Regulators;Telemetry Suite", "Technology");
            AddNewType(CommodityCategory, "Conductive Fabrics;Leather;Military Grade Fabrics;Natural Fabrics;Synthetic Fabrics", "Textiles");
            AddNewType(CommodityCategory, "Biowaste;Chemical Waste;Scrap;Toxic Waste", "Waste");
            AddNewType(CommodityCategory, "Battle Weapons;Landmines;Non-lethal Weapons;Personal Weapons;Reactive Armour", "Weapons");

            // very common data
            AddNewTypeF(MaterialEncodedCategory, Color.Cyan, "Anomalous Bulk Scan Data", "Very Common", "ABSD", "bulkscandata");
            AddNewTypeF(MaterialEncodedCategory, Color.Cyan, "Atypical Disrupted Wake Echoes", "Very Common", "ADWE", "disruptedwakeechoes");
            AddNewTypeF(MaterialEncodedCategory, Color.Cyan, "Distorted Shield Cycle Recordings", "Very Common", "DSCR", "shieldcyclerecordings");
            AddNewTypeF(MaterialEncodedCategory, Color.Cyan, "Exceptional Scrambled Emission Data", "Very Common", "ESED", "scrambledemissiondata");
            AddNewTypeF(MaterialEncodedCategory, Color.Cyan, "Specialised Legacy Firmware", "Very Common", "SLF", "legacyfirmware");
            AddNewTypeF(MaterialEncodedCategory, Color.Cyan, "Unusual Encrypted Files", "Very Common", "UEF", "encryptedfiles");
            // common data
            AddNewTypeF(MaterialEncodedCategory, Color.Green, "Anomalous FSD Telemetry", "Common", "AFT", "fsdtelemetry");
            AddNewTypeF(MaterialEncodedCategory, Color.Green, "Inconsistent Shield Soak Analysis", "Common", "ISSA", "shieldsoakanalysis");
            AddNewTypeF(MaterialEncodedCategory, Color.Green, "Irregular Emission Data", "Common", "IED", "archivedemissiondata");
            AddNewTypeF(MaterialEncodedCategory, Color.Green, "Modified Consumer Firmware", "Common", "MCF", "consumerfirmware");
            AddNewTypeF(MaterialEncodedCategory, Color.Green, "Tagged Encryption Codes", "Common", "TEC", "encryptioncodes");
            AddNewTypeF(MaterialEncodedCategory, Color.Green, "Unidentified Scan Archives", "Common", "USA", "scanarchives");
            // standard data
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Classified Scan Databanks", "Standard", "CSD", "scandatabanks");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Cracked Industrial Firmware", "Standard", "CIF", "industrialfirmware");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Open Symmetric Keys", "Standard", "OSK", "symmetrickeys");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Strange Wake Solutions", "Standard", "SWS", "wakesolutions");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Unexpected Emission Data", "Standard", "UED", "emissiondata");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Untypical Shield Scans", "Standard", "USS", "shielddensityreports");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Shield Frequency Data", "Standard", "SFD", "shieldfrequencydata");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Classified Scan Data", "Standard", "CFSD", "classifiedscandata");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Compact Emissions Data", "Standard", "CED", "compactemissionsdata");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Embedded Firmware", "Standard", "EFW", "embeddedfirmware");

            // rare data
            AddNewTypeF(MaterialEncodedCategory, Color.Yellow, "Aberrant Shield Pattern Analysis", "Rare", "ASPA", "shieldpatternanalysis");
            AddNewTypeF(MaterialEncodedCategory, Color.Yellow, "Atypical Encryption Archives", "Rare", "AEA", "encryptionarchives");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Decoded Emission Data", "Rare", "DED");
            AddNewTypeF(MaterialEncodedCategory, Color.Yellow, "Divergent Scan Data", "Rare", "DSD", "encodedscandata");
            AddNewTypeF(MaterialEncodedCategory, Color.Yellow, "Eccentric Hyperspace Trajectories", "Rare", "EHT", "hyperspacetrajectories");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Pattern Alpha Obelisk Data", "Rare", "ODA");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Pattern Beta Obelisk Data", "Rare", "ODB");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Pattern Gamma Obelisk Data", "Rare", "ODG");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Pattern Delta Obelisk Data", "Rare", "ODD");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Pattern Epsilon Obelisk Data", "Rare", "ODE");
            AddNewTypeF(MaterialEncodedCategory, Color.Yellow, "Security Firmware Patch", "Rare", "SFP", "securityfirmware");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Unknown Ship Signature", "Rare", "USSig");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Unknown Wake Data", "Rare", "UWD");
            // very rare data
            AddNewTypeC(MaterialEncodedCategory, Color.Red, "Abnormal Compact Emission Data", "Very Rare", "ACED");
            AddNewTypeF(MaterialEncodedCategory, Color.Red, "Adaptive Encryptors Capture", "Very Rare", "AEC", "adaptiveencryptors");
            AddNewTypeC(MaterialEncodedCategory, Color.Red, "Classified Scan Fragment", "Very Rare", "CSF");
            AddNewTypeF(MaterialEncodedCategory, Color.Red, "Datamined Wake Exceptions", "Very Rare", "DWEx", "dataminedwake");
            AddNewTypeC(MaterialEncodedCategory, Color.Red, "Modified Embedded Firmware", "Very Rare", "MEF");
            AddNewTypeC(MaterialEncodedCategory, Color.Red, "Peculiar Shield Frequency Data", "Very Rare", "PSFD");

            //very common manufactured
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Basic Conductors", "Very Common", "BaC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Chemical Storage Units", "Very Common", "CSU");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Compact Composites", "Very Common", "CC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Crystal Shards", "Very Common", "CS");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Grid Resistors", "Very Common", "GR");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Heat Conduction Wiring", "Very Common", "HCW");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Mechanical Scrap", "Very Common", "MS");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Salvaged Alloys", "Very Common", "SAll");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Tempered Alloys", "Very Common", "TeA");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Worn Shield Emitters", "Very Common", "WSE");
            // common manufactured
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Chemical Processors", "Common", "CP");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Conductive Components", "Common", "CCo");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Filament Composites", "Common", "FiC");
            AddNewTypeF(MaterialManufacturedCategory, Color.Green, "Flawed Focus Crystals", "Common", "FFC", "uncutfocuscrystals");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Galvanising Alloys", "Common", "GA");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Heat Dispersion Plate", "Common", "HDP");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Heat Resistant Ceramics", "Common", "HRC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Hybrid Capacitors", "Common", "HC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Mechanical Equipment", "Common", "ME");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Shield Emitters", "Common", "SE");
            AddNewTypeF(MaterialManufacturedCategory, Color.Green, "Federal Core Composites", "Common", "FCC", "fedcorecomposites");
            AddNewTypeF(MaterialManufacturedCategory, Color.Green, "Federal Proprietary Composites", "Common", "FPC", "fedproprietarycomposites");
            AddNewTypeF(MaterialManufacturedCategory, Color.Green, "Unknown Energy Source", "Common", "UES" , "unknownenergysource");

            // standard manufactured
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Chemical Distillery", "Standard", "CD");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Conductive Ceramics", "Standard", "CCe");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Electrochemical Arrays", "Standard", "EA");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Focus Crystals", "Standard", "FoC");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Heat Exchangers", "Standard", "HE");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "High Density Composites", "Standard", "HDC");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Mechanical Components", "Standard", "MC");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Phase Alloys", "Standard", "PA");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Precipitated Alloys", "Standard", "PAll");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Shielding Sensors", "Standard", "SS");
            // rare manufactured
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Chemical Manipulators", "Rare", "CM");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Compound Shielding", "Rare", "CoS");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Conductive Polymers", "Rare", "CPo");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Configurable Components", "Rare", "CCom");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Heat Vanes", "Rare", "HV");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Polymer Capacitors", "Rare", "PCa");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Proprietary Composites", "Rare", "PCo");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Proto Light Alloys", "Rare", "PLA");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Refined Focus Crystals", "Rare", "RFC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Thermic Alloys", "Rare", "ThA");
            // very rare manufactured
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Biotech Conductors", "Very Rare", "BiC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Core Dynamics Composites", "Very Rare", "CDC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Exquisite Focus Crystals", "Very Rare", "EFC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Imperial Shielding", "Very Rare", "IS");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Improvised Components", "Very Rare", "IC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Military Grade Alloys", "Very Rare", "MGA");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Military Supercapacitors", "Very Rare", "MSC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Pharmaceutical Isolators", "Very Rare", "PI");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Proto Heat Radiators", "Very Rare", "PHR");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Proto Radiolic Alloys", "Very Rare", "PRA");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Unknown Fragment", "Very Rare", "UF");
        }

        #endregion
    }
}


