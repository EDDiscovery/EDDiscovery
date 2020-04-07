/*
 * Copyright © 2016-2020 EDDiscovery development team
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
        public enum CatType { Commodity, Raw, Encoded, Manufactured };
        public CatType Category { get; private set; }                // either Commodity, Encoded, Manufactured, Raw

        public string TranslatedCategory { get; private set; }      // translation of above..

        public string Name { get; private set; }                    // name of it in nice text
        public string FDName { get; private set; }                  // fdname, lower case..

        public enum ItemType
        {
            VeryRare, Rare, Standard, Common, VeryCommon, Unknown,
            ConsumerItems, Chemicals, Drones, Foods, IndustrialMaterials, LegalDrugs, Machinery, Medicines, Metals, Minerals, Narcotics, PowerPlay,
            Salvage, Slaves, Technology, Textiles, Waste, Weapons,
        };

        public ItemType Type { get; private set; }                  // and its type, for materials its commonality, for commodities its group ("Metals" etc).
        public string TranslatedType { get; private set; }          // translation of above..        

        public string Shortname { get; private set; }               // short abv. name
        public Color Colour { get; private set; }                   // colour if its associated with one
        public bool Rarity { get; private set; }                    // if it is a rare commodity

        public bool IsCommodity { get { return Category == CatType.Commodity; } }
        public bool IsRaw { get { return Category == CatType.Raw; } }
        public bool IsEncoded { get { return Category == CatType.Encoded; } }
        public bool IsManufactured { get { return Category == CatType.Manufactured; } }
        public bool IsEncodedOrManufactured { get { return Category == CatType.Encoded || Category == CatType.Manufactured; } }
        public bool IsRareCommodity { get { return Rarity && Category.Equals(CatType.Commodity); } }
        public bool IsCommonMaterial { get { return Type == ItemType.Common || Type == ItemType.VeryCommon; } }
        public bool IsJumponium
        {
            get
            {
                return (FDName.Contains("arsenic") || FDName.Contains("cadmium") || FDName.Contains("carbon")
                    || FDName.Contains("germanium") || FDName.Contains("niobium") || FDName.Contains("polonium")
                    || FDName.Contains("vanadium") || FDName.Contains("yttrium"));
            }
        }

        static public CatType? CategoryFrom(string s)
        {
            if (Enum.TryParse<CatType>(s, true, out CatType res))
                return res;
            else
                return null;
        }

        public const int VeryCommonCap = 300;
        public const int CommonCap = 250;
        public const int StandardCap = 200;
        public const int RareCap = 150;
        public const int VeryRareCap = 100;

        public int? MaterialLimit()
        {
            if (Type == ItemType.VeryCommon) return VeryCommonCap;
            if (Type == ItemType.Common) return CommonCap;
            if (Type == ItemType.Standard) return StandardCap;
            if (Type == ItemType.Rare) return RareCap;
            if (Type == ItemType.VeryRare) return VeryRareCap;
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

        public static MaterialCommodityData GetByName(string longname)
        {
            if (cachelist == null)
                FillTable();

            List<MaterialCommodityData> lst = cachelist.Values.ToList();
            int i = lst.FindIndex(x => x.Name.Equals(longname));
            return i >= 0 ? lst[i] : null;
        }


        public static MaterialCommodityData[] GetAll()
        {
            if (cachelist == null)
                FillTable();

            return cachelist.Values.ToArray();
        }


        // use this delegate to find them
        public static MaterialCommodityData[] Get(Func<MaterialCommodityData, bool> func, bool sorted)
        {
            if (cachelist == null)
                FillTable();

            MaterialCommodityData[] items = cachelist.Values.Where(func).ToArray();

            if (sorted)
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
            return Get(x => x.Category == CatType.Commodity, sorted);
        }

        public static MaterialCommodityData[] GetMaterials(bool sorted)
        {
            return Get(x => x.Category != CatType.Commodity, sorted);
        }

        public static Tuple<ItemType, string>[] GetTypes(Func<MaterialCommodityData, bool> func, bool sorted)        // given predate, return type/translated types combos.
        {
            MaterialCommodityData[] mcs = GetAll();
            var types = mcs.Where(func).Select(x => new Tuple<ItemType, string>(x.Type, x.TranslatedType)).Distinct().ToArray();
            if (sorted)
                Array.Sort(types, delegate (Tuple<ItemType, string> l, Tuple<ItemType, string> r) { return l.Item2.CompareTo(r.Item2); });
            return types;
        }

        public static Tuple<CatType, string>[] GetCategories(Func<MaterialCommodityData, bool> func, bool sorted)   // given predate, return cat/translated cat combos.
        {
            MaterialCommodityData[] mcs = GetAll();
            var types = mcs.Where(func).Select(x => new Tuple<CatType, string>(x.Category, x.TranslatedCategory)).Distinct().ToArray();
            if (sorted)
                Array.Sort(types, delegate (Tuple<CatType, string> l, Tuple<CatType, string> r) { return l.Item2.CompareTo(r.Item2); });
            return types;
        }

        public static string[] GetMembersOfType(ItemType typename, bool sorted)
        {
            MaterialCommodityData[] mcs = GetAll();
            var members = mcs.Where(x => x.Type == typename).Select(x => x.Name).ToArray();
            if (sorted)
                Array.Sort(members);
            return members;
        }

        public static string[] GetFDNameMembersOfType(ItemType typename, bool sorted)
        {
            MaterialCommodityData[] mcs = GetAll();
            string[] members = mcs.Where(x => x.Type == typename).Select(x => x.FDName).ToArray();
            if (sorted)
                Array.Sort(members);
            return members;
        }


        public static string[] GetFDNameMembersOfCategory(CatType catname, bool sorted)
        {
            MaterialCommodityData[] mcs = GetAll();
            string[] members = mcs.Where(x => x.Category == catname).Select(x => x.FDName).ToArray();
            if (sorted)
                Array.Sort(members);
            return members;
        }

        #endregion

        public MaterialCommodityData()
        {
        }

        public MaterialCommodityData(CatType cs, string n, string fd, ItemType t, string shortn, Color cl, bool rare)
        {
            Category = cs;
            TranslatedCategory = Category.ToString().Tx(typeof(MaterialCommodityData));        // valid to pass this thru the Tx( system
            Name = n;
            FDName = fd;
            Type = t;
            string tn = Type.ToString().SplitCapsWord();
            TranslatedType = tn.Tx(typeof(MaterialCommodityData));                // valid to pass this thru the Tx( system
            Shortname = shortn;
            Colour = cl;
            Rarity = rare;
        }

        private void SetCache()
        {
            cachelist[this.FDName.ToLowerInvariant()] = this;
        }

        public static MaterialCommodityData EnsurePresent(CatType cat, string fdname)  // By FDNAME
        {
            if (!cachelist.ContainsKey(fdname.ToLowerInvariant()))
            {
                MaterialCommodityData mcdb = new MaterialCommodityData(cat, fdname.SplitCapsWordFull(), fdname, ItemType.Unknown, "", Color.Green, false);
                mcdb.SetCache();
                System.Diagnostics.Debug.WriteLine("Material not present: " + cat + "," + fdname);
            }

            return cachelist[fdname.ToLowerInvariant()];
        }


        #region Initial setup

        static Color CByType(ItemType s)
        {
            if (s == ItemType.VeryRare)
                return Color.Red;
            if (s == ItemType.Rare)
                return Color.Yellow;
            if (s == ItemType.VeryCommon)
                return Color.Cyan;
            if (s == ItemType.Common)
                return Color.Green;
            if (s == ItemType.Standard)
                return Color.SandyBrown;
            if (s == ItemType.Unknown)
                return Color.Red;
            System.Diagnostics.Debug.Assert(false);
            return Color.Black;
        }

        // Mats

        private static bool AddRaw(string name, ItemType typeofit, string shortname, string fdname = "")
        {
            return AddEntry(CatType.Raw, CByType(typeofit), name, typeofit, shortname, fdname);
        }

        private static bool AddEnc(string name, ItemType typeofit, string shortname, string fdname = "")
        {
            return AddEntry(CatType.Encoded, CByType(typeofit), name, typeofit, shortname, fdname);
        }

        private static bool AddManu(string name, ItemType typeofit, string shortname, string fdname = "")
        {
            return AddEntry(CatType.Manufactured, CByType(typeofit), name, typeofit, shortname, fdname);
        }

        // Commods

        private static bool AddCommodityRare(string aliasname, ItemType typeofit, string fdname)
        {
            return AddEntry(CatType.Commodity, Color.Green, aliasname, typeofit, "", fdname, true);
        }

        private static bool AddCommodity(string aliasname, ItemType typeofit, string fdname)        // fdname only if not a list.
        {
            return AddEntry(CatType.Commodity, Color.Green, aliasname, typeofit, "", fdname);
        }

        private static bool AddCommoditySN(string aliasname, ItemType typeofit, string shortname, string fdname)
        {
            return AddEntry(CatType.Commodity, Color.Green, aliasname, typeofit, shortname, fdname);
        }

        // fdname only useful if aliasname is not a list.
        private static bool AddCommodityList(string aliasnamelist, ItemType typeofit)
        {
            string[] list = aliasnamelist.Split(';');

            foreach (string name in list)
            {
                if (name.Length > 0)   // just in case a semicolon slips thru
                {
                    if (!AddEntry(CatType.Commodity, Color.Green, name, typeofit, "", ""))
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

        private static bool AddEntry(CatType catname, Color colour, string aliasname, ItemType typeofit, string shortname, string fdName, bool comrare = false)
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

            AddRaw("Carbon", ItemType.VeryCommon, "C");
            AddRaw("Iron", ItemType.VeryCommon, "Fe");
            AddRaw("Nickel", ItemType.VeryCommon, "Ni");
            AddRaw("Phosphorus", ItemType.VeryCommon, "P");
            AddRaw("Sulphur", ItemType.VeryCommon, "S");
            AddRaw("Lead", ItemType.VeryCommon, "Pb");
            AddRaw("Rhenium", ItemType.VeryCommon, "Re");

            AddRaw("Chromium", ItemType.Common, "Cr");
            AddRaw("Germanium", ItemType.Common, "Ge");
            AddRaw("Manganese", ItemType.Common, "Mn");
            AddRaw("Vanadium", ItemType.Common, "V");
            AddRaw("Zinc", ItemType.Common, "Zn");
            AddRaw("Zirconium", ItemType.Common, "Zr");
            AddRaw("Arsenic", ItemType.Common, "As");

            AddRaw("Niobium", ItemType.Standard, "Nb");        // realign to Anthors standard
            AddRaw("Tungsten", ItemType.Standard, "W");
            AddRaw("Molybdenum", ItemType.Standard, "Mo");
            AddRaw("Mercury", ItemType.Standard, "Hg");
            AddRaw("Boron", ItemType.Standard, "B");
            AddRaw("Cadmium", ItemType.Standard, "Cd");
            AddRaw("Tin", ItemType.Standard, "Sn");

            AddRaw("Selenium", ItemType.Rare, "Se");
            AddRaw("Yttrium", ItemType.Rare, "Y");
            AddRaw("Technetium", ItemType.Rare, "Tc");
            AddRaw("Tellurium", ItemType.Rare, "Te");
            AddRaw("Ruthenium", ItemType.Rare, "Ru");
            AddRaw("Polonium", ItemType.Rare, "Po");
            AddRaw("Antimony", ItemType.Rare, "Sb");

            // very common data
            AddEnc("Anomalous Bulk Scan Data", ItemType.VeryCommon, "ABSD", "bulkscandata");
            AddEnc("Atypical Disrupted Wake Echoes", ItemType.VeryCommon, "ADWE", "disruptedwakeechoes");
            AddEnc("Distorted Shield Cycle Recordings", ItemType.VeryCommon, "DSCR", "shieldcyclerecordings");
            AddEnc("Exceptional Scrambled Emission Data", ItemType.VeryCommon, "ESED", "scrambledemissiondata");
            AddEnc("Specialised Legacy Firmware", ItemType.VeryCommon, "SLF", "legacyfirmware");
            AddEnc("Unusual Encrypted Files", ItemType.VeryCommon, "UEF", "encryptedfiles");
            // common data
            AddEnc("Anomalous FSD Telemetry", ItemType.Common, "AFT", "fsdtelemetry");
            AddEnc("Inconsistent Shield Soak Analysis", ItemType.Common, "ISSA", "shieldsoakanalysis");
            AddEnc("Irregular Emission Data", ItemType.Common, "IED", "archivedemissiondata");
            AddEnc("Modified Consumer Firmware", ItemType.Common, "MCF", "consumerfirmware");
            AddEnc("Tagged Encryption Codes", ItemType.Common, "TEC", "encryptioncodes");
            AddEnc("Unidentified Scan Archives", ItemType.Common, "USA", "scanarchives");
            AddEnc("Pattern Beta Obelisk Data", ItemType.Common, "PBOD", "ancientculturaldata");
            AddEnc("Pattern Gamma Obelisk Data", ItemType.Common, "PGOD", "ancienthistoricaldata");
            // standard data
            AddEnc("Classified Scan Databanks", ItemType.Standard, "CSD", "scandatabanks");
            AddEnc("Cracked Industrial Firmware", ItemType.Standard, "CIF", "industrialfirmware");
            AddEnc("Open Symmetric Keys", ItemType.Standard, "OSK", "symmetrickeys");
            AddEnc("Strange Wake Solutions", ItemType.Standard, "SWS", "wakesolutions");
            AddEnc("Unexpected Emission Data", ItemType.Standard, "UED", "emissiondata");
            AddEnc("Untypical Shield Scans", ItemType.Standard, "USS", "shielddensityreports");
            AddEnc("Abnormal Compact Emissions Data", ItemType.Standard, "CED", "compactemissionsdata");
            AddEnc("Pattern Alpha Obelisk Data", ItemType.Standard, "PAOD", "ancientbiologicaldata");
            // rare data
            AddEnc("Aberrant Shield Pattern Analysis", ItemType.Rare, "ASPA", "shieldpatternanalysis");
            AddEnc("Atypical Encryption Archives", ItemType.Rare, "AEA", "encryptionarchives");
            AddEnc("Decoded Emission Data", ItemType.Rare, "DED");
            AddEnc("Divergent Scan Data", ItemType.Rare, "DSD", "encodedscandata");
            AddEnc("Eccentric Hyperspace Trajectories", ItemType.Rare, "EHT", "hyperspacetrajectories");
            AddEnc("Security Firmware Patch", ItemType.Rare, "SFP", "securityfirmware");
            AddEnc("Pattern Delta Obelisk Data", ItemType.Rare, "PDOD", "ancientlanguagedata");
            // very rare data
            AddEnc("Classified Scan Fragment", ItemType.VeryRare, "CFSD", "classifiedscandata");
            AddEnc("Modified Embedded Firmware", ItemType.VeryRare, "EFW", "embeddedfirmware");
            AddEnc("Adaptive Encryptors Capture", ItemType.VeryRare, "AEC", "adaptiveencryptors");
            AddEnc("Datamined Wake Exceptions", ItemType.VeryRare, "DWEx", "dataminedwake");
            AddEnc("Peculiar Shield Frequency Data", ItemType.VeryRare, "PSFD", "shieldfrequencydata");
            AddEnc("Pattern Epsilon Obelisk Data", ItemType.VeryRare, "PEOD", "ancienttechnologicaldata");
            //very common manufactured
            AddManu("Basic Conductors", ItemType.VeryCommon, "BaC");
            AddManu("Chemical Storage Units", ItemType.VeryCommon, "CSU");
            AddManu("Compact Composites", ItemType.VeryCommon, "CC");
            AddManu("Crystal Shards", ItemType.VeryCommon, "CS");
            AddManu("Grid Resistors", ItemType.VeryCommon, "GR");
            AddManu("Heat Conduction Wiring", ItemType.VeryCommon, "HCW");
            AddManu("Mechanical Scrap", ItemType.VeryCommon, "MS");
            AddManu("Salvaged Alloys", ItemType.VeryCommon, "SAll");
            AddManu("Worn Shield Emitters", ItemType.VeryCommon, "WSE");
            AddManu("Tempered Alloys", ItemType.VeryCommon, "TeA");
            // common manufactured
            AddManu("Chemical Processors", ItemType.Common, "CP");
            AddManu("Conductive Components", ItemType.Common, "CCo");
            AddManu("Filament Composites", ItemType.Common, "FiC");
            AddManu("Flawed Focus Crystals", ItemType.Common, "FFC", "uncutfocuscrystals");
            AddManu("Galvanising Alloys", ItemType.Common, "GA");
            AddManu("Heat Dispersion Plate", ItemType.Common, "HDP");
            AddManu("Heat Resistant Ceramics", ItemType.Common, "HRC");
            AddManu("Hybrid Capacitors", ItemType.Common, "HC");
            AddManu("Mechanical Equipment", ItemType.Common, "ME");
            AddManu("Shield Emitters", ItemType.Common, "SHE");
            // standard manufactured
            AddManu("Chemical Distillery", ItemType.Standard, "CHD");
            AddManu("Conductive Ceramics", ItemType.Standard, "CCe");
            AddManu("Electrochemical Arrays", ItemType.Standard, "EA");
            AddManu("Focus Crystals", ItemType.Standard, "FoC");
            AddManu("Heat Exchangers", ItemType.Standard, "HE");
            AddManu("High Density Composites", ItemType.Standard, "HDC");
            AddManu("Mechanical Components", ItemType.Standard, "MC");
            AddManu("Phase Alloys", ItemType.Standard, "PA");
            AddManu("Precipitated Alloys", ItemType.Standard, "PAll");
            AddManu("Shielding Sensors", ItemType.Standard, "SS");

            // new to 3.1 frontier data

            AddManu("Guardian Power Cell", ItemType.VeryCommon, "GPCe", "guardian_powercell");
            AddManu("Guardian Power Conduit", ItemType.Common, "GPC", "guardian_powerconduit");
            AddManu("Guardian Technology Component", ItemType.Standard, "GTC", "guardian_techcomponent");
            AddManu("Guardian Sentinel Weapon Parts", ItemType.Standard, "GSWP", "guardian_sentinel_weaponparts");
            AddManu("Guardian Sentinel Wreckage Components", ItemType.VeryCommon, "GSWC", "guardian_sentinel_wreckagecomponents");
            AddEnc("Guardian Weapon Blueprint Segment", ItemType.Rare, "GWBS", "guardian_weaponblueprint");
            AddEnc("Guardian Module Blueprint Segment", ItemType.Rare, "GMBS", "guardian_moduleblueprint");

            // new to 3.2 frontier data
            AddEnc("Guardian Vessel Blueprint Segment", ItemType.VeryRare, "GMVB", "guardian_vesselblueprint");

            AddManu("Bio-Mechanical Conduits", ItemType.Standard, "BMC", "TG_BioMechanicalConduits");
            AddManu("Propulsion Elements", ItemType.Standard, "PE", "TG_PropulsionElement");
            AddManu("Weapon Parts", ItemType.Standard, "WP", "TG_WeaponParts");
            AddManu("Wreckage Components", ItemType.Standard, "WRC", "TG_WreckageComponents");
            AddEnc("Ship Flight Data", ItemType.Standard, "SFD", "TG_ShipFlightData");
            AddEnc("Ship Systems Data", ItemType.Standard, "SSD", "TG_ShipSystemsData");

            // rare manufactured
            AddManu("Chemical Manipulators", ItemType.Rare, "CM");
            AddManu("Compound Shielding", ItemType.Rare, "CoS");
            AddManu("Conductive Polymers", ItemType.Rare, "CPo");
            AddManu("Configurable Components", ItemType.Rare, "CCom");
            AddManu("Heat Vanes", ItemType.Rare, "HV");
            AddManu("Polymer Capacitors", ItemType.Rare, "PCa");
            AddManu("Proto Light Alloys", ItemType.Rare, "PLA");
            AddManu("Refined Focus Crystals", ItemType.Rare, "RFC");
            AddManu("Proprietary Composites", ItemType.Rare, "FPC", "fedproprietarycomposites");
            AddManu("Thermic Alloys", ItemType.Rare, "ThA");
            // very rare manufactured
            AddManu("Core Dynamics Composites", ItemType.VeryRare, "FCC", "fedcorecomposites");
            AddManu("Biotech Conductors", ItemType.VeryRare, "BiC");
            AddManu("Exquisite Focus Crystals", ItemType.VeryRare, "EFC");
            AddManu("Imperial Shielding", ItemType.VeryRare, "IS");
            AddManu("Improvised Components", ItemType.VeryRare, "IC");
            AddManu("Military Grade Alloys", ItemType.VeryRare, "MGA");
            AddManu("Military Supercapacitors", ItemType.VeryRare, "MSC");
            AddManu("Pharmaceutical Isolators", ItemType.VeryRare, "PI");
            AddManu("Proto Heat Radiators", ItemType.VeryRare, "PHR");
            AddManu("Proto Radiolic Alloys", ItemType.VeryRare, "PRA");

            ItemType sv = ItemType.Salvage;
            AddCommodity("Thargoid Sensor", sv, "UnknownArtifact");
            AddCommodity("Thargoid Probe", sv, "UnknownArtifact2");
            AddCommodity("Thargoid Link", sv, "UnknownArtifact3");
            AddCommodity("Thargoid Resin", sv, "UnknownResin");
            AddCommodity("Thargoid Biological Matter", sv, "UnknownBiologicalMatter");
            AddCommodity("Thargoid Technology Samples", sv, "UnknownTechnologySamples");

            AddManu("Thargoid Carapace", ItemType.Common, "UKCP", "unknowncarapace");
            AddManu("Thargoid Energy Cell", ItemType.Standard, "UKEC", "unknownenergycell");
            AddManu("Thargoid Organic Circuitry", ItemType.VeryRare, "UKOC", "unknownorganiccircuitry");
            AddManu("Thargoid Technological Components", ItemType.Rare, "UKTC", "unknowntechnologycomponents");
            AddManu("Sensor Fragment", ItemType.VeryRare, "UES", "unknownenergysource");

            AddEnc("Thargoid Material Composition Data", ItemType.Standard, "UMCD", "tg_compositiondata");
            AddEnc("Thargoid Structural Data", ItemType.Common, "UKSD", "tg_structuraldata");
            AddEnc("Thargoid Residue Data", ItemType.Rare, "URDA", "tg_residuedata");
            AddEnc("Thargoid Ship Signature", ItemType.Standard, "USSig", "unknownshipsignature");
            AddEnc("Thargoid Wake Data", ItemType.Rare, "UWD", "unknownwakedata");

            #endregion

            #region Commodities - checked by netlogentry frontierdata against their spreadsheets. Use this tool to update the tables

            AddCommodity("Rockforth Fertiliser", ItemType.Chemicals, "RockforthFertiliser");
            AddCommodity("Agronomic Treatment", ItemType.Chemicals, "AgronomicTreatment");
            AddCommodityList("Explosives;Hydrogen Fuel;Hydrogen Peroxide;Liquid Oxygen;Mineral Oil;Nerve Agents;Pesticides;Surface Stabilisers;Synthetic Reagents;Water", ItemType.Chemicals);

            ItemType ci = ItemType.ConsumerItems;
            AddCommodityList("Clothing;Consumer Technology;Domestic Appliances;Evacuation Shelter;Survival Equipment", ci);
            AddCommodity("Duradrives", ci, "Duradrives");

            ItemType fd = ItemType.Foods;
            AddCommodityList("Algae;Animal Meat;Coffee;Fish;Food Cartridges;Fruit and Vegetables;Grain;Synthetic Meat;Tea", fd);

            ItemType im = ItemType.IndustrialMaterials;
            AddCommodityList("Ceramic Composites;Insulating Membrane;Polymers;Semiconductors;Superconductors", im);
            AddCommoditySN("Meta-Alloys", im, "MA", "metaalloys");
            AddCommoditySN("Micro-Weave Cooling Hoses", im, "MWCH", "coolinghoses");
            AddCommoditySN("Neofabric Insulation", im, "NFI", "");
            AddCommoditySN("CMM Composite", im, "CMMC", "");

            ItemType ld = ItemType.LegalDrugs;
            AddCommodityList("Beer;Bootleg Liquor;Liquor;Tobacco;Wine", ld);

            ItemType m = ItemType.Machinery;
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

            ItemType md = ItemType.Medicines;
            AddCommodityList("Advanced Medicines;Basic Medicines;Combat Stabilisers;Performance Enhancers;Progenitor Cells", md);
            AddCommodity("Agri-Medicines", md, "agriculturalmedicines");

            AddCommodity("Nanomedicines", md, "Nanomedicines"); // not in frontier data. Keep for now Jan 2020

            ItemType mt = ItemType.Metals;
            AddCommodityList("Aluminium;Beryllium;Bismuth;Cobalt;Copper;Gallium;Gold;Hafnium 178;Indium;Lanthanum;Lithium;Osmium;Palladium;Platinum;Praseodymium;Samarium;Silver;Tantalum;Thallium;Thorium;Titanium;Uranium", mt);
            AddCommodity("Platinum Alloy", mt, "PlatinumAloy");

            ItemType mi = ItemType.Minerals;
            AddCommodityList("Bauxite;Bertrandite;Bromellite;Coltan;Cryolite;Gallite;Goslarite;Methane Clathrate", mi);
            AddCommodityList("Indite;Jadeite;Lepidolite;Lithium Hydroxide;Moissanite;Painite;Pyrophyllite;Rutile;Taaffeite;Uraninite", mi);
            AddCommodity("Methanol Monohydrate Crystals", mi, "methanolmonohydratecrystals");
            AddCommodity("Low Temperature Diamonds", mi, "lowtemperaturediamond");
            AddCommodity("Void Opal", mi, "Opal");

            AddCommodity("Rhodplumsite", mi, "Rhodplumsite");
            AddCommodity("Serendibite", mi, "Serendibite");
            AddCommodity("Monazite", mi, "Monazite");
            AddCommodity("Musgravite", mi, "Musgravite");
            AddCommodity("Benitoite", mi, "Benitoite");
            AddCommodity("Grandidierite", mi, "Grandidierite");
            AddCommodity("Alexandrite", mi, "Alexandrite");

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
            AddCommodityList("Geological Samples;Military Intelligence;Mysterious Idol;Occupied CryoPod;Personal Effects;Precious Gems;Prohibited Research Materials", sv);
            AddCommodityList("Sap 8 Core Container;Scientific Research;Scientific Samples;Space Pioneer Relics;Tactical Data;Unstable Data Core", sv);
            AddCommodity("Large Survey Data Cache", sv, "largeexplorationdatacash");
            AddCommodity("Small Survey Data Cache", sv, "smallexplorationdatacash");
            AddCommodity("Ancient Artefact", sv, "USSCargoAncientArtefact");
            AddCommodity("Black Box", sv, "USSCargoBlackBox");
            AddCommodity("Political Prisoners", sv, "PoliticalPrisoner");
            AddCommodity("Hostages", sv, "Hostage");
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
            AddCommodity("Guardian Relic", sv, "AncientRelic");
            AddCommodity("Guardian Orb", sv, "AncientOrb");
            AddCommodity("Guardian Casket", sv, "AncientCasket");
            AddCommodity("Guardian Tablet", sv, "AncientTablet");
            AddCommodity("Guardian Urn", sv, "AncientUrn");
            AddCommodity("Guardian Totem", sv, "AncientTotem");

            AddCommodity("Mollusc Soft Tissue", sv, "M_TissueSample_Soft");
            AddCommodity("Pod Core Tissue", sv, "S_TissueSample_Cells");
            AddCommodity("Pod Dead Tissue", sv, "S_TissueSample_Surface");
            AddCommodity("Pod Surface Tissue", sv, "S_TissueSample_Core");
            AddCommodity("Mollusc Membrane", sv, "M3_TissueSample_Membrane");
            AddCommodity("Mollusc Mycelium", sv, "M3_TissueSample_Mycelium");
            AddCommodity("Mollusc Spores", sv, "M3_TissueSample_Spores");
            AddCommodity("Pod Shell Tissue", sv, "S6_TissueSample_Coenosarc");


            ItemType nc = ItemType.Narcotics;
            AddCommodity("Narcotics", nc, "BasicNarcotics");

            ItemType sl = ItemType.Slaves;
            AddCommodityList("Imperial Slaves;Slaves", sl);

            ItemType tc = ItemType.Technology;
            AddCommoditySN("Ion Distributor", tc, "IOD", "IonDistributor");
            AddCommodityList("Advanced Catalysers;Animal Monitors;Aquaponic Systems;Bioreducing Lichen;Computer Components", tc);
            AddCommodity("Auto-Fabricators", tc, "autofabricators");
            AddCommoditySN("Micro Controllers", tc, "MCC", "MicroControllers");
            AddCommodityList("Medical Diagnostic Equipment", tc);
            AddCommodityList("Nanobreakers;Resonating Separators;Robotics;Structural Regulators;Telemetry Suite", tc);
            AddCommodity("H.E. Suits", tc, "hazardousenvironmentsuits");
            AddCommoditySN("Hardware Diagnostic Sensor", tc, "DIS", "diagnosticsensor");
            AddCommodity("Muon Imager", tc, "mutomimager");
            AddCommodity("Land Enrichment Systems", tc, "TerrainEnrichmentSystems");

            ItemType tx = ItemType.Textiles;

            AddCommodityList("Conductive Fabrics;Leather;Military Grade Fabrics;Natural Fabrics;Synthetic Fabrics", tx);

            ItemType ws = ItemType.Waste;

            AddCommodityList("Biowaste;Chemical Waste;Scrap;Toxic Waste", ws);

            ItemType wp = ItemType.Weapons;
            AddCommodityList("Battle Weapons;Landmines;Personal Weapons;Reactive Armour", wp);
            AddCommodity("Non-Lethal Weapons", wp, "nonlethalweapons");

            #endregion

            #region Rare Commodities - checked by netlogentry frontierdata against their spreadsheets. Use this tool to update the tables

            AddCommodityRare("Apa Vietii", ItemType.Narcotics, "ApaVietii");
            AddCommodityRare("The Hutton Mug", ItemType.ConsumerItems, "TheHuttonMug");
            AddCommodityRare("Eranin Pearl Whisky", ItemType.LegalDrugs, "EraninPearlWhisky");
            AddCommodityRare("Lavian Brandy", ItemType.LegalDrugs, "LavianBrandy");
            AddCommodityRare("HIP 10175 Bush Meat", ItemType.Foods, "HIP10175BushMeat");
            AddCommodityRare("Albino Quechua Mammoth Meat", ItemType.Foods, "AlbinoQuechuaMammoth");
            AddCommodityRare("Utgaroar Millennial Eggs", ItemType.Foods, "UtgaroarMillenialEggs");
            AddCommodityRare("Witchhaul Kobe Beef", ItemType.Foods, "WitchhaulKobeBeef");
            AddCommodityRare("Karsuki Locusts", ItemType.Foods, "KarsukiLocusts");
            AddCommodityRare("Giant Irukama Snails", ItemType.Foods, "GiantIrukamaSnails");
            AddCommodityRare("Baltah'sine Vacuum Krill", ItemType.Foods, "BaltahSineVacuumKrill");
            AddCommodityRare("Ceti Rabbits", ItemType.Foods, "CetiRabbits");
            AddCommodityRare("Kachirigin Filter Leeches", ItemType.Medicines, "KachiriginLeaches");
            AddCommodityRare("Lyrae Weed", ItemType.Narcotics, "LyraeWeed");
            AddCommodityRare("Onionhead", ItemType.Narcotics, "OnionHead");
            AddCommodityRare("Tarach Spice", ItemType.Narcotics, "TarachTorSpice");
            AddCommodityRare("Wolf Fesh", ItemType.Narcotics, "Wolf1301Fesh");
            AddCommodityRare("Borasetani Pathogenetics", ItemType.Weapons, "BorasetaniPathogenetics");
            AddCommodityRare("HIP 118311 Swarm", ItemType.Weapons, "HIP118311Swarm");
            AddCommodityRare("Kongga Ale", ItemType.LegalDrugs, "KonggaAle");
            AddCommodityRare("Wuthielo Ku Froth", ItemType.LegalDrugs, "WuthieloKuFroth");
            AddCommodityRare("Alacarakmo Skin Art", ItemType.ConsumerItems, "AlacarakmoSkinArt");
            AddCommodityRare("Eleu Thermals", ItemType.ConsumerItems, "EleuThermals");
            AddCommodityRare("Eshu Umbrellas", ItemType.ConsumerItems, "EshuUmbrellas");
            AddCommodityRare("Karetii Couture", ItemType.ConsumerItems, "KaretiiCouture");
            AddCommodityRare("Njangari Saddles", ItemType.ConsumerItems, "NjangariSaddles");
            AddCommodityRare("Any Na Coffee", ItemType.Foods, "AnyNaCoffee");
            AddCommodityRare("CD-75 Kitten Brand Coffee", ItemType.Foods, "CD75CatCoffee");
            AddCommodityRare("Goman Yaupon Coffee", ItemType.Foods, "GomanYauponCoffee");
            AddCommodityRare("Volkhab Bee Drones", ItemType.Machinery, "VolkhabBeeDrones");
            AddCommodityRare("Kinago Violins", ItemType.ConsumerItems, "KinagoInstruments");
            AddCommodityRare("Nguna Modern Antiques", ItemType.ConsumerItems, "NgunaModernAntiques");
            AddCommodityRare("Rajukru Multi-Stoves", ItemType.ConsumerItems, "RajukruStoves");
            AddCommodityRare("Tiolce Waste2Paste Units", ItemType.ConsumerItems, "TiolceWaste2PasteUnits");
            AddCommodityRare("Chi Eridani Marine Paste", ItemType.Foods, "ChiEridaniMarinePaste");
            AddCommodityRare("Esuseku Caviar", ItemType.Foods, "EsusekuCaviar");
            AddCommodityRare("Live Hecate Sea Worms", ItemType.Foods, "LiveHecateSeaWorms");
            AddCommodityRare("Helvetitj Pearls", ItemType.Foods, "HelvetitjPearls");
            AddCommodityRare("HIP Proto-Squid", ItemType.Foods, "HIP41181Squid");
            AddCommodityRare("Coquim Spongiform Victuals", ItemType.Foods, "CoquimSpongiformVictuals");
            AddCommodityRare("Eden Apples Of Aerial", ItemType.Foods, "AerialEdenApple");
            AddCommodityRare("Neritus Berries", ItemType.Foods, "NeritusBerries");
            AddCommodityRare("Ochoeng Chillies", ItemType.Foods, "OchoengChillies");
            AddCommodityRare("Deuringas Truffles", ItemType.Foods, "DeuringasTruffles");
            AddCommodityRare("HR 7221 Wheat", ItemType.Foods, "HR7221Wheat");
            AddCommodityRare("Jaroua Rice", ItemType.Foods, "JarouaRice");
            AddCommodityRare("Belalans Ray Leather", ItemType.Textiles, "BelalansRayLeather");
            AddCommodityRare("Damna Carapaces", ItemType.Textiles, "DamnaCarapaces");
            AddCommodityRare("Rapa Bao Snake Skins", ItemType.Textiles, "RapaBaoSnakeSkins");
            AddCommodityRare("Vanayequi Ceratomorpha Fur", ItemType.Textiles, "VanayequiRhinoFur");
            AddCommodityRare("Bast Snake Gin", ItemType.LegalDrugs, "BastSnakeGin");
            AddCommodityRare("Thrutis Cream", ItemType.LegalDrugs, "ThrutisCream");
            AddCommodityRare("Wulpa Hyperbore Systems", ItemType.Machinery, "WulpaHyperboreSystems");
            AddCommodityRare("Aganippe Rush", ItemType.Medicines, "AganippeRush");
            AddCommodityRare("Terra Mater Blood Bores", ItemType.Medicines, "TerraMaterBloodBores");
            AddCommodityRare("Holva Duelling Blades", ItemType.Weapons, "HolvaDuellingBlades");
            AddCommodityRare("Kamorin Historic Weapons", ItemType.Weapons, "KamorinHistoricWeapons");
            AddCommodityRare("Gilya Signature Weapons", ItemType.Weapons, "GilyaSignatureWeapons");
            AddCommodityRare("Delta Phoenicis Palms", ItemType.Chemicals, "DeltaPhoenicisPalms");
            AddCommodityRare("Toxandji Virocide", ItemType.Chemicals, "ToxandjiVirocide");
            AddCommodityRare("Xihe Biomorphic Companions", ItemType.Technology, "XiheCompanions");
            AddCommodityRare("Sanuma Decorative Meat", ItemType.Foods, "SanumaMEAT");
            AddCommodityRare("Ethgreze Tea Buds", ItemType.Foods, "EthgrezeTeaBuds");
            AddCommodityRare("Ceremonial Heike Tea", ItemType.Foods, "CeremonialHeikeTea");
            AddCommodityRare("Tanmark Tranquil Tea", ItemType.Foods, "TanmarkTranquilTea");
            AddCommodityRare("AZ Cancri Formula 42", ItemType.Technology, "AZCancriFormula42");
            AddCommodityRare("Sothis Crystalline Gold", ItemType.Metals, "SothisCrystallineGold");
            AddCommodityRare("Kamitra Cigars", ItemType.LegalDrugs, "KamitraCigars");
            AddCommodityRare("Rusani Old Smokey", ItemType.LegalDrugs, "RusaniOldSmokey");
            AddCommodityRare("Yaso Kondi Leaf", ItemType.LegalDrugs, "YasoKondiLeaf");
            AddCommodityRare("Chateau De Aegaeon", ItemType.LegalDrugs, "ChateauDeAegaeon");
            AddCommodityRare("The Waters Of Shintara", ItemType.Medicines, "WatersOfShintara");
            AddCommodityRare("Ophiuch Exino Artefacts", ItemType.ConsumerItems, "OphiuchiExinoArtefacts");
            AddCommodityRare("Baked Greebles", ItemType.Foods, "BakedGreebles");
            AddCommodityRare("Aepyornis Egg", ItemType.Foods, "CetiAepyornisEgg");
            AddCommodityRare("Saxon Wine", ItemType.LegalDrugs, "SaxonWine");
            AddCommodityRare("Centauri Mega Gin", ItemType.LegalDrugs, "CentauriMegaGin");
            AddCommodityRare("Anduliga Fire Works", ItemType.Chemicals, "AnduligaFireWorks");
            AddCommodityRare("Banki Amphibious Leather", ItemType.Textiles, "BankiAmphibiousLeather");
            AddCommodityRare("Cherbones Blood Crystals", ItemType.Minerals, "CherbonesBloodCrystals");
            AddCommodityRare("Motrona Experience Jelly", ItemType.Narcotics, "MotronaExperienceJelly");
            AddCommodityRare("Geawen Dance Dust", ItemType.Narcotics, "GeawenDanceDust");
            AddCommodityRare("Gerasian Gueuze Beer", ItemType.LegalDrugs, "GerasianGueuzeBeer");
            AddCommodityRare("Haiden Black Brew", ItemType.Foods, "HaidneBlackBrew");
            AddCommodityRare("Havasupai Dream Catcher", ItemType.ConsumerItems, "HavasupaiDreamCatcher");
            AddCommodityRare("Burnham Bile Distillate", ItemType.LegalDrugs, "BurnhamBileDistillate");
            AddCommodityRare("Hip Organophosphates", ItemType.Chemicals, "HIPOrganophosphates");
            AddCommodityRare("Jaradharre Puzzle Box", ItemType.ConsumerItems, "JaradharrePuzzlebox");
            AddCommodityRare("Koro Kung Pellets", ItemType.Chemicals, "KorroKungPellets");
            AddCommodityRare("Void Extract Coffee", ItemType.Foods, "LFTVoidExtractCoffee");
            AddCommodityRare("Honesty Pills", ItemType.Medicines, "HonestyPills");
            AddCommodityRare("Non Euclidian Exotanks", ItemType.Machinery, "NonEuclidianExotanks");
            AddCommodityRare("LTT Hyper Sweet", ItemType.Foods, "LTTHyperSweet");
            AddCommodityRare("Mechucos High Tea", ItemType.Foods, "MechucosHighTea");
            AddCommodityRare("Medb Starlube", ItemType.IndustrialMaterials, "MedbStarlube");
            AddCommodityRare("Mokojing Beast Feast", ItemType.Foods, "MokojingBeastFeast");
            AddCommodityRare("Mukusubii Chitin-os", ItemType.Foods, "MukusubiiChitinOs");
            AddCommodityRare("Mulachi Giant Fungus", ItemType.Foods, "MulachiGiantFungus");
            AddCommodityRare("Ngadandari Fire Opals", ItemType.Minerals, "NgadandariFireOpals");
            AddCommodityRare("Tiegfries Synth Silk", ItemType.Textiles, "TiegfriesSynthSilk");
            AddCommodityRare("Uzumoku Low-G Wings", ItemType.ConsumerItems, "UzumokuLowGWings");
            AddCommodityRare("V Herculis Body Rub", ItemType.Medicines, "VHerculisBodyRub");
            AddCommodityRare("Wheemete Wheat Cakes", ItemType.Foods, "WheemeteWheatCakes");
            AddCommodityRare("Vega Slimweed", ItemType.Medicines, "VegaSlimWeed");
            AddCommodityRare("Altairian Skin", ItemType.ConsumerItems, "AltairianSkin");
            AddCommodityRare("Pavonis Ear Grubs", ItemType.Narcotics, "PavonisEarGrubs");
            AddCommodityRare("Jotun Mookah", ItemType.ConsumerItems, "JotunMookah");
            AddCommodityRare("Giant Verrix", ItemType.Machinery, "GiantVerrix");
            AddCommodityRare("Indi Bourbon", ItemType.LegalDrugs, "IndiBourbon");
            AddCommodityRare("Arouca Conventual Sweets", ItemType.Foods, "AroucaConventualSweets");
            AddCommodityRare("Tauri Chimes", ItemType.Medicines, "TauriChimes");
            AddCommodityRare("Zeessze Ant Grub Glue", ItemType.ConsumerItems, "ZeesszeAntGlue");
            AddCommodityRare("Pantaa Prayer Sticks", ItemType.Medicines, "PantaaPrayerSticks");
            AddCommodityRare("Fujin Tea", ItemType.Medicines, "FujinTea");
            AddCommodityRare("Chameleon Cloth", ItemType.Textiles, "ChameleonCloth");
            AddCommodityRare("Orrerian Vicious Brew", ItemType.Foods, "OrrerianViciousBrew");
            AddCommodityRare("Uszaian Tree Grub", ItemType.Foods, "UszaianTreeGrub");
            AddCommodityRare("Momus Bog Spaniel", ItemType.ConsumerItems, "MomusBogSpaniel");
            AddCommodityRare("Diso Ma Corn", ItemType.Foods, "DisoMaCorn");
            AddCommodityRare("Leestian Evil Juice", ItemType.LegalDrugs, "LeestianEvilJuice");
            AddCommodityRare("Azure Milk", ItemType.LegalDrugs, "BlueMilk");
            AddCommodityRare("Leathery Eggs", ItemType.ConsumerItems, "AlienEggs");
            AddCommodityRare("Alya Body Soap", ItemType.Medicines, "AlyaBodilySoap");
            AddCommodityRare("Vidavantian Lace", ItemType.ConsumerItems, "VidavantianLace");
            AddCommodityRare("Lucan Onionhead", ItemType.Narcotics, "TransgenicOnionHead");
            AddCommodityRare("Jaques Quinentian Still", ItemType.ConsumerItems, "JaquesQuinentianStill");
            AddCommodityRare("Soontill Relics", ItemType.ConsumerItems, "SoontillRelics");
            AddCommodityRare("Onionhead Alpha Strain", ItemType.Narcotics, "OnionHeadA");
            AddCommodityRare("Onionhead Beta Strain", ItemType.Narcotics, "OnionHeadB");
            AddCommodityRare("Galactic Travel Guide", sv, "GalacticTravelGuide");
            AddCommodityRare("Crom Silver Fesh", ItemType.Narcotics, "AnimalEffigies");
            AddCommodityRare("Shan's Charis Orchid", ItemType.ConsumerItems, "ShansCharisOrchid");
            AddCommodityRare("Buckyball Beer Mats", ItemType.ConsumerItems, "BuckyballBeerMats");
            AddCommodityRare("Master Chefs", ItemType.Slaves, "MasterChefs");
            AddCommodityRare("Personal Gifts", ItemType.ConsumerItems, "PersonalGifts");
            AddCommodityRare("Crystalline Spheres", ItemType.ConsumerItems, "CrystallineSpheres");
            AddCommodityRare("Ultra-Compact Processor Prototypes", ItemType.ConsumerItems, "Advert1");
            AddCommodityRare("Harma Silver Sea Rum", ItemType.Narcotics, "HarmaSilverSeaRum");
            AddCommodityRare("Earth Relics", sv, "EarthRelics");

            #endregion

            #region Powerplay - checked by netlogentry frontierdata against their spreadsheets. Use this tool to update the tables

            AddCommodity("Aisling Media Materials", ItemType.PowerPlay, "AislingMediaMaterials");
            AddCommodity("Aisling Sealed Contracts", ItemType.PowerPlay, "AislingMediaResources");
            AddCommodity("Aisling Programme Materials", ItemType.PowerPlay, "AislingPromotionalMaterials");
            AddCommodity("Alliance Trade Agreements", ItemType.PowerPlay, "AllianceTradeAgreements");
            AddCommodity("Alliance Legislative Contracts", ItemType.PowerPlay, "AllianceLegaslativeContracts");
            AddCommodity("Alliance Legislative Records", ItemType.PowerPlay, "AllianceLegaslativeRecords");
            AddCommodity("Lavigny Corruption Reports", ItemType.PowerPlay, "LavignyCorruptionDossiers");
            AddCommodity("Lavigny Field Supplies", ItemType.PowerPlay, "LavignyFieldSupplies");
            AddCommodity("Lavigny Garrison Supplies", ItemType.PowerPlay, "LavignyGarisonSupplies");
            AddCommodity("Core Restricted Package", ItemType.PowerPlay, "RestrictedPackage");
            AddCommodity("Liberal Propaganda", ItemType.PowerPlay, "LiberalCampaignMaterials");
            AddCommodity("Liberal Federal Aid", ItemType.PowerPlay, "FederalAid");
            AddCommodity("Liberal Federal Packages", ItemType.PowerPlay, "FederalTradeContracts");
            AddCommodity("Marked Military Arms", ItemType.PowerPlay, "LoanedArms");
            AddCommodity("Patreus Field Supplies", ItemType.PowerPlay, "PatreusFieldSupplies");
            AddCommodity("Patreus Garrison Supplies", ItemType.PowerPlay, "PatreusGarisonSupplies");
            AddCommodity("Hudson's Restricted Intel", ItemType.PowerPlay, "RestrictedIntel");
            AddCommodity("Hudson's Field Supplies", ItemType.PowerPlay, "RepublicanFieldSupplies");
            AddCommodity("Hudson Garrison Supplies", ItemType.PowerPlay, "RepublicanGarisonSupplies");
            AddCommodity("Sirius Franchise Package", ItemType.PowerPlay, "SiriusFranchisePackage");
            AddCommodity("Sirius Corporate Contracts", ItemType.PowerPlay, "SiriusCommercialContracts");
            AddCommodity("Sirius Industrial Equipment", ItemType.PowerPlay, "SiriusIndustrialEquipment");
            AddCommodity("Torval Trade Agreements", ItemType.PowerPlay, "TorvalCommercialContracts");
            AddCommodity("Torval Political Prisoners", ItemType.PowerPlay, "ImperialPrisoner");
            AddCommodity("Utopian Publicity", ItemType.PowerPlay, "UtopianPublicity");
            AddCommodity("Utopian Supplies", ItemType.PowerPlay, "UtopianFieldSupplies");
            AddCommodity("Utopian Dissident", ItemType.PowerPlay, "UtopianDissident");
            AddCommodity("Kumo Contraband Package", ItemType.PowerPlay, "IllicitConsignment");
            AddCommodity("Unmarked Military supplies", ItemType.PowerPlay, "UnmarkedWeapons");
            AddCommodity("Onionhead Samples", ItemType.PowerPlay, "OnionheadSamples");
            AddCommodity("Revolutionary supplies", ItemType.PowerPlay, "CounterCultureSupport");
            AddCommodity("Onionhead Derivatives", ItemType.PowerPlay, "OnionheadDerivatives");
            AddCommodity("Out Of Date Goods", ItemType.PowerPlay, "OutOfDateGoods");
            AddCommodity("Grom Underground Support", ItemType.PowerPlay, "UndergroundSupport");
            AddCommodity("Grom Counter Intelligence", ItemType.PowerPlay, "GromCounterIntelligence");
            AddCommodity("Yuri Grom's Military Supplies", ItemType.PowerPlay, "GromWarTrophies");
            AddCommodity("Marked Slaves", ItemType.PowerPlay, "MarkedSlaves");
            AddCommodity("Torval Deeds", ItemType.PowerPlay, "TorvalDeeds");

            #endregion

            // seen in logs
            AddCommodity("Drones", ItemType.Drones, "Drones");

            foreach (var x in cachelist.Values)
            {
                x.Name = BaseUtils.Translator.Instance.Translate(x.Name, "MaterialCommodityData." + x.FDName);
            }

            foreach (MaterialCommodityData d in cachelist.Values) System.Diagnostics.Debug.WriteLine(string.Format("{0},{1},{2},{3},{4}, {5}", d.Category, d.Type.ToString().SplitCapsWord(), d.FDName, d.Name, d.Shortname, d.Rarity ));
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


