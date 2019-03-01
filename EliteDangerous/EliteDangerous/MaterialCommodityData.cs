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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EliteDangerousCore
{
    // [System.Diagnostics.DebuggerDisplay("MatDB {Category} {Name} {FDName} {Type} {Shortname}")]
    public class MaterialCommodityData
    {
        public string Category { get; private set; }                // either Commodity, or one of the Category types from the MaterialCollected type.
        public string TranslatedCategory { get; private set; }      // translation of above..
        public string Name { get; private set; }                    // name of it in nice text
        public string FDName { get; private set; }                  // fdname, lower case..
        public string Type { get; private set; }                    // and its type, for materials its commonality, for commodities its group ("Metals" etc).
        public string TranslatedType { get; private set; }          // translation of above..        
        public string Shortname { get; private set; }               // short abv. name
        public Color Colour { get; private set; }                   // colour if its associated with one
        public bool Rarity { get; private set; }                    // if it is a rare commodity

        public bool IsCommodity { get { return Category == CommodityCategory; } }
        public bool IsRaw { get { return Category == MaterialRawCategory; } }
        public bool IsEncoded { get { return Category == MaterialEncodedCategory; } }
        public bool IsManufactured { get { return Category == MaterialManufacturedCategory; } }
        public bool IsEncodedOrManufactured { get { return Category == MaterialEncodedCategory || Category == MaterialManufacturedCategory; } }
        public bool IsRareCommodity { get { return Rarity && Category.Equals(CommodityCategory); } }
        public bool IsCommonMaterial { get { return Type == MaterialFreqCommon || Type == MaterialFreqVeryCommon; } }
        public bool IsJumponium
        {
            get
            {
                return (FDName.Contains("arsenic") || FDName.Contains("cadmium") || FDName.Contains("carbon")
                    || FDName.Contains("germanium") || FDName.Contains("niobium") || FDName.Contains("polonium")
                    || FDName.Contains("vanadium") || FDName.Contains("yttrium"));
            }
        }

        public static string CommodityCategory = "Commodity";       // Categories 
        public static string MaterialRawCategory = "Raw";
        public static string MaterialEncodedCategory = "Encoded";
        public static string MaterialManufacturedCategory = "Manufactured";

        public static string MaterialFreqVeryRare = "Very Rare";    // type field for materials
        public static string MaterialFreqRare = "Rare";
        public static string MaterialFreqStandard = "Standard";
        public static string MaterialFreqCommon = "Common";
        public static string MaterialFreqVeryCommon = "Very Common";

        public const int VeryCommonCap = 300;
        public const int CommonCap = 250;
        public const int StandardCap = 200;
        public const int RareCap = 150;
        public const int VeryRareCap = 100;

        public int? MaterialLimit()
        {
            string type = Type;
            if (type == MaterialFreqVeryCommon) return VeryCommonCap;
            if (type == MaterialFreqCommon) return CommonCap;
            if (type == MaterialFreqStandard) return StandardCap;
            if (type == MaterialFreqRare) return RareCap;
            if (type == MaterialFreqVeryRare) return VeryRareCap;
            return null;
        }

        #region Static interface

        // name key is lower case normalised
        private static Dictionary<string, MaterialCommodityData> cachelist = null;

        public static MaterialCommodityData GetByFDName(string fdname)
        {
            if (cachelist == null)
                FillTable();

            fdname = fdname.ToLowerInvariant();
            return cachelist.ContainsKey(fdname) ? cachelist[fdname] : null;
        }

        public static string GetNameByFDName(string fdname) // if we have it, give name, else give alt.  Also see RMat in journal field naming
        {
            if (cachelist == null)
                FillTable();

            fdname = fdname.ToLowerInvariant();
            return cachelist.ContainsKey(fdname) ? cachelist[fdname].Name : fdname.SplitCapsWordFull();
        }

        public static MaterialCommodityData GetByShortName(string shortname)
        {
            if (cachelist == null)
                FillTable();

            List<MaterialCommodityData> lst = cachelist.Values.ToList();
            int i = lst.FindIndex(x => x.Shortname.Equals(shortname));
            return i >= 0 ? lst[i] : null;
        }

        public static MaterialCommodityData[] GetAll()
        {
            if (cachelist == null)
                FillTable();

            return cachelist.Values.ToArray();
        }


        // use this delegate to find them
        public static MaterialCommodityData[] Get(Func<MaterialCommodityData,bool> func, bool sorted)
        {
            if (cachelist == null)
                FillTable();

            MaterialCommodityData[] items = cachelist.Values.Where(func).ToArray();

            if ( sorted )
            {
                Array.Sort(items, delegate (MaterialCommodityData left, MaterialCommodityData right)     // in order, name
                {
                    return left.Name.CompareTo(right.Name.ToString());
                });

            }

            return items;
        }

        public static MaterialCommodityData[] GetCommodities(bool sorted)
        {
            return Get(x => x.Category == CommodityCategory, sorted);
        }

        public static MaterialCommodityData[] GetMaterials(bool sorted)
        {
            return Get(x => x.Category != CommodityCategory, sorted);
        }

        public static Tuple<string, string>[] GetTypes(Func<MaterialCommodityData, bool> func, bool sorted)
        {
            MaterialCommodityData[] mcs = GetAll();
            Tuple<string, string>[] types = mcs.Where(func).Select(x => new Tuple<string, string>(x.Type, x.TranslatedType)).Distinct().ToArray();
            if (sorted)
                Array.Sort(types, delegate (Tuple<string, string> l, Tuple<string, string> r) { return l.Item2.CompareTo(r.Item2); });
            return types;
        }

        public static Tuple<string, string>[] GetCategories(Func<MaterialCommodityData, bool> func, bool sorted)
        {
            MaterialCommodityData[] mcs = GetAll();
            Tuple<string, string>[] types = mcs.Where(func).Select(x => new Tuple<string, string>(x.Category, x.TranslatedCategory)).Distinct().ToArray();
            if (sorted)
                Array.Sort(types, delegate (Tuple<string, string> l, Tuple<string, string> r) { return l.Item2.CompareTo(r.Item2); });
            return types;
        }

        public static string[] GetMembersOfType(string typename, bool sorted)
        {
            MaterialCommodityData[] mcs = GetAll();
            string[] members = mcs.Where(x => x.Type.Equals(typename, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Name).ToArray();
            if (sorted)
                Array.Sort(members);
            return members;
        }

        public static string[] GetFDNameMembersOfType(string typename, bool sorted)
        {
            MaterialCommodityData[] mcs = GetAll();
            string[] members = mcs.Where(x => x.Type.Equals(typename, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.FDName).ToArray();
            if (sorted)
                Array.Sort(members);
            return members;
        }


        public static string[] GetFDNameMembersOfCategory(string catname, bool sorted)
        {
            MaterialCommodityData[] mcs = GetAll();
            string[] members = mcs.Where(x => x.Category.Equals(catname, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.FDName).ToArray();
            if (sorted)
                Array.Sort(members);
            return members;
        }

        #endregion

        public MaterialCommodityData()
        {
        }

        public MaterialCommodityData(string cs, string n, string fd, string t, string shortn, Color cl, bool rare)
        {
            Category = cs;
            TranslatedCategory = Category.Tx(typeof(MaterialCommodityData));
            Name = n;
            FDName = fd;
            Type = t;
            TranslatedType = Type.Tx(typeof(MaterialCommodityData));
            Shortname = shortn;
            Colour = cl;
            Rarity = rare;
        }

        private void SetCache()
        {
            cachelist[this.FDName.ToLowerInvariant()] = this;
        }

        public static MaterialCommodityData EnsurePresent(string cat, string fdname)  // By FDNAME
        {
            if (!cachelist.ContainsKey(fdname.ToLowerInvariant()))
            {
                MaterialCommodityData mcdb = new MaterialCommodityData(cat, fdname.SplitCapsWordFull(), fdname, "Unknown", "", Color.Green, false);
                mcdb.SetCache();
                System.Diagnostics.Debug.WriteLine("Material not present: " + cat + "," + fdname);
            }

            return cachelist[fdname.ToLowerInvariant()];
        }


        #region Initial setup

        static Color CByType(string s)
        {
            if (s == MaterialFreqVeryRare)
                return Color.Red;
            if (s == MaterialFreqRare)
                return Color.Yellow;
            if (s == MaterialFreqVeryCommon)
                return Color.Cyan;
            if (s == MaterialFreqCommon)
                return Color.Green;
            if (s == MaterialFreqStandard)
                return Color.SandyBrown;
            if (s == "Unknown")
                return Color.Red;
            System.Diagnostics.Debug.Assert(false);
            return Color.Black;
        }

        // Mats

        private static bool AddRaw(string name, string typeofit, string shortname, string fdname = "")
        {
            return AddEntry(MaterialRawCategory, CByType(typeofit), name, typeofit, shortname, fdname);
        }

        private static bool AddEnc(string name, string typeofit, string shortname, string fdname = "")
        {
            return AddEntry(MaterialEncodedCategory, CByType(typeofit), name, typeofit, shortname, fdname);
        }

        private static bool AddManu(string name, string typeofit, string shortname, string fdname = "")
        {
            return AddEntry(MaterialManufacturedCategory, CByType(typeofit), name, typeofit, shortname, fdname);
        }

        // Commods

        private static bool AddCommodityRare(string aliasname, string typeofit, string fdname)
        {
            return AddEntry(CommodityCategory, Color.Green, aliasname, typeofit, "", fdname, true);
        }

        private static bool AddCommodity(string aliasname, string typeofit, string fdname)        // fdname only if not a list.
        {
            return AddEntry(CommodityCategory, Color.Green, aliasname, typeofit, "", fdname);
        }

        private static bool AddCommoditySN(string aliasname, string typeofit, string shortname, string fdname)
        {
            return AddEntry(CommodityCategory, Color.Green, aliasname, typeofit, shortname, fdname);
        }

        // fdname only useful if aliasname is not a list.
        private static bool AddCommodityList(string aliasnamelist, string typeofit)
        {
            string[] list = aliasnamelist.Split(';');

            foreach (string name in list)
            {
                if (name.Length > 0)   // just in case a semicolon slips thru
                {
                    if (!AddEntry(CommodityCategory, Color.Green, name, typeofit, "", ""))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static string FDNameCnv(string normal)
        {
            string n = new string(normal.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
            return n.ToLowerInvariant();
        }

        private static bool AddEntry(string catname, Color colour, string aliasname, string typeofit, string shortname, string fdName, bool comrare = false)
        {
            System.Diagnostics.Debug.Assert(!shortname.HasChars() || cachelist.Values.ToList().Find(x => x.Shortname.Equals(shortname, StringComparison.InvariantCultureIgnoreCase)) == null, "ShortName repeat " + aliasname + " " + shortname);
            System.Diagnostics.Debug.Assert(cachelist.ContainsKey(fdName) == false, "Repeated entry " + fdName);

            string fdn = (fdName.Length > 0) ? fdName.ToLowerInvariant() : FDNameCnv(aliasname);       // always lower case fdname

            MaterialCommodityData mc = new MaterialCommodityData(catname, aliasname, fdn, typeofit, shortname, colour, comrare);
            mc.SetCache();
            return true;
        }

        private static void FillTable()
        {
            #region Materials  - checked by netlogentry frontierdata against their spreadsheets. Use this tool to update the tables

            cachelist = new Dictionary<string, MaterialCommodityData>();

            AddRaw("Carbon", MaterialFreqVeryCommon, "C");
            AddRaw("Iron", MaterialFreqVeryCommon, "Fe");
            AddRaw("Nickel", MaterialFreqVeryCommon, "Ni");
            AddRaw("Phosphorus", MaterialFreqVeryCommon, "P");
            AddRaw("Sulphur", MaterialFreqVeryCommon, "S");
            AddRaw("Lead", MaterialFreqVeryCommon, "Pb");
            AddRaw("Rhenium", MaterialFreqVeryCommon, "Re");

            AddRaw("Chromium", MaterialFreqCommon, "Cr");
            AddRaw("Germanium", MaterialFreqCommon, "Ge");
            AddRaw("Manganese", MaterialFreqCommon, "Mn");
            AddRaw("Vanadium", MaterialFreqCommon, "V");
            AddRaw("Zinc", MaterialFreqCommon, "Zn");
            AddRaw("Zirconium", MaterialFreqCommon, "Zr");
            AddRaw("Arsenic", MaterialFreqCommon, "As");

            AddRaw("Niobium", MaterialFreqStandard, "Nb");        // realign to Anthors standard
            AddRaw("Tungsten", MaterialFreqStandard, "W");
            AddRaw("Molybdenum", MaterialFreqStandard, "Mo");
            AddRaw("Mercury", MaterialFreqStandard, "Hg");
            AddRaw("Boron", MaterialFreqStandard, "B");
            AddRaw("Cadmium", MaterialFreqStandard, "Cd");
            AddRaw("Tin", MaterialFreqStandard, "Sn");

            AddRaw("Selenium", MaterialFreqRare, "Se");
            AddRaw("Yttrium", MaterialFreqRare, "Y");
            AddRaw("Technetium", MaterialFreqRare, "Tc");
            AddRaw("Tellurium", MaterialFreqRare, "Te");
            AddRaw("Ruthenium", MaterialFreqRare, "Ru");
            AddRaw("Polonium", MaterialFreqRare, "Po");
            AddRaw("Antimony", MaterialFreqRare, "Sb");

            // very common data
            AddEnc("Anomalous Bulk Scan Data", MaterialFreqVeryCommon, "ABSD", "bulkscandata");
            AddEnc("Atypical Disrupted Wake Echoes", MaterialFreqVeryCommon, "ADWE", "disruptedwakeechoes");
            AddEnc("Distorted Shield Cycle Recordings", MaterialFreqVeryCommon, "DSCR", "shieldcyclerecordings");
            AddEnc("Exceptional Scrambled Emission Data", MaterialFreqVeryCommon, "ESED", "scrambledemissiondata");
            AddEnc("Specialised Legacy Firmware", MaterialFreqVeryCommon, "SLF", "legacyfirmware");
            AddEnc("Unusual Encrypted Files", MaterialFreqVeryCommon, "UEF", "encryptedfiles");
            // common data
            AddEnc("Anomalous FSD Telemetry", MaterialFreqCommon, "AFT", "fsdtelemetry");
            AddEnc("Inconsistent Shield Soak Analysis", MaterialFreqCommon, "ISSA", "shieldsoakanalysis");
            AddEnc("Irregular Emission Data", MaterialFreqCommon, "IED", "archivedemissiondata");
            AddEnc("Modified Consumer Firmware", MaterialFreqCommon, "MCF", "consumerfirmware");
            AddEnc("Tagged Encryption Codes", MaterialFreqCommon, "TEC", "encryptioncodes");
            AddEnc("Unidentified Scan Archives", MaterialFreqCommon, "USA", "scanarchives");
            AddEnc("Pattern Beta Obelisk Data", MaterialFreqCommon, "PBOD", "ancientculturaldata");
            AddEnc("Pattern Gamma Obelisk Data", MaterialFreqCommon, "PGOD", "ancienthistoricaldata");
            // standard data
            AddEnc("Classified Scan Databanks", MaterialFreqStandard, "CSD", "scandatabanks");
            AddEnc("Cracked Industrial Firmware", MaterialFreqStandard, "CIF", "industrialfirmware");
            AddEnc("Open Symmetric Keys", MaterialFreqStandard, "OSK", "symmetrickeys");
            AddEnc("Strange Wake Solutions", MaterialFreqStandard, "SWS", "wakesolutions");
            AddEnc("Unexpected Emission Data", MaterialFreqStandard, "UED", "emissiondata");
            AddEnc("Untypical Shield Scans", MaterialFreqStandard, "USS", "shielddensityreports");
            AddEnc("Abnormal Compact Emissions Data", MaterialFreqStandard, "CED", "compactemissionsdata");
            AddEnc("Pattern Alpha Obelisk Data", MaterialFreqStandard, "PAOD", "ancientbiologicaldata");
            // rare data
            AddEnc("Aberrant Shield Pattern Analysis", MaterialFreqRare, "ASPA", "shieldpatternanalysis");
            AddEnc("Atypical Encryption Archives", MaterialFreqRare, "AEA", "encryptionarchives");
            AddEnc("Decoded Emission Data", MaterialFreqRare, "DED");
            AddEnc("Divergent Scan Data", MaterialFreqRare, "DSD", "encodedscandata");
            AddEnc("Eccentric Hyperspace Trajectories", MaterialFreqRare, "EHT", "hyperspacetrajectories");
            AddEnc("Security Firmware Patch", MaterialFreqRare, "SFP", "securityfirmware");
            AddEnc("Pattern Delta Obelisk Data", MaterialFreqRare, "PDOD", "ancientlanguagedata");
            // very rare data
            AddEnc("Classified Scan Fragment", MaterialFreqVeryRare, "CFSD", "classifiedscandata");
            AddEnc("Modified Embedded Firmware", MaterialFreqVeryRare, "EFW", "embeddedfirmware");
            AddEnc("Adaptive Encryptors Capture", MaterialFreqVeryRare, "AEC", "adaptiveencryptors");
            AddEnc("Datamined Wake Exceptions", MaterialFreqVeryRare, "DWEx", "dataminedwake");
            AddEnc("Peculiar Shield Frequency Data", MaterialFreqVeryRare, "PSFD", "shieldfrequencydata");
            AddEnc("Pattern Epsilon Obelisk Data", MaterialFreqVeryRare, "PEOD", "ancienttechnologicaldata");
            //very common manufactured
            AddManu("Basic Conductors", MaterialFreqVeryCommon, "BaC");
            AddManu("Chemical Storage Units", MaterialFreqVeryCommon, "CSU");
            AddManu("Compact Composites", MaterialFreqVeryCommon, "CC");
            AddManu("Crystal Shards", MaterialFreqVeryCommon, "CS");
            AddManu("Grid Resistors", MaterialFreqVeryCommon, "GR");
            AddManu("Heat Conduction Wiring", MaterialFreqVeryCommon, "HCW");
            AddManu("Mechanical Scrap", MaterialFreqVeryCommon, "MS");
            AddManu("Salvaged Alloys", MaterialFreqVeryCommon, "SAll");
            AddManu("Worn Shield Emitters", MaterialFreqVeryCommon, "WSE");
            AddManu("Tempered Alloys", MaterialFreqVeryCommon, "TeA");
            // common manufactured
            AddManu("Chemical Processors", MaterialFreqCommon, "CP");
            AddManu("Conductive Components", MaterialFreqCommon, "CCo");
            AddManu("Filament Composites", MaterialFreqCommon, "FiC");
            AddManu("Flawed Focus Crystals", MaterialFreqCommon, "FFC", "uncutfocuscrystals");
            AddManu("Galvanising Alloys", MaterialFreqCommon, "GA");
            AddManu("Heat Dispersion Plate", MaterialFreqCommon, "HDP");
            AddManu("Heat Resistant Ceramics", MaterialFreqCommon, "HRC");
            AddManu("Hybrid Capacitors", MaterialFreqCommon, "HC");
            AddManu("Mechanical Equipment", MaterialFreqCommon, "ME");
            AddManu("Shield Emitters", MaterialFreqCommon, "SHE");
            // standard manufactured
            AddManu("Chemical Distillery", MaterialFreqStandard, "CHD");
            AddManu("Conductive Ceramics", MaterialFreqStandard, "CCe");
            AddManu("Electrochemical Arrays", MaterialFreqStandard, "EA");
            AddManu("Focus Crystals", MaterialFreqStandard, "FoC");
            AddManu("Heat Exchangers", MaterialFreqStandard, "HE");
            AddManu("High Density Composites", MaterialFreqStandard, "HDC");
            AddManu("Mechanical Components", MaterialFreqStandard, "MC");
            AddManu("Phase Alloys", MaterialFreqStandard, "PA");
            AddManu("Precipitated Alloys", MaterialFreqStandard, "PAll");
            AddManu("Shielding Sensors", MaterialFreqStandard, "SS");

            // new to 3.1 frontier data

            AddManu("Guardian Power Cell", MaterialFreqVeryCommon, "GPCe", "guardian_powercell");
            AddManu("Guardian Power Conduit", MaterialFreqCommon, "GPC", "guardian_powerconduit");
            AddManu("Guardian Technology Component", MaterialFreqStandard, "GTC", "guardian_techcomponent");
            AddManu("Guardian Sentinel Weapon Parts", MaterialFreqStandard, "GSWP", "guardian_sentinel_weaponparts");
            AddManu("Guardian Sentinel Wreckage Components", MaterialFreqVeryCommon, "GSWC", "guardian_sentinel_wreckagecomponents");
            AddEnc("Guardian Weapon Blueprint Segment", MaterialFreqRare, "GWBS", "guardian_weaponblueprint");
            AddEnc("Guardian Module Blueprint Segment", MaterialFreqRare, "GMBS", "guardian_moduleblueprint");

            // new to 3.2 frontier data
            AddEnc("Guardian Vessel Blueprint Segment", MaterialFreqVeryRare, "GMVB", "guardian_vesselblueprint");

            AddManu("Bio-Mechanical Conduits", MaterialFreqStandard, "BMC", "TG_BioMechanicalConduits");
            AddManu("Propulsion Elements", MaterialFreqStandard, "PE", "TG_PropulsionElement");
            AddManu("Weapon Parts", MaterialFreqStandard, "WP", "TG_WeaponParts");
            AddManu("Wreckage Components", MaterialFreqStandard, "WRC", "TG_WreckageComponents");
            AddEnc("Ship Flight Data", MaterialFreqStandard, "SFD", "TG_ShipFlightData");
            AddEnc("Ship Systems Data", MaterialFreqStandard, "SSD", "TG_ShipSystemsData");

            // rare manufactured
            AddManu("Chemical Manipulators", MaterialFreqRare, "CM");
            AddManu("Compound Shielding", MaterialFreqRare, "CoS");
            AddManu("Conductive Polymers", MaterialFreqRare, "CPo");
            AddManu("Configurable Components", MaterialFreqRare, "CCom");
            AddManu("Heat Vanes", MaterialFreqRare, "HV");
            AddManu("Polymer Capacitors", MaterialFreqRare, "PCa");
            AddManu("Proto Light Alloys", MaterialFreqRare, "PLA");
            AddManu("Refined Focus Crystals", MaterialFreqRare, "RFC");
            AddManu("Proprietary Composites", MaterialFreqRare, "FPC", "fedproprietarycomposites");
            AddManu("Thermic Alloys", MaterialFreqRare, "ThA");
            // very rare manufactured
            AddManu("Core Dynamics Composites", MaterialFreqVeryRare, "FCC", "fedcorecomposites");
            AddManu("Biotech Conductors", MaterialFreqVeryRare, "BiC");
            AddManu("Exquisite Focus Crystals", MaterialFreqVeryRare, "EFC");
            AddManu("Imperial Shielding", MaterialFreqVeryRare, "IS");
            AddManu("Improvised Components", MaterialFreqVeryRare, "IC");
            AddManu("Military Grade Alloys", MaterialFreqVeryRare, "MGA");
            AddManu("Military Supercapacitors", MaterialFreqVeryRare, "MSC");
            AddManu("Pharmaceutical Isolators", MaterialFreqVeryRare, "PI");
            AddManu("Proto Heat Radiators", MaterialFreqVeryRare, "PHR");
            AddManu("Proto Radiolic Alloys", MaterialFreqVeryRare, "PRA");

            string sv = "Salvage";
            AddCommodity("Thargoid Sensor", sv, "UnknownArtifact");
            AddCommodity("Thargoid Probe", sv, "UnknownArtifact2");
            AddCommodity("Thargoid Link", sv, "UnknownArtifact3");
            AddCommodity("Thargoid Resin", sv, "UnknownResin");
            AddCommodity("Thargoid Biological Matter", sv, "UnknownBiologicalMatter");
            AddCommodity("Thargoid Technology Samples", sv, "UnknownTechnologySamples");

            AddManu("Thargoid Carapace", MaterialFreqCommon, "UKCP", "unknowncarapace");
            AddManu("Thargoid Energy Cell", MaterialFreqStandard, "UKEC", "unknownenergycell");
            AddManu("Thargoid Organic Circuitry", MaterialFreqVeryRare, "UKOC", "unknownorganiccircuitry");
            AddManu("Thargoid Technological Components", MaterialFreqRare, "UKTC", "unknowntechnologycomponents");
            AddManu("Sensor Fragment", MaterialFreqVeryRare, "UES", "unknownenergysource");

            AddEnc("Thargoid Material Composition Data", MaterialFreqStandard, "UMCD", "tg_compositiondata");
            AddEnc("Thargoid Structural Data", MaterialFreqCommon, "UKSD", "tg_structuraldata");
            AddEnc("Thargoid Residue Data", MaterialFreqRare, "URDA", "tg_residuedata");
            AddEnc("Thargoid Ship Signature", MaterialFreqStandard, "USSig", "unknownshipsignature");
            AddEnc("Thargoid Wake Data", MaterialFreqRare, "UWD", "unknownwakedata");

            #endregion

            #region Commodities - checked by netlogentry frontierdata against their spreadsheets. Use this tool to update the tables

            AddCommodityList("Explosives;Hydrogen Fuel;Hydrogen Peroxide;Liquid Oxygen;Mineral Oil;Nerve Agents;Pesticides;Surface Stabilisers;Synthetic Reagents;Water", "Chemicals");

            string ci = "Consumer Items";
            AddCommodityList("Clothing;Consumer Technology;Domestic Appliances;Evacuation Shelter;Survival Equipment", ci);
            AddCommodity("Duradrives", ci, "Duradrives");

            AddCommodityList("Algae;Animal Meat;Coffee;Fish;Food Cartridges;Fruit and Vegetables;Grain;Synthetic Meat;Tea", "Foods");

            string im = "Industrial Materials";
            AddCommodityList("Ceramic Composites;Insulating Membrane;Polymers;Semiconductors;Superconductors", im);
            AddCommoditySN("Meta-Alloys", im, "MA", "metaalloys");
            AddCommoditySN("Micro-Weave Cooling Hoses", im, "MWCH", "coolinghoses");
            AddCommoditySN("Neofabric Insulation", im, "NFI", "");
            AddCommoditySN("CMM Composite", im, "CMMC", "");

            AddCommodityList("Beer;Bootleg Liquor;Liquor;Tobacco;Wine", "Legal Drugs");

            string m = "Machinery";

            AddCommodity("Atmospheric Processors", m, "AtmosphericExtractors");
            AddCommodity("Marine Equipment", m, "MarineSupplies");
            AddCommodity("Microbial Furnaces", m, "HeliostaticFurnaces");
            AddCommodity("Skimmer Components", m, "SkimerComponents");

            AddCommodityList("Building Fabricators;Crop Harvesters;Emergency Power Cells;Exhaust Manifold;Geological Equipment", m);
            AddCommoditySN("HN Shock Mount", m, "HNSM", "");
            AddCommodityList("Mineral Extractors;Modular Terminals;Power Generators", m);
            AddCommodityList("Thermal Cooling Units;Water Purifiers", m);
            AddCommoditySN("Heatsink Interlink", m, "HSI", "");
            AddCommoditySN("Energy Grid Assembly", m, "EGA", "powergridassembly");
            AddCommoditySN("Radiation Baffle", m, "RB", "");
            AddCommoditySN("Magnetic Emitter Coil", m, "MEC", "");
            AddCommoditySN("Articulation Motors", m, "AM", "");
            AddCommoditySN("Reinforced Mounting Plate", m, "RMP", "");
            AddCommoditySN("Power Transfer Bus", m, "PTB", "PowerTransferConduits");
            AddCommoditySN("Power Converter", m, "PC", "");
            AddCommoditySN("Ion Distributor", m, "ID", "");


            string md = "Medicines";
            AddCommodityList("Advanced Medicines;Basic Medicines;Combat Stabilisers;Performance Enhancers;Progenitor Cells", md);
            AddCommodity("Agri-Medicines", md, "agriculturalmedicines");
            AddCommodityRare("Nanomedicines", md, "Nanomedicines");

            AddCommodityList("Aluminium;Beryllium;Bismuth;Cobalt;Copper;Gallium;Gold;Hafnium 178;Indium;Lanthanum;Lithium;Osmium;Palladium;Platinum;Praseodymium;Samarium;Silver;Tantalum;Thallium;Thorium;Titanium;Uranium", "Metals");
            AddCommodity("Platinum Alloy", "Metals", "PlatinumAloy");

            string mi = "Minerals";
            AddCommodityList("Bauxite;Bertrandite;Bromellite;Coltan;Cryolite;Gallite;Goslarite;Methane Clathrate", mi);
            AddCommodityList("Indite;Jadeite;Lepidolite;Lithium Hydroxide;Moissanite;Painite;Pyrophyllite;Rutile;Taaffeite;Uraninite", mi);
            AddCommodity("Methanol Monohydrate Crystals", mi, "methanolmonohydratecrystals");
            AddCommodity("Low Temperature Diamonds", mi, "lowtemperaturediamond");

            AddCommodity("Rhodplumsite", mi, "Rhodplumsite");
            AddCommodity("Serendibite", mi, "Serendibite");
            AddCommodity("Monazite", mi, "Monazite");
            AddCommodity("Musgravite", mi, "Musgravite");
            AddCommodity("Benitoite", mi, "Benitoite");
            AddCommodity("Grandidierite", mi, "Grandidierite");
            AddCommodity("Alexandrite", mi, "Alexandrite");
            AddCommodity("Opal", mi, "Opal");

            AddCommodity("Trinkets of Hidden Fortune", sv, "TrinketsOfFortune");
            AddCommodity("Gene Bank", sv, "GeneBank");
            AddCommodity("Time Capsule", sv, "TimeCapsule");
            AddCommodity("Damaged Escape Pod", sv, "DamagedEscapePod");
            AddCommodity("Thargoid Heart", sv, "ThargoidHeart");
            AddCommodity("Thargoid Cyclops Tissue Sample", sv, "ThargoidTissueSampleType1");
            AddCommodity("Thargoid Basilisk Tissue Sample", sv, "ThargoidTissueSampleType2");
            AddCommodity("Thargoid Medusa Tissue Sample", sv, "ThargoidTissueSampleType3");
            AddCommodity("Thargoid Scout Tissue Sample", sv, "ThargoidScoutTissueSample");
            AddCommodity("Wreckage Components", sv, "WreckageComponents");
            AddCommodity("Antique Jewellery", sv, "AntiqueJewellery");
            AddCommodity("Thargoid Hydra Tissue Sample", sv, "ThargoidTissueSampleType4");
            AddCommodity("Ancient Key", sv, "AncientKey");

            AddCommodityList("Ai Relics;Antimatter Containment Unit;Antiquities;Assault Plans;Data Core;Diplomatic Bag;Encrypted Correspondence;Fossil Remnants", sv);
            AddCommodityList("Geological Samples;Hostage;Military Intelligence;Mysterious Idol;Occupied CryoPod;Personal Effects;Political Prisoner;Precious Gems;Prohibited Research Materials", sv);
            AddCommodityList("Sap 8 Core Container;Scientific Research;Scientific Samples;Space Pioneer Relics;Tactical Data;Unstable Data Core", sv);
            AddCommodity("Large Survey Data Cache", sv, "largeexplorationdatacash");
            AddCommodity("Small Survey Data Cache", sv, "smallexplorationdatacash");
            AddCommodity("Ancient Artefact", sv, "USSCargoAncientArtefact");
            AddCommodity("Black Box", sv, "USSCargoBlackBox");
            AddCommodity("Commercial Samples", sv, "ComercialSamples");
            AddCommodity("Encrypted Data Storage", sv, "EncriptedDataStorage");
            AddCommodity("Experimental Chemicals", sv, "USSCargoExperimentalChemicals");
            AddCommodity("Military Plans", sv, "USSCargoMilitaryPlans");
            AddCommodity("Occupied Escape Pod", sv, "OccupiedCryoPod");
            AddCommodity("Prototype Tech", sv, "USSCargoPrototypeTech");
            AddCommodity("Rare Artwork", sv, "USSCargoRareArtwork");
            AddCommodity("Rebel Transmissions", sv, "USSCargoRebelTransmissions");
            AddCommodity("Technical Blueprints", sv, "USSCargoTechnicalBlueprints");
            AddCommodity("Trade Data", sv, "USSCargoTradeData");
            AddCommodity("Ancient Relic", sv, "AncientRelic");
            AddCommodity("Ancient Orb", sv, "AncientOrb");
            AddCommodity("Ancient Casket", sv, "AncientCasket");
            AddCommodity("Ancient Tablet", sv, "AncientTablet");
            AddCommodity("Ancient Urn", sv, "AncientUrn");
            AddCommodity("Ancient Totem", sv, "AncientTotem");

            AddCommodity("Narcotics", "Narcotics", "BasicNarcotics");

            AddCommodityList("Imperial Slaves;Slaves", "Slaves");

            string tc = "Technology";
            AddCommoditySN("Ion Distributor", "Technology", "IOD", "IonDistributor");
            AddCommodityList("Advanced Catalysers;Animal Monitors;Aquaponic Systems;Bioreducing Lichen;Computer Components", tc);
            AddCommodity("Auto-Fabricators", tc, "autofabricators");
            AddCommoditySN("Micro Controllers", tc, "MCC", "MicroControllers");
            AddCommodityList("Medical Diagnostic Equipment", tc);
            AddCommodityList("Nanobreakers;Resonating Separators;Robotics;Structural Regulators;Telemetry Suite", tc);
            AddCommodity("H.E. Suits", tc, "hazardousenvironmentsuits");
            AddCommoditySN("Hardware Diagnostic Sensor", tc, "DIS", "diagnosticsensor");
            AddCommodity("Muon Imager", tc, "mutomimager");
            AddCommodity("Land Enrichment Systems", "Technology", "TerrainEnrichmentSystems");

            AddCommodityList("Conductive Fabrics;Leather;Military Grade Fabrics;Natural Fabrics;Synthetic Fabrics", "Textiles");

            AddCommodityList("Biowaste;Chemical Waste;Scrap;Toxic Waste", "Waste");

            string wp = "Weapons";
            AddCommodityList("Battle Weapons;Landmines;Personal Weapons;Reactive Armour", wp);
            AddCommodity("Non-Lethal Weapons", wp, "nonlethalweapons");

            #endregion

            #region Rare Commodities - checked by netlogentry frontierdata against their spreadsheets. Use this tool to update the tables

            AddCommodityRare("The Hutton Mug", "Consumer Items", "TheHuttonMug");
            AddCommodityRare("Eranin Pearl Whisky", "Legal Drugs", "EraninPearlWhisky");
            AddCommodityRare("Lavian Brandy", "Legal Drugs", "LavianBrandy");
            AddCommodityRare("HIP 10175 Bush Meat", "Foods", "HIP10175BushMeat");
            AddCommodityRare("Albino Quechua Mammoth Meat", "Foods", "AlbinoQuechuaMammoth");
            AddCommodityRare("Utgaroar Millennial Eggs", "Foods", "UtgaroarMillenialEggs");
            AddCommodityRare("Witchhaul Kobe Beef", "Foods", "WitchhaulKobeBeef");
            AddCommodityRare("Karsuki Locusts", "Foods", "KarsukiLocusts");
            AddCommodityRare("Giant Irukama Snails", "Foods", "GiantIrukamaSnails");
            AddCommodityRare("Baltah'sine Vacuum Krill", "Foods", "BaltahSineVacuumKrill");
            AddCommodityRare("Ceti Rabbits", "Foods", "CetiRabbits");
            AddCommodityRare("Kachirigin Filter Leeches", "Medicines", "KachiriginLeaches");
            AddCommodityRare("Lyrae Weed", "Narcotics", "LyraeWeed");
            AddCommodityRare("Onionhead", "Narcotics", "OnionHead");
            AddCommodityRare("Tarach Spice", "Narcotics", "TarachTorSpice");
            AddCommodityRare("Wolf Fesh", "Narcotics", "Wolf1301Fesh");
            AddCommodityRare("Borasetani Pathogenetics", "Weapons", "BorasetaniPathogenetics");
            AddCommodityRare("HIP 118311 Swarm", "Weapons", "HIP118311Swarm");
            AddCommodityRare("Kongga Ale", "Legal Drugs", "KonggaAle");
            AddCommodityRare("Wuthielo Ku Froth", "Legal Drugs", "WuthieloKuFroth");
            AddCommodityRare("Alacarakmo Skin Art", "Consumer Items", "AlacarakmoSkinArt");
            AddCommodityRare("Eleu Thermals", "Consumer Items", "EleuThermals");
            AddCommodityRare("Eshu Umbrellas", "Consumer Items", "EshuUmbrellas");
            AddCommodityRare("Karetii Couture", "Consumer Items", "KaretiiCouture");
            AddCommodityRare("Njangari Saddles", "Consumer Items", "NjangariSaddles");
            AddCommodityRare("Any Na Coffee", "Foods", "AnyNaCoffee");
            AddCommodityRare("CD-75 Kitten Brand Coffee", "Foods", "CD75CatCoffee");
            AddCommodityRare("Goman Yaupon Coffee", "Foods", "GomanYauponCoffee");
            AddCommodityRare("Volkhab Bee Drones", "Machinery", "VolkhabBeeDrones");
            AddCommodityRare("Kinago Violins", "Consumer Items", "KinagoInstruments");
            AddCommodityRare("Nguna Modern Antiques", "Consumer Items", "NgunaModernAntiques");
            AddCommodityRare("Rajukru Multi-Stoves", "Consumer Items", "RajukruStoves");
            AddCommodityRare("Tiolce Waste2Paste Units", "Consumer Items", "TiolceWaste2PasteUnits");
            AddCommodityRare("Chi Eridani Marine Paste", "Foods", "ChiEridaniMarinePaste");
            AddCommodityRare("Esuseku Caviar", "Foods", "EsusekuCaviar");
            AddCommodityRare("Live Hecate Sea Worms", "Foods", "LiveHecateSeaWorms");
            AddCommodityRare("Helvetitj Pearls", "Foods", "HelvetitjPearls");
            AddCommodityRare("HIP Proto-Squid", "Foods", "HIP41181Squid");
            AddCommodityRare("Coquim Spongiform Victuals", "Foods", "CoquimSpongiformVictuals");
            AddCommodityRare("Eden Apples Of Aerial", "Foods", "AerialEdenApple");
            AddCommodityRare("Neritus Berries", "Foods", "NeritusBerries");
            AddCommodityRare("Ochoeng Chillies", "Foods", "OchoengChillies");
            AddCommodityRare("Deuringas Truffles", "Foods", "DeuringasTruffles");
            AddCommodityRare("HR 7221 Wheat", "Foods", "HR7221Wheat");
            AddCommodityRare("Jaroua Rice", "Foods", "JarouaRice");
            AddCommodityRare("Belalans Ray Leather", "Textiles", "BelalansRayLeather");
            AddCommodityRare("Damna Carapaces", "Textiles", "DamnaCarapaces");
            AddCommodityRare("Rapa Bao Snake Skins", "Textiles", "RapaBaoSnakeSkins");
            AddCommodityRare("Vanayequi Ceratomorpha Fur", "Textiles", "VanayequiRhinoFur");
            AddCommodityRare("Bast Snake Gin", "Legal Drugs", "BastSnakeGin");
            AddCommodityRare("Thrutis Cream", "Legal Drugs", "ThrutisCream");
            AddCommodityRare("Wulpa Hyperbore Systems", "Machinery", "WulpaHyperboreSystems");
            AddCommodityRare("Aganippe Rush", "Medicines", "AganippeRush");
            AddCommodityRare("Terra Mater Blood Bores", "Medicines", "TerraMaterBloodBores");
            AddCommodityRare("Holva Duelling Blades", "Weapons", "HolvaDuellingBlades");
            AddCommodityRare("Kamorin Historic Weapons", "Weapons", "KamorinHistoricWeapons");
            AddCommodityRare("Gilya Signature Weapons", "Weapons", "GilyaSignatureWeapons");
            AddCommodityRare("Delta Phoenicis Palms", "Chemicals", "DeltaPhoenicisPalms");
            AddCommodityRare("Toxandji Virocide", "Chemicals", "ToxandjiVirocide");
            AddCommodityRare("Xihe Biomorphic Companions", "Technology", "XiheCompanions");
            AddCommodityRare("Sanuma Decorative Meat", "Foods", "SanumaMEAT");
            AddCommodityRare("Ethgreze Tea Buds", "Foods", "EthgrezeTeaBuds");
            AddCommodityRare("Ceremonial Heike Tea", "Foods", "CeremonialHeikeTea");
            AddCommodityRare("Tanmark Tranquil Tea", "Foods", "TanmarkTranquilTea");
            AddCommodityRare("AZ Cancri Formula 42", "Technology", "AZCancriFormula42");
            AddCommodityRare("Sothis Crystalline Gold", "Metals", "SothisCrystallineGold");
            AddCommodityRare("Kamitra Cigars", "Legal Drugs", "KamitraCigars");
            AddCommodityRare("Rusani Old Smokey", "Legal Drugs", "RusaniOldSmokey");
            AddCommodityRare("Yaso Kondi Leaf", "Legal Drugs", "YasoKondiLeaf");
            AddCommodityRare("Chateau De Aegaeon", "Legal Drugs", "ChateauDeAegaeon");
            AddCommodityRare("The Waters Of Shintara", "Medicines", "WatersOfShintara");
            AddCommodityRare("Ophiuch Exino Artefacts", "Consumer Items", "OphiuchiExinoArtefacts");
            AddCommodityRare("Baked Greebles", "Foods", "BakedGreebles");
            AddCommodityRare("Aepyornis Egg", "Foods", "CetiAepyornisEgg");
            AddCommodityRare("Saxon Wine", "Legal Drugs", "SaxonWine");
            AddCommodityRare("Centauri Mega Gin", "Legal Drugs", "CentauriMegaGin");
            AddCommodityRare("Anduliga Fire Works", "Chemicals", "AnduligaFireWorks");
            AddCommodityRare("Banki Amphibious Leather", "Textiles", "BankiAmphibiousLeather");
            AddCommodityRare("Cherbones Blood Crystals", "Minerals", "CherbonesBloodCrystals");
            AddCommodityRare("Motrona Experience Jelly", "Narcotics", "MotronaExperienceJelly");
            AddCommodityRare("Geawen Dance Dust", "Narcotics", "GeawenDanceDust");
            AddCommodityRare("Gerasian Gueuze Beer", "Legal Drugs", "GerasianGueuzeBeer");
            AddCommodityRare("Haiden Black Brew", "Foods", "HaidneBlackBrew");
            AddCommodityRare("Havasupai Dream Catcher", "Consumer Items", "HavasupaiDreamCatcher");
            AddCommodityRare("Burnham Bile Distillate", "Legal Drugs", "BurnhamBileDistillate");
            AddCommodityRare("Hip Organophosphates", "Chemicals", "HIPOrganophosphates");
            AddCommodityRare("Jaradharre Puzzle Box", "Consumer Items", "JaradharrePuzzlebox");
            AddCommodityRare("Koro Kung Pellets", "Chemicals", "KorroKungPellets");
            AddCommodityRare("Void Extract Coffee", "Foods", "LFTVoidExtractCoffee");
            AddCommodityRare("Honesty Pills", "Medicines", "HonestyPills");
            AddCommodityRare("Non Euclidian Exotanks", "Machinery", "NonEuclidianExotanks");
            AddCommodityRare("LTT Hyper Sweet", "Foods", "LTTHyperSweet");
            AddCommodityRare("Mechucos High Tea", "Foods", "MechucosHighTea");
            AddCommodityRare("Medb Starlube", "Industrial Materials", "MedbStarlube");
            AddCommodityRare("Mokojing Beast Feast", "Foods", "MokojingBeastFeast");
            AddCommodityRare("Mukusubii Chitin-os", "Foods", "MukusubiiChitinOs");
            AddCommodityRare("Mulachi Giant Fungus", "Foods", "MulachiGiantFungus");
            AddCommodityRare("Ngadandari Fire Opals", "Minerals", "NgadandariFireOpals");
            AddCommodityRare("Tiegfries Synth Silk", "Textiles", "TiegfriesSynthSilk");
            AddCommodityRare("Uzumoku Low-G Wings", "Consumer Items", "UzumokuLowGWings");
            AddCommodityRare("V Herculis Body Rub", "Medicines", "VHerculisBodyRub");
            AddCommodityRare("Wheemete Wheat Cakes", "Foods", "WheemeteWheatCakes");
            AddCommodityRare("Vega Slimweed", "Medicines", "VegaSlimWeed");
            AddCommodityRare("Altairian Skin", "Consumer Items", "AltairianSkin");
            AddCommodityRare("Pavonis Ear Grubs", "Narcotics", "PavonisEarGrubs");
            AddCommodityRare("Jotun Mookah", "Consumer Items", "JotunMookah");
            AddCommodityRare("Giant Verrix", "Machinery", "GiantVerrix");
            AddCommodityRare("Indi Bourbon", "Legal Drugs", "IndiBourbon");
            AddCommodityRare("Arouca Conventual Sweets", "Foods", "AroucaConventualSweets");
            AddCommodityRare("Tauri Chimes", "Medicines", "TauriChimes");
            AddCommodityRare("Zeessze Ant Grub Glue", "Consumer Items", "ZeesszeAntGlue");
            AddCommodityRare("Pantaa Prayer Sticks", "Medicines", "PantaaPrayerSticks");
            AddCommodityRare("Fujin Tea", "Medicines", "FujinTea");
            AddCommodityRare("Chameleon Cloth", "Textiles", "ChameleonCloth");
            AddCommodityRare("Orrerian Vicious Brew", "Foods", "OrrerianViciousBrew");
            AddCommodityRare("Uszaian Tree Grub", "Foods", "UszaianTreeGrub");
            AddCommodityRare("Momus Bog Spaniel", "Consumer Items", "MomusBogSpaniel");
            AddCommodityRare("Diso Ma Corn", "Foods", "DisoMaCorn");
            AddCommodityRare("Leestian Evil Juice", "Legal Drugs", "LeestianEvilJuice");
            AddCommodityRare("Azure Milk", "Legal Drugs", "BlueMilk");
            AddCommodityRare("Leathery Eggs", "Consumer Items", "AlienEggs");
            AddCommodityRare("Alya Body Soap", "Medicines", "AlyaBodilySoap");
            AddCommodityRare("Vidavantian Lace", "Consumer Items", "VidavantianLace");
            AddCommodityRare("Lucan Onionhead", "Narcotics", "TransgenicOnionHead");
            AddCommodityRare("Jaques Quinentian Still", "Consumer Items", "JaquesQuinentianStill");
            AddCommodityRare("Soontill Relics", "Consumer Items", "SoontillRelics");
            AddCommodityRare("Onionhead Alpha Strain", "Narcotics", "OnionHeadA");
            AddCommodityRare("Onionhead Beta Strain", "Narcotics", "OnionHeadB");
            AddCommodityRare("Galactic Travel Guide", sv, "GalacticTravelGuide");
            AddCommodityRare("Crom Silver Fesh", "Narcotics", "AnimalEffigies");
            AddCommodityRare("Shan's Charis Orchid", "Consumer Items", "ShansCharisOrchid");
            AddCommodityRare("Buckyball Beer Mats", "Consumer Items", "BuckyballBeerMats");
            AddCommodityRare("Master Chefs", "Slaves", "MasterChefs");
            AddCommodityRare("Personal Gifts", "Consumer Items", "PersonalGifts");
            AddCommodityRare("Crystalline Spheres", "Consumer Items", "CrystallineSpheres");
            AddCommodityRare("Ultra-Compact Processor Prototypes", "Consumer Items", "Advert1");
            AddCommodityRare("Harma Silver Sea Rum", "Narcotics", "HarmaSilverSeaRum");
            AddCommodityRare("Earth Relics", sv, "EarthRelics");

            #endregion

            #region Powerplay - checked by netlogentry frontierdata against their spreadsheets. Use this tool to update the tables

            AddCommodity("Aisling Media Materials", "PowerPlay", "AislingMediaMaterials");
            AddCommodity("Aisling Sealed Contracts", "PowerPlay", "AislingMediaResources");
            AddCommodity("Aisling Programme Materials", "PowerPlay", "AislingPromotionalMaterials");
            AddCommodity("Alliance Trade Agreements", "PowerPlay", "AllianceTradeAgreements");
            AddCommodity("Alliance Legislative Contracts", "PowerPlay", "AllianceLegaslativeContracts");
            AddCommodity("Alliance Legislative Records", "PowerPlay", "AllianceLegaslativeRecords");
            AddCommodity("Lavigny Corruption Reports", "PowerPlay", "LavignyCorruptionDossiers");
            AddCommodity("Lavigny Field Supplies", "PowerPlay", "LavignyFieldSupplies");
            AddCommodity("Lavigny Garrison Supplies", "PowerPlay", "LavignyGarisonSupplies");
            AddCommodity("Core Restricted Package", "PowerPlay", "RestrictedPackage");
            AddCommodity("Liberal Propaganda", "PowerPlay", "LiberalCampaignMaterials");
            AddCommodity("Liberal Federal Aid", "PowerPlay", "FederalAid");
            AddCommodity("Liberal Federal Packages", "PowerPlay", "FederalTradeContracts");
            AddCommodity("Marked Military Arms", "PowerPlay", "LoanedArms");
            AddCommodity("Patreus Field Supplies", "PowerPlay", "PatreusFieldSupplies");
            AddCommodity("Patreus Garrison Supplies", "PowerPlay", "PatreusGarisonSupplies");
            AddCommodity("Hudson's Restricted Intel", "PowerPlay", "RestrictedIntel");
            AddCommodity("Hudson's Field Supplies", "PowerPlay", "RepublicanFieldSupplies");
            AddCommodity("Hudson Garrison Supplies", "PowerPlay", "RepublicanGarisonSupplies");
            AddCommodity("Sirius Franchise Package", "PowerPlay", "SiriusFranchisePackage");
            AddCommodity("Sirius Corporate Contracts", "PowerPlay", "SiriusCommercialContracts");
            AddCommodity("Sirius Industrial Equipment", "PowerPlay", "SiriusIndustrialEquipment");
            AddCommodity("Torval Trade Agreements", "PowerPlay", "TorvalCommercialContracts");
            AddCommodity("Torval Political Prisoners", "PowerPlay", "ImperialPrisoner");
            AddCommodity("Utopian Publicity", "PowerPlay", "UtopianPublicity");
            AddCommodity("Utopian Supplies", "PowerPlay", "UtopianFieldSupplies");
            AddCommodity("Utopian Dissident", "PowerPlay", "UtopianDissident");
            AddCommodity("Kumo Contraband Package", "PowerPlay", "IllicitConsignment");
            AddCommodity("Unmarked Military supplies", "PowerPlay", "UnmarkedWeapons");
            AddCommodity("Onionhead Samples", "PowerPlay", "OnionheadSamples");
            AddCommodity("Revolutionary supplies", "PowerPlay", "CounterCultureSupport");
            AddCommodity("Onionhead Derivatives", "PowerPlay", "OnionheadDerivatives");
            AddCommodity("Out Of Date Goods", "PowerPlay", "OutOfDateGoods");
            AddCommodity("Grom Underground Support", "PowerPlay", "UndergroundSupport");
            AddCommodity("Grom Counter Intelligence", "PowerPlay", "GromCounterIntelligence");
            AddCommodity("Yuri Grom's Military Supplies", "PowerPlay", "GromWarTrophies");
            AddCommodity("Marked Slaves", "PowerPlay", "MarkedSlaves");
            AddCommodity("Torval Deeds", "PowerPlay", "TorvalDeeds");

            #endregion

            // seen in logs
            AddCommodity("Drones", "Drones", "Drones");

            foreach (var x in cachelist.Values)
            {
                x.Name = BaseUtils.Translator.Instance.Translate(x.Name, "MaterialCommodityData." + x.FDName);
            }

            //foreach (MaterialCommodityDB d in cachelist.Values) System.Diagnostics.Debug.WriteLine(string.Format("{0},{1},{2},{3}", d.fdname, d.name, d.category, d.type));
        }


        public static Dictionary<string, string> fdnamemangling = new Dictionary<string, string>() // Key: old_identifier, Value: new_identifier
        {
            //2.2 to 2.3 changed some of the identifier names.. change the 2.2 ones to 2.3!  Anthor data from his materials db file

            // July 2018 - removed many, changed above, to match FD 3.1 excel output - we use their IDs.  Netlogentry frontierdata checks these..

            { "aberrantshieldpatternanalysis"       ,  "shieldpatternanalysis" },
            { "adaptiveencryptorscapture"           ,  "adaptiveencryptors" },
            { "alyabodysoap"                        ,  "alyabodilysoap" },
            { "anomalousbulkscandata"               ,  "bulkscandata" },
            { "anomalousfsdtelemetry"               ,  "fsdtelemetry" },
            { "atypicaldisruptedwakeechoes"         ,  "disruptedwakeechoes" },
            { "atypicalencryptionarchives"          ,  "encryptionarchives" },
            { "azuremilk"                           ,  "bluemilk" },
            { "cd-75kittenbrandcoffee"              ,  "cd75catcoffee" },
            { "crackedindustrialfirmware"           ,  "industrialfirmware" },
            { "dataminedwakeexceptions"             ,  "dataminedwake" },
            { "distortedshieldcyclerecordings"      ,  "shieldcyclerecordings" },
            { "eccentrichyperspacetrajectories"     ,  "hyperspacetrajectories" },
            { "edenapplesofaerial"                  ,  "aerialedenapple" },
            { "eraninpearlwhiskey"                  ,  "eraninpearlwhisky" },
            { "exceptionalscrambledemissiondata"    ,  "scrambledemissiondata" },
            { "inconsistentshieldsoakanalysis"      ,  "shieldsoakanalysis" },
            { "kachiriginfilterleeches"             ,  "kachiriginleaches" },
            { "korokungpellets"                     ,  "korrokungpellets" },
            { "leatheryeggs"                        ,  "alieneggs" },
            { "lucanonionhead"                      ,  "transgeniconionhead" },
            { "modifiedconsumerfirmware"            ,  "consumerfirmware" },
            { "modifiedembeddedfirmware"            ,  "embeddedfirmware" },
            { "opensymmetrickeys"                   ,  "symmetrickeys" },
            { "peculiarshieldfrequencydata"         ,  "shieldfrequencydata" },
            { "rajukrumulti-stoves"                 ,  "rajukrustoves" },
            { "sanumadecorativemeat"                ,  "sanumameat" },
            { "securityfirmwarepatch"               ,  "securityfirmware" },
            { "specialisedlegacyfirmware"           ,  "legacyfirmware" },
            { "strangewakesolutions"                ,  "wakesolutions" },
            { "taggedencryptioncodes"               ,  "encryptioncodes" },
            { "unidentifiedscanarchives"            ,  "scanarchives" },
            { "unusualencryptedfiles"               ,  "encryptedfiles" },
            { "utgaroarmillennialeggs"              ,  "utgaroarmillenialeggs" },
            { "xihebiomorphiccompanions"            ,  "xihecompanions" },
            { "zeesszeantgrubglue"                  ,  "zeesszeantglue" },

            {"micro-weavecoolinghoses","coolinghoses"},
            {"energygridassembly","powergridassembly"},

            {"methanolmonohydrate","methanolmonohydratecrystals"},
            {"muonimager","mutomimager"},
            {"hardwarediagnosticsensor","diagnosticsensor"},

        };

        static public string FDNameTranslation(string old)
        {
            old = old.ToLowerInvariant();
            if (fdnamemangling.ContainsKey(old))
            {
                //System.Diagnostics.Debug.WriteLine("Sub " + old);
                return fdnamemangling[old];
            }
            else
                return old;
        }



        #endregion



    }
}


